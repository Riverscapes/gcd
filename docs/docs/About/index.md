---
title: About GCD Software
sidebar_position: 300
---

GCD 7 is available as three separate products:

- **GCD Standalone** is independent of ESRI and does not require ArcGIS. It has no built-in map display, but is otherwise fully functional.
- **GCD AddIn for ArcGIS Pro** for read only display of GCD project data in ArcGIS Pro. 
- **GCD AddIn for ArcGIS 10.x** fully functional GCD version that includes all of Standalone plus map display.

## GCD Standalone

The [latest GCD release](https://github.com/Riverscapes/gcd/releases/latest) for ArcGIS Pro version 3.4 and higher. This is a limited version capable of displaying spatial GCD data in ArcGIS Pro. This version is read only and does not create or alter GCD projects.

GCD 7 also has a legacy version for ArcGIS 10.4 and higher. This is a fully operational version of GCD that combines both map display as well as full functionality to create and manipulate GCD projects.



### Prerequisites

The GCD Standalone version requires [.NET Framework 4.5.2](https://www.microsoft.com/en-ca/download/details.aspx?id=42642) (that should already included with Windows 10).

The GCD AddIn for ArcGIS Pro requires .NET Framework 8 as well as ArcGIS Pro 3.4 or higher.


## Other Notes

- **Incompatibility**: GCD 6 and 7 projects cannot be opened across versions.
- **Running Both Versions**: AddIn and Standalone can coexist. The only shared resource is the FIS Library. You can use both on the same `*.gcd` file. [Details](https://github.com/Riverscapes/gcd/issues/196)

## Tutorials & Extensions

To build surfaces in ArcGIS, you need **Spatial Analyst** and **3D Analyst** extensions enabled. You do not need these extensions, or indeed any GIS at all, to use the GCD standalone. The standalone performs all it's own geoprocessing internally.

<YouTubeEmbed videoId="JgBlCnGco9M"/>
