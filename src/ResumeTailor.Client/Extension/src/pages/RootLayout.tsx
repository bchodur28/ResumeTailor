import { Outlet } from "react-router-dom";
import Navigation from "../components/Navigation";

export const RootLayout = () => {
  return (
    <>
      <Navigation />
      <main>
        <Outlet />
      </main>
    </>
  );
};
