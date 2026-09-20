import { useAccount } from "../../contexts/AccountContext";
import { states } from "../../data/states";

type ProfileMenuProps = {
  onEditProfile: () => void;
  onManageEducation: () => void;
  onManageCompanies: () => void;
  onManageProjects: () => void;
};

const ProfileMenu = ({
  onEditProfile,
  onManageEducation,
  onManageCompanies,
  onManageProjects,
}: ProfileMenuProps) => {
  const { account } = useAccount();
  if (!account) return null;

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

            {account.city && account.state && (
              <div className="text-sm text-gray-600">
                {account.city},{" "}
                {states.find((state) => state.name === account.state)?.code}
              </div>
            )}
          </div>
        </div>
      </div>

      <div className="border-t border-gray-200 py-2">
        <button
          type="button"
          onClick={onEditProfile}
          className="w-full px-4 py-2 text-left text-sm text-gray-700 hover:bg-gray-100"
        >
          Edit profile
        </button>

        <button
          type="button"
          onClick={onManageEducation}
          className="w-full px-4 py-2 text-left text-sm text-gray-700 hover:bg-gray-100"
        >
          Manage education
        </button>
        <button
          type="button"
          onClick={onManageCompanies}
          className="w-full px-4 py-2 text-left text-sm text-gray-700 hover:bg-gray-100"
        >
          Manage companies
        </button>
        <button
          type="button"
          onClick={onManageProjects}
          className="w-full px-4 py-2 text-left text-sm text-gray-700 hover:bg-gray-100"
        >
          Manage projects
        </button>
      </div>

      <div className="border-t border-gray-200 py-2">
        <button
          type="button"
          className="w-full px-4 py-2 text-left text-sm text-gray-700 hover:bg-gray-100"
        >
          Sign out
        </button>
      </div>
    </div>
  );
};

export default ProfileMenu;
