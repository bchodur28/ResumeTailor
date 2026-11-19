import { Outlet } from "react-router-dom";

const RootLayout = () => {
  return (
    <main className="mx-auto my-6 w-10/12 max-[1800px]:w-11/12">
      <Outlet />
    </main>
  );
};

export default RootLayout;
