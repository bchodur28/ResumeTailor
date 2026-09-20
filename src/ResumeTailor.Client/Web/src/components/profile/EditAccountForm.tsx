import React, { useEffect, useState } from "react";
import { updateAccountAsync } from "../../api/accounts";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { Controller, useFieldArray, useForm } from "react-hook-form";
import { stateOptions } from "../../data/states";
import { countryOptions } from "../../data/countries";
import type { AccountEditForm } from "../../models/forms/AccountEditForm";
import { useAccount } from "../../contexts/AccountContext";
import { useAuth0 } from "@auth0/auth0-react";
import type { AccountRequest } from "../../models/api/AccountRequest";
import Message from "../ui/Message";
import Input from "../forms/Input";
import SearchableSelect from "../forms/SearchableSelect";
import { Trash3 } from "react-bootstrap-icons";

const EditAccountForm = () => {
  const { getAccessTokenSilently } = useAuth0();
  const queryClient = useQueryClient();

  const { account, refreshAccount } = useAccount();

  const [isAccountUpdateSuccess, setIsAccountUpdateSuccess] = useState<
    boolean | null
  >(null);

  const {
    control: accountControl,
    register: registerAccount,
    handleSubmit: handleAccountSubmit,
    reset,
    formState: { errors, isValid },
  } = useForm<AccountEditForm>({
    mode: "onChange",
  });

  useEffect(() => {
    if (!account) {
      return;
    }

    const userState = stateOptions.find((x) => x.label === account.state);
    const userCountry = countryOptions.find((x) => x.label === account.country);
    const primaryTitle = account.titles.find((x) => x.isPrimary);

    reset({
      email: account.email,
      displayName: account.displayName,
      city: account.city,
      state: userState,
      country: userCountry,
      primaryTitle: primaryTitle
        ? { titleId: primaryTitle.id, value: primaryTitle.value }
        : { value: "" },
      additionalTitles: account.titles
        .filter((x) => !x.isPrimary)
        .map((x) => ({ titleId: x.id, value: x.value })),
      personalLinks: account.personalLinks.map((x) => ({
        personalLinkId: x.id,
        displayName: x.displayName,
        url: x.url,
      })),
    });
  }, [account, reset]);

  const {
    fields: titleFields,
    append: appendTitle,
    remove: removeTitle,
  } = useFieldArray({
    control: accountControl,
    name: "additionalTitles",
  });

  const {
    fields: personalLinkFields,
    append: appendPersonalLink,
    remove: removePersonalLink,
  } = useFieldArray({
    control: accountControl,
    name: "personalLinks",
  });

  const handleRemovePersonalLink = (index: number) => {
    removePersonalLink(index);
  };

  const handleRemoveTitle = (index: number) => {
    removeTitle(index);
  };

  const updateAccountMutation = useMutation({
    mutationFn: async (request: AccountRequest) => {
      const token = await getAccessTokenSilently({
        authorizationParams: { audience: import.meta.env.VITE_AUTH0_AUDIENCE },
      });

      return await updateAccountAsync(request, token);
    },
    onError: () => {
      console.error("Failed to update account");
      setIsAccountUpdateSuccess(false);
    },
    onSuccess: async () => {
      await refreshAccount();
      setIsAccountUpdateSuccess(true);
    },
  });

  const onSubmitAccount = (data: AccountEditForm) => {
    const request: AccountRequest = {
      email: data.email,
      displayName: data.displayName,
      city: data.city,
      state: data.state?.label ?? "",
      country: data.country?.label ?? "",
      titles: [
        {
          id: data.primaryTitle.titleId,
          value: data.primaryTitle.value,
          isPrimary: true,
        },
        ...data.additionalTitles.map((tf) => ({
          id: tf.titleId,
          value: tf.value,
          isPrimary: false,
        })),
      ],
      personalLinks: data.personalLinks.map((x) => ({
        id: x.personalLinkId,
        displayName: x.displayName,
        url: x.url,
      })),
    };
    updateAccountMutation.mutate(request);
  };

  if (!account) {
    return null;
  }
  return (
    <form
      className="flex flex-col gap-4"
      onSubmit={handleAccountSubmit(onSubmitAccount)}
    >
      <Input
        id="email"
        label="Email"
        registration={registerAccount("email", {
          required: "This field is required.",
          validate: (value) => value.includes("@") || "Invalid email address",
        })}
        error={errors.email?.message}
      />
      <Input
        id="displayName"
        label="Display Name"
        registration={registerAccount("displayName", {
          required: "This field is required.",
        })}
        error={errors.displayName?.message}
      />
      <Input
        id="city"
        label="City"
        registration={registerAccount("city", {
          required: "This field is required.",
        })}
        error={errors.city?.message}
      />
      <Controller
        name="state"
        control={accountControl}
        rules={{ required: "This field is required." }}
        render={({ field, fieldState }) => (
          <SearchableSelect
            id="state"
            label="State"
            options={stateOptions}
            value={field.value}
            onChange={field.onChange}
            onBlur={field.onBlur}
            isRequired={true}
            error={fieldState.error?.message}
          />
        )}
      />
      <Controller
        name="country"
        control={accountControl}
        rules={{ required: "This field is required." }}
        render={({ field, fieldState }) => (
          <SearchableSelect
            id="country"
            label="Country"
            options={countryOptions}
            value={field.value}
            onChange={field.onChange}
            onBlur={field.onBlur}
            isRequired={true}
            error={fieldState.error?.message}
          />
        )}
      />
      <Input
        id="primary-title"
        label="Primary Title"
        registration={registerAccount("primaryTitle.value", {
          required: "This field is required.",
        })}
        error={errors.primaryTitle?.value?.message}
      />

      {/* Additional Titles Section */}
      <div className="flex flex-col gap-2">
        <div className="flex justify-between items-center">
          <h4 className="font-semibold text-gray-900">Additional Titles</h4>
          <p className="text-sm text-gray-500">
            You can add up to 4 additional titles.
          </p>
        </div>
        {titleFields.map((title, index) => (
          <div key={title.id} className="flex gap-2">
            <Input
              id={`additional-title-${index}`}
              label=""
              registration={registerAccount(`additionalTitles.${index}.value`, {
                required: "Secondary title cannot be blank.",
              })}
              error={errors.additionalTitles?.[index]?.value?.message}
            />
            <button
              type="button"
              className="remove-btn"
              onClick={() => handleRemoveTitle(index)}
            >
              Remove
            </button>
          </div>
        ))}
        {titleFields.length < 4 && (
          <button
            type="button"
            className="btn-secondary"
            onClick={() => appendTitle({ value: "" })}
          >
            Add Title
          </button>
        )}
      </div>
      {/* Personal Websites Section */}
      <div className="flex flex-col gap-2">
        <div className="flex justify-between items-center">
          <h4 className="font-semibold text-gray-900">Personal Websites</h4>
          <p className="text-sm text-gray-500">
            You can add up to 10 websites.
          </p>
        </div>
        <div className="flex flex-col gap-6">
          {personalLinkFields.map((personalLink, index) => (
            <div
              key={personalLink.id}
              className="flex flex-col gap-4 border border-gray-300 p-4 rounded-md"
            >
              <div className="w-full flex flex-col gap-2 ">
                <div className="flex justify-between items-center">
                  <h4 className="text-lg font-semibold">
                    Personal Website{" "}
                    {personalLinkFields.length > 1 ? index + 1 : ""}
                  </h4>
                  <button
                    type="button"
                    className="remove-btn"
                    onClick={() => handleRemovePersonalLink(index)}
                  >
                    <Trash3 />
                  </button>
                </div>
                <Input
                  id={`website-${index}`}
                  label={`Website ${index + 1}`}
                  registration={registerAccount(
                    `personalLinks.${index}.displayName`,
                    {
                      required: "Website cannot be blank.",
                    },
                  )}
                  error={errors.personalLinks?.[index]?.displayName?.message}
                />
                <Input
                  id={`website-url-${index}`}
                  label={`Website ${index + 1} URL`}
                  registration={registerAccount(`personalLinks.${index}.url`, {
                    required: "Website URL cannot be blank.",
                  })}
                  error={errors.personalLinks?.[index]?.url?.message}
                />
              </div>
            </div>
          ))}
        </div>
        {personalLinkFields.length < 10 && (
          <button
            type="button"
            className="btn-secondary"
            onClick={() => appendPersonalLink({ displayName: "", url: "" })}
          >
            Add Website Link
          </button>
        )}
      </div>
      <button
        type="submit"
        className="confirm-btn"
        disabled={!isValid || updateAccountMutation.isPending}
      >
        {updateAccountMutation.isPending ? "Saving..." : "Save Account Profile"}
      </button>
      {isAccountUpdateSuccess === true && (
        <Message
          type="success"
          message="Account profile updated successfully."
          onClose={() => setIsAccountUpdateSuccess(null)}
        />
      )}
      {isAccountUpdateSuccess === false && (
        <Message
          type="error"
          message="Failed to update account profile."
          onClose={() => setIsAccountUpdateSuccess(null)}
        />
      )}
    </form>
  );
};

export default EditAccountForm;
