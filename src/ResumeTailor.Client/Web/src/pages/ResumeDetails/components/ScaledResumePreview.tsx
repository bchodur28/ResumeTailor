import { useEffect, useRef, useState, type ReactNode } from "react";

type ScaledResumePreviewProps = {
  children: ReactNode;
  documentWidth?: number;
};

const ScaledResumePreview = ({
  children,
  documentWidth = 816,
}: ScaledResumePreviewProps) => {
  const containerRef = useRef<HTMLDivElement>(null);
  const contentRef = useRef<HTMLDivElement>(null);

  const [scale, setScale] = useState(1);
  const [height, setHeight] = useState(0);

  useEffect(() => {
    const container = containerRef.current;
    const content = contentRef.current;

    if (!container || !content) return;

    const updateSize = () => {
      const availableWidth = container.clientWidth;

      const nextScale = Math.min(1, availableWidth / documentWidth);

      setScale(nextScale);
      setHeight(content.offsetHeight * nextScale);
    };

    const observer = new ResizeObserver(updateSize);

    observer.observe(container);
    observer.observe(content);

    updateSize();

    return () => observer.disconnect();
  }, [documentWidth]);

  return (
    <div ref={containerRef} className="w-full min-w-0">
      <div
        className="relative mx-auto"
        style={{
          width: documentWidth * scale,
          height,
        }}
      >
        <div
          ref={contentRef}
          style={{
            width: documentWidth,
            transform: `scale(${scale})`,
            transformOrigin: "top left",
          }}
        >
          {children}
        </div>
      </div>
    </div>
  );
};

export default ScaledResumePreview;
