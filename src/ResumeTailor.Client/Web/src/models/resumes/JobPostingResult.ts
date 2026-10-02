export type JobPostingResult = {
  id: string;
  companyName: string | null;
  jobTitle: string | null;
  location: string | null;
  workStyle: string | null;
  salaryMin: number | null;
  salaryMax: number | null;
  salary: number | null;
  salaryPeriod: string | null;
  salaryCurrency: string | null;
};
