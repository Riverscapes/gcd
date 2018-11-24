---
title: DEM Surveys
weight: 1
---

<div class="float-right">
<img src="{{ site.baseurl }}/assets/images/datasets/feshie_200h.png">
</div>
DEM Surveys are the fundamental building block of a GCD project. Project can contain multiple DEM surveys, each represented by a single raster file. Typically the surveys within a GCD project capture the same general location, although each raster can have slightly different extents. However all rasters within a GCD project **must** share some specific properties:

* identical spatial reference (map projection)
* the same cell resolution
* be [divisble]() but not necessarily concurrent
* possess the same vertical units

The GCD project maintains a list of all DEM surveys within a project. This list is displayed under the **Inputs** node within the [project explorer](). Right clicking on the DEM Surveys node reveals a context menu for adding new DEM surveys to the project and adding all the existing DEM surveys to the current ArcMap document (GCD AddIn only).

![DEM Surveys]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/dem-surveys/dem-surveys.png)

# Add Existing DEM Survey

Right click on the DEM Surveys node in the project explorer and choose **Add Existing DEM Survey**. You will be prompted to browse to an existing raster file on your computer. GCD is compatible with GeoTiff (`*.tiff`) and Erdas Imagine (`*.img`) raster formats. It is not compatible with any other formats including rasters stored in geodatabases, PostGIS etc. 

The raster import form is displayed after the raster is selected. The top half of this form displays the fundamental raster properties of the original raster that was selected. This includes the dimensions and cell size etc. If the original raster is not [divisible]() then the edge coordinates are highlighted in red.

The bottom half of the raster import form shows the same fundamental raster properties but for the raster that will be generated inside the GCD project. Since GCD rasters are always divisible these properties might be different from the original raster. If the edge coordinates of the GCD raster match those of the original raster then they are highlighted in green.

The GCD raster cell resolution and horizontal precision are only editable for the **first raster** imported into a GCD project. These inputs are fixed for subsequent rasters given that all rasters within a GCD project must share identical cell resolution (see above).

The interpolation method refers to how the GCD will process the original raster as it copies it into the GCD project. If the original raster is divisible and the output cell resolution matches that of the input, then a "straight cell-wise copy" will be used. This involves no resampling and even if the output edge coordinates are adjusted the output cell values will precisely match those of the original raster. However, if the original raster is not divisible or the cell resolution chages then a bilinear interpolation will be used to resample the original raster. See the [best practices] on why resampling should be avoided if at all possible.

Clicking **Add Raster** causes the form to validate all the inputs and copy the raster into the GCD Project, adding a new DEM Survey.

![Import]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/dem-surveys/import-dem.png)

This video walks you through the process:

<div class="responsive-embed">
<iframe width="560" height="315" src="https://www.youtube.com/embed/JMmf8xFgMug?rel=0" frameborder="0" allow="autoplay; encrypted-media" allowfullscreen></iframe>
</div>

# Context Menu

The DEM Survey context menu is accessed by right clicking on any DEM in the GCD project. The tools available on this menu are described below. Note that the Add DEM Survey To Map option is only available in the GCD AddIn version.

![edit]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/dem-surveys/dem-menu-edit.png)

# Edit Properties

Editing displays several basic properties for a DEM Survey. Only the name is editable and this must be unique across all DEM Surveys within the current GCD project. The project path shows the path to the underlying raster, relative within the GCD project path. The raster properties and statistics display the fundamental properties of the underlying raster file.

![edit]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/dem-surveys/dem-properties.png)

Click the settings cog button to change the survey date for a DEM Survey. A survey date is made up of separate date and time components. You must always set the value from largest to smallest unit. i.e. you cannot select a month without selecting a year. 

Selecting survey dates is optional but recommended because it helps the GCD understand the chronological sequence of DEMs within a project. This determines the order that DEMs are displayed in tools such as the [batch change detection]().

![edit]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/dem-surveys/survey-date.png)

# Add To Map

The **Add DEM Survey to Map** command adds the selected DEM to the map (GCD AddIn only). Note this command does not add associated surfaces or error surfaces. This is a convenient command if you want to add your symbolized DEM inputs one at a time to the current ArcMap document. 

All DEM Surveys within a GCD project can be added to the map with a single operation by right clicking on the DEM Surveys parent node in the project explorer (GCD ArcAddIn only).

<div class="responsive-embed">
<iframe width="560" height="315" src="https://www.youtube.com/embed/dAgkkCwc3kA?rel=0" frameborder="0" allow="autoplay; encrypted-media" allowfullscreen></iframe>
</div>

# Delete

The **Delete DEM Survey** command deletes a DEM Survey from the GCD Project. This does several things:

- Checks to see if there are any analyses depdendent on this DEM Survey. If there are, GCD will not allow you to delete it from map. If any change detection analyses have been performed already, you will get a warning that you need to delete those first. GCD works on the principle that any analysis should be transparent and reproducible in the context of its inputs. Therefore, GCD prevents you from deleting inputs that were used in any analysis. 

![DEMSurvey_InUse]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/dem-surveys/dem-in-use.png)

- Checks if there are any childern associated surfaces or error surfaces. If user proceeds, the GCD removes the DEM as well as any associated surfaces or error surfaces from the table of contents (GCD AddIn version only).
- After, it deletes the `Inputs\Surveys\DEM000n` folder associated with the selected DEM Survey from the project directory (note this does not impact where you loaded or copied the DEM survey from originally). This physcially deletes the files from your system.
- It then deletes all the project information about this DEM Survey from the GCD project.

# Sorting DEM Surveys

The DEM Surveys within a GCD project can be organized in the project explorer tree in several different ways by right clicking on the parent **DEM Surveys**.

![DEMSurvey_InUse]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/dem-surveys/sort-dems.png)

The alaphabetical sorting is based on the name that is provided by the user. The chronological sorting relies on the user having set survey dates for each DEM Survey (see above).
