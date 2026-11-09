import Card from "../../components/ui/Card";
import { useAuth0 } from "@auth0/auth0-react";
import { useState } from "react";
import { useLibraryExperienceListItems } from "../../hooks/useLibraryExperienceListItems";
import {
  archiveLibraryExperience,
  deleteLibraryExperience,
} from "../../api/experienceApi";
import { restoreLibraryExperience } from "../../api/experienceApi";
import type { ExperienceTypeValue } from "../../models/resumes/ExperienceType";
import Button from "../../components/ui/buttons/Button";
import {
  Archive,
  ArrowCounterclockwise,
  Plus,
  Trash3,
} from "react-bootstrap-icons";
import { useNavigate } from "react-router-dom";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { LibraryExperienceListItemResponse } from "../../api/contracts/library/LibraryExperienceListItemResponse";
import Message from "../../components/ui/feedback/Message";

type ExperienceAction = "archive" | "delete" | "restore";

type FeedbackMessage = {
  type: "success" | "error";
  message: string;
};

const getExperienceTypeLabel = (experienceTypeValue: ExperienceTypeValue) => {
  switch (experienceTypeValue) {
    case "employment":
      return "Employment";
    case "internship":
      return "Internship";
    case "freelance":
      return "Freelance";
    case "personalProject":
      return "Personal Project";
    case "academicProject":
      return "Academic Project";
    case "other":
      return "Other";
    default:
      return "Unknown";
  }
};

const LibraryExperiencePage = () => {
  const navigate = useNavigate();
  const { getAccessTokenSilently } = useAuth0();

  const [showArchived, setShowArchived] = useState(false);
  const [feedbackMessage, setFeedbackMessage] =
    useState<FeedbackMessage | null>(null);

  const {
    data: experiences,
    isLoading,
    error,
  } = useLibraryExperienceListItems(showArchived);

  const queryClient = useQueryClient();

  const experienceMutation = useMutation({
    mutationFn: async ({
      experience,
      action,
    }: {
      experience: LibraryExperienceListItemResponse;
      action: ExperienceAction;
    }) => {
      const token = await getAccessTokenSilently({
        authorizationParams: {
          audience: import.meta.env.VITE_AUTH0_AUDIENCE,
        },
      });

      switch (action) {
        case "archive":
          return archiveLibraryExperience(experience.id, token);

        case "delete":
          return deleteLibraryExperience(experience.id, token);

        case "restore":
          return restoreLibraryExperience(experience.id, token);
      }
    },

    onSuccess: async (_, variables) => {
      const messages: Record<ExperienceAction, string> = {
        archive: "Experience successfully archived",
        delete: "Experience successfully deleted",
        restore: "Experience successfully restored",
      };
      setFeedbackMessage({
        type: "success",
        message: messages[variables.action],
      });
      await queryClient.invalidateQueries({
        queryKey: ["library-experiences-list-items"],
      });
    },
    onError: (error, variables) => {
      console.error("Failed to update experience:", error);
      const messages: Record<ExperienceAction, string> = {
        archive: "Failed to archive experience.",
        restore: "Failed to restore experience.",
        delete: "Failed to delete experience.",
      };
      setFeedbackMessage({
        type: "error",
        message: messages[variables.action],
      });
    },
  });

  const handleExperienceAction = (
    experience: LibraryExperienceListItemResponse,
  ) => {
    let action: ExperienceAction;

    if (showArchived) {
      action = "restore";
    } else if (experience.resumeCount > 0) {
      action = "archive";
    } else {
      action = "delete";
    }

    if (action === "delete") {
      const confirmed = window.confirm(
        "Are you sure you want to permanently delete this experience?",
      );

      if (!confirmed) {
        return;
      }
    }

    setFeedbackMessage(null);
    experienceMutation.mutate({ experience, action });
  };

  return (
    <div className="flex flex-col gap-4 items-center w-xl max-[640px]:w-full">
      <h1 className="text-2xl font-bold">Experience Library</h1>
      <p className=" text-gray-600">
        Add and manage your professional experiences. You can include multiple
        entries, including jobs, internships, and other relevant work
        experiences.
      </p>
      <Card className="w-xl max-[640px]:w-full">
        <div className="flex flex-col gap-2">
          {feedbackMessage && (
            <Message
              type={feedbackMessage.type}
              message={feedbackMessage.message}
              onClose={() => setFeedbackMessage(null)}
            />
          )}
          {isLoading && <p>Loading...</p>}
          {error && <p>Error loading experiences</p>}
          {experiences && experiences.length === 0 && (
            <div className="text-center text-gray-500 border border-gray-300 p-4 rounded-lg">
              <p>
                {showArchived
                  ? "No archived experiences found"
                  : "No experiences found"}
              </p>
            </div>
          )}
          {experiences && experiences.length > 0 && (
            <ul className="w-full flex flex-col gap-2">
              {experiences.map((experience) => (
                <li
                  className="primary-border p-2 rounded-lg cursor-pointer hover:bg-gray-50 hover:border-gray-400 hover:shadow-sm transition-colors duration-300"
                  key={experience.id}
                  onClick={() =>
                    navigate(`/library/experience/${experience.id}/edit`)
                  }
                >
                  <div className="flex justify-between">
                    <h4 className="text-base font-bold primary-color-dark">
                      {experience.organization
                        ? experience.organization
                        : experience.title}
                    </h4>
                    <p className="primary-color text-sm font-semibold">
                      {getExperienceTypeLabel(experience.type)}
                    </p>
                  </div>
                  <div className="flex justify-between">
                    <div>
                      <p className="text-gray-600 text-sm font-medium">
                        Bullet Count:{" "}
                        <span className="text-base font-medium text-gray-700">
                          {experience.bulletCount}
                        </span>
                      </p>
                      <p className="text-gray-600 text-sm font-medium">
                        Summary Count:{" "}
                        <span className="text-base font-medium text-gray-700">
                          {experience.summaryCount}
                        </span>
                      </p>
                      <p className="text-gray-600 text-sm font-medium">
                        Resume Count:{" "}
                        <span className="text-base font-medium text-gray-700">
                          {experience.resumeCount}
                        </span>
                      </p>
                    </div>
                    <div className="flex items-end gap-2">
                      <Button
                        label={
                          showArchived
                            ? "Restore"
                            : experience.resumeCount > 0
                              ? "Archive"
                              : "Delete"
                        }
                        iconComponent={
                          showArchived
                            ? ArrowCounterclockwise
                            : experience.resumeCount > 0
                              ? Archive
                              : Trash3
                        }
                        labelSlideOut={true}
                        styleType={
                          showArchived
                            ? "plain"
                            : experience.resumeCount > 0
                              ? "plain"
                              : "danger"
                        }
                        onClick={() => handleExperienceAction(experience)}
                      />
                    </div>
                  </div>
                </li>
              ))}
            </ul>
          )}
          <div className="flex flex-1 justify-between gap-2">
            <Button
              label={showArchived ? "Show Active" : "Show Archived"}
              onClick={() => setShowArchived((prev) => !prev)}
              labelSlideOut={false}
              styleType="plain"
              grow
            />
            <Button
              label="Create Experience"
              onClick={() => {}}
              labelSlideOut={false}
              grow
            />
          </div>
        </div>
      </Card>
    </div>
  );
};

export default LibraryExperiencePage;
