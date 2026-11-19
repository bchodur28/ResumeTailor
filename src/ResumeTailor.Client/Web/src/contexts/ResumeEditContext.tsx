import {
  createContext,
  useContext,
  useEffect,
  useState,
  type ReactNode,
} from "react";

type ResumeEditContextValue = {
  activeEditorId: string | null;
  toggleEditor: (editorId: string) => void;
  closeEditor: () => void;
};

const ResumeEditContext = createContext<ResumeEditContextValue | null>(null);

type ResumeEditProviderProps = {
  children: ReactNode;
};

export const ResumeEditProvider = ({ children }: ResumeEditProviderProps) => {
  const [activeEditorId, setActiveEditorId] = useState<string | null>(null);

  const toggleEditor = (editorId: string) => {
    setActiveEditorId((current) => (current === editorId ? null : editorId));
  };

  const closeEditor = () => {
    setActiveEditorId(null);
  };

  useEffect(() => {
    if (activeEditorId === null) return;

    const handleClickOutside = (event: PointerEvent) => {
      const activeEditor = document.querySelector<HTMLElement>(
        '[data-resume-editor-active="true"]',
      );

      if (
        activeEditor &&
        event.target instanceof Node &&
        !activeEditor.contains(event.target)
      ) {
        closeEditor();
      }
    };

    document.addEventListener("pointerdown", handleClickOutside);

    return () => {
      document.removeEventListener("pointerdown", handleClickOutside);
    };
  }, [activeEditorId]);

  return (
    <ResumeEditContext.Provider
      value={{
        activeEditorId,
        toggleEditor,
        closeEditor,
      }}
    >
      {children}
    </ResumeEditContext.Provider>
  );
};

export const useResumeEdit = () => {
  const context = useContext(ResumeEditContext);

  if (!context) {
    throw new Error("useResumeEdit must be used inside ResumeEditProvider");
  }

  return context;
};
