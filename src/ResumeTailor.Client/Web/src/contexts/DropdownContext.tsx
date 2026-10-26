import { createContext, useContext } from "react";

export type DropdownContextValue = {
  openId: string | null;
  toggle: (id: string) => void;
};

export const DropdownContext = createContext<DropdownContextValue | null>(null);

export const useDropdownContext = () => useContext(DropdownContext);
