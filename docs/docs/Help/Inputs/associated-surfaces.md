---
title: Associated Surfaces
slug: /help/inputs/associated-surfaces
sidebar_position: 2
---

![Feshie dataset](/img/datasets/feshie_200h.png)

Associated surfaces represent rasters that accompany a DEM Survey. They must have exactly the same spatial extent as the parent DEM Survey and typically store some value that describes additional information about the DEM. For example, an associated surface might store slope values or point density of the original topographic survey data. These associated surfaces are optional and typically combined to generate error surface rasters.

Since associated surfaces belong to a parent DEM survey there are  specific requirements regarding these rasters. Associated surface rasters and the parent DEM survey must have **identical**:

* spatial reference
* cell resolution
* spatial extent

## Add Existing Associated Surface

To use an existing raster as an associated surface, first ensure that the project explorer node for the parent DEM Survey is already expanded. Then right click on the Associated Surfaces node and choose **Add Associated Surface**.

![Add Assoc](/img/CommandRefs/00_ProjectExplorer/inputs/assoc/add-assoc.png)

You will be prompted to browse to an existing raster file on your computer. GCD is compatible with GeoTiff (`*.tiff`) and Erdas Imagine (`*.img`) raster formats. It is not compatible with any other formats including rasters stored in geodatabases, PostGIS etc. 

The same raster import form is displayed after the raster is selected as that used to [add an existing DEM survey](/Help/Inputs/dem-surveys#add-existing-dem-survey). The form appears slightly differently when used for adding associated surfaces. First, the output edge coordinates and cell resolution are **fixed** and locked to those of the parent DEM Survey. This ensures that all associated surfaces are perfectly concurrent with the parent DEM.

## Calculating Point Density

GCD can calculate point density associated surface rasters providing that you have a point ShapeFile containing all the survey points that were used to generated the parent DEM survey. The surface raster will be created with the correct spatial extent, cell resolution and units. It will be stored in the appropriate folder within the GCD project and the correct type attached.

![assoc context](/img/CommandRefs/00_ProjectExplorer/inputs/assoc/assoc_point_density_cms.png)

On the point density form, click the "plus" button to browse and choose a single ShapeFile that contains all the points that were used to generate the parent DEM. Provide a name and then specify how you want to define the point density neighborhood. You can choose between a square and round kernel.

![assoc context](/img/CommandRefs/00_ProjectExplorer/inputs/assoc/assoc_point_density.png)

Square kernels are defined by the length of one side of the square. Round kernels are defined by the radius of the circle (not the diameter). The value entered is always in the same horizontal units of the parent DEM Survey spatial reference.

Point density is calculated by first creating a blank template raster with an identical spatial extent and cell resolution of the parent DEM survey. The kernel is then placed over top of each cell that possesses elevation data in the DEM Survey (i.e. not `NoData`) and the number of points in the ShapeFile calculated. This tally is converted to a density by dividing by the area of the kernel.

| ![Round kernel](/img/CommandRefs/00_ProjectExplorer/inputs/assoc/point_density_round_kernal.png) | ![Square kernel](/img/CommandRefs/00_ProjectExplorer/inputs/assoc/point_density_square_kernal.png) |
|:--:|:--:|
| Round kernel | Square kernel |

## Calculating Slope

GCD can calculate associated surface slope rasters in either decimal degrees or percent. Right click on the Associated Surface node under the parent DEM and choose the desired type. The surface raster will be created with the correct spatial extent, cell resolution and units. It will be stored in the appropriate folder within the GCD project and the correct type attached.

![assoc context](/img/CommandRefs/00_ProjectExplorer/inputs/assoc/assoc_slope.png)

## Context Menu

Right clicking on any associated surface brings up the context menu that allows you to perform the three operations described below. Note that the Add To Map option is only available in the ArcGIS Addin version of GCD and not the Standalone.

![assoc context](/img/CommandRefs/00_ProjectExplorer/inputs/assoc/assoc_context.png)

## Edit Properties

Editing the properties of an associated surface it is possible to change the name used to refer to it and also assign a type from one of the following:

- 3D Point Quality
- Point Density
- Roughness
- Slope Percent Rise
- Slope Degrees
- Interpolation Error
- Grain Size Statistic
- [Undefined]

The only place that this choice of associated surface type impacts GCD is in determining which map symbolization is used when the raster is displayed in ArcMap (AddIn version only). For all other intents and purposes the choice of type is merely a label.

![Assoc properties](/img/CommandRefs/00_ProjectExplorer/inputs/assoc/assoc_properties.png)

## Add To Map

Associated surfaces are added to the current ArcMap document with a symbology that depends on the specified type. See edit properties above. Associated surfaces that do not have a type specified as added with an orange continuous color ramp.

![Assoc properties](/img/CommandRefs/00_ProjectExplorer/inputs/assoc/assoc_add_to_map.png)

## Delete

Deleting an associated surface removes the GCD project reference to the raster and permanently deletes the underlying raster file within the GCD project.

![Assoc properties](/img/CommandRefs/00_ProjectExplorer/inputs/assoc/assoc_delete.png)