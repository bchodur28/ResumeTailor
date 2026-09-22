import { useAuth0 } from "@auth0/auth0-react";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useFieldArray, useForm } from "react-hook-form";
import { useEffect, useState } from "react";
import { Trash3 } from "react-bootstrap-icons";
import type { CompanyForm } from "../../models/forms/CompanyForm";
import type { CompanyRequest } from "../../models/profile/CompanyRequest";
import { useCompanies } from "../../hooks/useCompanies";
import {
  createCompanies,
  deleteCompanies,
  updateCompanies,
} from "../../api/experience";
import Input from "../forms/Input";
import Checkbox from "../forms/Checkbox";
import Message from "../ui/Message";

const EditCompaniesForm = () => {
  const { getAccessTokenSilently } = useAuth0();
  const queryClient = useQueryClient();
  const { data: companies, isLoading, isError } = useCompanies();
  const [isSavedSuccessfully, setIsSavedSuccessfully] = useState<
    boolean | null
  >(null);

  const {
    control,
    register,
    handleSubmit,
    reset,
    watch,
    formState: { errors },
  } = useForm<CompanyForm>({ defaultValues: { companies: companies ?? [] } });

  const formCompanies = watch("companies");

  const {
    fields: companyFields,
    append: appendCompany,
    remove: removeCompany,
  } = useFieldArray({ control, name: "companies" });

  useEffect(() => {
    if (companies) {
      reset({
        companies: companies.map((company) => ({
          id: company.id,
          name: company.name,
          title: company.title,
          location: company.location,
          started: company.started,
          ended: company.ended,
          generateBullets: company.generateBullets,
          maxGeneratedBulletCount: company.maxGeneratedBulletCount,
        })),
      });
    }
  }, [companies, reset]);

  const saveCompaniesMutation = useMutation({
    mutationFn: async (data: CompanyForm) => {
      const token = await getAccessTokenSilently({
        authorizationParams: { audience: import.meta.env.VITE_AUTH0_AUDIENCE },
      });
      const normalizedData = data.companies.map((company) => ({
        ...company,
        ended: company.ended === "" ? null : company.ended,
      }));
      const existingCompanies = companies ?? [];
      const toCreate: CompanyRequest[] = normalizedData.filter(
        (company) => company.id == null,
      );
      const toUpdate: CompanyRequest[] = normalizedData.filter(
        (company) => company.id != null,
      );
      const submittedIds = new Set(toUpdate.map((company) => company.id!));
      const toDelete = existingCompanies
        .filter((company) => !submittedIds.has(company.id))
        .map((company) => company.id);
      const requests: Promise<void>[] = [];
      if (toCreate.length > 0) requests.push(createCompanies(toCreate, token));
      if (toUpdate.length > 0) requests.push(updateCompanies(toUpdate, token));
      if (toDelete.length > 0) requests.push(deleteCompanies(toDelete, token));
      await Promise.all(requests);
    },
    onSuccess: () => {
      setIsSavedSuccessfully(true);
      queryClient.invalidateQueries({ queryKey: ["companies"] });
    },
    onError: (error) => {
      setIsSavedSuccessfully(false);
      console.error("Failed to save companies:", error);
    },
  });

  if (isLoading) return <div>Loading Companies...</div>;
  if (isError) return <div>Failed to load companies.</div>;

  return (
    <form
      className="flex flex-col gap-2"
      onSubmit={handleSubmit((data) => saveCompaniesMutation.mutate(data))}
    >
      {isSavedSuccessfully && (
        <Message
          type="success"
          message="Companies saved successfully."
          onClose={() => setIsSavedSuccessfully(null)}
        />
      )}
      {isSavedSuccessfully === false && (
        <Message
          type="error"
          message="Failed to save companies."
          onClose={() => setIsSavedSuccessfully(null)}
        />
      )}
      {companyFields.map((field, index) => {
        const companyId = formCompanies[index]?.id;

        const companyResponse = companies?.find(
          (company) => company.id === companyId,
        );

        const bulletCount = companyResponse?.bulletCount ?? 0;

        return (
          <div
            key={field.id}
            className="flex flex-col gap-4 border border-gray-300 p-4 rounded-md"
          >
            <div className="flex justify-between items-center">
              <h4 className="text-lg font-semibold">
                Company {companyFields.length > 1 ? index + 1 : ""}
              </h4>
              <button
                type="button"
                className="remove-btn"
                onClick={() => removeCompany(index)}
              >
                <Trash3 />
              </button>
            </div>
            <Input
              id={`companies[${index}].name`}
              label="Company Name"
              registration={register(`companies.${index}.name`, {
                required: "This field is required.",
              })}
              error={errors.companies?.[index]?.name?.message}
            />
            <Input
              id={`companies[${index}].title`}
              label="Title"
              registration={register(`companies.${index}.title`, {
                required: "This field is required.",
              })}
              error={errors.companies?.[index]?.title?.message}
            />
            <Input
              id={`companies[${index}].location`}
              label="Location"
              registration={register(`companies.${index}.location`, {
                required: "This field is required.",
              })}
              error={errors.companies?.[index]?.location?.message}
            />
            <Input
              id={`companies[${index}].started`}
              label="Started"
              type="date"
              registration={register(`companies.${index}.started`, {
                required: "This field is required.",
              })}
              error={errors.companies?.[index]?.started?.message}
            />
            <Input
              id={`companies[${index}].ended`}
              label="Ended"
              type="date"
              registration={register(`companies.${index}.ended`)}
            />
            <Checkbox
              id={`companies[${index}].generateBullets`}
              label="Generate Bullets"
              registration={register(`companies.${index}.generateBullets`)}
            />
            <Input
              id={`companies[${index}].maxGeneratedBulletCount`}
              label="Maximum Generated Bullets"
              type="number"
              registration={register(
                `companies.${index}.maxGeneratedBulletCount`,
                {
                  valueAsNumber: true,
                  min: { value: 0, message: "Must be zero or greater." },
                },
              )}
              error={
                errors.companies?.[index]?.maxGeneratedBulletCount?.message
              }
            />
            <p className="font-semibold">
              This company contains {bulletCount} bullets.
            </p>
          </div>
        );
      })}
      {companyFields.length === 0 && (
        <div className="flex justify-center rounded-lg bg-gray-200 border border-gray-300 p-4 shadow-md">
          <p className="text-gray-700 font-semibold">
            No company entries. Click to add.
          </p>
        </div>
      )}
      <div className="flex justify-end gap-2">
        <button
          type="button"
          className="btn-secondary"
          onClick={() =>
            appendCompany({
              id: null,
              name: "",
              title: "",
              location: "",
              started: "",
              ended: "",
              generateBullets: false,
              maxGeneratedBulletCount: 0,
            })
          }
        >
          Add Company
        </button>
        <button type="submit" className="confirm-btn">
          Save Companies
        </button>
      </div>
    </form>
  );
};

export default EditCompaniesForm;
