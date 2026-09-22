import type { AccountRequest } from "../../models/profile/AccountRequest";
import { updateAccountAsync } from "../../api/accounts";
import type { AccountEditForm } from "../../models/forms/AccountEditForm";
import { useEffect, useState } from "react";
import Input from "../forms/Input";
import { useAccount } from "../../contexts/AccountContext";
import { stateOptions } from "../../data/states";
import { countryOptions } from "../../data/countries";
import SearchableSelect from "../forms/SearchableSelect";
import { useAuth0 } from "@auth0/auth0-react";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useForm, useFieldArray, Controller } from "react-hook-form";
import Message from "../ui/Message";
import EditAccountForm from "./EditAccountForm";

type PopupModelProps = {
  title: string;
  children: React.ReactNode;
  onClose: () => void;
};

const PopupModel = ({ title, children, onClose }: PopupModelProps) => {
  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/40">
      <div className="max-h-[90vh] w-full max-w-2xl overflow-y-auto rounded-xl bg-white shadow-xl">
        <div className="flex items-center justify-between border-b border-gray-200 px-6 py-4">
          <h2 className="text-lg font-semibold text-gray-900">{title}</h2>
          <button
            type="button"
            onClick={onClose}
            className="text-gray-500 hover:text-gray-700"
          >
            x
          </button>
        </div>
        <div className="space-y-8 p-6">
          <section className="flex flex-col gap-2">{children}</section>
        </div>
        <div className="flex justify-end gap-3 border-t border-gray-200 px-6 py-4">
          <button type="button" className="btn-secondary" onClick={onClose}>
            Cancel
          </button>
        </div>
      </div>
    </div>
  );
};

export default PopupModel;
