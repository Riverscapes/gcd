---
title: DEM Surveys
weight: 1
---

<div class="float-right">
<img src="{{ site.baseurl }}/assets/images/datasets/feshie_200h.png">
</div>
DEM Surveys are the fundamental building block of a GCD project. A GCD project can have multiple DEM surveys, each represented by a single raster file. Typically the surveys within a GCD project capture the same general location, although each raster can have slightly different extents. All rasters within a GCD project must share some specific properties:

* identical spatial reference (map projection)
* the same cell resolution
* be [divisble]() but not necessarily concurrent
* possess the same vertical units

The GCD project maintains a list of all DEM surveys within a project that is displayed under the Inputs node within the project explorer. Right clicking on the DEM Surveys node reveals a context menu for adding new DEM surveys to the project and adding all the existing DEM surveys to the current ArcMap document (GCD AddIn only).

![DEM Surveys]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/dem-surveys/dem-surveys.png)

# Add Existing DEM Survey
