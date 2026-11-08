import type { ResolvedSummary } from "./ResolvedSummary";
import type { ResolvedBullet } from "./ResolvedBullets";

import type { Experience } from "./Experience";

export type ResolvedExperience = Experience & {
  resumeExperienceId: number | null;
  libraryExperienceId: number;
  summaries: ResolvedSummary[];
  sortOrder: number;
  bullets: ResolvedBullet[];
  useInResume: boolean;
};
