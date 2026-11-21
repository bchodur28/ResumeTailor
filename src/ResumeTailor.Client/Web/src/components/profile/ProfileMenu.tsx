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
        <div className="flex flex-col gap-2">
          <div className="min-w-0">
            <p className="truncate font-bold text-gray-900">
              {account.displayName}
            </p>

            <p className="truncate text-sm font-semibold text-gray-900">
              {account.titles.find((title) => title.isPrimary)?.value}
            </p>

            <p className="truncate text-sm text-gray-600">{account.email}</p>

            {account.location && (
              <p className="text-sm text-gray-600">{account.location}</p>
            )}
          </div>
          <div>
            <p className="text-sm text-gray-500 font-semibold">
              Available Credits:{" "}
              <span className="font-bold text-gray-600">
                {account.creditWallet?.availableCredits ?? 0}
              </span>
            </p>
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
