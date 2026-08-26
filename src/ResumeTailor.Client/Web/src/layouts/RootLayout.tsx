import { NavLink, Outlet } from "react-router-dom";

const RootLayout = () => {
  return (
    <div>
      <header className="root-nav">
        <div className="flex justify-between w-9/12 mx-auto">
          <div>
            <h1 className="primary-color text-2xl font-bold">Resumade</h1>
          </div>
          <nav className="flex gap-1.5">
            <NavLink className="nav-btn" to="/">
              Generate
            </NavLink>
            <NavLink className="nav-btn" to="/track">
              Track
            </NavLink>
            <NavLink className="nav-btn" to="/manage">
              Manage
            </NavLink>
          </nav>
        </div>
      </header>
      <main className="w-9/12 mx-auto bg-white my-6">
        <Outlet />
      </main>
    </div>
  );
};

export default RootLayout;
