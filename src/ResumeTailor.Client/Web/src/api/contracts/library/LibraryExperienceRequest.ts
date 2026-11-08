import type { Experience } from "../../../models/Experience/Experience";
import type { LibrarySummaryRequest } from "./LibrarySummaryRequest";
import type { LibraryBulletRequest } from "./LibraryBulletRequest";

export type LibraryExperienceRequest = Experience & {
  letAiChooseBullets: boolean;
  letAiChooseSummary: boolean;
  maxBulletsToChoose: number | null;
  summaries: LibrarySummaryRequest[];
  bullets: LibraryBulletRequest[];
};
