---
title: iii. Create TIN and/or DEM
---

The `Create TIN and/or DEM` tool allows you to derive a TIN and raster DEM in one step. 

![GCD6_Form_PointCloudtoTINDEM]({{ site.baseurl }}/assets/images/GCD6_Form_PointCloudtoTINDEM.png)

The below video gives a succinct tutorial on how to use and the details of the Create TIN and/or DEM tool:

<iframe width="560" height="315" src="https://www.youtube.com/embed/RR9_VHl9S0g" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

**INPUTS:**

 

Create TIN and/or DEM

 

- **Point Cloud Shapefile**

- - point shapefile containing the original surveyed z values. This file can be created using the [Create Point Feature Class]({{ site.baseurl }}/gcd-command-reference/data-prep-menu/c-survey-preparation-menu/i-create-point-feature-class) tool located under the Data Preparation tab.

- **Extent Polygon**

- - polygon shapefile of the survey extent for the point cloud shapfile. This can be created through in GCD with the [Create Survey Extent Polygon]({{ site.baseurl }}/gcd-command-reference/data-prep-menu/c-survey-preparation-menu/ii-create-survey-extent-polygon) Tool.

- **Spatial Reference **(optional)

- - can be in the form of a .prj file or a shapefile that contains a spatial reference.

- **Cell Size of DEM **(optional)

- - The cell size to for the DEM, this option is only activated once the user checks the *Create DEM *option. 2 feet is currently the default

**OUTPUTS:**

The outputs for the Create TIN and/or DEM  tool are:

- **TIN **(optional)

- - TIN created from the original surveyed values contained in the point cloud shapefile. This will not be output if the user selects the *Delete TIN *option

- **DEM **(optional)

- - DEM created from the TIN. This is only created if the user selects the *Create DEM *option.

← Back to  [ii. Create Survey Extent Polygon]({{ site.baseurl }}/gcd-command-reference/data-prep-menu/c-survey-preparation-menu/ii-create-survey-extent-polygon)  

------
<div align="center">
	<a class="hollow button" href="{{ site.baseurl }}/Help"><i class="fa fa-chevron-circle-left"></i>  Back to GCD Help </a>  
	<a class="hollow button" href="{{ site.baseurl }}/"><img src="{{ site.baseurl}}/assets/images/icons/GCDAddIn.png">  Back to GCD Home </a>  
</div>