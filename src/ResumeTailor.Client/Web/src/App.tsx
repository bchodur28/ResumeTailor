import {
  createBrowserRouter,
  createRoutesFromElements,
  Route,
  RouterProvider,
} from "react-router-dom";
import AccountSetup from "./pages/AccountSetup";
import RootLayout from "./layouts/RootLayout";
import Generate from "./pages/Generate";
import Manage from "./pages/Manage";
import Track from "./pages/Track";
import Profile from "./pages/Profile";
import ProtectedRoute from "./components/routes/ProtectedRoute";
import Login from "./pages/Login";
import AccountRequiredRoute from "./components/routes/AccountRequiredRoute";
import ResumeDetails from "./pages/ResumeDetails";

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
            <Route path="resumes/:id" element={<ResumeDetails />} />
            <Route path="profile" element={<Profile />} />
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
