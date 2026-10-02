import { useMutation } from "@tanstack/react-query";
import { useNavigate } from "react-router-dom";
import Card from "../components/ui/Card";
import { useAccount } from "../contexts/AccountContext";
import { generateResume } from "../api/resumeApi";
import { useState } from "react";
import type { GenerateResumeRequest } from "../api/contracts/GenerateResumeRequest";

const Generate = () => {
  const { account, isLoadingAccount } = useAccount();
  const navigate = useNavigate();

  const [description, setDescription] = useState("");

  const generateResumeMutation = useMutation({
    mutationFn: ({
      accountId,
      request,
    }: {
      accountId: number;
      request: GenerateResumeRequest;
    }) => generateResume(accountId, request),

    onSuccess: (resumeId) => {
      navigate(`/resumes/${resumeId}`);
    },
  });

  if (account == null) {
    return <div>No Account</div>;
  }

  if (isLoadingAccount) {
    return <div>Loading account...</div>;
  }

  return (
    <div className="flex justify-center">
      <div>
        <h1 className="text-2xl font-semibold mb-4">Generate your Resume</h1>
        <Card className="w-5xl flex flex-col">
          <textarea
            className="border border-gray-300 rounded p-2 w-full h-[80vh]"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
          ></textarea>
          <button
            className="btn self-end mt-2"
            disabled={generateResumeMutation.isPending}
            onClick={() =>
              generateResumeMutation.mutate({
                accountId: account.id,
                request: { description },
              })
            }
          >
            {generateResumeMutation.isPending
              ? "Generating resume..."
              : "Generate Resume"}
          </button>
        </Card>
      </div>
    </div>
  );
};

export default Generate;
