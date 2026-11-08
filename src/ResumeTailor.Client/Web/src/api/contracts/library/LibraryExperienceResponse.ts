import type { LibraryBulletResponse } from "./LibraryBulletResponse";
import type { LibrarySummaryResponse } from "./LibrarySummaryResponse";
import type { Experience } from "../../../models/Experience/Experience";

export type LibraryExperienceResponse = Experience & {
  id: number;
  primarySummary: LibrarySummaryResponse | null;
  letAiChooseSummary: boolean;
  summaries: LibrarySummaryResponse[];
  letAiChooseBullets: boolean;
  maxBulletsToChoose: number | null;
  bullets: LibraryBulletResponse[];
  resumeCount: number;
};
