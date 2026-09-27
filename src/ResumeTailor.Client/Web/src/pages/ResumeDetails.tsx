import { useState } from "react";
import ListItem from "../components/ui/ListItem";
import { useParams } from "react-router-dom";
import { useResumeDetails } from "../hooks/useResumeDetails";
import Card from "../components/ui/Card";
import Resume from "../components/resume/Resume";

const ResumeDetails = () => {
  const { id } = useParams();
  const resumeId = Number(id);

  const { data: resumeDetails, isLoading, error } = useResumeDetails(resumeId);

  const [showResumeView, setShowResumeView] = useState(true);

  if (isLoading) {
    return <div>Loading resume details...</div>;
  }

  if (error) {
    return <div>Error loading resume details: {error.message}</div>;
  }

  if (resumeDetails?.resume == null) {
    return <h1>Resume not found</h1>;
  }

  return (
    <div className="flex flex-col gap-6 items-center">
      <div className="flex gap-4">
        <button
          className="btn-secondary"
          onClick={() => setShowResumeView(false)}
        >
          Edit
        </button>
        <button
          className="btn-secondary"
          onClick={() => setShowResumeView(true)}
        >
          View
        </button>
      </div>
      <div className="flex items-start gap-6 ">
        {/*Actual Resume */}
        <div className="resume-preview">
          <Resume resume={resumeDetails.resume} />
        </div>
        {/*Resume Summary */}
        {showResumeView && (
          <div className="w-full flex flex-col items-center gap-6">
            {/* AI Summary Section */}
            <Card className="w-full">
              <h3 className="primary-color text-xl">AI Summary</h3>
              <p>{resumeDetails?.aiAnalysis.summary}</p>
            </Card>
            {/* Alternative Bullets Section */}
            <Card className="w-full">
              <h3 className="primary-color text-xl">Alternative Bullets</h3>
              <ul>
                {resumeDetails?.resume.companies.map((company) =>
                  company.bullets
                    .map((bullet) => bullet.alternativeValue)
                    .filter((value) => value !== null)
                    .map((value, index) => (
                      <ListItem key={index} item={{ content: [value] }}>
                        <ListItem.Content />
                      </ListItem>
                    )),
                )}
              </ul>
            </Card>
            {/*Weaknesses and Strengths Sections*/}
            <Card className="w-full flex flex-col gap-6">
              {/*Strengths*/}
              <div>
                <h3 className="primary-color text-xl">Strengths</h3>
                <ul>
                  {resumeDetails?.aiAnalysis.strengths.map(
                    (strength, index) => (
                      <ListItem key={index} item={{ content: [strength] }}>
                        <ListItem.Content />
                      </ListItem>
                    ),
                  )}
                </ul>
              </div>
              {/*Weaknesses*/}
              <div>
                <h3 className="primary-color text-xl">Weaknesses</h3>
                <ul>
                  {resumeDetails?.aiAnalysis.weaknesses.map(
                    (weakness, index) => (
                      <ListItem key={index} item={{ content: [weakness] }}>
                        <ListItem.Content />
                      </ListItem>
                    ),
                  )}
                </ul>
              </div>
            </Card>
          </div>
        )}

        {/*Edit Resume */}
        {!showResumeView && (
          <div className="w-full flex flex-col items-center gap-6">
            {/* Companies Section */}
            <Card className="w-full">
              <h1 className="primary-color text-2xl font-semibold">
                Companies
              </h1>
              <div className="flex flex-col gap-6">
                {resumeDetails?.resume.companies.map((company, index) => (
                  <div key={index}>
                    <h4 className="primary-color text-lg">{company.name}</h4>
                    <ul>
                      {company.bullets.map((bullet, bulletIndex) => (
                        <ListItem
                          key={bulletIndex}
                          item={{ content: [bullet.value] }}
                        >
                          <ListItem.Content />
                        </ListItem>
                      ))}
                    </ul>
                  </div>
                ))}
              </div>
            </Card>
            {/*Projects Section */}
            <Card className="w-full">
              <h1 className="primary-color text-2xl font-semibold">Projects</h1>
              <div className="flex flex-col gap-6">
                {resumeDetails?.resume.projects.map((project, index) => (
                  <div key={index}>
                    <h4 className="primary-color text-lg">{project.name}</h4>

                    <p>{project.description}</p>
                    <p className="mt-2">
                      <p className="font-semibold text-gray-800">Tech Stack:</p>{" "}
                      {project.techStack}
                    </p>
                  </div>
                ))}
              </div>
            </Card>
            {/*Education Section */}
            <Card className="w-full">
              <h1 className="primary-color text-2xl font-semibold">
                Education
              </h1>
              <div className="flex flex-col gap-6">
                {resumeDetails?.resume.education.map((education, index) => (
                  <div key={index}>
                    <div className="flex justify-between">
                      <h4 className="primary-color text-lg">
                        {education.schoolName}
                      </h4>
                      <p>
                        {education.ended && `${education.ended}`}
                        {!education.ended && education.started + " - Present"}
                      </p>
                    </div>
                    <div className="flex justify-between">
                      <p>{education.degree}</p>
                      <p>{education.major}</p>
                    </div>
                  </div>
                ))}
              </div>
            </Card>
          </div>
        )}
      </div>
    </div>
  );
};

export default ResumeDetails;
