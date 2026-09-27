import type { PropsWithChildren, ReactNode } from "react";
import { createContext, useContext } from "react";

type EmptyStateContextValue = {
  main: ReactNode;
  secondary: ReactNode;
};

const EmptyStateContext = createContext<EmptyStateContextValue | undefined>(
  undefined,
);

function useEmptyStateContext() {
  const context = useContext(EmptyStateContext);
  if (!context) {
    throw new Error("EmptyState subcomponents must be used within EmptyState");
  }
  return context;
}

type EmptyStateProps = PropsWithChildren<EmptyStateContextValue> & {
  className?: string;
};

const EmptyState = ({
  main,
  secondary,
  children,
  className,
}: EmptyStateProps) => (
  <EmptyStateContext.Provider value={{ main, secondary }}>
    <section className={`empty-state ${className ?? ""}`}>
      {children}
    </section>
  </EmptyStateContext.Provider>
);

EmptyState.Main = function EmptyStateMain() {
  const { main } = useEmptyStateContext();
  return <h2 className="empty-state__main">{main}</h2>;
};

EmptyState.Secondary = function EmptyStateSecondary() {
  const { secondary } = useEmptyStateContext();
  return <p className="empty-state__secondary">{secondary}</p>;
};

EmptyState.Actions = function EmptyStateActions({
  children,
}: PropsWithChildren) {
  return <div className="empty-state__actions">{children}</div>;
};

export default EmptyState;
