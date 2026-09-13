import {
  createBrowserRouter,
  createRoutesFromElements,
  Route,
  RouterProvider,
} from "react-router-dom";
import RootLayout from "./layouts/RootLayout";
import Generate from "./pages/Generate";
import Manage from "./pages/Manage";
import Track from "./pages/Track";
import ProtectedRoute from "./components/ProtectedRoute";
import Login from "./pages/Login";

const router = createBrowserRouter(
  createRoutesFromElements(
    <>
      <Route path="/login" element={<Login />} />
      <Route element={<ProtectedRoute />}>
        <Route path="/" element={<RootLayout />}>
          <Route index element={<Generate />} />
          <Route path="manage" element={<Manage />} />
          <Route path="track" element={<Track />} />
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
  console.log("window.location.origin", window.location.origin);
  return <RouterProvider router={router} />;
}

export default App;
