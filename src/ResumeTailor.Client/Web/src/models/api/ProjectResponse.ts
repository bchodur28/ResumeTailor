export type ProjectResponse = {
  id: number;
  name: string;
  description: string;
  started: string;
  ended: string | null;
  techStack: string | null;
  link: string | null;
  useForResume: boolean;
};
