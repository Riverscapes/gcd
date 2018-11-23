---
title: Associated Surfaces
weight: 2
---

<div class="float-right">
<img src="{{ site.baseurl }}/assets/images/datasets/feshie_200h.png">
</div>
Associated surfaces represent rasters that accompany a [DEM Survey](). They have exactly the same spatial extent as the parent DEM Survey and typically store some value that describes additional information about the DEM. For example, an associated surface might store slope values or point density of the original topographic survey data. These associated surfaces are typically combined to generate [error surface rasters]().

Since associated surfaces belong to a parent DEM survey there are  specific requirements regarding these rasters. Associated surface rasters and the parent DEM survey must have **identical**:

* spatial reference
* cell resolution
* spatial extent

# Add Existing Associated Surface

To use an existing raster as an associated surface, first ensure that the project explorer node for the parent DEM Survey is already expanded. Then right click on the Associated Surfaces node and choose **Add Associated Surface**.

![Add Assoc]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/assoc/add-assoc.png)

You will be prompted to browse to an existing raster file on your computer. GCD is compatible with GeoTiff (`*.tiff`) and Erdas Imagine (`*.img`) raster formats. It is not compatible with any other formats including rasters stored in geodatabases, PostGIS etc. 

The same raster import form is displayed after the raster is selected as that used to [add an existing DEM survey](/Help/gcd-project-explorer/Inputs/dem-surveys.html#add-existing-dem-survey). The form is 