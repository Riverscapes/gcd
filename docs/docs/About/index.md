---
title: About GCD Software
sidebar_position: 300
---

GCD 7 is available as two separate products:

- **GCD AddIn** (for users with ArcGIS) includes map display integration with ArcMap.
- **GCD Standalone** is independent of ESRI and does not require ArcGIS. It has no built-in map display, but is otherwise fully functional.

---

## GCD AddIn for ArcGIS

The following table shows compatibility of GCD versions with ArcGIS:

| GCD Version | 10.1 | 10.2 | 10.3 | 10.4 | 10.5 | 10.6 | ArcPro |
|-------------|------|------|------|------|------|------|--------|
| GCD 6       | ✅   | ✅   | ✅   | ✅   | ✅   | ✅   | ❌     |
| GCD 7       | ❌   | ❌   | ❌   | ✅   | ✅   | ✅   | ❌     |

### Free ArcGIS for Students & Researchers

Most academic institutions offer ArcGIS licenses. ESRI also provides [free one-year licenses](http://www.esri.com/landing-pages/software/arcgis/arcgis-desktop-student-trial) for students and researchers.

- [Installation Guide](http://gis.joewheaton.org/about/software/arcgis_install)<br />
- [Latest Service Pack](http://resources.arcgis.com/content/patches-and-service-packs?fa=viewPatch&PID=66&MetaID=1843)<br />
- [More on student licenses](http://www.joewheaton.org/Home/students-teaching/teaching-materials/gis-help/arcgis-desktop-10---education-edition)

---

## GCD Standalone

The **GCD Standalone** version lets you:

- Create projects
- Specify DEM surveys
- Perform change detection

> It does **not** include a map display. Ideal for those using QGIS or other GIS viewers.

### Key Features

- 64-bit application (faster than 32-bit AddIn)
- No ArcGIS required
- Uses **ClickOnce** technology:  
  → Installs to your profile folder  
  → **No admin rights** needed  
  → Follow prompts carefully when downloading `.exe`

### Prerequisites

Both AddIn and Standalone versions require:

- [.NET Framework 4.5.2](https://www.microsoft.com/en-ca/download/details.aspx?id=42642)

> Note: Already included with Windows 10.

---

## Other Notes

- **Incompatibility**: GCD 6 and 7 projects cannot be opened across versions.
- **Running Both Versions**: AddIn and Standalone can coexist. Only shared resource is the FIS Library. You can use both on the same `*.gcd` file. [Details](https://github.com/Riverscapes/gcd/issues/196)

---

## Tutorials & Extensions

To build surfaces in ArcGIS, you need **Spatial Analyst** and **3D Analyst** extensions enabled.

- [How to Enable Extensions](http://gis.joewheaton.org/assignments/labs/lab01/getting-organized-and-oriented/arcgis-10-desktop-orientation/about-the-software#TOC-Using-Extensions)<br />
- [Building DEMs Tutorial](/Tutorials/Building_DEMs/building-dems)

[Watch video](https://www.youtube.com/embed/JgBlCnGco9M?rel=0)
