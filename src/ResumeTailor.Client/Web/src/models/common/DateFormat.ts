export const DateFormat = {
  MMM_YYYY: "mmM_YYYY",
  MMMM_YYYY: "mmmM_YYYY",
  MM_YYYY: "mM_YYYY",
  MM_YY: "mM_YY",
} as const;

export type DateFormatValue = (typeof DateFormat)[keyof typeof DateFormat];

export const dateFormatOptions = [
  { id: DateFormat.MMM_YYYY, label: "Jan 2026" },
  { id: DateFormat.MMMM_YYYY, label: "January 2026" },
  { id: DateFormat.MM_YYYY, label: "01/2026" },
  { id: DateFormat.MM_YY, label: "01/26" },
];

export const formatDate = (
  date: string | null | undefined,
  format: DateFormatValue = DateFormat.MMM_YYYY,
): string => {
  if (!date) return "";

  // Supports DateOnly strings from your .NET API: YYYY-MM-DD
  const [year, month] = date.split("-").map(Number);

  if (!year || !month || month < 1 || month > 12) return "";

  const dateValue = new Date(Date.UTC(year, month - 1, 1));

  switch (format) {
    case DateFormat.MMM_YYYY:
      return `${dateValue.toLocaleString("en-US", {
        month: "short",
        timeZone: "UTC",
      })} ${year}`;

    case DateFormat.MMMM_YYYY:
      return `${dateValue.toLocaleString("en-US", {
        month: "long",
        timeZone: "UTC",
      })} ${year}`;

    case DateFormat.MM_YYYY:
      return `${String(month).padStart(2, "0")}/${year}`;

    case DateFormat.MM_YY:
      return `${String(month).padStart(2, "0")}/${String(year).slice(-2)}`;

    default:
      return "";
  }
};
