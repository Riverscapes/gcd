// Import this component in MDX files using:
// import YouTube from "@site/docs/src/components/YouTube";

// Usage:
// <YouTube embedId="VIDEO_ID" title="Descriptive Title" />

import React from "react";

export default function YouTube({ embedId, title }) {
  return (
    <iframe
      width="560"
      height="315"
      src={`https://www.youtube.com/embed/${embedId}`}
      frameBorder="0"
      allow="encrypted-media"
      allowFullScreen
      title={title}
    />
  );
}