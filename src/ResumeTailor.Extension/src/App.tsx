import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import "./styles/main.css";
import "./styles/_reset.css";
import PageExtraction from "./pages/PageExtraction";
import { createHashRouter, RouterProvider } from "react-router-dom";
import ExtractionDefinition from "./pages/ExtractionDefinition";
import { RootLayout } from "./pages/RootLayout";

const queryClient = new QueryClient();

const router = createHashRouter([
  {
    element: <RootLayout />,
    children: [
      {
        path: "/",
        element: <PageExtraction />,
      },
      {
        path: "/form",
        element: <ExtractionDefinition />,
      },
    ],
  },
]);

function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <RouterProvider router={router} />
    </QueryClientProvider>
  );
}

export default App;
