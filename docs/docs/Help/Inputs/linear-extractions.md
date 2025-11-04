---
title: Linear Extractions
sidebar_position: 6 
slug: /help/inputs/linear-extractions
---

![Profile Routes](/img/CommandRefs/00_ProjectExplorer/inputs/profile/profile_routes.png)
Linear extractions are the product of reading the values of a DEM survey, [reference surface](/Help/Inputs/reference-surfaces) or [change detection](/Help/Analyses/Change_Detection/change-detection) raster along the polylines contained in a profile route. Each linear extraction operation produces a comma separated value (CSV) file that contains the raster values from the selected raster at user-defined stations along the polylines. Values from a corresponding error surface are also written to the linear extraction CSV file, taken from the same coincident points as the values from the DEM or DoD raster.

Before you can use the linear extraction feature you must already have one or more rasters in your GCD project as well as imported a [profile route](/Help/Inputs/profile-routes) that possesses the polylines that you want to use.

The GCD software itself does not possess features for viewing linear extractions. However, the [Cross Section Viewer](http://xsviewer.northarrowresearch.com/) can ingest entire GCD projects and does possess several sophisticated tools for viewing and analyzing these data.

## DEM Survey & Reference Surface Extractions

The same method is used to generate a linear extraction from a [DEM Survey](/Help/Inputs/dem-surveys) or reference surface. Right click on one of these items in the GCD Project Explorer and choose

![mask cms](/img/CommandRefs/00_ProjectExplorer/inputs/linear/linear_add.png)

1. Enter a name for the linear extraction. the name must be unique among the linear extractions for that particular parent item (DEM Survey, reference surface or change detection).
2. Choose the [profile route](/Help/Inputs/profile-routes) that you want to use.
3. Specify a sample distance that is the sample distance along each line at which raster values will be extracted. the units are those of the horizontal units for the DEM Surveys in the GCD project.
4. Select an error surface for which values will be extracted.
5. Click Create to generate the linear extraction CSV file.

![Add linear extraction](/img/CommandRefs/00_ProjectExplorer/inputs/linear/linear_config.png)


## Change Detection Linear Extractions

Generating a linear extraction from a [change detection](/Help/Analyses/Change_Detection/change-detection) is done in much the same way as described above. The main difference is that with change detections the user doesn't choose an error surface. This is chosen for you based on the type of thresholding that was used to generate the change detection.

|---|---|
|Thresholding Method|Linear Extraction Error Surface Values|
|---|---|
|Minimum Level of Detection|A constant value that corresponds to the minimum level of detection.|
|Propagated Error|The propagated error value at each cell location is used.|
|Probabilistic Thresholding| See below |

![Add linear extraction](/img/CommandRefs/00_ProjectExplorer/inputs/linear/linear_cms_dod.png)

## Context Menu

Right clicking on any regular mask brings up the context menu that allows you to perform the three operations described below. Note that the Add To Map option is only available in the ArcGIS Addin version of GCD and not the Standalone.

![mask cms](/img/CommandRefs/00_ProjectExplorer/inputs/linear/linear_cms.png)

## View Linear Extraction Folder

Right click on a linear extraction and choose to view the folder. This will open Windows Explorer at the corresponding path. It is strongly advised that you don't edit or manipulate the CSV file within the GCD project. If you want to work with the file, it is recommended that you copy it and save it to a location outside of the GCD project folder structure and work with it there.

![mask cms](/img/CommandRefs/00_ProjectExplorer/inputs/linear/linear_folder.png)

## Visualizing Linear Extractions

See the [Cross Section Viewer](http://xsviewer.northarrowresearch.com/Online_Help/File_Menu/import_gcd_project.html) help for instructions on how to view the contents of GCD linear extractions.

![Cross Section Viewer](/img/CommandRefs/00_ProjectExplorer/cross_section_viewer.png)

## Edit Properties

Editing the properties of a linear extraction it is possible to change the name. No other attributes are editable.

## Delete

Deleting a linear extraction removes it from the GCD project and permanently deletes the underlying CSV file within the GCD folder structure.