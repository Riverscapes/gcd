/**
 * GoogleSlides component for embedding Google Slides presentations in MDX/Docusaurus.
 * Import this component in MDX files using:
         import GoogleSlides from "@site/docs/src/components/GoogleSlides";
 * Usage: <GoogleSlides src="..." width={960} height={749} />
 */

import React from 'react'

export default function GoogleSlides({ src, width = 960, height = 749 }) {
  return (
    <div
      style={{
        position: 'relative',
        paddingBottom: `${(height / width) * 100}%`,
        height: 0,
        overflow: 'hidden',
        maxWidth: width,
      }}
    >
      <iframe
        src={src}
        width={width}
        height={height}
        allowFullScreen
        style={{ position: 'absolute', top: 0, left: 0, width: '100%', height: '100%' }}
        title="Google Slides Presentation"
      />
    </div>
  )
}
