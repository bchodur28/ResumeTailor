export const SummaryGenerationMode = {
  None: "none",
  UseDefault: "useDefault",
  TailorDefault: "tailorDefault",
  GenerateNew: "generateNew",
} as const;

export type SummaryGenerationModeValue =
  (typeof SummaryGenerationMode)[keyof typeof SummaryGenerationMode];

export const SummaryGenerationModeOptions = [
  { id: SummaryGenerationMode.None, label: "None" },
  {
    id: SummaryGenerationMode.UseDefault,
    label: "Use Default",
  },
  {
    id: SummaryGenerationMode.TailorDefault,
    label: "Tailor Default",
  },
  {
    id: SummaryGenerationMode.GenerateNew,
    label: "Generate New",
  },
];
