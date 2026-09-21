import { useAuth0 } from "@auth0/auth0-react";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useFieldArray, useForm } from "react-hook-form";
import { useEffect, useState } from "react";
import { Trash3 } from "react-bootstrap-icons";
import type { BulletForm } from "../../models/forms/BulletForm";
import type { BulletRequest } from "../../models/api/BulletRequest";
import type { BulletDeleteRequest } from "../../models/api/BulletDeleteRequest";
import { useCompanyBullets } from "../../hooks/useCompanyBullet";
import {
  createBullets,
  updateBullets,
  deleteBullets,
} from "../../api/experience";
import TextArea from "../forms/TextArea";
import Message from "../ui/Message";

const EditCompanyBulletsForm = () => {
  const { getAccessTokenSilently } = useAuth0();
  const queryClient = useQueryClient();

  const { data: companyBullets, isLoading, isError } = useCompanyBullets();

  const [isSavedSuccessfully, setIsSavedSuccessfully] = useState<
    boolean | null
  >(null);

  const {
    control,
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<BulletForm>({
    defaultValues: { Bullets: [] },
  });

  const {
    fields: bulletFields,
    append: appendBullet,
    remove: removeBullet,
  } = useFieldArray({
    control,
    name: "Bullets",
  });

  useEffect(() => {
    if (!companyBullets) {
      return;
    }

    reset({
      Bullets: companyBullets.flatMap((company) =>
        company.bullets.map((bullet) => ({
          id: bullet.id,
          companyId: bullet.companyId,
          Value: bullet.Value,
          AiScore: bullet.AiScore,
        })),
      ),
    });
  }, [companyBullets, reset]);

  const saveBulletsMutation = useMutation({
    mutationFn: async (data: BulletForm) => {
      const token = await getAccessTokenSilently({
        authorizationParams: {
          audience: import.meta.env.VITE_AUTH0_AUDIENCE,
        },
      });

      const existingBullets = (companyBullets ?? []).flatMap(
        (company) => company.bullets,
      );

      const bulletsToCreate: BulletRequest[] = data.Bullets.filter(
        (bullet) => bullet.id == null,
      );

      const bulletsToUpdate: BulletRequest[] = data.Bullets.filter(
        (bullet) => bullet.id != null,
      );

      const submittedIds = new Set(
        bulletsToUpdate.map((bullet) => bullet.id!),
      );

      const bulletsToDelete: BulletDeleteRequest[] = existingBullets
        .filter((bullet) => !submittedIds.has(bullet.id))
        .map((bullet) => ({
          bulletId: bullet.id,
          companyId: bullet.companyId,
        }));

      const requests: Promise<void>[] = [];

      if (bulletsToCreate.length > 0) {
        requests.push(createBullets(bulletsToCreate, token));
      }
      if (bulletsToUpdate.length > 0) {
        requests.push(updateBullets(bulletsToUpdate, token));
      }
      if (bulletsToDelete.length > 0) {
        requests.push(deleteBullets(bulletsToDelete, token));
      }
      await Promise.all(requests);
    },
    onSuccess: () => {
      setIsSavedSuccessfully(true);
      queryClient.invalidateQueries({ queryKey: ["company-bullets"] });
    },
    onError: (error) => {
      setIsSavedSuccessfully(false);
      console.error("Failed to save bullets:", error);
    },
  });

  if (isLoading) {
    return <div>Loading Bullets...</div>;
  }

  if (isError) {
    return <div>Failed to load bullets.</div>;
  }

  return (
    <form
      className="flex flex-col gap-2"
      onSubmit={handleSubmit((data) => saveBulletsMutation.mutate(data))}
    >
      {isSavedSuccessfully && (
        <Message
          type="success"
          message="Bullets saved successfully."
          onClose={() => setIsSavedSuccessfully(null)}
        />
      )}
      {isSavedSuccessfully === false && (
        <Message
          type="error"
          message="Failed to save bullets."
          onClose={() => setIsSavedSuccessfully(null)}
        />
      )}
      {(companyBullets ?? []).map((company) => {
        const companyBulletIndexes = bulletFields
          .map((field, index) => ({ field, index }))
          .filter(({ field }) => field.companyId === company.companyId);

        return (
          <div
            key={company.companyId}
            className="flex flex-col gap-4 border border-gray-300 p-4 rounded-md"
          >
            <h4 className="text-lg font-semibold">{company.companyName}</h4>
            {companyBulletIndexes.map(({ field, index }) => (
              <div key={field.id} className="flex gap-2 items-start">
                <TextArea
                  id={`bullets[${index}].Value`}
                  label={`Bullet ${
                    companyBulletIndexes.length > 1
                      ? companyBulletIndexes.findIndex(
                          (entry) => entry.index === index,
                        ) + 1
                      : ""
                  }`}
                  registration={register(`Bullets.${index}.Value`, {
                    required: "This field is required.",
                  })}
                  error={errors.Bullets?.[index]?.Value?.message}
                  rows={2}
                />
                <button
                  type="button"
                  className="remove-btn"
                  onClick={() => removeBullet(index)}
                >
                  <Trash3 />
                </button>
              </div>
            ))}
            {companyBulletIndexes.length === 0 && (
              <div className="flex justify-center rounded-lg bg-gray-200 border border-gray-300 p-4 shadow-md">
                <p className="text-gray-700 font-semibold">
                  No bullets for this company. Click to add.
                </p>
              </div>
            )}
            <div className="flex justify-end">
              <button
                type="button"
                className="btn-secondary"
                onClick={() =>
                  appendBullet({
                    id: null,
                    companyId: company.companyId,
                    Value: "",
                    AiScore: null,
                  })
                }
              >
                Add Bullet
              </button>
            </div>
          </div>
        );
      })}
      {(companyBullets ?? []).length === 0 && (
        <div className="flex justify-center rounded-lg bg-gray-200 border border-gray-300 p-4 shadow-md">
          <p className="text-gray-700 font-semibold">
            No companies found. Add a company to manage bullets.
          </p>
        </div>
      )}
      <div className="flex justify-end gap-2">
        <button type="submit" className="confirm-btn">
          Save Bullets
        </button>
      </div>
    </form>
  );
};

export default EditCompanyBulletsForm;
