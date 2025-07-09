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