import { useAccount } from "../contexts/AccountContext";
import { Navigate, useNavigate } from "react-router-dom";
import {
  useForm,
  useFieldArray,
  Controller,
  type FieldErrors,
} from "react-hook-form";
import type { AccountSetupForm } from "../models/forms/AccountSetupForm";
import type { AccountRequest } from "../models/api/AccountRequest";
import Card from "../components/ui/Card";
import Input from "../components/forms/Input";
import SearchableSelect from "../components/forms/SearchableSelect";
import { stateOptions } from "../data/states";
import { countryOptions } from "../data/countries";
import { PlusLg, Trash3 } from "react-bootstrap-icons";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useAuth0 } from "@auth0/auth0-react";
import { createAccount } from "../api/accounts";
import { useState } from "react";

const AccountSetup = () => {
  const { getAccessTokenSilently } = useAuth0();
  const queryClient = useQueryClient();

  const { account, isLoadingAccount, refreshAccount } = useAccount();
  const navigate = useNavigate();

  const [isEmailFocused, setIsEmailFocused] = useState(false);

  const {
    control,
    register,
    handleSubmit,
    formState: { errors, isValid, touchedFields },
  } = useForm<AccountSetupForm>({
    mode: "onChange",
    defaultValues: {
      email: "",
      displayName: "",
      city: "",
      state: undefined,
      country: undefined,
      primaryWebsiteDisplay: "",
      primaryWebsiteUrl: "",
      primaryTitle: "",
      additionalTitles: [],
      additionalWebsites: [],
    },
  });

  const {
    fields: websiteFields,
    append: appendWebsite,
    remove: removeWebsite,
  } = useFieldArray({
    control,
    name: "additionalWebsites",
  });

  const {
    fields: titleFields,
    append: appendTitle,
    remove: removeTitle,
  } = useFieldArray({
    control,
    name: "additionalTitles",
  });

  const onInvalid = (errors: FieldErrors<AccountSetupForm>) => {
    console.log("Form invalid:", errors);
  };

  const createAccountMutation = useMutation({
    mutationFn: async (request: AccountRequest) => {
      console.log("Mutation started");
      const token = await getAccessTokenSilently({
        authorizationParams: { audience: import.meta.env.VITE_AUTH0_AUDIENCE },
      });
      console.log("Got token");
      const payload = JSON.parse(atob(token.split(".")[1]));

      console.log("JWT Payload:", payload);
      return createAccount(request, token);
    },

    onError: (error) => {
      console.error("Create account mutation failed:", error);
    },

    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: ["account"] });

      await refreshAccount();

      navigate("/");
    },
  });

  const onSubmit = (data: AccountSetupForm) => {
    console.log("Form submitted:", data);
    const request: AccountRequest = {
      email: data.email,
      displayName: data.displayName,
      city: data.city,
      state: data.state!.label,
      country: data.country!.label,
      personalLinks: [
        {
          displayName: data.primaryWebsiteDisplay,
          url: data.primaryWebsiteUrl,
        },
        ...data.additionalWebsites.map((website) => ({
          displayName: website.display,
          url: website.url,
        })),
      ],
      titles: [
        {
          value: data.primaryTitle,
          isPrimary: true,
        },
        ...data.additionalTitles.map((title) => ({
          value: title.value,
          isPrimary: false,
        })),
      ],
    };
    createAccountMutation.mutate(request);
  };

  const equivalentTitleExamples = [
    "Software Developer",
    "Full Stack Developer",
    "Full Stack Engineer",
    "Application Developer",
  ];

  if (isLoadingAccount) {
    return <div>Loading...</div>;
  }

  if (account) {
    return <Navigate to="/" replace />;
  }

  const emailError =
    touchedFields.email && errors.email?.message === "This field is required."
      ? errors.email.message
      : touchedFields.email && !isEmailFocused
        ? errors.email?.message
        : undefined;

  return (
    <div>
      <div className="min-h-screen flex items-center justify-center">
        <div className="flex flex-col items-center w-full max-w-lg gap-4">
          <h2 className="text-2xl font-bold">Account Setup</h2>
          <p className="text-md">
            The information you fill out here will be the information used on
            your resumes. You can change this information later.
          </p>
          <Card className="w-full">
            <form
              onSubmit={handleSubmit(onSubmit, onInvalid)}
              className="flex flex-col gap-4"
            >
              <Input
                id="email"
                label="Email*"
                registration={register("email", {
                  required: "This field is required.",
                  validate: (value) =>
                    value.includes("@") || "Invalid email address",
                })}
                error={emailError}
                onFocus={() => setIsEmailFocused(true)}
                onBlur={() => setIsEmailFocused(false)}
              />
              <Input
                id="displayName"
                label="Display Name*"
                placeholder="The name that will go on your resumes."
                registration={register("displayName", {
                  required: "This field is required.",
                })}
                error={
                  touchedFields.displayName
                    ? errors.displayName?.message
                    : undefined
                }
              />
              <Controller
                name="country"
                control={control}
                rules={{ required: "This field is required." }}
                render={({ field, fieldState }) => (
                  <SearchableSelect
                    id="country"
                    label="Country*"
                    options={countryOptions}
                    value={field.value}
                    onChange={field.onChange}
                    onBlur={field.onBlur}
                    isRequired={true}
                    error={fieldState.error?.message}
                  />
                )}
              />
              <Controller
                name="state"
                control={control}
                rules={{ required: "This field is required." }}
                render={({ field, fieldState }) => (
                  <SearchableSelect
                    id="state"
                    label="State*"
                    options={stateOptions}
                    value={field.value}
                    onChange={field.onChange}
                    onBlur={field.onBlur}
                    isRequired={true}
                    error={fieldState.error?.message}
                  />
                )}
              />
              <Input
                id="city"
                label="City*"
                registration={register("city", {
                  required: "This field is required.",
                })}
                error={touchedFields.city ? errors.city?.message : undefined}
              />
              <Input
                id="website-display"
                label="Website 1*"
                placeholder="Ex: LinkedIn"
                registration={register("primaryWebsiteDisplay", {
                  required: "This field is required.",
                })}
                error={
                  touchedFields.primaryWebsiteDisplay
                    ? errors.primaryWebsiteDisplay?.message
                    : undefined
                }
              />

              <div className="flex flex-col gap-2">
                <Input
                  id="website-url"
                  label="Website Url 1*"
                  registration={register("primaryWebsiteUrl", {
                    required: "This field is required.",
                    validate: (value) => {
                      try {
                        new URL(value);
                        return true;
                      } catch {
                        return "Invalid URL.";
                      }
                    },
                  })}
                  error={
                    touchedFields.primaryWebsiteUrl
                      ? errors.primaryWebsiteUrl?.message
                      : undefined
                  }
                />
                {websiteFields.map((website, index) => (
                  <div className="flex flex-col gap-4" key={website.id}>
                    <Input
                      id={`additional-website-display-${index}`}
                      label={`Website ${index + 2}*`}
                      placeholder="Ex: LinkedIn"
                      registration={register(
                        `additionalWebsites.${index}.display`,
                        {
                          required: "Website name cannot be blank.",
                        },
                      )}
                      error={
                        errors.additionalWebsites?.[index]?.display?.message
                      }
                    />
                    <Input
                      id={`additional-website-url-${index}`}
                      label={`Website Url ${index + 2}*`}
                      registration={register(
                        `additionalWebsites.${index}.url`,
                        {
                          required: "Website URL cannot be blank.",
                          validate: (value) => {
                            try {
                              new URL(value);
                              return true;
                            } catch {
                              return "Invalid URL.";
                            }
                          },
                        },
                      )}
                      error={errors.additionalWebsites?.[index]?.url?.message}
                    />
                  </div>
                ))}
                <div className="flex justify-end items-center gap-2 ">
                  <p className="text-sm font-semibold text-gray-500">
                    Up to 10 websites
                  </p>
                  {websiteFields.length !== 0 && (
                    <button
                      type="button"
                      className="btn-secondary"
                      onClick={() => removeWebsite(websiteFields.length - 1)}
                    >
                      <Trash3 />
                    </button>
                  )}
                  {websiteFields.length < 9 && (
                    <button
                      type="button"
                      className="btn-secondary"
                      onClick={() => appendWebsite({ display: "", url: "" })}
                    >
                      <PlusLg />
                    </button>
                  )}
                </div>
              </div>

              <div className="flex flex-col gap-2">
                <Input
                  id="title"
                  label="Professional Title 1*"
                  placeholder="Ex: Software Engineer (Primary Title)"
                  registration={register("primaryTitle", {
                    required: "This field is required.",
                  })}
                  error={
                    touchedFields.primaryTitle
                      ? errors.primaryTitle?.message
                      : undefined
                  }
                />
                {titleFields.map((title, index) => (
                  <Input
                    key={title.id}
                    id={`additional-title-${index}`}
                    label={`Professional Title ${index + 2}*`}
                    placeholder={`Ex: ${equivalentTitleExamples[index]}`}
                    registration={register(`additionalTitles.${index}.value`, {
                      required: "Secondary title cannot be blank.",
                    })}
                    error={errors.additionalTitles?.[index]?.value?.message}
                  />
                ))}
                <div className="flex justify-end items-center gap-2">
                  <p className="text-sm font-semibold text-gray-500">
                    Up to 5 titles
                  </p>
                  {titleFields.length !== 0 && (
                    <button
                      type="button"
                      className="btn-secondary"
                      onClick={() => removeTitle(titleFields.length - 1)}
                    >
                      <Trash3 />
                    </button>
                  )}
                  {titleFields.length < 4 && (
                    <button
                      type="button"
                      className="btn-secondary"
                      onClick={() => appendTitle({ value: "" })}
                    >
                      <PlusLg />
                    </button>
                  )}
                </div>
              </div>

              <button
                type="submit"
                className="btn"
                disabled={!isValid || createAccountMutation.isPending}
              >
                {createAccountMutation.isPending
                  ? "Creating..."
                  : "Create Account"}
              </button>
            </form>
          </Card>
        </div>
      </div>
    </div>
  );
};

export default AccountSetup;
