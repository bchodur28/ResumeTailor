import {
  createBrowserRouter,
  createRoutesFromElements,
  Route,
  RouterProvider,
} from "react-router-dom";
import AccountSetup from "./pages/AccountSetup";
import RootLayout from "./layouts/RootLayout";
import Generate from "./pages/Generate";
import LibraryLayout from "./layouts/LibraryLayout";
import Manage from "./pages/Manage";
import Track from "./pages/ResumeTracker/ResumeTrackingPage";
import Profile from "./pages/Profile";
import ProtectedRoute from "./components/routes/ProtectedRoute";
import Login from "./pages/Login";
import AccountRequiredRoute from "./components/routes/AccountRequiredRoute";
import ResumeDetailsPage from "./pages/ResumeDetails/ResumeDetailsPage";
import LibraryExperiencePage from "./pages/Library/LibraryExperiencePage";
import LibraryEducationPage from "./pages/Library/LibraryEducationPage";
import LibraryHeaderPage from "./pages/Library/LibraryHeaderPage";

const router = createBrowserRouter(
  createRoutesFromElements(
    <>
      <Route path="/login" element={<Login />} />
      <Route element={<ProtectedRoute />}>
        <Route path="/account/setup" element={<AccountSetup />} />

        <Route element={<AccountRequiredRoute />}>
          <Route path="/" element={<RootLayout />}>
            <Route index element={<Track />} />
            <Route path="generate" element={<Generate />} />
            <Route path="manage" element={<Manage />} />
            <Route path="resumes/:id" element={<ResumeDetailsPage />} />
            <Route path="profile" element={<Profile />} />
            <Route path="library" element={<LibraryLayout />}>
              <Route index element={<LibraryHeaderPage />} />
              <Route path="experience" element={<LibraryExperiencePage />} />
              <Route path="education" element={<LibraryEducationPage />} />
            </Route>
          </Route>
        </Route>
      </Route>

      <Route
        path="*"
        element={
          <div>
            <h1>Not Found</h1>
          </div>
        }
      />
    </>,
  ),
);

function App() {
  return <RouterProvider router={router} />;
}

export default App;
