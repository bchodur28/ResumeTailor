import React from "react";

interface MessageProps {
  type: "success" | "error";
  message: string;
  onClose: () => void;
}

const Message: React.FC<MessageProps> = ({ type, message, onClose }) => {
  return (
    <div>
      <div
        className={`p-2 rounded border shadow-md flex justify-between ${
          type === "success"
            ? "bg-green-100 border-green-200 text-green-600"
            : "bg-red-100 border-red-200 text-red-600"
        }`}
      >
        <p>{message}</p>
        <button
          className={`font-bold ${type === "success" ? "text-green-600" : "text-red-600"} hover:${type === "success" ? "text-green-800" : "text-red-800"}`}
          onClick={onClose}
        >
          x
        </button>
      </div>
    </div>
  );
};

export default Message;
