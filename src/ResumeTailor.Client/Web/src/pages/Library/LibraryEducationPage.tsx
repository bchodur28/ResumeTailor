import { useState } from "react";
import { useLibraryEducation } from "../../hooks/useLibraryEducation";
import { useDateFormat } from "../../hooks/useDateFormat";
import {
  EducationType,
  getEducationTypeLabel,
  type EducationTypeValue,
} from "../../models/education/EducationType";
import Card from "../../components/ui/Card";
import DropdownButton from "../../components/ui/buttons/DropdownButton";
import ActionList from "../../components/ui/lists/ActionList";
import LibraryEducationItem from "./components/LibraryEducationItem";
import Button from "../../components/ui/buttons/Button";
import Message from "../../components/ui/feedback/Message";

type NewEducation = {
  key: string;
  type: EducationTypeValue;
};

const LibraryEducationPage = () => {
  const [showArchived, setShowArchived] = useState(false);
  const [newEducation, setNewEducation] = useState<NewEducation[]>([]);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  const {
    data: education,
    isLoading,
    isError,
    error,
    isFetching,
  } = useLibraryEducation(showArchived);

  console.log({
    education,
    isLoading,
    isError,
    error,
    isFetching,
    showArchived,
  });

  const handleAddEducation = (type: EducationTypeValue) => {
    setNewEducation((prev) => [...prev, { key: crypto.randomUUID(), type }]);
  };

  const removeNewEducation = (key: string) => {
    setNewEducation((prev) => prev.filter((item) => item.key !== key));
  };

  const handleEducationSuccess = (
    action: "archived" | "restored" | "deleted" | "saved",
  ) => {
    const messages = {
      archived: "Education archived successfully.",
      restored: "Education restored successfully.",
      deleted: "Education deleted successfully.",
      saved: "Education saved successfully.",
    };

    setSuccessMessage(messages[action]);
  };

  if (isLoading) return <div>Loading Education...</div>;
  if (isError) return <div>Failed to load education.</div>;

  return (
    <div className="flex flex-col gap-4 items-center w-xl max-[640px]:w-full">
      <h1 className="text-2xl font-bold">Education Library</h1>

      <p className="text-gray-600">
        Add and manage your educational background.
      </p>

      <Card className="w-xl max-[640px]:w-full flex flex-col gap-4">
        {successMessage && (
          <Message
            type="success"
            message={successMessage}
            onClose={() => setSuccessMessage(null)}
          />
        )}
        {education?.map((item) => (
          <LibraryEducationItem
            key={item.id}
            education={item}
            archived={showArchived}
            onSuccess={handleEducationSuccess}
          />
        ))}

        {!showArchived &&
          newEducation.map((item) => (
            <LibraryEducationItem
              key={item.key}
              educationType={item.type}
              onRemove={() => removeNewEducation(item.key)}
              onCreated={() => removeNewEducation(item.key)}
              onSuccess={handleEducationSuccess}
            />
          ))}

        {education?.length === 0 && newEducation.length === 0 && (
          <div className="flex justify-center rounded-lg bg-gray-200 border border-gray-300 p-4">
            <p className="text-gray-700 font-semibold">No education entries.</p>
          </div>
        )}

        <div className="flex gap-2">
          <Button
            label={showArchived ? "Show Active" : "Show Archived"}
            styleType="plain"
            onClick={() => {
              setShowArchived(!showArchived);
              setSuccessMessage(null);
            }}
            grow
          />
          {!showArchived && (
            <DropdownButton
              label="Add"
              closeOnClick
              grow
              positionRelativeTo="container"
            >
              <ActionList
                actions={Object.values(EducationType).map((type) => ({
                  actionName: getEducationTypeLabel(type),
                  actionFn: () => handleAddEducation(type),
                }))}
              />
            </DropdownButton>
          )}
        </div>
      </Card>
    </div>
  );
};

export default LibraryEducationPage;
