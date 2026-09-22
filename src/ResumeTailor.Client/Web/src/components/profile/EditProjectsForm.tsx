import { useAuth0 } from "@auth0/auth0-react";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useFieldArray, useForm } from "react-hook-form";
import { useEffect, useState } from "react";
import { Trash3 } from "react-bootstrap-icons";
import type { ProjectForm } from "../../models/forms/ProjectForm";
import type { ProjectRequest } from "../../models/profile/ProjectRequest";
import { useProjects } from "../../hooks/useProjects";
import {
  createProjects,
  deleteProjects,
  updateProjects,
} from "../../api/experience";
import Input from "../forms/Input";
import Checkbox from "../forms/Checkbox";
import Message from "../ui/Message";
import TextArea from "../forms/TextArea";

const EditProjectsForm = () => {
  const { getAccessTokenSilently } = useAuth0();
  const queryClient = useQueryClient();
  const { data: projects, isLoading, isError } = useProjects();
  const [isSavedSuccessfully, setIsSavedSuccessfully] = useState<
    boolean | null
  >(null);
  const {
    control,
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<ProjectForm>({ defaultValues: { projects: projects ?? [] } });
  const {
    fields: projectFields,
    append: appendProject,
    remove: removeProject,
  } = useFieldArray({ control, name: "projects" });

  useEffect(() => {
    if (projects) {
      reset({ projects: projects.map((project) => ({ ...project })) });
    }
  }, [projects, reset]);

  const saveProjectsMutation = useMutation({
    mutationFn: async (data: ProjectForm) => {
      const token = await getAccessTokenSilently({
        authorizationParams: {
          audience: import.meta.env.VITE_AUTH0_AUDIENCE,
        },
      });

      const normalizedData = data.projects.map((project) => ({
        ...project,
        ended: project.ended === "" ? null : project.ended,
        techStack: project.techStack === "" ? null : project.techStack,
        link: project.link === "" ? null : project.link,
      }));

      const existingProjects = projects ?? [];

      const toCreate: ProjectRequest[] = normalizedData.filter(
        (project) => project.id == null,
      );

      const toUpdate: ProjectRequest[] = normalizedData.filter(
        (project) => project.id != null,
      );

      const submittedIds = new Set(toUpdate.map((project) => project.id!));

      const toDelete = existingProjects
        .filter((project) => !submittedIds.has(project.id))
        .map((project) => project.id);

      const requests: Promise<void>[] = [];

      if (toCreate.length > 0) {
        requests.push(createProjects(toCreate, token));
      }
      if (toUpdate.length > 0) {
        requests.push(updateProjects(toUpdate, token));
      }
      if (toDelete.length > 0) {
        requests.push(deleteProjects(toDelete, token));
      }
      await Promise.all(requests);
    },
    onSuccess: () => {
      setIsSavedSuccessfully(true);
      queryClient.invalidateQueries({ queryKey: ["projects"] });
    },
    onError: (error) => {
      setIsSavedSuccessfully(false);
      console.error("Failed to save projects:", error);
    },
  });

  if (isLoading) return <div>Loading Projects...</div>;
  if (isError) return <div>Failed to load projects.</div>;

  return (
    <form
      className="flex flex-col gap-2"
      onSubmit={handleSubmit((data) => saveProjectsMutation.mutate(data))}
    >
      {isSavedSuccessfully && (
        <Message
          type="success"
          message="Projects saved successfully."
          onClose={() => setIsSavedSuccessfully(null)}
        />
      )}
      {isSavedSuccessfully === false && (
        <Message
          type="error"
          message="Failed to save projects."
          onClose={() => setIsSavedSuccessfully(null)}
        />
      )}
      {projectFields.map((field, index) => (
        <div
          key={field.id}
          className="flex flex-col gap-4 border border-gray-300 p-4 rounded-md"
        >
          <div className="flex justify-between items-center">
            <h4 className="text-lg font-semibold">
              Project {projectFields.length > 1 ? index + 1 : ""}
            </h4>
            <button
              type="button"
              className="remove-btn"
              onClick={() => removeProject(index)}
            >
              <Trash3 />
            </button>
          </div>
          <Input
            id={`projects[${index}].name`}
            label="Project Name"
            registration={register(`projects.${index}.name`, {
              required: "This field is required.",
            })}
            error={errors.projects?.[index]?.name?.message}
          />
          <TextArea
            id={`projects[${index}].description`}
            label="Description"
            registration={register(`projects.${index}.description`, {
              required: "This field is required.",
            })}
            error={errors.projects?.[index]?.description?.message}
          />
          <Input
            id={`projects[${index}].started`}
            label="Started"
            type="date"
            registration={register(`projects.${index}.started`, {
              required: "This field is required.",
            })}
            error={errors.projects?.[index]?.started?.message}
          />
          <Input
            id={`projects[${index}].ended`}
            label="Ended"
            type="date"
            registration={register(`projects.${index}.ended`)}
          />
          <TextArea
            id={`projects[${index}].techStack`}
            label="Tech Stack"
            registration={register(`projects.${index}.techStack`)}
            rows={2}
          />
          <Input
            id={`projects[${index}].link`}
            label="Link"
            type="url"
            registration={register(`projects.${index}.link`)}
          />
          <Checkbox
            id={`projects[${index}].useForResume`}
            label="Use For Resume"
            registration={register(`projects.${index}.useForResume`)}
          />
        </div>
      ))}
      {projectFields.length === 0 && (
        <div className="flex justify-center rounded-lg bg-gray-200 border border-gray-300 p-4 shadow-md">
          <p className="text-gray-700 font-semibold">
            No project entries. Click to add.
          </p>
        </div>
      )}
      <div className="flex justify-end gap-2">
        <button
          type="button"
          className="btn-secondary"
          onClick={() =>
            appendProject({
              id: null,
              name: "",
              description: "",
              started: "",
              ended: "",
              techStack: "",
              link: "",
              useForResume: false,
            })
          }
        >
          Add Project
        </button>
        <button type="submit" className="confirm-btn">
          Save Projects
        </button>
      </div>
    </form>
  );
};

export default EditProjectsForm;
