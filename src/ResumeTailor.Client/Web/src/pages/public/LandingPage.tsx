import {
  ArrowRight,
  Book,
  CardChecklist,
  CheckCircleFill,
  FileEarmarkText,
  LightningCharge,
  PatchCheck,
  PencilSquare,
  Stars,
  TextParagraph,
} from "react-bootstrap-icons";
import { useAuth0 } from "@auth0/auth0-react";
import { Link } from "react-router-dom";

const features = [
  {
    icon: Book,
    title: "Build Your Experience Library",
    description:
      "Your career is more than a two-page resume. Store all your experiences, projects, and accomplishments in one place, giving Resumade a rich collection of bullet points to choose from when tailoring each resume.",
  },
  {
    icon: Stars,
    title: "AI-Tailored Resumes",
    description:
      "Paste a job description and let AI help select relevant accomplishments and tailor your resume to the opportunity.",
  },
  {
    icon: PencilSquare,
    title: "Make It Your Own",
    description:
      "Review and edit your generated resume, customize its appearance, and export a professional PDF.",
  },
  {
    icon: CardChecklist,
    title: "Track Your Applications",
    description:
      "Keep your resumes and job applications organized so you can see where you stand throughout your job search.",
  },
];

const steps = [
  {
    number: "01",
    title: "Build Your Library",
    description:
      "Add your experience, projects, education, and accomplishments. Capture more than you'd normally include on a single resume.",
  },
  {
    number: "02",
    title: "Generate a Tailored Resume",
    description:
      "Paste a job description and let Resumade select your most relevant experience and bullet points, while keeping your original accomplishments intact.",
  },
  {
    number: "03",
    title: "Refine, Export & Track",
    description:
      "Review and customize your resume, export it as a PDF, and track your application.",
  },
];

const LandingPage = () => {
  const { isAuthenticated, loginWithRedirect } = useAuth0();

  const handleGetStarted = () => {
    void loginWithRedirect({
      appState: { returnTo: "/app" },
      authorizationParams: {
        screen_hint: "signup",
      },
    });
  };

  const GetStartedButton = ({ dark = false }: { dark?: boolean }) =>
    isAuthenticated ? (
      <Link
        to="/app"
        className={`inline-flex items-center justify-center gap-2 rounded-xl px-6 py-3.5 font-semibold transition-all hover:-translate-y-0.5 ${
          dark
            ? "bg-white text-slate-900 hover:bg-blue-50"
            : "primary-bg-300 text-white shadow-lg shadow-blue-600/20 hover:primary-bg-400"
        }`}
      >
        Go to Tracker
        <ArrowRight size={18} />
      </Link>
    ) : (
      <button
        type="button"
        onClick={handleGetStarted}
        className={`inline-flex items-center justify-center gap-2 rounded-xl px-6 py-3.5 font-semibold transition-all hover:-translate-y-0.5 ${
          dark
            ? "bg-white text-slate-900 hover:bg-blue-50"
            : "primary-bg-300 text-white shadow-lg shadow-blue-600/20 hover:primary-bg-400"
        }`}
      >
        Get Started
        <ArrowRight size={18} />
      </button>
    );

  return (
    <div className="overflow-hidden bg-white text-slate-900">
      {/* HERO */}
      <section className="relative overflow-hidden bg-gradient-to-b from-blue-50 via-white to-white">
        <div className="pointer-events-none absolute -right-40 -top-48 h-[500px] w-[500px] rounded-full bg-blue-100/50 blur-3xl" />
        <div className="pointer-events-none absolute -left-40 top-48 h-[400px] w-[400px] rounded-full bg-indigo-100/40 blur-3xl" />

        <div className="relative mx-auto grid max-w-7xl items-center gap-16 px-6 py-20 lg:grid-cols-2 lg:py-32">
          <div className="max-w-2xl">
            <div className="mb-7 inline-flex items-center gap-2 rounded-full border border-blue-200 bg-blue-50 px-4 py-2 text-sm font-medium primary-color">
              <Stars size={16} />A smarter way to build resumes
            </div>

            <h1 className="text-5xl font-bold leading-[1.1] tracking-tight sm:text-6xl lg:text-7xl">
              Your experience.
              <br />
              <span className="primary-color">The right resume.</span>
              <br />
              Every time.
            </h1>

            <p className="mt-7 max-w-xl text-lg leading-relaxed text-slate-600">
              Stop rebuilding your resume for every job. Build your experience
              library once, tailor resumes to each opportunity, and manage your
              job search in one place.
            </p>

            <div className="mt-9 flex flex-wrap items-center gap-4">
              <GetStartedButton />

              <a
                href="#how-it-works"
                className="inline-flex items-center justify-center gap-2 rounded-xl border border-slate-200 bg-white px-6 py-3.5 font-semibold text-slate-700 transition-colors hover:bg-slate-50"
              >
                See How It Works
                <ArrowRight size={16} />
              </a>
            </div>

            <div className="mt-9 flex flex-wrap gap-x-6 gap-y-3 text-sm text-slate-600">
              <span className="flex items-center gap-2">
                <CheckCircleFill className="primary-color" />
                Reusable experience library
              </span>
              <span className="flex items-center gap-2">
                <CheckCircleFill className="primary-color" />
                PDF resume exports
              </span>
            </div>
          </div>

          {/* PRODUCT PREVIEW */}
          <div className="relative">
            <p className="mt-3 text-center text-xs text-slate-600 mb-1">
              Illustrative preview · Sample resume and AI analysis
            </p>
            <div className="absolute -inset-5 rounded-[2rem] bg-gradient-to-br from-blue-200/60 to-indigo-100/50 blur-2xl" />

            <div className="relative overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-2xl shadow-slate-300/40">
              <div className="flex items-center gap-2 border-b border-slate-100 bg-slate-50 px-5 py-4">
                <span className="h-3 w-3 rounded-full bg-red-300" />
                <span className="h-3 w-3 rounded-full bg-yellow-300" />
                <span className="h-3 w-3 rounded-full bg-green-300" />
                <span className="ml-4 text-xs font-medium text-slate-400">
                  Resumade / Resume Editor
                </span>
              </div>

              <div className="grid grid-cols-1 items-start gap-3 bg-slate-50/70 p-3 sm:grid-cols-[minmax(0,2fr)_minmax(0,1fr)] sm:gap-4 sm:p-5">
                {/* Resume editor */}
                <div className="flex min-w-0 flex-col items-center gap-3">
                  {/* Editor toolbar */}
                  <div className="flex max-w-full items-center justify-center gap-4 rounded-full border border-blue-200 bg-white px-4 py-2 shadow-sm sm:gap-5">
                    <div
                      className="flex items-center justify-center text-slate-500"
                      title="Resume settings"
                    >
                      <PatchCheck size={17} />
                    </div>

                    <span className="text-xs font-medium text-slate-600 sm:text-sm">
                      H1
                    </span>

                    <span className="text-xs font-medium text-slate-600 sm:text-sm">
                      H2
                    </span>

                    <TextParagraph
                      size={17}
                      className="shrink-0 text-slate-500"
                    />
                  </div>

                  {/* Resume document */}
                  <div className="w-full min-w-0 bg-white p-3 shadow-md sm:p-5">
                    {/* Resume header */}
                    <div className="text-center">
                      <h3 className="text-[11px] font-bold text-slate-900 sm:text-base">
                        Alex Morgan
                      </h3>

                      <p className="text-[9px] text-slate-700 sm:text-xs">
                        Software Engineer
                      </p>

                      <p className="mt-1 text-[7px] text-slate-500 sm:text-[9px]">
                        Minneapolis, MN
                      </p>

                      <p className="text-[7px] text-slate-500 sm:text-[9px]">
                        alex@example.com
                      </p>
                    </div>

                    {/* Skills */}
                    <div className="mt-4 sm:mt-6">
                      <h4 className="border-b border-slate-400 pb-0.5 text-[8px] font-bold text-slate-900 sm:text-[10px]">
                        SKILLS
                      </h4>

                      <p className="mt-1 text-[7px] leading-relaxed text-slate-700 sm:text-[9px]">
                        C#, .NET, React, TypeScript, SQL Server, Azure, REST
                        APIs, Entity Framework, Docker
                      </p>
                    </div>

                    {/* Professional Experience */}
                    <div className="mt-3 sm:mt-4">
                      <h4 className="border-b border-slate-400 pb-0.5 text-[8px] font-bold text-slate-900 sm:text-[10px]">
                        PROFESSIONAL EXPERIENCE
                      </h4>

                      <div className="mt-2 flex items-start justify-between gap-2">
                        <div className="min-w-0">
                          <p className="text-[8px] font-bold text-slate-900 sm:text-[10px]">
                            Software Engineer
                          </p>

                          <p className="text-[7px] text-slate-600 sm:text-[9px]">
                            Acme Technologies
                          </p>
                        </div>

                        <span className="shrink-0 text-[7px] text-slate-600 sm:text-[8px]">
                          2020 – 2025
                        </span>
                      </div>

                      <ul className="mt-2 list-disc space-y-1.5 pl-3 text-[7px] leading-relaxed text-slate-700 sm:text-[9px]">
                        <li>
                          Built and maintained scalable .NET services supporting
                          enterprise applications.
                        </li>

                        <li>
                          Developed REST APIs and integrated third-party
                          platforms.
                        </li>

                        <li>
                          Improved system performance through database and API
                          optimizations.
                        </li>
                      </ul>
                    </div>

                    {/* Projects */}
                    <div className="mt-3 sm:mt-4">
                      <h4 className="border-b border-slate-400 pb-0.5 text-[8px] font-bold text-slate-900 sm:text-[10px]">
                        PROJECTS
                      </h4>

                      <p className="mt-2 text-[8px] font-semibold text-slate-900 sm:text-[10px]">
                        Full-Stack Web Application
                      </p>

                      <ul className="mt-1 list-disc pl-3 text-[7px] leading-relaxed text-slate-700 sm:text-[9px]">
                        <li>
                          Built a React and .NET application with secure
                          authentication and SQL storage.
                        </li>
                      </ul>
                    </div>

                    {/* Education */}
                    <div className="mt-3 sm:mt-4">
                      <h4 className="border-b border-slate-400 pb-0.5 text-[8px] font-bold text-slate-900 sm:text-[10px]">
                        EDUCATION
                      </h4>

                      <p className="mt-1 text-[7px] text-slate-700 sm:text-[9px]">
                        B.S. Software Engineering
                      </p>
                    </div>
                  </div>
                </div>

                {/* AI Analysis panel */}
                <div className="min-w-0 space-y-3">
                  {/* Match score */}
                  <div className="rounded-xl border border-slate-100 bg-white p-3 shadow-sm">
                    <div className="mb-2 flex items-center justify-between gap-1">
                      <p className="text-[9px] font-semibold text-slate-800 sm:text-xs">
                        Match Score
                      </p>

                      <span className="text-[9px] font-bold text-green-600 sm:text-xs">
                        85%
                      </span>
                    </div>

                    <div className="h-1.5 overflow-hidden rounded-full bg-slate-100 sm:h-2">
                      <div
                        className="h-full w-[85%] rounded-full"
                        style={{
                          background:
                            "linear-gradient(to right, #ef4444, #f97316, #eab308, #84cc16)",
                        }}
                      />
                    </div>
                  </div>

                  {/* AI Summary */}
                  <div className="rounded-xl border border-slate-100 bg-white p-3 shadow-sm">
                    <h4 className="primary-color mb-2 text-[10px] font-semibold sm:text-sm">
                      AI Summary
                    </h4>

                    <p className="text-[8px] leading-relaxed text-slate-600 sm:text-[10px]">
                      This position emphasizes full-stack development, API
                      design, and database optimization. Alex's experience with
                      .NET, React, and SQL Server aligns with the role's core
                      requirements.
                    </p>
                  </div>

                  {/* Alternative Bullets */}
                  <div className="rounded-xl border border-slate-100 bg-white p-3 shadow-sm">
                    <h4 className="primary-color mb-2 text-[10px] font-semibold sm:text-sm">
                      Alternative Bullets
                    </h4>

                    <div className="rounded-lg border border-blue-100 bg-blue-50/50 p-2">
                      <p className="text-[8px] leading-relaxed text-slate-700 sm:text-[10px]">
                        Optimized .NET services and SQL queries to improve
                        application performance and reliability.
                      </p>
                    </div>

                    <div className="mt-2 flex items-center gap-1.5 text-[8px] text-green-700 sm:text-[10px]">
                      <span className="h-1.5 w-1.5 shrink-0 rounded-full bg-green-500" />
                      Relevant experience highlighted
                    </div>
                  </div>
                </div>
              </div>

              <div className="flex items-center justify-between border-t border-slate-100 bg-white px-6 py-4">
                <span className="flex items-center gap-2 text-xs font-medium text-slate-600">
                  <CheckCircleFill className="text-green-500" />
                  Resume ready
                </span>
                <div className="rounded-lg primary-bg-300 px-3 py-2 text-xs font-medium text-white">
                  Export PDF
                </div>
              </div>
            </div>

            <div className="absolute -bottom-6 -left-5 hidden items-center gap-3 rounded-xl border border-slate-100 bg-white p-4 shadow-xl sm:flex">
              <div className="rounded-lg bg-blue-100 p-3 primary-color">
                <LightningCharge size={22} />
              </div>
              <div>
                <p className="text-sm font-bold">Tailored for the role</p>
                <p className="text-xs text-slate-500">
                  Your best experience, highlighted
                </p>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* FEATURES */}
      <section id="features" className="bg-white py-24">
        <div className="mx-auto max-w-7xl px-6">
          <div className="mx-auto mb-14 max-w-2xl text-center">
            <p className="mb-3 text-sm font-bold uppercase tracking-widest primary-color">
              Everything in one place
            </p>
            <h2 className="text-3xl font-bold tracking-tight sm:text-4xl">
              Spend less time formatting.
              <br />
              More time applying.
            </h2>
            <p className="mt-5 text-lg text-slate-600">
              From organizing your experience to tracking applications, Resumade
              helps simplify the resume-building process.
            </p>
          </div>

          <div className="grid gap-6 md:grid-cols-2">
            {features.map(({ icon: Icon, title, description }) => (
              <div
                key={title}
                className="group rounded-2xl border border-slate-200 bg-white p-8 transition-all hover:-translate-y-1 hover:border-blue-200 hover:shadow-xl hover:shadow-blue-100/50"
              >
                <div className="mb-6 inline-flex rounded-xl bg-blue-50 p-4 primary-color transition-colors group-hover:bg-blue-100">
                  <Icon size={26} />
                </div>

                <h3 className="mb-3 text-xl font-bold">{title}</h3>
                <p className="leading-relaxed text-slate-600">{description}</p>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* HOW IT WORKS */}
      <section
        id="how-it-works"
        className="border-y border-slate-100 bg-slate-50 py-24"
      >
        <div className="mx-auto max-w-7xl px-6">
          <div className="mb-16 text-center">
            <p className="mb-3 text-sm font-bold uppercase tracking-widest primary-color">
              Simple by design
            </p>
            <h2 className="text-3xl font-bold tracking-tight sm:text-4xl">
              Three steps to your next resume
            </h2>
            <p className="mt-4 text-lg text-slate-600">
              Set up your experience once. Reuse it for every opportunity.
            </p>
          </div>

          <div className="grid gap-8 md:grid-cols-3">
            {steps.map((step) => (
              <div
                key={step.number}
                className="relative rounded-2xl border border-slate-200 bg-white p-8 shadow-sm"
              >
                <span className="mb-6 block text-5xl font-bold text-blue-100">
                  {step.number}
                </span>

                <h3 className="mb-3 text-xl font-bold">{step.title}</h3>

                <p className="leading-relaxed text-slate-600">
                  {step.description}
                </p>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* BENEFITS */}
      <section className="py-24">
        <div className="mx-auto grid max-w-7xl items-center gap-14 px-6 lg:grid-cols-2">
          <div className="rounded-2xl border border-blue-100 bg-gradient-to-br from-blue-50 to-indigo-50 p-8 sm:p-12">
            <div className="rounded-2xl border border-slate-200 bg-white p-7 shadow-xl shadow-blue-200/30">
              <div className="mb-6 flex items-center gap-3">
                <div className="rounded-lg bg-blue-100 p-3 primary-color">
                  <FileEarmarkText size={22} />
                </div>
                <div>
                  <p className="font-bold">Your Experience Library</p>
                  <p className="text-xs text-slate-500">
                    Your career, organized
                  </p>
                </div>
              </div>

              {[
                "Professional Experience",
                "Projects & Accomplishments",
                "Education & Certifications",
              ].map((item) => (
                <div
                  key={item}
                  className="mb-3 flex items-center justify-between rounded-lg border border-slate-100 bg-slate-50 px-4 py-3"
                >
                  <span className="text-sm font-medium text-slate-700">
                    {item}
                  </span>
                  <CheckCircleFill size={16} className="primary-color" />
                </div>
              ))}
            </div>
          </div>

          <div>
            <p className="mb-3 text-sm font-bold uppercase tracking-widest primary-color">
              Built around your experience
            </p>
            <h2 className="text-3xl font-bold tracking-tight sm:text-4xl">
              Your accomplishments deserve more than copy and paste.
            </h2>

            <p className="mt-6 text-lg leading-relaxed text-slate-600">
              Your career experience doesn't change every time you apply for a
              job. The way you present it does.
            </p>

            <p className="mt-4 leading-relaxed text-slate-600">
              Resumade keeps your original accomplishments in one reusable
              library, helping you select and present relevant experience
              without starting from scratch.
            </p>

            <div className="mt-8 space-y-4">
              {[
                "Keep your original experience organized",
                "Reuse accomplishments across multiple resumes",
                "Edit and personalize every generated resume",
                "Keep track of the applications you've created",
              ].map((item) => (
                <div key={item} className="flex items-start gap-3">
                  <CheckCircleFill
                    size={18}
                    className="mt-1 shrink-0 primary-color"
                  />
                  <span className="text-slate-700">{item}</span>
                </div>
              ))}
            </div>
          </div>
        </div>
      </section>

      {/* FINAL CTA */}
      <section className="px-6 pb-24">
        <div className="relative mx-auto max-w-7xl overflow-hidden rounded-3xl bg-slate-900 px-6 py-20 text-center text-white sm:px-12">
          <div className="pointer-events-none absolute -right-20 -top-40 h-96 w-96 rounded-full bg-blue-600/30 blur-3xl" />
          <div className="pointer-events-none absolute -bottom-48 -left-20 h-96 w-96 rounded-full bg-indigo-600/30 blur-3xl" />

          <div className="relative mx-auto max-w-2xl">
            <div className="mb-6 flex justify-center">
              <div className="rounded-2xl bg-white/10 p-4">
                <Stars size={30} />
              </div>
            </div>

            <h2 className="text-3xl font-bold tracking-tight sm:text-5xl">
              Ready to build your next resume?
            </h2>

            <p className="mx-auto mt-6 max-w-xl text-lg leading-relaxed text-slate-300">
              Organize your experience, create resumes tailored to your next
              opportunity, and simplify your job search.
            </p>

            <div className="mt-9">
              <GetStartedButton dark />
            </div>
          </div>
        </div>
      </section>
    </div>
  );
};

export default LandingPage;
