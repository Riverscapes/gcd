---
title: Error Surfaces
weight: 3
---

Error surfaces are rasters that represent uncertainty within a DEM Survey. The values in an error surface possess the same vertical units as those of the corresponding DEM and also, since error surfaces belong to a parent DEM survey, there are specific requirements regarding these rasters. Error surface rasters and the parent DEM survey must have **identical**:

* spatial reference
* cell resolution
* spatial extent

Each error surface for a particular DEM survey must have a unique name. Only one of the error surfaces can be designated as the [default error surface](#default-error-surfaces).

# Add Existing Error Surface

To use an existing raster as an error surface, first ensure that the project explorer node for the parent DEM Survey is already expanded. Then right click on the Error Surfaces node and choose **Add Error Surface**.

![error]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/error/error_add_existing.png)

You will be prompted to browse to an existing raster file on your computer. GCD is compatible with GeoTiff (`*.tiff`) and Erdas Imagine (`*.img`) raster formats. It is not compatible with any other formats including rasters stored in geodatabases, PostGIS etc. 

The same raster import form is displayed after the raster is selected as that used to [add an existing DEM survey]({{site.baseurl}}/Help/Inputs/dem-surveys.html#add-existing-dem-survey). The form appears slightly differently when used for adding error surfaces. First, the output edge coordinates and cell resolution are **fixed** and locked to those of the parent DEM Survey. This ensures that all error surfaces are perfectly concurrent with the parent DEM.

# Calculating Error Surfaces

GCD can calculate various types of error surfaces from very simple spatially uniform rasters to more sophisticated fuzzy inference system rasters.

## Uniform Error Surfaces

Uniform error surfaces are simple rasters that possess the same value in all cells. These are useful when the uncertainty is constant across the parent DEM survey. GCD generates a raster with the user specified value in every cell where the DEM survey possesses data (i.e. not in `NoData` cells).

![uniform]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/error/error_uniform.png)

## Associated Surface Error Surfaces

Associated error surfaces allow you to pick a raster that has already been specified as an [associated surface]({{site.baseurl}}/Help/Inputs/associated-surfaces.html) as also representing error for the DEM Survey.

## FIS Error Surfaces

GCD can calculate elevation uncertainty from a [fuzzy inference system]({{ site.baseurl }}/Concepts/fuzzy-inference-systems-for-modeling-dem-error.html). Before performing this step you must first ensure that you have imported all the rasters that you want to use as FIS inputs as associated surfaces for the parent DEM.

1. Check the box beside FIS error model.
2. Choose an FIS rule set from the dropdown. This list is populated from the [GCD FIS Library](). You can learn more about each FIS rule set by first selecting the desired item in the drop down and then clicking the information button to the right.
3. Choose the associated surface that you want to use for each FIS input. All inputs must have an associated surface specified.

![fis]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/error/error_fis.png)

## Multi-Method Error Surfaces

The methods of defining error surfaces described above presume that you want to apply the same method across the entire extent of the DEM survey. In many cases DEM Surveys might be an amalgamation of data from different topographic survey methods (e.g. total station on land and sonar under water). In these cases it is possible to generate an error surface that uses different error definitions for different areas.

Before attempting to create a multi-method error surface you need to import a [regular mask]({{site.baseurl}}/Help/Inputs/Masks/regular-masks.html) into your GCD project. This polygon layer will be used to define the different areas where error surface methods should be applied. Note that a mask  can have multiple polygons with the same value. All polygons with the same mask value will have the same error surface calculation method applied.

![fis]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/error/multi_method.png)

When specifying a multi-method error surface you can specify a different error surface calculation method for each mask value. Click on each mask region to open the error surface properties form shown above. Once you have defined the error surface method for a particular region, the mask regions grid will update with information about each region.

![multi method]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/error/multi_method_config.png)

# Context Menu

Right clicking on an error surface brings up the context menu that allows you to perform the three operations described below. Note that the Add To Map option is only available in the ArcGIS Addin version of GCD and not the Standalone.

![error context]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/error/error_context.png)

# Edit Properties

Right clicking on an error surface displays a form showing the properties of the error surface raster. The particular form displayed varies depending on the method used to generate the raster (browse to existing, uniform or multi method). In all three cases, the form is locked down such that the user can only change the name of the GCD error surface and not change any other information about it. Basic raster properties are also shown on this form.

# Default Error Surfaces

Each DEM Survey can only have one error surface designated as the default. The first error surface that is added to each DEM survey is always configured to be the default. Adding additional error surfaces then allows the user to pick and choose which one should be the default. Switching the default status is done either by editing the error surface and checking the box underneath the name field (see above) or by right clicking on the error surface in the GCD Project Explorer:

![error default]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/error/error_default.png)

The ability to specify a default error surface affects the GCD in three places. The default error surface will always be the selected error surface when configuring a:

1. [change detection analysis]().
2. [generic batch change detection analysis]().
3. [multi-epoch change detection analysis]().

# Add To Map

Error surfaces are added to the current ArcMap document with a color ramp symbology that is stretched to the range of values in the underlying raster.

![error add to map]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/error/error_add_to_map.png)

# Delete

Deleting an error surface removes the GCD project reference to the raster and permanently deletes the underlying raster file within the GCD project.

![error properties]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/error/error_delete.png)