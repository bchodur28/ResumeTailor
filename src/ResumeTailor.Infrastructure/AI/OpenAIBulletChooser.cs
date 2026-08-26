#pragma warning disable OPENAI001

using ResumeTailor.Application.Resumes.Interfaces;
using ResumeTailor.Application.Resumes.Models;
using OpenAI.Responses;
using System.Text.Json;
using Microsoft.Extensions.Options;
using System.ClientModel;

namespace ResumeTailor.Infrastructure.AI;

public sealed class OpenAIBulletChooser(ResponsesClient client, IOptions<OpenAIOptions> options) : IAiBulletChooser
{
    public async Task<BulletSelectionResult> ChooseBullets(IReadOnlyList<BulletSelectionContext> contexts, string jobDescription, CancellationToken cancellationToken = default)
    {
        var prompt = BuildPrompt(contexts, jobDescription);

        var responseOptions = new CreateResponseOptions
        {
            Model = options.Value.Model,
            TextOptions = new ResponseTextOptions
            {
                TextFormat = ResponseTextFormat.CreateJsonSchemaFormat(
                    jsonSchemaFormatName: "bullet_selection",
                    jsonSchema: BinaryData.FromString("""
                    {
                      "type": "object",
                      "properties": {
                        "companies": {
                          "type": "array",
                          "items": {
                            "type": "object",
                            "properties": {
                              "company": {
                                "type": "string"
                              },
                              "bullets": {
                                "type": "array",
                                "items": {
                                  "type": "string"
                                }
                              }
                            },
                            "required": [
                              "company",
                              "bullets"
                            ],
                            "additionalProperties": false
                          }
                        }
                      },
                      "required": [
                        "companies"
                      ],
                      "additionalProperties": false
                    }
                    """))
            }
        };

        responseOptions.InputItems.Add(ResponseItem.CreateUserMessageItem(prompt));

        var response = await client.CreateResponseAsync(responseOptions, cancellationToken);

        var json = response.Value.GetOutputText();

        var bulletSelection = JsonSerializer.Deserialize<BulletSelection>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new InvalidOperationException("OpenAI returned an empty bullet selection response.");

        var companyBullets = ValidateAndMap(bulletSelection, contexts);

        var usage = CreateAiUsage(response);

        return new BulletSelectionResult(companyBullets, usage);
    }

    private static AiUsage CreateAiUsage(ClientResult<ResponseResult> response)
    {
        return new AiUsage(
            InputTokens: response.Value.Usage.InputTokenCount,
            OutputTokens: response.Value.Usage.OutputTokenCount,
            TotalTokens: response.Value.Usage.TotalTokenCount,
            EstimatedCost: null);
    }

    private static Dictionary<string, IReadOnlyList<string>> ValidateAndMap(BulletSelection bulletSelection, IReadOnlyList<BulletSelectionContext> contexts)
    {
        var result = new Dictionary<string, IReadOnlyList<string>>();

        foreach (var context in contexts)
        {
            var companyResult = bulletSelection.Companies.FirstOrDefault(
                c => string.Equals(c.Company, context.Company, StringComparison.OrdinalIgnoreCase));

            if(companyResult is null)
            {
                result[context.Company] = [];
                continue;
            }

            var validBullets = companyResult.Bullets
                .Where(bullet => context.Bullets.Contains(bullet))
                .Take(context.MaxBullets)
                .ToList();

            result[context.Company] = validBullets;
        }

        return result;
    }

    private static string BuildPrompt(IReadOnlyList<BulletSelectionContext> context, string descrption)
    {
        var contextJson = JsonSerializer.Serialize(context, new JsonSerializerOptions { WriteIndented = true });

        return $$"""
            You are selecting resume bullets that best match a job description.

            JOB DESCRIPTION:
            {{descrption}}

            RESUME EXPERIENCE BULLETS:
            {{contextJson}}

            For each company:

            - Select no more the MaxBullets.
            - Only select bullets from that company's Bullets collection.
            - Do not rewrite or modify the bullets.
            - Rank the selected bullets from strongest to weakest match.
            - Follow AdditionalInstruction if provided.
            - Do not move bullets between campanies.

            Return the selected bullets for each company.
            """;
    }

    private sealed record BulletSelection(IReadOnlyList<CompanyBulletSelection> Companies);

    private sealed record CompanyBulletSelection(string Company, IReadOnlyList<string> Bullets);
}

#pragma warning restore OPENAI001

