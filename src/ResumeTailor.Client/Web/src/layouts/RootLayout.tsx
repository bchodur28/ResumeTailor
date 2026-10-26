import {
  Book,
  CardList,
  List,
  Openai,
  Pen,
  PersonCircle,
  X,
} from "react-bootstrap-icons";
import ProfileMenu from "../components/profile/ProfileMenu";
import { NavLink, Outlet } from "react-router-dom";
import { useState } from "react";
import PopupModel from "../components/profile/PopupModel";
import EditAccountForm from "../components/profile/EditAccountForm";
import EditEducationForm from "../components/profile/EditEducationForm";
import EditCompaniesForm from "../components/profile/EditCompaniesForm";
import EditProjectsForm from "../components/profile/EditProjectsForm";
import DropdownButton from "../components/ui/buttons/DropdownButton";

const RootLayout = () => {
  const [isProfileOpen, setIsProfileOpen] = useState(false);
  const [isEditProfileOpen, setIsEditProfileOpen] = useState(false);
  const [isManageEducationOpen, setIsManageEducationOpen] = useState(false);
  const [isManageCompaniesOpen, setIsManageCompaniesOpen] = useState(false);
  const [isManageProjectsOpen, setIsManageProjectsOpen] = useState(false);
  const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false);

  const handleEditProfile = () => {
    setIsProfileOpen(false);
    setIsEditProfileOpen(true);
  };

  const handleManageEducation = () => {
    setIsProfileOpen(false);
    setIsManageEducationOpen(true);
  };

  const handleManageCompanies = () => {
    setIsProfileOpen(false);
    setIsManageCompaniesOpen(true);
  };

  const handleManageProjects = () => {
    setIsProfileOpen(false);
    setIsManageProjectsOpen(true);
  };

  return (
    <div className="w-full flex flex-col">
      <header className="root-nav box-shadow relative z-50">
        <div className="flex items-center justify-between mx-auto px-4 lg:px-0 w-10/12 max-[1800px]:w-11/12">
          <h1 className="primary-color text-2xl font-bold">Resumade</h1>

          {/* Desktop navigation */}
          <nav className="hidden lg:flex relative gap-4">
            <NavLink className="nav-btn" to="/generate">
              <Openai size={24} />
              Generate
            </NavLink>
            <NavLink className="nav-btn" to="/">
              <CardList size={24} />
              Tracker
            </NavLink>
            <NavLink className="nav-btn" to="/manage">
              <Pen size={24} />
              Bullets
            </NavLink>
            <NavLink className="nav-btn" to="/library">
              <Book size={24} />
              Library
            </NavLink>
            <button
              onClick={() => setIsProfileOpen(!isProfileOpen)}
              className="profile-btn"
            >
              <PersonCircle size={24} />
              Me
            </button>
            {isProfileOpen && (
              <ProfileMenu
                onEditProfile={handleEditProfile}
                onManageEducation={handleManageEducation}
                onManageCompanies={handleManageCompanies}
                onManageProjects={handleManageProjects}
              />
            )}
            {isEditProfileOpen && (
              <PopupModel
                title="Edit Account"
                onClose={() => setIsEditProfileOpen(false)}
              >
                <EditAccountForm />
              </PopupModel>
            )}
            {isManageEducationOpen && (
              <PopupModel
                title="Manage Education"
                onClose={() => setIsManageEducationOpen(false)}
              >
                <EditEducationForm />
              </PopupModel>
            )}
            {isManageCompaniesOpen && (
              <PopupModel
                title="Manage Companies"
                onClose={() => setIsManageCompaniesOpen(false)}
              >
                <EditCompaniesForm />
              </PopupModel>
            )}
            {isManageProjectsOpen && (
              <PopupModel
                title="Manage Projects"
                onClose={() => setIsManageProjectsOpen(false)}
              >
                <EditProjectsForm />
              </PopupModel>
            )}
          </nav>

          {/* Mobile toggle */}
          <button
            type="button"
            className="lg:hidden p-2 rounded-lg hover:bg-gray-100"
            onClick={() => setIsMobileMenuOpen(!isMobileMenuOpen)}
            aria-label={isMobileMenuOpen ? "Close menu" : "Open menu"}
            aria-expanded={isMobileMenuOpen}
            aria-controls="mobile-navigation"
          >
            {isMobileMenuOpen ? <X size={26} /> : <List size={26} />}
          </button>
        </div>

        {/* Mobile dropdown */}
        {isMobileMenuOpen && (
          <nav
            id="mobile-navigation"
            aria-label="Mobile navigation"
            className="lg:hidden absolute top-full left-0 w-full
                 bg-white border-t border-gray-200
                 shadow-xl rounded-b-xl p-3"
          >
            <p
              className="text-xs font-semibold text-gray-400
                    uppercase tracking-wider px-3 py-2"
            >
              Navigation
            </p>

            {[
              { to: "/generate", label: "Generate", Icon: Openai },
              { to: "/", label: "Tracker", Icon: CardList },
              { to: "/manage", label: "Bullets", Icon: Pen },
              { to: "/library", label: "Library", Icon: Book },
            ].map(({ to, label, Icon }) => (
              <NavLink
                key={to}
                to={to}
                end={to === "/"}
                onClick={() => setIsMobileMenuOpen(false)}
                className={({ isActive }) =>
                  `flex items-center gap-3 px-4 py-3 rounded-lg
             text-sm font-medium transition-colors
             ${
               isActive
                 ? "bg-blue-50 text-blue-700"
                 : "text-gray-700 hover:bg-gray-100"
             }`
                }
              >
                <Icon size={20} />
                {label}
              </NavLink>
            ))}
          </nav>
        )}
      </header>
      <main className="mx-auto my-6 w-10/12 max-[1800px]:w-11/12">
        <Outlet />
      </main>
    </div>
  );
};

export default RootLayout;
