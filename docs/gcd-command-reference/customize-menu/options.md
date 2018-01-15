---
title: Options
---

The Options dialog has five tabs for customizing different aspects of the GCD software. All options are user-specific, meaning that any changes are saved for the current user only.

The *Options* dialog is located under the *Customize* menu in the main tool bar as shown in the image below:

![OptionsForm_AccessFromMainToolBar]({{ site.baseurl }}/assets/images/OptionsForm_AccessFromMainToolBar.png)

### Workspace Tab

The workspace tab controls several features that determine the default behavior of GCD.

![OptionsForm_WorkspaceTab_Numbered]({{ site.baseurl }}/assets/images/OptionsForm_WorkspaceTab_Numbered.png)

In the image above each number coincides to a different option. The options are described below:

1. Determines if the temporary workspace should be cleared every time GCD is opened. (**recommended**)
2. If checked when a GCD tool is being used its inputs will be automatically added to the map, unless they are already in the map.
3. If checked outputs from GCD tools will be automatically added to the map.
4. Determines if GCD should perform validation of a GCD project when it is loaded. Occasionally errors in writing to the .gcd project file when creating new inputs/ouputs and deleting inputs/outputs, these errors can cause unexpected behavior when navigating the GCD Project Explorer and other places in the software. To avoid these errors the project validates and fixes these errors when project is opened. (**recommended**)
5. Generally Windows has a maximum length for paths of 256 characters. If an input or output is going to be created that exceeds this length checking this option will warn the user before it tries to complete the operation. (**recommended**)
6. GCD is shipped with a repository of FIS for a range of surveying methods. Select this option to automatically have the FIS in this repo added to the [FIS Library]({{ site.baseurl }}/gcd-command-reference/customize-menu/fis-library). This will make all FIS in the repository automatically available to the user when creating a FIS. (**recommended**)
7. Determines which raster format will be used when creating outputs from GCD tools.

This video explains the different commands:

<iframe width="560" height="315" src="https://www.youtube.com/embed/XIS_Iyk4kLI" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

### Survey Types Tab

Survey types can be customized to represent different topographic survey methods. This list controls which survey types are available as the *Survey Method*  int he `DEM Survey` Tab of the `DEM Survey Properties` dialog of the [`Survey Library`]({{ site.baseurl }}/system/errors/NodeNotFound?suri=wuid:gx:3ed05905e41de6f6). The error value is in map units (e.g. meters) and represents the default elevation uncertainty used for this survey method when it is a spatially uniform error estimate (i.e the `Error Calculations` tab of the `DEM Survey Properties` dialog). The default values shown are *very crude* rules of thumb from our experience and the literature, but can vary dramatically depending on the specifics of survey implementation, instrumentatoin, sampling design, post-processing, and surface creation.

![Dialog_GCD_Options_SuveyTypes]({{ site.baseurl }}/assets/images/Dialog_GCD_Options_SuveyTypes.png)

This video shows you how to modify and/or add new survey types.

<iframe width="560" height="315" src="https://www.youtube.com/embed/ncR_m23hy18" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

### Symbology Tab

This tab has no functionality in the current release. It is a placeholder for allowing the user to over-ride the default symbology when rasters are loaded to the Survey Library or created by the GCD are added to the map.

![Dialog_GCD_Options_Symbology]({{ site.baseurl }}/assets/images/Dialog_GCD_Options_Symbology.png)

### Graphs Tab

The Graphs tab simply controls the output resolution and dimensions (in pixels) of the output `*.png `graph image files that are automatically produced by GCD. The default is for a square graph at 1000 x 1000 pixels. The graphs currently exported by GCD include:

- Volumetric and Areal Elevation Change Distributions associated with both the [`Change Detection`]({{ site.baseurl }}/gcd-command-reference/gcd-analysis-menu/c-geomorphic-change-detection-submenu/change-detection) and [`Budget Segregation`]({{ site.baseurl }}/gcd-command-reference/gcd-analysis-menu/c-geomorphic-change-detection-submenu/budget-segregation) panels.
- Pie-Charts from Budget Segregation Panel.

NOTE: All graphs produced in GCD can also be exported manually from their respective panels/dialogs (with a right click) and that all the data required to produce these graphs automatically is also exported to the output folder.

![Dialog_GCD_Options_Graphs]({{ site.baseurl }}/assets/images/Dialog_GCD_Options_Graphs.png)

### Coordinate Precision Tab

This tab determines how GCD checks for [grid orthogonality and dimensional divisibility]({{ site.baseurl }}/gcd-concepts/data-preparation---best-practices). ArcGIS introduces small rounding errors in raster dimensions (width and height) as well as raster resolution. The result is a raster intended to be `0.1` m in cell resolution, is actually stored and treated as either `0.0999999999999998372` or `0.10000000000000003432` (even though when you check the raster properties it may still say `0.1`), and this can lead to the recorded height and width of the raster not being evenly divisible by the cell resolution. Although most users can ignore this issue, it is critical during change detection, because it leads to unnecessary re-sampling of rasters and introduction of minor interpolation errors in your data.  When we run Orthogonality, Divisibility and Concurrency checks in the software, we round the ESRI-reported values (by default) to 4 decimal places so that these minor precision inaccuracies do not propagate. In the above example this will treat what gets reported as 0.1 as 0.1000, despite how the value is actually stored in memory. You can change that precision here:

![Dialog_GCD_Options_CoordinatePrecision]({{ site.baseurl }}/assets/images/Dialog_GCD_Options_CoordinatePrecision.png)