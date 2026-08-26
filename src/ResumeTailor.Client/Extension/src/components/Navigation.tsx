import { NavLink } from "react-router-dom";

const Navigation = () => {
  return (
    <nav className="nav">
      <NavLink
        to="/"
        className={({ isActive }) =>
          isActive ? "nav-button active" : "nav-button"
        }
      >
        Extraction
      </NavLink>
      <NavLink
        to="/form"
        className={({ isActive }) =>
          isActive ? "nav-button active" : "nav-button"
        }
      >
        Extraction Form
      </NavLink>
    </nav>
  );
};

export default Navigation;
