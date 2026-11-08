import { CheckCircle, ExclamationCircle } from "react-bootstrap-icons";

type SlideoutMessageProps = {
  messsageType: "positive" | "negative" | "neutral";
  message: string;
};

const SlideoutMessage = ({ messsageType, message }: SlideoutMessageProps) => {
  return (
    <div className="slide-out-icon">
      {messsageType === "neutral" ? (
        <>
          <ExclamationCircle size={20} color="var(--color-yellow-600)" />
          <span className="text-yellow-600 text-sm font-semibold">
            {message}
          </span>
        </>
      ) : messsageType === "positive" ? (
        <>
          <CheckCircle size={20} color="var(--color-green-600)" />
          <span className="text-green-600 text-sm font-semibold">
            {message}
          </span>
        </>
      ) : (
        <>
          <ExclamationCircle size={20} color="var(--color-red-600)" />
          <span className="text-red-600 text-sm font-semibold">{message}</span>
        </>
      )}
    </div>
  );
};

export default SlideoutMessage;
