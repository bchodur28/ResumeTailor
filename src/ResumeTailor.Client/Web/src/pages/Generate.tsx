import { useState } from "react";
import Card from "../components/ui/Card";
import SkillIcon from "../components/ui/SkillIcon";
import Resume from "../components/resume/Resume";
import ListItem from "../components/ui/ListItem";
import RatingBar from "../components/ui/RatingBar";
import {
  PinMapFill,
  CashCoin,
  HouseDoor,
  Building,
  Geo,
  ArrowDownSquare,
  ArrowUpSquare,
} from "react-bootstrap-icons";
import Toolbar from "../components/resume/Toolbar";
import { generateResumeDetails } from "../api/resumeApi";
import { mockResumeGenerationResult } from "../data/mockResume";

const Generate = () => {
  const [showResumeView, setShowResumeView] = useState(false);
  //const mockResumeGenerationResult = generateResumeDetails();
  const mockResumeGenerationResult = generateResume();
  return (
    <div className="flex justify-center">
      <div>
        <h1 className="text-2xl font-semibold mb-4">Generate your Resume</h1>
        <Card className="w-5xl flex flex-col">
          <textarea className="border border-gray-300 rounded p-2 w-full h-[80vh]"></textarea>
          <button
            className="btn self-end mt-2"
            onClick={() => setShowResumeView(!showResumeView)}
          >
            Generate Resume
          </button>
        </Card>
      </div>
    </div>
  );
};

export default Generate;
function generateResume() {
  return mockResumeGenerationResult;
}
