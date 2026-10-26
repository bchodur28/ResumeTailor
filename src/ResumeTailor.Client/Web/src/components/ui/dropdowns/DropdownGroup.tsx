import { useState, type ReactNode } from "react";
import ButtonContainer from "../buttons/ButtonContainer";
import DropdownContainer from "./DropdownContainer";

type DropdownItem = {
  id: string;
  button: (toggle: () => void, isOpen: boolean) => ReactNode;
  dropdown: ReactNode;
};

type DropdownGroupProps = {
  items: DropdownItem[];
};

const DropdownGroup = ({ items }: DropdownGroupProps) => {
  const [openId, setOpenId] = useState<string | null>(null);

  const toggleDropdown = (id: string) => {
    setOpenId((current) => (current === id ? null : id));
  };

  const activeItem = items.find((item) => item.id === openId);

  return (
    <div className="relative w-full">
      <ButtonContainer>
        {items.map((item) => (
          <div key={item.id}>
            {item.button(() => toggleDropdown(item.id), openId === item.id)}
          </div>
        ))}
      </ButtonContainer>

      {activeItem && (
        <DropdownContainer>{activeItem.dropdown}</DropdownContainer>
      )}
    </div>
  );
};

export default DropdownGroup;
