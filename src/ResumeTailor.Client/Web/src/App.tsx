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

const router = createBrowserRouter(
  createRoutesFromElements(
    <Route path="/" element={<RootLayout />}>
      <Route index element={<Generate />} />
      <Route path="manage" element={<Manage />} />
      <Route path="track" element={<Track />} />
      <Route
        path="*"
        element={
          <div>
            <h1>Not Found</h1>
          </div>
        }
      />
    </Route>,
  ),
);

function App() {
  return <RouterProvider router={router} />;
}

export default App;
