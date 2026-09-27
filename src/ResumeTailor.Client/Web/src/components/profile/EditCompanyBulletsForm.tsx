import { useAuth0 } from "@auth0/auth0-react";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useFieldArray, useForm } from "react-hook-form";
import { useEffect, useState } from "react";
import { Trash3 } from "react-bootstrap-icons";
import type { BulletForm } from "../../models/forms/BulletForm";
import type { BulletRequest } from "../../api/contracts/bullets/BulletRequest";
import type { BulletDeleteRequest } from "../../api/contracts/bullets/BulletDeleteRequest";
import { useCompanyBullets } from "../../hooks/useCompanyBullet";
import {
  createBullets,
  updateBullets,
  deleteBullets,
} from "../../api/experienceApi";
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
    defaultValues: { bullets: [] },
  });

  const {
    fields: bulletFields,
    append: appendBullet,
    remove: removeBullet,
  } = useFieldArray({
    control,
    name: "bullets",
    keyName: "fieldId",
  });

  useEffect(() => {
    if (!companyBullets) {
      return;
    }

    reset({
      bullets: companyBullets.flatMap((company) =>
        company.bullets.map((bullet) => ({
          id: bullet.id,
          companyId: bullet.companyId,
          value: bullet.value,
          aiScore: bullet.aiScore,
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

      const bulletsToCreate: BulletRequest[] = data.bullets.filter(
        (bullet) => bullet.id == null,
      );

      const existingBulletMap = new Map(
        existingBullets.map((bullet) => [bullet.id, bullet]),
      );

      const bulletsToUpdate: BulletRequest[] = data.bullets.filter((bullet) => {
        if (bullet.id == null) {
          return false;
        }
        const existingBullet = existingBulletMap.get(bullet.id);

        if (!existingBullet) {
          return false;
        }

        return existingBullet.value !== bullet.value;
      });

      const submittedIds = new Set(
        data.bullets
          .filter((bullet) => bullet.id != null)
          .map((bullet) => bullet.id!),
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
      console.log("Create: ", bulletsToCreate);
      console.log("Update: ", bulletsToUpdate);
      console.log("Delete: ", bulletsToDelete);
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
      onSubmit={handleSubmit((data) => {
        saveBulletsMutation.mutate(data);
      })}
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
              <div key={field.fieldId} className="flex gap-2 items-end">
                <TextArea
                  id={`bullets[${index}].value`}
                  label={`Bullet ${
                    companyBulletIndexes.length > 1
                      ? companyBulletIndexes.findIndex(
                          (entry) => entry.index === index,
                        ) + 1
                      : ""
                  }`}
                  registration={register(`bullets.${index}.value`, {
                    required: "This field is required.",
                  })}
                  error={errors.bullets?.[index]?.value?.message}
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
                    value: "",
                    aiScore: null,
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
