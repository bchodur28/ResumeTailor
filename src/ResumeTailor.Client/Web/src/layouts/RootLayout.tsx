import { CardList, Openai, Pen, PersonCircle } from "react-bootstrap-icons";
import ProfileMenu from "../components/profile/ProfileMenu";
import { NavLink, Outlet } from "react-router-dom";
import { useState } from "react";
import PopupModel from "../components/profile/PopupModel";
import EditAccountForm from "../components/profile/EditAccountForm";
import EditEducationForm from "../components/profile/EditEducationForm";
import EditCompaniesForm from "../components/profile/EditCompaniesForm";
import EditProjectsForm from "../components/profile/EditProjectsForm";

const RootLayout = () => {
  const [isProfileOpen, setIsProfileOpen] = useState(false);
  const [isEditProfileOpen, setIsEditProfileOpen] = useState(false);
  const [isManageEducationOpen, setIsManageEducationOpen] = useState(false);
  const [isManageCompaniesOpen, setIsManageCompaniesOpen] = useState(false);
  const [isManageProjectsOpen, setIsManageProjectsOpen] = useState(false);

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
    <div>
      <header className="root-nav box-shadow">
        <div className="flex justify-between w-9/12 mx-auto">
          <div>
            <h1 className="primary-color text-2xl font-bold">Resumade</h1>
          </div>
          <nav className="relative flex gap-4">
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
        </div>
      </header>
      <main className="w-9/12 mx-auto my-6">
        <Outlet />
      </main>
    </div>
  );
};

export default RootLayout;
