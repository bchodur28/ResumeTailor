import Card from "../../components/ui/Card";
import { useLibraryExperienceListItems } from "../../hooks/useLibraryExperienceListItems";
import type { ExperienceTypeValue } from "../../models/resumes/ExperienceType";
import Button from "../../components/ui/buttons/Button";
import { Pen, PencilSquare, Trash3 } from "react-bootstrap-icons";
import { useNavigate } from "react-router-dom";

const getExperienceTypeLabel = (experienceTypeValue: ExperienceTypeValue) => {
  switch (experienceTypeValue) {
    case "employment":
      return "Employment";
    case "internship":
      return "Internship";
    case "freelance":
      return "Freelance";
    case "personalProject":
      return "Personal Project";
    case "academicProject":
      return "Academic Project";
    case "other":
      return "Other";
    default:
      return "Unknown";
  }
};

const LibraryExperiencePage = () => {
  const navigate = useNavigate();

  const {
    data: experiences,
    isLoading,
    error,
  } = useLibraryExperienceListItems();

  console.log("Library experiences:", experiences);
  return (
    <div className="flex flex-col gap-4 items-center w-xl max-[640px]:w-full">
      <h1 className="text-2xl font-bold">Experience Library</h1>
      <p className=" text-gray-600">
        Add and manage your professional experiences. You can include multiple
        entries, including jobs, internships, and other relevant work
        experiences.
      </p>
      <Card className="w-xl max-[640px]:w-full">
        <div>
          {isLoading && <p>Loading...</p>}
          {error && <p>Error loading experiences</p>}
          {experiences && experiences.length === 0 && (
            <p>No experiences found</p>
          )}
          {experiences && experiences.length > 0 && (
            <ul className="w-full flex flex-col gap-2">
              {experiences.map((experience) => (
                <li
                  className="border border-gray-300 p-2 rounded-lg cursor-pointer hover:bg-gray-200 transition-colors duration-300"
                  key={experience.id}
                  onClick={() =>
                    navigate(`/library/experience/${experience.id}/edit`)
                  }
                >
                  <div className="flex justify-between">
                    <h4 className="text-base font-semibold text-gray-700 underline">
                      {experience.organization
                        ? experience.organization
                        : experience.title}
                    </h4>
                    <p className="text-gray-600 text-sm font-semibold">
                      {getExperienceTypeLabel(experience.type)}
                    </p>
                  </div>
                  <div className="flex justify-between">
                    <div>
                      <p className="text-gray-500 text-sm font-medium">
                        Bullet Count:{" "}
                        <span className="text-base font-medium text-gray-700">
                          {experience.bulletCount}
                        </span>
                      </p>
                      <p className="text-gray-500 text-sm font-medium">
                        Summary Count:{" "}
                        <span className="text-base font-medium text-gray-700">
                          {experience.summaryCount}
                        </span>
                      </p>
                      <p className="text-gray-500 text-sm font-medium">
                        Resume Count:{" "}
                        <span className="text-base font-medium text-gray-700">
                          {experience.resumeCount}
                        </span>
                      </p>
                    </div>
                    <div className="flex items-end gap-2">
                      <Button
                        label="Delete"
                        iconComponent={Trash3}
                        labelSlideOut={true}
                        styleType="danger"
                      />
                    </div>
                  </div>
                </li>
              ))}
            </ul>
          )}
        </div>
      </Card>
    </div>
  );
};

export default LibraryExperiencePage;
