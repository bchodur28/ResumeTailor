type InnerSectionProps = {
  topPtSpacing: number;
  children: React.ReactNode;
};

const InnerSection = ({ topPtSpacing, children }: InnerSectionProps) => {
  return (
    <div style={{ paddingTop: `${topPtSpacing}pt` }}>
      <div>{children}</div>
    </div>
  );
};

export default InnerSection;
