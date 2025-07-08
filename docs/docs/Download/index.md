---
title: Download
sidebar_position: 200
---

The GCD 7 software is now available in two different flavors. 





<p align="center">
  
  ![GCD Addin](/img/gcd_addin.png)
  &nbsp;&nbsp;&nbsp;
  ![GCD Standalone](/img/gcd_standalone.png)

</p>





import { ToolsWrapper } from "../src/components/ToolsWrapper/ToolsWrapper";

<ToolsWrapper
  cards={[
    {
      title: "GCD 7 AddIn for ArcGIS 10.x",
      logoUrl: "/img/gcd_addin.png",
      toolUrl: "https://github.com/Riverscapes/gcd/releases/latest"
    },
    {
      title: "GCD 7 Standalone",
      logoUrl: "/img/gcd_standalone.png",
      toolUrl: "https://github.com/Riverscapes/gcd/releases/latest"
    }
  ]}
  cardsize="md"
/>

------


The [GCD Addin for ArcGIS](/Download/about.html#gcd-addin-for-arcgis) provides full GCD functionality, including map display of GCD inputs and outputs. If you have ArcGIS 10.4 or newer (not ArcPro) then this version will give you 'Add to Map' and spatial visualization functionality embedded within ArcGIS. The downside is ArcGIS is a 32 Bit Application and can only use 4 GB of RAM, so it is slower. 

[GCD Standalone](/Download/about.html#gcd-standalone) is a desktop software version that performs all the core GCD functionality (building projects and performing change detection analyses etc) but does not include any map display. If you don't possess ArcGIS, or you want to use an alternative map display tool, such as [QGIS](https://www.qgis.org/en/site), then we recommend this version. It also works as a nice companion to the Add-In and you can go back and forth between the Add-In and Stand-Alone seamlessly. Note that GCD performs all spatial operations using the free and open source [GDAL](https://www.gdal.org) library. Therefore you do not need ArcGIS to use GCD Standalone.

## Setup, Release Information & More

Use the navigation menu for help on [installation](/Download/install), [more information about the GCD software](/Download/about), [how to access the underlying source code](/Download/sourcecode) and more. 

