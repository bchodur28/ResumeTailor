import type { ResumeGenerationResult } from "../models/resume/ResumeGenerationResult";
import { cornerstoneBullets, buildertrendBullets } from "./mockBullets";

export const mockResumeGenerationResult: ResumeGenerationResult = {
  aiResume: {
    personName: "Brian Chodur",
    profession: "Software Engineer",
    location: "St. Paul, MN",
    phoneNumber: "(641) 903-1178",
    college: "Iowa State University",
    degree: "Bachelor of Science",
    major: "Software Engineer",
    collegeStatus: "Dec 2019",
    personalSite1: "linkedin.com/in/brianchodur",
    personalSite2: "github.com/brianchodur",
    personalSite3: undefined,
    skills: ["JavaScript", "TypeScript", "React", "Node.js", "CSS", "HTML"],
    experience: [
      {
        name: "Cornerstone OnDemand",
        position: "Software Engineer",
        workingStatus: "Jan 2020 - Nov 2025",
        location: "Remote",
        bullets: cornerstoneBullets,
      },
      {
        name: "Buildertrend",
        position: "Software Engineer Intern",
        workingStatus: "May 2019 - Aug 2019",
        location: "Omaha, NE",
        bullets: buildertrendBullets,
      },
    ],
    projects: [
      {
        name: "ResumeTailor",
        status: "Actively Developing",
        description:
          "A full-stack application and Chrome extension that extracts structured data from job postings and helps tailor resumes by selecting relevant accomplishments from a user-curated bullet bank.",
        techStack: [
          "C#",
          ".NET",
          "ASP.NET Core Web API",
          "Entity Framework Core",
          "SQLite",
          "React",
          "TypeScript",
          "HTML",
          "CSS",
          "TanStack Query",
          "React Hook Form",
          "Vite",
          "Chrome Extension APIs",
          "REST APIs",
        ],
        link: "https://github.com/bchodur28/ResumeTailor",
      },
    ],
  },
  aiSummary: {
    summary:
      "Your resume demonstrates a strong match for this Senior Software Engineer position, particularly through your experience with C#, .NET, REST APIs, microservices, SQL, and high-volume enterprise applications. Your background includes building scalable services, improving system performance, troubleshooting critical production issues, and modernizing legacy applications. Experience with asynchronous processing and event-driven architecture further aligns with the technical requirements of the role. Your history of mentoring developers and collaborating across teams also supports the position’s senior-level expectations. While there are a few areas where your experience could be highlighted more clearly, your overall technical background and professional experience make you a competitive candidate for this position.",

    alternativeBullet: [
      "Built and maintained .NET microservices and ASP.NET REST APIs supporting daily synchronization of 200,000+ learning resources.",
      "Improved content synchronization performance through asynchronous queue processing, allowing workloads to execute concurrently.",
      "Resolved a critical production memory issue by implementing a custom JSON parser that reduced payload size by approximately 80%.",
    ],
    strengths: [
      "6+ years of professional software development experience",
      "Strong C# and .NET background",
      "Experience designing and maintaining REST APIs",
      "Hands-on experience with microservices architecture",
      "Experience optimizing SQL Server and MySQL workloads",
      "Experience mentoring developers and collaborating across teams",
    ],
    weaknesses: [
      "Limited direct experience with PostgreSQL",
      "Resume does not demonstrate extensive Azure production experience",
      "Could provide more examples of system design ownership",
    ],
    rating: 87,
    location: "Minneapolis, MN",
    salary: "$125,000 - $145,000",
    workStyle: "Onsite",
  },
  aiUsage: {
    inputTokens: 1000,
    outputTokens: 500,
    estimatedCost: 0.05,
  },
};
