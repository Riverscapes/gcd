---
title: Explore Change Detection Folder
---

#### Accessing Change Detection Folder From ArcMap

Every time a change detection is performed between two DEM a folder is created in the project folder structure to house the results; tables, figures, and DoD rasters. To access the folder for a specific change detection right click a specific change detection and select `Explore Change Detection Folder` by selecting it from the pop-up menu as is shown below:

![AccessingAnalysisFromTOC_ExploreChangeDetectionFolder]({{ site.baseurl}}/assets/images/AccessingAnalysisFromTOC_ExploreChangeDetectionFolder.png)

This will bring up a windows explorer where the results for the selected change detection can be explored.

#### Overview of Change Detection Folder Structure

All change detection results are stored in the following folder:

The results for an individual change detection are placed in a sub folder of this structure called GCD appended by the sequential number of change detections performed for that project. For example the first change detection ran in a project will be placed in a folder called GCD0001, the fifth change detection would be placed in a folder called GCD0005. The first change detection for a project will be placed in the following folder:

[ProjectName]\Analyses\CD\GCD0001

Within this folder are the following files and folders that contain the results of the GCD:

- dodraw.tif

- - The spatial results of the change detection in raster format without applying any thresholding techniques. Simply DEMnew - DEMold.

- dodThresh.tif

- - The spatial results of the change detection in raster format with the error with the thresholding techniques applied.

- raw.csv

- - numerical results of the change detection in comma separated values format without applying any thresholding techniques.
  - It contains the following fields Lower Elevation Range, Upper Elevation Range, Total Area, Total Volume, Number of Cells
  - This is the areal and volumetric changes placed into preset bins. 

- Summary.xml

- - summary of numerical results of the change detection in tabular/.xml format.
  - contains results without applying any thresholding techniques and with applying thresholding techniques.
  - Same table accessed within ArcMap from the *Change Detection Results Panel*. That is accessed by right clicking an existing Change Detection and clicking the View Change Detection Results [![img]({{ site.baseurl }}/_/rsrc/1472842987873/gcd-command-reference/gcd-project-explorer/l-individual-change-detection-context-menu/iii-explore-change-detection-folder/GCD.png)]({{ site.baseurl }}/gcd-command-reference/gcd-project-explorer/l-individual-change-detection-context-menu/iii-explore-change-detection-folder/GCD.png?attredirects=0) button.


- thresholded.csv

- - numerical results of the change detection in comma separated values format with the thresholding techniques applied. Simply DEMnew - DEMold.
  - It contains the following fields Lower Elevation Range, Upper Elevation Range, Total Area, Total Volume, Number of Cells
  - This is the areal and volumetric changes placed into preset bins. 

- Figs

- - folder of figures

  - houses the following figures which are all in .png format:

  - - ChangeBars_AreaAbsolute

    - - Bar chart of the actual area of change for erosion and deposition.
      - Red is erosion and is represented by negative values.
      - Blue is deposition and is represented by positive values.

    - ChangeBars_AreaRelative

    - - Bar chart of the relative area of change for erosion and deposition, absolute values are used to allow for easier comparisons.
      - Both erosion and deposition are represented by positive values.

    - ChangeBars_DepthAbsolute

    - - Bar chart of the actual average depth of change for erosion and deposition.
      - Red is erosion and is represented by negative values.
      - Blue is deposition and is represented by positive values.

    - ChangeBars_DepthRelative

    - - Bar chart of the absolute average depth of change for erosion and deposition, absolute values are used to allow for easier comparisons.
      - Both erosion and deposition are represented by positive values.

    - ChangeBars_VolumeAbsolute

    - - Bar chart of the actual volume of change for erosion and deposition.
      - Red is erosion and is represented by negative values.
      - Blue is deposition and is represented by positive values.

    - ChangeBars_VolumeRelative

    - - Bar chart of the relative volume of change for erosion and deposition, absolute values are used to allow for easier comparisons.
      - Both erosion and deposition are represented by positive values.

    - Histogram_Area

    - - Histogram of the area of change.
      - Red is erosion.
      - Blue is deposition.

    - Histogram_Volume

    - - Histogram of the volume of change.
      - Red is erosion.
      - Blue is deposition.

------
<div align="center">
	<a class="hollow button" href="{{ site.baseurl }}/Help"><i class="fa fa-chevron-circle-left"></i>  Back to GCD Help </a>  
	<a class="hollow button" href="{{ site.baseurl }}/"><img src="{{ site.baseurl}}/assets/images/icons/GCDAddIn.png">  Back to GCD Home </a>  
</div>