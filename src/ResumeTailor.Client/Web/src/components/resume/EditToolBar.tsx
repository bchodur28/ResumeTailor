import styles from "./Resume.module.css";

export type EditToolBarProps = {
  children: React.ReactNode;
  isRelative?: boolean;
};

const EditToolBar = ({ children, isRelative }: EditToolBarProps) => {
  return (
    <div
      className={
        isRelative
          ? styles.editToolBarContainerRelative
          : styles.editToolBarContainerAbsolute
      }
    >
      <div className={styles.editToolBar}>{children}</div>
    </div>
  );
};

export default EditToolBar;
