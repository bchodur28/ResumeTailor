import type { GenerateResumeRequest } from "../../api/contracts/GenerateResumeRequest";

export type GenerateResumeForm = Omit<GenerateResumeRequest, "requestId">;
