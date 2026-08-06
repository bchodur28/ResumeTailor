import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import "./App.css";
import { Extraction } from "./pages/Extraction";

const queryClient = new QueryClient();

function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <Extraction />
    </QueryClientProvider>
  );
}

export default App;
