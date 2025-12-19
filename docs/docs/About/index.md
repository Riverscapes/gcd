---
title: About GCD Software
sidebar_position: 300
---

GCD 7 is available as three separate products:

- **GCD Standalone** is independent of ESRI and does not require ArcGIS. It has no built-in map display, but is otherwise fully functional.
- **GCD AddIn for ArcGIS Pro** is a lightweight addin for ArcGIS Pro for viewing GCD 7 projects and data cartographically.
- **GCD AddIn for ArcGIS 10.x** includes map display integration with ArcMap.

## GCD Standalone

The GCD Standalone is a version lets you create projects, specify DEM surveys and perform change detection. It is a fully functional version of GCD, with the exception that it does not possess and map display. It has a rich user interface, packaged as a standalone Windows application.

The standalone is 64 bit and the fastest version of GCD!

![standalone](/img/gcd_standalone2.png)

## GCD Viewer AddIn for ArcGIS Pro

The GCD Viewer can display the contents GCD 7 projects and visualize data cartographically. This version is "readonly". It cannot perform GCD operations or change GCD projects in any way. It's sole purpose is to display GCD data in ArcGIS Pro with curated symbology and map table of contents.

The Viewer for ArcPro is the latest GCD product. The goal is to use it in conjunction with GCD standalone, running the two applications side-by-side. GCD operations can be performed in the standalone and then viewed cartographically in ArcPro (after refreshing the project there once operations are complete).

![GCD Viewer](/img/gcd_arcpro_viewer.png)

## GCD AddIn for ArcGIS 10.x


## ArcGIS Compatibility
The following table shows compatibility of GCD versions with ArcGIS:

| GCD Version | 10.1 | 10.2 | 10.3 | 10.4 | 10.5 | 10.6 | ArcPro |
|:------------|:----:|:----:|:----:|:----:|:----:|:----:|:------:|
| GCD 6       | ✅   | ✅   | ✅   | ✅   | ✅   | ✅   | ❌     |
| GCD 7 AddIn for ArcGIS 10.x       | ❌   | ❌   | ❌   | ✅   | ✅   | ✅   | ✅     |
| GCD 7 Viewer AddIn for ArcGIS Pro       | ❌   | ❌   | ❌   | ❌   | ❌   | ❌   | ✅     |



### Prerequisites

The Standalone and AddIn for ArcGIS 10.x versions require [.NET Framework 4.5.2](https://www.microsoft.com/en-ca/download/details.aspx?id=42642).

The GCD Viewer for ArcGIS Pro requires .Net Version 8.

## Other Notes

- **Incompatibility**: GCD 6 and 7 projects cannot be opened across versions.
- **Running Both Versions**: AddIn and Standalone can coexist. The only shared resource is the FIS Library. You can use both on the same `*.gcd` file. [Details](https://github.com/Riverscapes/gcd/issues/196)

## Tutorials & Extensions

To build surfaces in ArcGIS, you need **Spatial Analyst** and **3D Analyst** extensions enabled. You do not need these extensions, or indeed any GIS at all, to use the GCD standalone. The standalone performs all it's own geoprocessing internally.

<YouTubeEmbed videoId="JgBlCnGco9M"/>
