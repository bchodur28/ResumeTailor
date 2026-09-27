import type { PropsWithChildren, ReactNode } from "react";
import { createContext, useContext } from "react";

type ListItemContext = {
  item: Item;
};

const ListItemContext = createContext<ListItemContext | undefined>(undefined);

function useListItemContext() {
  const context = useContext(ListItemContext);
  if (!context) {
    throw new Error("useListItemContext must be used within a ListItem");
  }
  return context;
}

type Item = {
  content: string[];
  icon?: ReactNode;
  actions?: { actionName: string; actionFn: () => void }[];
  borderColor?: string;
};

type ListItemProps = PropsWithChildren & {
  item: Item;
};

const ListItem = ({ children, item }: ListItemProps) => {
  return (
    <ListItemContext.Provider value={{ item }}>
      <li
        className={`flex items-center gap-2 border p-2 rounded-xl mt-2 ${item.borderColor ?? "border-gray-300"} shadow bg-white`}
      >
        {children}
      </li>
    </ListItemContext.Provider>
  );
};

ListItem.Icon = function ListItemIcon() {
  const { item } = useListItemContext();
  return <span>{item.icon}</span>;
};

ListItem.Content = function ListItemContent() {
  const { item } = useListItemContext();
  return (
    <div className="flex flex-1 gap-4">
      {item.content.map((line, index) => (
        <p key={index}>{line}</p>
      ))}
    </div>
  );
};

ListItem.Buttons = function ListItemButtons() {
  const { item } = useListItemContext();
  return (
    <div className="ml-auto flex gap-2">
      {item.actions &&
        item.actions.map((action, index) => (
          <button
            key={index}
            className="btn-secondary"
            onClick={action.actionFn}
          >
            {action.actionName}
          </button>
        ))}
    </div>
  );
};

export default ListItem;
