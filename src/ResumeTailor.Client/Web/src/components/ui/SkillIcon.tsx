import { useEffect, useState } from "react";

const iconDefenitions = {
  CSharp: {
    fileName: "c-sharp",
    label: "C#",
    showLabel: false,
    attribution: {
      url: "https://www.flaticon.com/free-icons/c-sharp",
      title: "C sharp icons created by Magnific - Flaticon",
    },
  },
  C: {
    fileName: "c-",
    label: "C",
    showLabel: false,
    attribution: {
      url: "https://www.flaticon.com/free-icons/coding",
      title: "Coding icons created by Magnific - Flaticon",
    },
  },
  JavaScript: {
    fileName: "js",
    label: "JavaScript",
    showLabel: false,
    attribution: {
      url: "https://www.flaticon.com/free-icons/coding",
      title: "Javascript icons created by Magnific - Flaticon",
    },
  },
  React: {
    fileName: "programing",
    label: "React",
    showLabel: true,
    attribution: {
      url: "https://www.flaticon.com/free-icons/react",
      title: "React icons created by pocike - Flaticon",
    },
  },
  Angular: {
    fileName: "angular",
    label: "Angular",
    showLabel: true,
    attribution: {
      url: "https://www.flaticon.com/free-icons/angular",
      title: "Angular icons created by pocike - Flaticon",
    },
  },
  Net: {
    fileName: "net-microsoft",
    label: ".NET",
    showLabel: false,
    attribution: {
      url: "https://www.flaticon.com/free-icons/framework",
      title: "Framework icons created by orvipixel - Flaticon",
    },
  },
  SqlServer: {
    fileName: "sql-server",
    label: "SQL Server",
    showLabel: false,
    attribution: {
      url: "https://www.flaticon.com/free-icons/sql",
      title: "Sql icons created by juicy_fish - Flaticon",
    },
  },
} as const;

export type IconName = keyof typeof iconDefenitions;

type IconProps = {
  name: IconName;
  className?: string;
};

const iconModule = import.meta.glob("../../assets/icons/*.png", {
  query: "?url",
  import: "default",
});

const SkillIcon = ({ name, className }: IconProps) => {
  const [src, setSrc] = useState<string>();

  const icon = iconDefenitions[name];

  useEffect(() => {
    const path = `../../assets/icons/${icon.fileName}.png`;

    const loadImage = iconModule[path] as (() => Promise<string>) | undefined;
    if (!loadImage) {
      console.error(`Icon not found: ${path}`);
      return;
    }

    loadImage().then(setSrc);
  }, [name, icon.fileName]);

  if (!src) {
    return null;
  }

  return (
    <span className="relative inline-flex group">
      <div className="flex items-end border border-gray-300 p-2 rounded-2xl gap-1">
        <img src={src} className={`w-8 h-8 ${className}`} alt={icon.label} />
        {icon.showLabel && (
          <p className="text-center text-xs font-bold mt-1">{icon.label}</p>
        )}
      </div>
      <span
        className="
        absolute
        bottom-full
        left-1/2
        -translate-x-1/2
        pb-2
        hidden
        group-hover:block
        group-focus-within:block
        z-50
      "
      >
        <span
          className="
          flex
          flex-col
          whitespace-nowrap
          rounded-md
          bg-gray-900
          px-3
          py-2
          text-sm
          text-white
          shadow-lg
        "
        >
          <span className="font-semibold">{icon.label}</span>

          <a
            href={icon.attribution.url}
            target="_blank"
            rel="noopener noreferrer"
            className="text-xs underline"
          >
            Icon By: {icon.attribution.title}
          </a>
        </span>
      </span>
    </span>
  );
};

export default SkillIcon;
