import styles from "./Resume.module.css";

type TwoColumnProps = {
  left: string;
  right: string;
  isBold?: boolean;
};

const TwoColumn = ({ left, right, isBold }: TwoColumnProps) => {
  return (
    <div className={`${styles.resumeTwoColumn} ${isBold ? "font-bold" : ""}`}>
      <p>{left}</p>
      <p>{right}</p>
    </div>
  );
};

export default TwoColumn;
