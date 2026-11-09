import { useFieldArray, useForm, useWatch } from "react-hook-form";
import Message from "../../components/ui/feedback/Message";
import { useMutation } from "@tanstack/react-query";
import { useEffect, useState } from "react";

import { useAuth0 } from "@auth0/auth0-react";
import type { AccountRequest } from "../../api/contracts/accounts/AccountRequest";
import type { AccountEditForm } from "../../models/forms/AccountEditForm";
import { updateAccountAsync } from "../../api/accountsApi";
import { useAccount } from "../../contexts/AccountContext";
import { Eyeglasses, Pen, PlusCircle, Trash3 } from "react-bootstrap-icons";
import Card from "../../components/ui/Card";
import Button from "../../components/ui/buttons/Button";
import CollapsibleSection from "./components/CollapsibleSection";
import InputView from "./components/InputView";
import Dropdown from "../ResumeDetails/components/Dropdown";
import ControllerView from "./components/ControllerView";
import { dateFormatOptions } from "../../models/common/DateFormat";
import TextAreaView from "./components/TextAreaView";

const LibraryHeaderPage = () => {
  const { getAccessTokenSilently } = useAuth0();

  const { account, refreshAccount } = useAccount();

  const [isAccountUpdateSuccess, setIsAccountUpdateSuccess] = useState<
    boolean | null
  >(null);

  const [isEditingAccount, setIsEditingAccount] = useState(false);

  const [isEditingTitles, setIsEditingTitles] = useState(false);
  const [isEditingPersonalLinks, setIsEditingPersonalLinks] = useState(false);

  const {
    control: accountControl,
    register: registerAccount,
    handleSubmit: handleAccountSubmit,
    reset,
    formState: { errors, isValid, isDirty },
  } = useForm<AccountEditForm>({
    mode: "onChange",
  });

  useEffect(() => {
    if (!account) {
      return;
    }

    const primaryTitle = account.titles.find((x) => x.isPrimary);

    reset({
      email: account.email,
      displayName: account.displayName,
      location: account.location,
      dateFormat: account.dateFormat,
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
      summary: account.summary,
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
      location: data.location,
      dateFormat: data.dateFormat,
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
      summary: data.summary,
    };
    updateAccountMutation.mutate(request);
  };

  const watchAccount = useWatch({
    control: accountControl,
  });

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
        {isDirty && (
          <div className="mb-2">
            <Message
              type="note"
              message="You have unsaved changes. Save to apply your updates."
            />
          </div>
        )}
        <h4 className="text-lg font-semibold primary-color-dark mb-4">
          Main Header Information
        </h4>
        <form
          className="flex flex-col gap-4"
          onSubmit={handleAccountSubmit(onSubmitAccount)}
        >
          <div className={"flex flex-col gap-4"}>
            <div
              className={`flex flex-col ${isEditingAccount ? "gap-4" : "gap-0.5"}`}
            >
              <InputView
                id="email"
                label="Email"
                isInput={isEditingAccount}
                valueToDisplay={watchAccount.email}
                registration={registerAccount("email", {
                  required: "This field is required.",
                  validate: (value) =>
                    value?.toString().includes("@") || "Invalid email address",
                })}
                error={errors.email?.message}
              />
              <InputView
                id="displayName"
                label="Display Name"
                isInput={isEditingAccount}
                valueToDisplay={watchAccount.displayName}
                registration={registerAccount("displayName", {
                  required: "This field is required.",
                })}
                error={errors.displayName?.message}
              />
              <InputView
                id="location"
                label="Location"
                isInput={isEditingAccount}
                valueToDisplay={watchAccount.location}
                registration={registerAccount("location", {
                  required: "This field is required.",
                })}
                error={errors.location?.message}
              />
              <ControllerView
                name="dateFormat"
                label="Date Format"
                options={dateFormatOptions}
                control={accountControl}
                isInput={isEditingAccount}
                isRequired={true}
              />
              <InputView
                id="primaryTitle"
                label="Primary Title"
                isInput={isEditingAccount}
                valueToDisplay={watchAccount.primaryTitle?.value}
                registration={registerAccount("primaryTitle.value", {
                  required: "This field is required.",
                })}
                error={errors.primaryTitle?.value?.message}
              />
              <TextAreaView
                id="summary"
                label="Summary"
                isInput={isEditingAccount}
                valueToDisplay={watchAccount.summary}
                registration={registerAccount("summary")}
                error={errors.summary?.message}
              />
            </div>
            <Button
              styleType="plain"
              label={isEditingAccount ? "View Header Info" : "Edit Header Info"}
              onClick={() => setIsEditingAccount(!isEditingAccount)}
              grow
            />
            {/* Additional Titles Section */}
            <CollapsibleSection
              title="Additional Titles"
              optionalMessage={`(${titleFields.length} out of 4)`}
              openByDefault={true}
              actions={[
                ...(titleFields.length <= 3
                  ? [
                      {
                        label: "Add",
                        iconComponent: PlusCircle,
                        onClick: () => {
                          if (!isEditingTitles) {
                            setIsEditingTitles(true);
                          }
                          appendTitle({ value: "" });
                        },
                        expandOnClick: true,
                      },
                    ]
                  : []),
                {
                  label: isEditingTitles ? "View" : "Edit",
                  iconComponent: isEditingTitles ? Eyeglasses : Pen,
                  onClick: () => setIsEditingTitles(!isEditingTitles),
                  expandOnClick: true,
                  hideLabelOnMobile: true,
                },
              ]}
            >
              <div className="flex flex-col gap-2">
                {titleFields.map((title, index) => (
                  <div key={title.id} className="flex gap-2">
                    <InputView
                      id={`additionalTitles.${index}.value`}
                      label={`Title ${index + 1}`}
                      isInput={isEditingTitles}
                      valueToDisplay={
                        watchAccount.additionalTitles?.[index]?.value
                      }
                      registration={registerAccount(
                        `additionalTitles.${index}.value`,
                        {
                          required: "This field is required.",
                        },
                      )}
                      error={errors.additionalTitles?.[index]?.value?.message}
                    />
                    {isEditingTitles && (
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
              </div>
            </CollapsibleSection>
            {/* Personal Websites Section */}
            <CollapsibleSection
              title="Links"
              optionalMessage={`(${personalLinkFields.length} out of 4)`}
              openByDefault={true}
              actions={[
                ...(personalLinkFields.length <= 3
                  ? [
                      {
                        label: "Add",
                        iconComponent: PlusCircle,
                        onClick: () => {
                          if (!isEditingPersonalLinks) {
                            setIsEditingPersonalLinks(true);
                          }
                          appendPersonalLink({ displayName: "", url: "" });
                        },
                        expandOnClick: true,
                      },
                    ]
                  : []),
                {
                  label: isEditingPersonalLinks ? "View" : "Edit",
                  iconComponent: isEditingPersonalLinks ? Eyeglasses : Pen,
                  onClick: () =>
                    setIsEditingPersonalLinks(!isEditingPersonalLinks),
                  expandOnClick: true,
                  hideLabelOnMobile: true,
                },
              ]}
            >
              <div className="flex flex-col gap-2 mt-2">
                {personalLinkFields.map((link, index) => (
                  <div key={link.id} className="flex flex-col gap-2">
                    <div className="flex flex-col gap-2 primary-border p-4 rounded-lg">
                      <InputView
                        id={`personalLinks.${index}.displayName`}
                        label="Name"
                        isInput={isEditingPersonalLinks}
                        valueToDisplay={
                          watchAccount.personalLinks?.[index]?.displayName
                        }
                        registration={registerAccount(
                          `personalLinks.${index}.displayName`,
                          {
                            required: "Website cannot be blank.",
                          },
                        )}
                        error={
                          errors.personalLinks?.[index]?.displayName?.message
                        }
                      />
                      <InputView
                        id={`personalLinks.${index}.url`}
                        label="URL"
                        isInput={isEditingPersonalLinks}
                        valueToDisplay={
                          watchAccount.personalLinks?.[index]?.url
                        }
                        registration={registerAccount(
                          `personalLinks.${index}.url`,
                          {
                            required: "Website URL cannot be blank.",
                          },
                        )}
                        error={errors.personalLinks?.[index]?.url?.message}
                      />
                      {isEditingPersonalLinks && (
                        <div className="flex justify-end">
                          <Button
                            label="Remove"
                            iconComponent={Trash3}
                            styleType="edit"
                            onClick={() => handleRemovePersonalLink(index)}
                            labelSlideOut={true}
                          />
                        </div>
                      )}
                    </div>
                  </div>
                ))}
              </div>
            </CollapsibleSection>
          </div>
          {isDirty && (
            <Button
              type="submit"
              label={
                updateAccountMutation.isPending ? "Saving..." : "Save Changes"
              }
              grow
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
