import { Outlet } from "react-router-dom";
import Navbar from "../components/navigation/Navbar";

const SiteLayout = () => {
  return (
    <div className="min-h-screen flex flex-col">
      <Navbar />

      <main className="flex-1">
        <Outlet />
      </main>

      <footer className="border-t border-gray-200 p-6 text-center text-sm text-gray-500">
        © {new Date().getFullYear()} Resumade
      </footer>
    </div>
  );
};

export default SiteLayout;
