import Input from "../../components/forms/Input";
import {
  Controller,
  get,
  useFieldArray,
  useForm,
  useWatch,
  type FieldPathByValue,
  type RegisterOptions,
} from "react-hook-form";
import Message from "../../components/ui/feedback/Message";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useEffect, useState } from "react";

import { useAuth0 } from "@auth0/auth0-react";
import SearchableSelect from "../../components/forms/SearchableSelect";
import { stateOptions } from "../../data/states";
import type { AccountRequest } from "../../api/contracts/accounts/AccountRequest";
import type { AccountEditForm } from "../../models/forms/AccountEditForm";
import { updateAccountAsync } from "../../api/accountsApi";
import { countryOptions } from "../../data/countries";
import { useAccount } from "../../contexts/AccountContext";
import { Trash3 } from "react-bootstrap-icons";
import Card from "../../components/ui/Card";
import type { Option } from "../../models/forms/Option";
import Button from "../../components/ui/buttons/Button";

type AccountTextField = FieldPathByValue<
  AccountEditForm,
  string | null | undefined
>;

type AccountSelectField = "state" | "country";

const createListTag = (label: string, value?: string | null) => {
  let valueToDisplay = value ?? "Not specified";
  return (
    <li className="flex gap-2">
      <p className="font-semibold text-gray-600">{label}:</p>
      <p className="font-bold text-gray-700">{valueToDisplay}</p>
    </li>
  );
};

const LibraryHeaderPage = () => {
  const { getAccessTokenSilently } = useAuth0();
  const queryClient = useQueryClient();

  const { account, refreshAccount } = useAccount();

  const [isAccountUpdateSuccess, setIsAccountUpdateSuccess] = useState<
    boolean | null
  >(null);

  const [isEditingAccount, setIsEditingAccount] = useState<boolean>(false);

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

  const watchAccount = useWatch({
    control: accountControl,
  });

  function createInputOrView(
    label: string,
    field: AccountTextField,
    options?: RegisterOptions<AccountEditForm, AccountTextField>,
  ) {
    const value = get(watchAccount, field);
    if (!isEditingAccount) {
      return createListTag(label, value?.toString());
    }
    return (
      <Input
        id={field}
        label={label}
        registration={registerAccount(field, options)}
        error={get(errors, field)?.message}
      />
    );
  }

  function createControllerOrView(
    label: string,
    name: AccountSelectField,
    options: Option[],
    isRequired = false,
  ) {
    if (!isEditingAccount) {
      return createListTag(label, watchAccount[name]?.label);
    }
    return (
      <Controller
        name={name}
        control={accountControl}
        rules={{ required: isRequired ? "This field is required." : false }}
        render={({ field, fieldState }) => (
          <SearchableSelect
            id={field.name}
            label={label}
            options={options}
            value={field.value}
            onChange={field.onChange}
            onBlur={field.onBlur}
            isRequired={isRequired}
            error={fieldState.error?.message}
          />
        )}
      />
    );
  }

  if (!account) {
    return null;
  }

  return (
    <div className="flex flex-col gap-4 items-center w-xl max-[640px]:w-full">
      <h1 className="text-2xl font-bold">Header Library</h1>
      <p className=" text-gray-600">
        Add and manage your header information. This is the information that
        will be displayed at the top of your resume. AI can be used to pick
        additional titles for you, if selected on the generated page.
      </p>
      <Card className="w-xl max-[640px]:w-full">
        <h4 className="font-semibold text-gray-900 text-lg underline">
          Main Header Information
        </h4>
        <form
          className="flex flex-col gap-4"
          onSubmit={handleAccountSubmit(onSubmitAccount)}
        >
          <div
            className={`flex flex-col ${isEditingAccount ? "gap-4" : "gap-1"}`}
          >
            {createInputOrView("Email", "email", {
              required: "This field is required.",
              validate: (value) =>
                value?.toString().includes("@") || "Invalid email address",
            })}
            {createInputOrView("Display Name", "displayName", {
              required: "This field is required.",
            })}
            {createInputOrView("City", "city", {
              required: "This field is required.",
            })}
            {createControllerOrView("State", "state", stateOptions, true)}
            {createControllerOrView("Country", "country", countryOptions, true)}
            {createInputOrView("Primary Title", "primaryTitle.value", {
              required: "This field is required.",
            })}
            {/* Additional Titles Section */}
            <div className="flex flex-col gap-2 mt-2">
              <div className="flex justify-between items-center max-[640px]:flex-col max-[640px]:items-start">
                <h4 className="font-semibold text-gray-900 text-lg underline">
                  Additional Titles
                </h4>
                <p className="text-sm text-gray-500">
                  You can add up to 4 additional titles.
                </p>
              </div>
              {titleFields.map((title, index) => (
                <div key={title.id} className="flex gap-2">
                  {createInputOrView(
                    `Title ${index + 1}`,
                    `additionalTitles.${index}.value`,
                    {
                      required: "This field is required.",
                    },
                  )}
                  {isEditingAccount && (
                    <div className="flex items-end">
                      <Button
                        label="Remove"
                        iconComponent={Trash3}
                        styleType="danger"
                        onClick={() => handleRemoveTitle(index)}
                        labelSlideOut={true}
                      />
                    </div>
                  )}
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
            <div className="flex flex-col gap-2 mt-2">
              <div className="flex justify-between items-center max-[640px]:flex-col max-[640px]:items-start">
                <h4 className="font-semibold text-gray-900 text-lg underline">
                  Personal Websites
                </h4>
                <p className="text-sm text-gray-500 ">
                  You can add up to 10 websites.
                </p>
              </div>
              <div className="flex flex-col gap-2">
                {personalLinkFields.map((personalLink, index) => (
                  <div
                    key={personalLink.id}
                    className="flex flex-col gap-4 border border-gray-300 p-4 rounded-md"
                  >
                    <div className="w-full flex flex-col gap-2 ">
                      <div className="flex justify-between items-center">
                        <h4 className="text-md font-semibold">
                          Personal Website{" "}
                          {personalLinkFields.length > 1 ? index + 1 : ""}
                        </h4>
                        {isEditingAccount && (
                          <Button
                            label="Remove"
                            iconComponent={Trash3}
                            styleType="danger"
                            onClick={() => handleRemovePersonalLink(index)}
                            labelSlideOut={true}
                          />
                        )}
                      </div>

                      {createInputOrView(
                        `Website ${index + 1}`,
                        `personalLinks.${index}.displayName`,
                        {
                          required: "Website cannot be blank.",
                        },
                      )}
                      {createInputOrView(
                        `Website ${index + 1} URL`,
                        `personalLinks.${index}.url`,
                        {
                          required: "Website URL cannot be blank.",
                        },
                      )}
                    </div>
                  </div>
                ))}
              </div>
              {personalLinkFields.length < 10 && isEditingAccount && (
                <Button
                  label="Add Website Link"
                  styleType="plain"
                  onClick={() =>
                    appendPersonalLink({ displayName: "", url: "" })
                  }
                />
              )}
            </div>
          </div>
          <Button
            styleType="plain"
            label={isEditingAccount ? "View Header Info" : "Edit Header Info"}
            onClick={() => setIsEditingAccount(!isEditingAccount)}
          />
          {isEditingAccount && (
            <Button
              type="submit"
              styleType="confirm"
              label={
                updateAccountMutation.isPending
                  ? "Saving..."
                  : "Save Account Profile"
              }
            />
          )}
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
      </Card>
    </div>
  );
};

export default LibraryHeaderPage;
