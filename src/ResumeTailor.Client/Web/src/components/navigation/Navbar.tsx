import { useAuth0 } from "@auth0/auth0-react";
import {
  Book,
  CardList,
  List,
  Openai,
  Pen,
  PersonCircle,
  X,
} from "react-bootstrap-icons";
import { Link, NavLink } from "react-router-dom";
import { useState } from "react";
import ProfileMenu from "../profile/ProfileMenu";
import { useAccount } from "../../contexts/AccountContext";

const navItems = [
  { to: "/app/generate", label: "Generate", Icon: Openai },
  { to: "/app", label: "Tracker", Icon: CardList },
  { to: "/app/library", label: "Library", Icon: Book },
];

const Navbar = () => {
  const { isAuthenticated, isLoading, loginWithRedirect } = useAuth0();
  const { account } = useAccount();

  const [isProfileOpen, setIsProfileOpen] = useState(false);
  const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false);

  // Account setup must be completed before showing app navigation.
  const showAppNav = isAuthenticated && !!account;

  const getNavClass = ({ isActive }: { isActive: boolean }) =>
    `nav-btn ${isActive ? "nav-btn--active" : ""}`;

  const handleLogin = (signup = false) => {
    loginWithRedirect({
      appState: { returnTo: "/app" },
      ...(signup && {
        authorizationParams: {
          screen_hint: "signup",
        },
      }),
    });
  };

  return (
    <header className="root-nav box-shadow relative z-50">
      <div className="flex items-center justify-between mx-auto px-4 lg:px-0 w-10/12 max-[1800px]:w-11/12">
        <Link to="/" className="primary-color text-2xl font-bold">
          Resumade
        </Link>

        {!isLoading && (
          <>
            {showAppNav ? (
              <>
                {/* Desktop navigation */}
                <nav className="hidden lg:flex relative gap-4">
                  {navItems.map(({ to, label, Icon }) => (
                    <NavLink
                      key={to}
                      to={to}
                      end={to === "/app"}
                      className={getNavClass}
                    >
                      <Icon size={24} />
                      {label}
                    </NavLink>
                  ))}

                  <button
                    type="button"
                    onClick={() => setIsProfileOpen(!isProfileOpen)}
                    className="profile-btn"
                  >
                    <PersonCircle size={24} />
                    Me
                  </button>

                  {isProfileOpen && <ProfileMenu />}
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
              </>
            ) : isAuthenticated ? (
              // Authenticated but account setup is incomplete.
              <NavLink to="/account/setup" className="nav-btn">
                Complete Setup
              </NavLink>
            ) : (
              /* Logged-out navigation */
              <nav className="flex items-center gap-4">
                <button
                  type="button"
                  onClick={() => handleLogin()}
                  className="nav-btn"
                >
                  Log In
                </button>

                <button
                  type="button"
                  onClick={() => handleLogin(true)}
                  className="nav-btn nav-btn--active"
                >
                  Sign Up
                </button>
              </nav>
            )}
          </>
        )}
      </div>

      {/* Mobile navigation */}
      {showAppNav && isMobileMenuOpen && (
        <nav
          id="mobile-navigation"
          aria-label="Mobile navigation"
          className="lg:hidden absolute top-full left-0 w-full
            bg-white border-t border-gray-200
            shadow-xl rounded-b-xl p-3"
        >
          <p className="text-xs font-semibold text-gray-400 uppercase tracking-wider px-3 py-2">
            Navigation
          </p>

          {navItems.map(({ to, label, Icon }) => (
            <NavLink
              key={to}
              to={to}
              end={to === "/app"}
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

          <div className="border-t border-gray-200 mt-2 pt-2">
            <NavLink
              to="/app/profile"
              onClick={() => setIsMobileMenuOpen(false)}
              className="flex items-center gap-3 px-4 py-3 rounded-lg text-sm font-medium text-gray-700 hover:bg-gray-100"
            >
              <PersonCircle size={20} />
              Profile
            </NavLink>
          </div>
        </nav>
      )}
    </header>
  );
};

export default Navbar;
