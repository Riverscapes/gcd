---
title: Associated Surfaces
weight: 2
---

<div class="float-right">
<img src="{{ site.baseurl }}/assets/images/datasets/feshie_200h.png">
</div>
Associated surfaces represent rasters that accompany a [DEM Survey](). They have exactly the same spatial extent as the parent DEM Survey and typically store some value that describes additional information about the DEM. For example, an associated surface might store slope values or point density of the original topographic survey data. These associated surfaces are optional and typically combined to generate [error surface rasters]().

Since associated surfaces belong to a parent DEM survey there are  specific requirements regarding these rasters. Associated surface rasters and the parent DEM survey must have **identical**:

* spatial reference
* cell resolution
* spatial extent

# Add Existing Associated Surface

To use an existing raster as an associated surface, first ensure that the project explorer node for the parent DEM Survey is already expanded. Then right click on the Associated Surfaces node and choose **Add Associated Surface**.

![Add Assoc]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/assoc/add-assoc.png)

You will be prompted to browse to an existing raster file on your computer. GCD is compatible with GeoTiff (`*.tiff`) and Erdas Imagine (`*.img`) raster formats. It is not compatible with any other formats including rasters stored in geodatabases, PostGIS etc. 

The same raster import form is displayed after the raster is selected as that used to [add an existing DEM survey](/Help/gcd-project-explorer/Inputs/dem-surveys.html#add-existing-dem-survey). The form appears slightly differently when used for adding associated surfaces. First, the output edge coordinates and cell resolution are **fixed** and locked to those of the parent DEM Survey. This ensures that all associated surfaces are perfectly concurrent with the parent DEM.

# Calculating Point Density

# Calculating Slope

This video shows you how to use the project explorer dockable window to calculate a slope raster from a DEM already loaded to a GCD project.

<div class="responsive-embed">
<iframe width="560" height="315" src="https://www.youtube.com/embed/b9DQ-UbgePw" frameborder="0" allow="autoplay; encrypted-media" allowfullscreen></iframe>
</div>

# Context Menu

# Edit Properties

Editing the properties of an associated surface it is possible to change the name used to refer to it and also assign a type from one of the following:

- 3D Point Quality
- Point Density
- Roughness
- Slope Percent Rise
- Slope Degrees
- Interpolation Error
- Grain Size Statistic
- [Undefined]

# Add To Map

Associated surfaces are added to the current ArcMap document with a symbology that depends on the specified type. See edit properties above. Associated surfaces that do not have a type specified as added with an orange continuous color ramp.

# Delete

Deleting an associated surface removes the GCD project reference to the raster and permanently deletes the corresponding raster within the GCD project.