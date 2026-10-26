import { Outlet } from "react-router-dom";
import { NavLink } from "react-router-dom";
import { Book, CardHeading, CardList } from "react-bootstrap-icons";

const LibraryLayout = () => {
  return (
    <div className="w-full flex flex-col items-center gap-4">
      <div className="flex gap-2">
        <NavLink className="nav-btn" to="/library">
          <CardHeading size={24} />
          <p className="max-[640px]:text-sm font-semibold">Header</p>
        </NavLink>
        <NavLink className="nav-btn" to="/library/education">
          <Book size={24} />
          <p className="max-[640px]:text-sm font-semibold">Education</p>
        </NavLink>
        <NavLink className="nav-btn" to="/library/experience">
          <CardList size={24} />
          <p className="max-[640px]:text-sm font-semibold">Experience</p>
        </NavLink>
      </div>
      <div className="flex flex-col justify-center items-center gap-4 max-[640px]:w-full">
        <Outlet />
      </div>
    </div>
  );
};

export default LibraryLayout;
