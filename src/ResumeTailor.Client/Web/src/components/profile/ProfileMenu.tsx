import { useAuth0 } from "@auth0/auth0-react";
import { useAccount } from "../../contexts/AccountContext";

const ProfileMenu = () => {
  const { account } = useAccount();
  const { logout } = useAuth0();

  if (!account) return null;

  const handleLogout = () => {
    logout({
      logoutParams: {
        returnTo: window.location.origin,
      },
    });
  };

  return (
    <div
      className="
        absolute right-0 top-full z-50 mt-2
        w-80
        overflow-hidden
        rounded-xl
        border border-gray-200
        bg-white
        shadow-lg
      "
    >
      <div className="p-4">
        <div className="flex items-center gap-3">
          <div className="flex h-12 w-12 items-center justify-center rounded-full bg-gray-200 font-semibold text-gray-700">
            {account.displayName?.charAt(0)}
          </div>

          <div className="min-w-0">
            <div className="truncate font-bold text-gray-900">
              {account.displayName}
            </div>

            <div className="truncate text-sm font-semibold text-gray-900">
              {account.titles.find((title) => title.isPrimary)?.value}
            </div>

            <div className="truncate text-sm text-gray-600">
              {account.email}
            </div>

            {account.location && (
              <div className="text-sm text-gray-600">{account.location}</div>
            )}
          </div>
        </div>
      </div>

      <div className="border-t border-gray-200 py-2">
        <button
          type="button"
          className="w-full px-4 py-2 text-left text-sm text-gray-700 hover:bg-gray-100"
          onClick={handleLogout}
        >
          Sign out
        </button>
      </div>
    </div>
  );
};

export default ProfileMenu;
