---
title: i. Create Interpolation Error Surface
---

```
 Interpolation Error Surface 
```

For conceptual background on this tool and the analysis that can be used with its output [click here](http://mbes.joewheaton.org/background/conceptual-reference-pages/interpolation-analysis).

The Interpolation Error Surface Tool* *is located under the *Analysis *menu under the `*Uncertainty Analysis *`sub menu and the` `*Raster Based *sub menu therein:

![AddInToolbar_InterpolationErrorTool]({{ site.baseurl }}/assets/images/AddInToolbar_InterpolationErrorTool.png)

The video below describes how to use the Interpolation Error Surface Tool:

<iframe width="560" height="315" src="https://www.youtube.com/embed/DcKp1Ia2CKQ" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

When the Interpolation Error Surface Tool is run, the following dialog appears:

![InterpolationErrorForm]({{ site.baseurl }}/assets/images/InterpolationErrorForm.png)

#### INPUTS:

The inputs for this tool are:

- **Point Cloud Shapefile**

- - point shapefile containing the original surveyed z values. This file can be created using the [Create Point Feature Class Tool]({{ site.baseurl }}/gcd-command-reference/data-prep-menu/c-survey-preparation-menu/i-create-point-feature-class)l located under the Data Preparation tab.

- **Field to Create Surface From**

- - This drop-down list will automatically populate with fields from the point cloud shapefile.
  - Generally when using this tool to create a interpolation error surface to be used in a geomorphic change detection study the field containing the elevation values would be selected.*
  - *However any field can be used to see interpolation error values for that field.

- **Extent Polygon**

- - polygon shapefile of the survey extent for the point cloud shapefile. This can be created through in GCD with the *Create Bounding Polygon *Tool or the [Survey Extent Polygon Tool]({{ site.baseurl }}/gcd-command-reference/data-prep-menu/c-survey-preparation-menu/ii-create-survey-extent-polygon).

- **Input Raster**

- - The raster that was interpolated from the point cloud shapefile.

- **Spatial Reference **(optional)

- - can be in the form of a .prj file or you can load an existing shapefile that contains a spatial reference and that spatial reference will be imported. This is auto-populated if your point cloud shapefile includes a spatial reference.

- **Cell Size of Output Raster**

- - The cell size to for the interpolation error raster. 2 feet is currently the default.

#### OUTPUTS:

The outputs for the tool are:

- **Interpolation Error Raster**

- - This raster is created from the average difference between surveyed points and the cell that they are within the DEM they were used to create. The absolute value of difference is taken because statistical analysis of the overall distribution of the difference between surveyed points and DEM values is more robust when positive and negative values do not cancel each other out.

------
<div align="center">
	<a class="hollow button" href="{{ site.baseurl }}/Help"><i class="fa fa-chevron-circle-left"></i>  Back to GCD Help </a>  
	<a class="hollow button" href="{{ site.baseurl }}/"><img src="{{ site.baseurl}}/assets/images/icons/GCDAddIn.png">  Back to GCD Home </a>  
</div>
