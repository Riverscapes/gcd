import React from "react";

/**
 * GoogleSlides component for embedding Google Slides presentations in MDX/Docusaurus.
 * Usage: <GoogleSlides src="..." width={960} height={749} />
 */
export default function GoogleSlides({ src, width = 960, height = 749 }) {
  return (
    <div style={{ position: "relative", paddingBottom: `${(height / width) * 100}%`, height: 0, overflow: "hidden", maxWidth: width }}>
      <iframe
        src={src}
        width={width}
        height={height}
        frameBorder="0"
        allowFullScreen
        style={{ position: "absolute", top: 0, left: 0, width: "100%", height: "100%" }}
        title="Google Slides Presentation"
      />
    </div>
  );
}
