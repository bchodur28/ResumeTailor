import {
  createBrowserRouter,
  createRoutesFromElements,
  Route,
  RouterProvider,
} from "react-router-dom";
import NotFoundPage from "./pages/NotFoundPage";
import LibraryExperienceEditPage from "./pages/library/LibraryExperienceEditPage";
import AccountSetup from "./pages/AccountSetup";
import RootLayout from "./layouts/RootLayout";
import Generate from "./pages/Generate";
import LibraryLayout from "./layouts/LibraryLayout";
import Track from "./pages/resume-tracker/ResumeTrackingPage";
import Profile from "./pages/Profile";
import ProtectedRoute from "./components/routes/ProtectedRoute";
import AccountRequiredRoute from "./components/routes/AccountRequiredRoute";
import ResumeDetailsPage from "./pages/resume-details/ResumeDetailsPage";
import LibraryExperiencePage from "./pages/library/LibraryExperiencePage";
import LibraryEducationPage from "./pages/library/LibraryEducationPage";
import LibraryHeaderPage from "./pages/library/LibraryHeaderPage";
import AccountSetupRoute from "./components/routes/AccountSetupRoute";
import LandingPage from "./pages/public/LandingPage";
import SiteLayout from "./layouts/SiteLayout";

const router = createBrowserRouter(
  createRoutesFromElements(
    <Route element={<SiteLayout />}>
      {/* Public pages */}
      <Route path="/" element={<LandingPage />} />

      {/* Authenticated pages */}
      <Route element={<ProtectedRoute />}>
        <Route element={<AccountSetupRoute />}>
          <Route path="/account/setup" element={<AccountSetup />} />
        </Route>

        <Route element={<AccountRequiredRoute />}>
          <Route path="/app" element={<RootLayout />}>
            <Route index element={<Track />} />
            <Route path="generate" element={<Generate />} />
            <Route path="resumes/:id" element={<ResumeDetailsPage />} />
            <Route path="profile" element={<Profile />} />

            <Route path="library" element={<LibraryLayout />}>
              <Route index element={<LibraryHeaderPage />} />
              <Route path="experience" element={<LibraryExperiencePage />} />
              <Route
                path="experience/:experienceId/edit"
                element={<LibraryExperienceEditPage />}
              />
              <Route path="education" element={<LibraryEducationPage />} />
            </Route>
          </Route>
        </Route>
      </Route>

      <Route path="*" element={<NotFoundPage />} />
    </Route>,
  ),
);

function App() {
  return <RouterProvider router={router} />;
}

export default App;
