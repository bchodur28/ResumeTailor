import {
  EducationType,
  type EducationTypeValue,
} from "../../models/education/EducationType";

export const getEducationFieldConfig = (type: EducationTypeValue) => {
  switch (type) {
    case EducationType.Degree:
      return {
        institutionLabel: "School Name",
        qualificationLabel: "Degree",
        showFieldOfStudy: true,
        showGpa: true,
        showStarted: true,
        startedLabel: "Started",
        endedLabel: "Graduated",
      };

    case EducationType.Certificate:
      return {
        institutionLabel: "Institution",
        qualificationLabel: "Certificate Name",
        showFieldOfStudy: false,
        showGpa: false,
        showStarted: true,
        startedLabel: "Started",
        endedLabel: "Completed",
      };

    case EducationType.Certification:
      return {
        institutionLabel: "Issuing Organization",
        qualificationLabel: "Certification Name",
        showFieldOfStudy: false,
        showGpa: false,
        showStarted: false,
        startedLabel: "",
        endedLabel: "Issued",
      };

    default:
      return {
        institutionLabel: "Organization",
        qualificationLabel: "Program Name",
        showFieldOfStudy: false,
        showGpa: false,
        showStarted: true,
        startedLabel: "Started",
        endedLabel: "Completed",
      };
  }
};
