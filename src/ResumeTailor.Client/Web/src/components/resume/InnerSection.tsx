type InnerSectionProps = {
  topPtSpacing: number;
  children: React.ReactNode;
  additionalClassName?: string;
};

const InnerSection = ({
  topPtSpacing,
  children,
  additionalClassName,
}: InnerSectionProps) => {
  return (
    <div
      className={additionalClassName ?? ""}
      style={{ paddingTop: `${topPtSpacing}pt` }}
    >
      {children}
    </div>
  );
};

export default InnerSection;
