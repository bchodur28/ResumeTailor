import { Fonts, Palette } from "react-bootstrap-icons";
import lineSpacingIcon from "../../assets/icons/line-spacing.png";
import fontSize from "../../assets/icons/font-size.png";
import jobDescription from "../../assets/icons/job-description.png";

const Toolbar = () => {
  return (
    <div className="flex w-fit gap-2 mt-4 border border-gray-300 rounded-2xl shadow">
      <button className="flex items-center gap-1 border border-white hover:border-gray-400 hover:shadow hover:bg-gray-100 py-1 px-2 rounded-2xl  text-xs font-semibold">
        <span>Main</span>
        <img src={fontSize} alt="icon" className="w-6 h-6" />
      </button>

      <button className="flex items-center gap-1 border border-white hover:border-gray-400 hover:shadow hover:bg-gray-100 py-1 px-2 rounded-2xl  text-xs font-semibold">
        <span>Headers</span>
        <img src={fontSize} alt="icon" className="w-6 h-6" />
      </button>

      <button className="border border-white hover:border-gray-400 hover:shadow hover:bg-gray-100 py-1 px-2 rounded-2xl text-xs font-semibold">
        <Palette className="w-6 h-6" />
      </button>
      <button className="border border-white hover:border-gray-400 hover:shadow hover:bg-gray-100 py-1 px-2 rounded-2xl text-xs font-semibold">
        <img src={lineSpacingIcon} alt="icon" className="w-6 h-6" />
      </button>
      <button className="border border-white hover:border-gray-400 hover:shadow hover:bg-gray-100 py-1 px-2 rounded-2xl text-xs font-semibold">
        Summary
      </button>
      <button className="flex items-center gap-1 border border-white hover:border-gray-400 hover:shadow hover:bg-gray-100 py-1 px-2 rounded-2xl  text-xs font-semibold">
        Projects
      </button>
    </div>
  );
};

export default Toolbar;
