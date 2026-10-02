import React from "react";

interface MessageProps {
  type: "success" | "error" | "warning";
  message: string;
  onClose: () => void;
}

const Message: React.FC<MessageProps> = ({ type, message, onClose }) => {
  const getColorClasses = () => {
    switch (type) {
      case "success":
        return "bg-green-100 border-green-200 text-green-600";
      case "error":
        return "bg-red-100 border-red-200 text-red-600";
      case "warning":
        return "bg-yellow-100 border-yellow-200 text-yellow-700";
      default:
        return "";
    }
  };
  const getButtonColorClasses = () => {
    switch (type) {
      case "success":
        return "text-green-600 hover:text-green-800";
      case "error":
        return "text-red-600 hover:text-red-800";
      case "warning":
        return "text-yellow-700 hover:text-yellow-800";
      default:
        return "";
    }
  };
  const getStarterMessage = () => {
    switch (type) {
      case "success":
        return "Success: ";
      case "error":
        return "Error: ";
      case "warning":
        return "Warning: ";
      default:
        return "";
    }
  };
  return (
    <div>
      <div
        className={`p-2 rounded border shadow-md flex justify-between ${getColorClasses()}`}
      >
        <p>
          <b>{getStarterMessage()}</b> {message}
        </p>
        <button
          className={`font-bold ${getButtonColorClasses()}`}
          onClick={onClose}
        >
          x
        </button>
      </div>
    </div>
  );
};

export default Message;
