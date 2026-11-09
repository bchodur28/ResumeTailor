import React from "react";
import Button from "../../../components/ui/buttons/Button";
import { ChevronDown, ChevronUp, type Icon } from "react-bootstrap-icons";

type CollapsibleSectionProps = {
  title: string;
  optionalMessage?: string;
  openByDefault?: boolean;
  actions?: {
    label: string;
    iconComponent?: Icon;
    onClick: () => void;
    expandOnClick?: boolean;
    hideLabelOnMobile?: boolean;
  }[];
  children: React.ReactNode;
};

const CollapsibleSection = ({
  title,
  optionalMessage,
  actions,
  children,
  openByDefault,
}: CollapsibleSectionProps) => {
  const [isCollapsed, setIsCollapsed] = React.useState(!openByDefault);

  return (
    <div className="flex flex-col primary-border rounded-lg">
      <div
        onClick={() => setIsCollapsed((prev) => !prev)}
        className="flex justify-between items-center px-4 py-2 rounded-lg cursor-pointer hover:bg-gray-100 transition-colors duration-300"
      >
        <h2 className="text-lg font-semibold primary-color-dark">
          {title}
          <span className="text-sm text-gray-600 ml-1">{optionalMessage}</span>
        </h2>
        <div className="flex gap-2">
          {actions?.map((action, index) => (
            <Button
              key={index}
              label={action.label}
              onClick={() => {
                action.onClick();
                if (action.expandOnClick) {
                  setIsCollapsed(false);
                }
              }}
              iconComponent={action.iconComponent}
              styleType="edit"
              labelSlideOut={true}
              hideLabelOnMobile={action.hideLabelOnMobile}
            />
          ))}

          <Button
            label={isCollapsed ? "Show" : "Hide"}
            onClick={() => setIsCollapsed(!isCollapsed)}
            iconComponent={isCollapsed ? ChevronDown : ChevronUp}
            styleType="edit"
            labelSlideOut={true}
            hideLabelOnMobile={true}
          />
        </div>
      </div>
      {!isCollapsed && (
        <div className="px-4 pb-4">
          {/* Content to show when expanded */}
          {children}
        </div>
      )}
    </div>
  );
};

export default CollapsibleSection;
