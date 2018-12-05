---
title: Areas Of Interest
---

<div class="float-right">
<img src="{{ site.baseurl }} /assets/images/CommandRefs/00_ProjectExplorer/inputs/masks/aoi/aoi.png"></div>
Area of Interest (AOI) allow you to control the regions within your data that are considered during [change detection]() analyses. This is useful if your data have a larger extent than the area of change that you are particularly interested in. This is common in river channels where you might want to ignore out-of-channel changes and focus on specific areas within the channel itself. Alternatively you might have [LiDAR]() data in which you want to ignore the vegetation and focus instead on real topographic change.

The same The same basic requirements apply to AOIs that apply to all types of mask Polygon ShapeFiles:

* Must use the same spatial reference as the DEM Surveys in the project.
* Must contain at least one feature.
* Must not contain any multi-part features.
* Must not contain any features with null geometries.

Note that all polygons within the ShapeFile are considered part of the same, single AOI. You do not need to dissolve or merge together separate polygons. Any cells within any polygon feature are considered within of the AOI.

# Add Existing AOI

To use an existing polygon ShapeFile as an AOI right click on the Masks node of the GCD Project Explorer and choose **Add Existing Area of Interest**.

![cms]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/masks/aoi/aoi_add_cms.png)

Provide a unique name for the AOI and then click the plus icon to browse and select the polygon ShapeFile that you want to use. The project path will be assigned automatically. Click Create to copy the ShapeFile into the GCD project folder structure and save the details to the project itself.

![aoi details]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/masks/aoi/aoi_details.png)

# Context Menu

Right clicking on any AOI brings up the context menu that allows you to perform the three operations described below. Note that the Add To Map option is only available in the ArcGIS Addin version of GCD and not the Standalone.

![mask cms]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/masks/aoi/aoi_cms.png)

# Edit Properties

Editing the properties of an AOI it is possible to change the name.

# Add To Map

AOIs are added to the current ArcMap document with a red outline and a transparent fill symbology.

# Delete

Deleting an AOI removes it from the GCD project and permanently deletes the underlying ShpeFile within the GCD project. Note that you cannot delete a particular AOI until all [change detections]() that refer to it have also been deleted.

![Delete cms]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/masks/aoi/aoi_inuse.png)
