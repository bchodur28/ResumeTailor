import { QuestionCircle } from "react-bootstrap-icons";

type RatingBarProps = {
  rating: number;
};

const RatingBar = ({ rating }: RatingBarProps) => {
  const value = Math.min(100, Math.max(0, rating));
  return (
    <div className="w-full">
      <div className="mb-1 flex justify-between">
        <span className="text-sm">
          Match Score
          <QuestionCircle className="inline-block ml-1" />
        </span>
        <span className="text-sm font-semibold">{value}/100</span>
      </div>

      <div className="relative h-2 w-full rounded-full bg-gradient-to-r from-red-500 via-yellow-500 to-green-500">
        <div
          className="absolute top-1/2 h-5 w-2 -translate-x-1/2 -translate-y-1/2 rounded-full border-2 border-white bg-gray-800 shadow"
          style={{ left: `${value}%` }}
        ></div>
      </div>
    </div>
  );
};

export default RatingBar;
