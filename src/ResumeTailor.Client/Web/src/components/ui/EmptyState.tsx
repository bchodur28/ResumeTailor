import type { PropsWithChildren } from "react";

type EmptyStateProps = PropsWithChildren<{
  main: string;
  secondary: string;
}>;

const EmptyState = ({ main, secondary, children }: EmptyStateProps) => (
  <div className="flex justify-center">
    <section className="flex flex-col items-center empty-state w-lg box-shadow p-4 rounded-2xl bg-white color-border">
      <h2 className="font-semibold text-xl">{main}</h2>
      <p className="text-md text-gray-700">{secondary}</p>
      <div className="mt-4">{children}</div>
    </section>
  </div>
);

export default EmptyState;
