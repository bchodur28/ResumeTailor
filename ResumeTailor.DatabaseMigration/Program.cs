
using Microsoft.Data.Sqlite;
using Microsoft.Data.SqlClient;
using System.Globalization;

// Source: verified SQLite backup
var sqlitePath =
    @"C:\Databases\ResumeTailor\Backups\resume-tailor-backup-2026-11-18.db";

// Destination: new SQL Server database
var sqlServerConnection =
    "Server=(localdb)\\BrianSQLServer;" +
    "Database=ResumeTailor;" +
    "Integrated Security=True;" +
    "TrustServerCertificate=True;";

if (!File.Exists(sqlitePath))
    throw new FileNotFoundException("SQLite backup not found.", sqlitePath);

var sqliteConnectionString = new SqliteConnectionStringBuilder
{
    DataSource = sqlitePath,
    Mode = SqliteOpenMode.ReadOnly
}.ToString();

using var source = new SqliteConnection(sqliteConnectionString);
using var destination = new SqlConnection(sqlServerConnection);

source.Open();
destination.Open();

// Parent tables must precede tables referencing them.
// Based on the generated InitialSqlServer migration.
string[] tables =
[
    "Account",
    "SiteExtractionDefinition",
    "LibraryEducation",
    "LibraryExperience",
    "PersonalLink",
    "Resume",
    "Skill",
    "Title",
    "FieldExtractionDefinition",
    "LibraryBullet",
    "LibrarySummary",
    "ResumeAiAnalysis",
    "ResumeAiMetaData",
    "ResumeAppearance",
    "ResumeApplicationTracking",
    "ResumeEducationSelection",
    "ResumeExperience",
    "ResumeJobPosting",
    "FieldPattern",
    "ResumeAiInsight",
    "ResumeBullet",
    "ResumeSummary"
];

static string Quote(string name) => "[" + name.Replace("]", "]]") + "]";

static object ConvertValue(object value, string sqlType)
{
    if (value is DBNull)
        return DBNull.Value;

    return sqlType.ToLowerInvariant() switch
    {
        "date" or "datetime" or "datetime2" or "smalldatetime"
            when value is string text =>
                DateTime.Parse(
                    text,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.RoundtripKind),

        "decimal" or "numeric" or "money" or "smallmoney" =>
            Convert.ToDecimal(value, CultureInfo.InvariantCulture),

        "bit" => Convert.ToBoolean(value, CultureInfo.InvariantCulture),

        _ => value
    };
}

// Inspect actual source and destination tables.
var sourceTables = new HashSet<string>(
    StringComparer.OrdinalIgnoreCase);

using (var command = source.CreateCommand())
{
    command.CommandText =
        "SELECT name FROM sqlite_master WHERE type = 'table'";

    using var reader = command.ExecuteReader();
    while (reader.Read())
        sourceTables.Add(reader.GetString(0));
}

var destinationTables = new HashSet<string>(
    StringComparer.OrdinalIgnoreCase);

using (var command = new SqlCommand(
    "SELECT name FROM sys.tables WHERE schema_id = SCHEMA_ID('dbo')",
    destination))
{
    using var reader = command.ExecuteReader();
    while (reader.Read())
        destinationTables.Add(reader.GetString(0));
}

// Stop if the planned list doesn't cover the target schema.
var unplannedTargets = destinationTables.Except(tables,
    StringComparer.OrdinalIgnoreCase)
    .Where(t => t != "__EFMigrationsHistory")
    .ToArray();

if (unplannedTargets.Length > 0)
    throw new Exception(
        "Unplanned SQL Server tables: " +
        string.Join(", ", unplannedTargets));

// Report old tables that aren't in the new model.
var sourceOnly = sourceTables.Except(destinationTables,
    StringComparer.OrdinalIgnoreCase)
    .Where(t => t != "sqlite_sequence" &&
                t != "__EFMigrationsHistory")
    .ToArray();

if (sourceOnly.Length > 0)
    Console.WriteLine(
        "WARNING - source-only tables not imported: " +
        string.Join(", ", sourceOnly));

// Don't import into a partially populated database.
foreach (var table in tables)
{
    if (!destinationTables.Contains(table))
        throw new Exception($"Missing SQL Server table: {table}");

    using var command = new SqlCommand(
        $"SELECT COUNT_BIG(*) FROM dbo.{Quote(table)}",
        destination);

    if ((long)command.ExecuteScalar()! > 0)
        throw new Exception($"Destination table {table} is not empty.");
}

using var transaction = destination.BeginTransaction();

try
{
    foreach (var table in tables)
    {
        if (!sourceTables.Contains(table))
        {
            Console.WriteLine($"SKIPPED missing SQLite table: {table}");
            continue;
        }

        // Read source column names.
        var sourceColumns = new HashSet<string>(
            StringComparer.OrdinalIgnoreCase);

        using (var command = source.CreateCommand())
        {
            command.CommandText = $"PRAGMA table_info({Quote(table)})";

            using var reader = command.ExecuteReader();
            while (reader.Read())
                sourceColumns.Add(reader.GetString(1));
        }

        // Read destination column names and SQL types.
        var targetColumns = new List<(string Name, string Type, bool Identity)>();

        using (var command = new SqlCommand("""
            SELECT c.name, TYPE_NAME(c.user_type_id), c.is_identity
            FROM sys.columns c
            WHERE c.object_id = OBJECT_ID(@table)
              AND c.is_computed = 0
            ORDER BY c.column_id
            """, destination, transaction))
        {
            command.Parameters.AddWithValue("@table", "dbo." + table);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                targetColumns.Add((
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetBoolean(2)));
            }
        }

        var columns = targetColumns
            .Where(c => sourceColumns.Contains(c.Name))
            .ToArray();

        if (columns.Length == 0)
            throw new Exception($"No matching columns for {table}");

        // Fail rather than silently omit a source column
        // that still exists in the target schema.
        var missingTargetValues = targetColumns
            .Where(c => !sourceColumns.Contains(c.Name))
            .Select(c => c.Name)
            .ToArray();

        if (missingTargetValues.Length > 0)
            throw new Exception(
                $"{table} has destination-only columns: " +
                string.Join(", ", missingTargetValues));

        var columnNames = string.Join(", ",
            columns.Select(c => Quote(c.Name)));

        var parameterNames = string.Join(", ",
            columns.Select((_, i) => "@p" + i));

        using var readCommand = source.CreateCommand();
        readCommand.CommandText =
            $"SELECT {columnNames} FROM {Quote(table)}";

        using var rows = readCommand.ExecuteReader();

        bool hasIdentity = columns.Any(c => c.Identity);

        if (hasIdentity)
        {
            using var enable = new SqlCommand(
                $"SET IDENTITY_INSERT dbo.{Quote(table)} ON",
                destination, transaction);
            enable.ExecuteNonQuery();
        }

        int count = 0;

        while (rows.Read())
        {
            using var insert = new SqlCommand(
                $"INSERT INTO dbo.{Quote(table)} ({columnNames}) " +
                $"VALUES ({parameterNames})",
                destination, transaction);

            for (int i = 0; i < columns.Length; i++)
            {
                insert.Parameters.AddWithValue(
                    "@p" + i,
                    ConvertValue(rows.GetValue(i), columns[i].Type));
            }

            insert.ExecuteNonQuery();
            count++;
        }

        if (hasIdentity)
        {
            using var disable = new SqlCommand(
                $"SET IDENTITY_INSERT dbo.{Quote(table)} OFF",
                destination, transaction);
            disable.ExecuteNonQuery();
        }

        Console.WriteLine($"{table}: {count} rows copied");
    }

    transaction.Commit();
    Console.WriteLine("DATA IMPORT COMPLETED.");
} catch (Exception ex)
{
    transaction.Rollback();
    Console.WriteLine("IMPORT FAILED - TRANSACTION ROLLED BACK.");
    Console.WriteLine(ex.Message);
    throw;
}
