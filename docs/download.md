---
title: Download
---

The GCD 7 software is available in two different versions. The GCD Addin for ArcGIS provides full GCD functionality, including map display of GCD inputs and outputs. If you have ArcGIS 10.0 or newer (not ArcPro) then this version will give you 'Add to Map' and spatial visualization functionality embedded within ArcGIS. The downside is ArcGIS is a 32 Bit Application and can only use 4 GB of RAM, so it is slower. 

GCD Standalone is a desktop software version that performs all the core GCD functionality (building projects and perfoming change detection analyses etc) but does not include any map display. If you don't possess ArcGIS, or you want to use an alternative map display tool, such as [QGIS](https://www.qgis.org/en/site), then we recommend this version. It also works as a nice companion to the Add-In and you can go back and forth between the Add-In and Stand-Alone seamlessly. Note that GCD performs all spatial operations using the free and open source [GDAL](http://www.gdal.org/) library. Therefore you do not need ArcGIS to use GCD Standalone.

<div class="row medium-up-2 small-up-1">

<div class="column column-block">
  <img src="{{ site.baseurl }}/assets/images/gcd_addin.png" style="height:200px">
    <div>
    	<a class="button large fa fa-cloud-download expanded" href="http://releases.northarrowresearch.com/GCD/Desktop/2018_01_23_GCDAddIn_7_0_02.esriAddIn">&nbsp;&nbsp;GCD AddIn for ArcGIS 10.4</a>
    </div>  
</div>

<div class="column column-block">
	<div>
  <img src="{{ site.baseurl }}/assets/images/gcd_standalone.png" style="height:200px">
    <div>
    	<a class="button large fa fa-cloud-download expanded" href="https://github.com/Riverscapes/gcd/releases/tag/v1.0.0_StandAlone">&nbsp;&nbsp;GCD Standalone</a>
    </div>
</div>

</div>

</div>

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
