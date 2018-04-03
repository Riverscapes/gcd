---
title: About GCD Software
weight: 2
---

# Stand Alone vs. ArcGIS Add In

## GCD AddIn for ArcGIS

### AddIn Prerequisities

* ArcGIS 10.4 or newer version of ArcGIS 10.x. Unfortunately, the older versions (10.3 and lower) of ArcGIS are not supported for GCD 7.X. Note that GCD 6.0 will work on ArcGIS 10.0 - 10.5. Also note that in 10.4 and 10.5, both GCD 6.0 and GCD 7.0 can be installed concurrently. Currently GCD projects from 6.0 and 7.0 are not compatabile.  ArcPro is also **not** supported (yet).
* [Microsoft .Net Framework 4.5.2](https://www.microsoft.com/en-ca/download/details.aspx?id=42642). This should already be installed if you are running Windows 10. It's also required by ArcGIS 10.4.



## GCD Standalone

The GCD standalone software is a fully functional version of GCD that can be used to create projects, specify DEM surveys and perform change detections etc. The only difference is that the standalone does not possess any map display. Therefore, if you just want to build a GCD project quickly and don't care about seeing the data displayed in ArcGIS, or you want to use a different GIS package such as [QGIS](https://www.qgis.org/en/site) to visualize GIS datasets, then the standalone might be a good choice for you.

GCD standalone is available as a **64 bit** application only. It therefore should possess better performance than the AddIn version which is only available as a 32 bit version (because ArcGIS 10.x is only available as 32 bit).

GCD standalone is a deployed using Microsoft's ClickOnce technology. This allows the application to install itself in your user profile folder on your computer. You should **not** need administrator privileges to install it. That said, when you click on the above link to download the setup.exe installation you will need to read the prompts in your web browser carefully, since most browsers will warn you about downloading executable files.



### Standalone Prerequisites

* [Microsoft .Net Framework 4.5.2](https://www.microsoft.com/en-ca/download/details.aspx?id=42642). This should already be installed if you are running Windows 10. It's also required by ArcGIS 10.4.

## Running Both
The GCD Standalone and AddIn are independent of each other and installed in separate locations on your machine. Their only shared resource is the FIS Library, and they are able to both access the same `*.gcd` project simultaneously. It is not necessary or required that the Standalone and AddIn are both concurrent versions. For more information, see [here](https://github.com/Riverscapes/gcd/issues/196).

