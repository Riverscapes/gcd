---
title: Reference Surfaces
---

Reference surfaces are elevation rasters that originate from some other source than a topographic survey. They are very similar to [DEM Surveys]({{site.baseurl}}/Help/Inputs/dem-surveys.html) in that they share the same vertical units, cell resolution and spatial reference. They can also be used as one or both surfaces in a [change detection ]({{site.baseurl}}/Help/Analyses/Change_Detection/change-detection.html) analysis.

The role of reference surfaces within the GCD software is intentionally flexible, enabling users to leverage them for several purposes:

* Uniform constant reference planes for comparison with DEM surveys over time (e.g. a reservoir spill surface).
* Statistical summary of several DEM surveys over time (e.g. max, min or standard deviation from a series of DEMs).
* User-specified raster than can represent any kind of reference imaginable (providing the cell values of the raster are in the same units at the DEM surveys within the GCD project).

Each reference surface can have multiple error rasters associated with it. These error surfaces are then used when performing a [change detection]({{site.baseurl}}/Help/Analyses/Change_Detection/change-detection.html) analysis that uses spatially variable uncertainty.

# Add Existing Reference Surface

To use an existing raster as a reference surface right click on the Reference Surfaces node in the GCD Project Explorer and choose **Add Reference Surface**.

![Add ref]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/refsurface/refsurface_add_existing.png)

You will be prompted to browse to an existing raster file on your computer. GCD is compatible with GeoTiff (`*.tiff`) and Erdas Imagine (`*.img`) raster formats. It is not compatible with any other formats including rasters stored in geodatabases, PostGIS etc. 

The same raster import form is displayed after the raster is selected as that used to [add an existing DEM survey](/Help/gcd-project-explorer/Inputs/dem-surveys.html#add-existing-dem-survey). The form appears slightly differently when used for adding reference surfaces. The cell resolution and precision are both locked to those of the GCD project. This ensures that all reference surfaces are perfectly divisible with other rasters in the project. You can change the extent. Reference surfaces don't need to be concurrent with each other or DEM surveys.

# Reference Surfaces From DEM Surveys

Reference surfaces can represent a statistical summary from a collection of DEM Surveys. For this feature to work you must have two or more DEM Surveys already loaded into your GCD project. The statistics available are:

* Minimum
* Maximum
* Mean
* Standard Deviation

In each case, the reference surface cell values are calculated on a **cell-wise basis** from the chosen DEM surveys. Only cells that possess data in all the selected DEM Surveys will possess a value in the output reference surface. 

![uniform]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/refsurface/refsurface_fromdems.png)

Provide a unique name for the reference surface and then check or uncheck the box beside each DEM Survey to control whether it is included in the calculation. For each DEM Survey you can pick the desired error surface. You **must** pick an error surface for each one and DEM Surveys without an error surface cannot be used in this calculation.

![uniform]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/refsurface/refsurface_demselect.png)

A reference error surface ([see below](#reference-error-surfaces)) will be generated as part of the calculation. Also calculated on a cell-wise basis, this error surface is calculated as follows:

|---|---|
|Reference Surface Method|Error Calculation|
|---|---|
|Maximum|The maximum DEM error surface value in each cell.|
|Minimum|The minimum DEM error surface value in each cell.|
|Mean|The mean DEM error surface value in each cell.|
|Standard Deviation|The root mean square of all DEM error surface values in each cell.|

# Uniform Reference Surfaces

Uniform reference surfaces are constant rasters that possess the same value in every cell. They can be useful for representing engineered planes, safety thresholds, reservoir draw down levels and spill holes etc.

Right click on the Reference Planes node in the GCD Project Explorer and choose to "Calculate New Constant Reference Surface(s)". Provide a unique name and then choose the [DEM Survey]({{site.baseurl}}/Help/Inputs/dem-surveys.html) that possesses the spatial extent that you want to use. No values from the DEM are used, GCD merely takes the spatial extent, cell resolution and map projection and applies it to the reference surface.

Next pick whether you want to create a single reference surface or multiple over a user-defined range of vertical values. If you pick the latter then you need to specify the range of vertical values over which rasters will get produced. Note that if the range of values (upper minus lower) is not evenly divisible by the increment then a raster at the upper elevation might not get generated.

Each reference surface will get a reference error surface generated with it using the value specified at the bottom of the form. Like the reference surface itself, these error surfaces will possess the same spatial extent and cell resolution as the specified DEM Survey and will have a constant value in all cells.

![uniform]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/refsurface/refsurface_uniform.png)

# Reference Error Surfaces

Each reference surface can have one or more error surfaces specified. These surfaces are managed much in the same way as regular [error surfaces]({{site.baseurl}}/Help/Inputs/error-surfaces.html) for DEM surveys. 

# Context Menu

Right clicking on any reference surface brings up the context menu that allows you to perform the three operations described below. Note that the Add To Map option is only available in the ArcGIS Addin version of GCD and not the Standalone.

![ref surface cms]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/refsurface/refsurface_cms.png)

# Edit Properties

Right clicking and choosing to edit allows you to change the name that GCD uses to refer to the reference surface. This does not change the name of the underlying raster file or the folder in which it is stored. It only applies to the name used in the GCD project.

# Add To Map

Reference surfaces are added to the current ArcMap document with a stretched color ramp symbology.

![add to map]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/refsurface/ref_add_to_map.png)

# Delete

Deleting a reference surface removes the GCD project reference to the raster and permanently deletes the underlying raster file within the GCD project.

![reference surface delete]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/refsurface/ref_delete.png)