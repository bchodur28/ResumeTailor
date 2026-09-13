import { useState } from "react";
import { generateResume } from "../api/resumeGeneration";
import Card from "../components/ui/Card";
import SkillIcon from "../components/ui/SkillIcon";
import Resume from "../components/resume/Resume";
import ListItem from "../components/ui/ListItem";
import RatingBar from "../components/ui/RatingBar";
import {
  CheckCircleFill,
  Map,
  PinMap,
  PinMapFill,
  CashCoin,
  HouseDoor,
  Building,
  Geo,
  ArrowDownSquare,
  ArrowUpSquare,
} from "react-bootstrap-icons";
import Toolbar from "../components/resume/Toolbar";

const Generate = () => {
  const [showResumeView, setShowResumeView] = useState(true);
  const mockResumeGenerationResult = generateResume();
  const getWorkStyleIcon = (style: string) => {
    if (style.toLowerCase() === "remote") {
      return <HouseDoor className="primary-color" />;
    }
    if (style.toLowerCase() === "onsite") {
      return <Building className="primary-color" />;
    }
    if (style.toLowerCase() === "hybrid") {
      return <Geo className="primary-color" />;
    }
    return null;
  };
  return (
    <div>
      <div className="flex items-start gap-6">
        <Card className="w-full flex flex-col items-center">
          {/* <h3 className="primary-color text-xl">{`${showResumeView ? "Resume View" : "Paste Job Description"}`}</h3> */}
          {showResumeView && (
            <div className="w-3/4 mb-4">
              <RatingBar rating={mockResumeGenerationResult.aiSummary.rating} />
              <Toolbar />
            </div>
          )}
          {!showResumeView && (
            <textarea className="border border-gray-300 rounded p-2 w-full h-[80vh]"></textarea>
          )}
          {showResumeView && (
            <div className="w-full overflow-auto flex justify-center p-2">
              <div className="resume-preview border-gray-300 rounded p-2">
                <Resume resume={mockResumeGenerationResult.aiResume} />
              </div>
            </div>
          )}
          <button
            className="btn self-end mt-2"
            onClick={() => setShowResumeView(!showResumeView)}
          >
            Generate Resume
          </button>
        </Card>
        <div className="w-full flex flex-col gap-y-6">
          <Card className="w-full">
            <h3 className="primary-color text-xl">AI Summary</h3>
            <p>{mockResumeGenerationResult.aiSummary.summary}</p>
            <ul className="flex justify-start mt-2 gap-3">
              <ListItem
                value={`${mockResumeGenerationResult.aiSummary.location}`}
                leftContent={<PinMapFill className="primary-color" />}
              />
              <ListItem
                value={`${mockResumeGenerationResult.aiSummary.salary}`}
                leftContent={<CashCoin className="primary-color" />}
              />
              <ListItem
                value={`${mockResumeGenerationResult.aiSummary.workStyle}`}
                leftContent={getWorkStyleIcon(
                  mockResumeGenerationResult.aiSummary.workStyle ?? "",
                )}
              />
            </ul>
          </Card>
          <Card className="w-full">
            <h3 className="primary-color text-xl">
              Alternative Strong Bullets
            </h3>
            <ul>
              {mockResumeGenerationResult.aiSummary.alternativeBullet.map(
                (bullet, index) => (
                  <ListItem key={index} value={bullet} />
                ),
              )}
            </ul>
          </Card>
          <Card className="w-full">
            <h3 className="primary-color text-xl">Skills</h3>
            <div className="mt-2 flex flex-wrap gap-3">
              <SkillIcon name="C" />
              <SkillIcon name="CSharp" />
              <SkillIcon name="JavaScript" />
              <SkillIcon name="React" />
              <SkillIcon name="Angular" />
              <SkillIcon name="Net" />
              <SkillIcon name="SqlServer" />
            </div>
          </Card>

          <Card className="w-full">
            <h3 className="primary-color text-xl">Strengths/Weaknesses</h3>
            <div className="mt-2">
              <h4 className="font-semibold">Strengths</h4>
              <ul>
                {mockResumeGenerationResult.aiSummary.strengths.map(
                  (strength, index) => (
                    <ListItem
                      key={index}
                      value={strength}
                      borderColor="border-green-500"
                      leftContent={<ArrowUpSquare className="primary-color" />}
                    />
                  ),
                )}
              </ul>
              <h4 className="font-semibold">Weaknesses</h4>
              <ul>
                {mockResumeGenerationResult.aiSummary.weaknesses.map(
                  (weakness, index) => (
                    <ListItem
                      key={index}
                      value={weakness}
                      borderColor="border-red-500"
                      leftContent={
                        <ArrowDownSquare className="primary-color" />
                      }
                    />
                  ),
                )}
              </ul>
            </div>
          </Card>
        </div>
      </div>
    </div>
  );
};

export default Generate;
