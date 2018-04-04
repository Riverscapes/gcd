---
title: i. Create Point Feature Class
---

The `Create Point Feature Class` tool takes a raw xyz point cloud in a flat ascii file (e.g. `*.csv`, `*.txt`, `*.xyz`, `*.pts`) and converts it into a point shapefile for direct use in deriving a TIN and DEM. It also allows you to specify the spatial reference (coordinate system) of the point shapefile you will create, which is required to use the shapefile in a GCD project analyses (e.g. point density estimation). The tool is a precursor to `Create DEM and/or TIN` tool.

![CreatePointFeatureClassToolUI]({{ site.baseurl }}/assets/images/CreatePointFeatureClassToolUI.png)

The below video gives a succinct tutorial on how to use and the details of the Create Point Feature ClassTool:

<iframe width="560" height="315" src="https://www.youtube.com/embed/5e6e9j4v5Fc" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

#### INPUTS:

Raw Point Cloud to Feature Class 

- **raw point cloud**

- - An ascii text file formatted into columns of x, y, z. Other columns can be present but will not be included in the output shapefile. (headers can be commented out with a `#`)

- **spatial reference **(optional)

- - can be in the form of a .prj file or you can load an existing shapefile that contains a spatial reference and that spatial reference will be imported.

#### OUTPUTS:

The outputs for the tool are:

- **Shapefile or Feature Class**

- - contains x, y, z fields from the original surveyed points.

Ahead to: [ii. Create Survey Extent Polygon Tool]({{ site.baseurl }}/gcd-command-reference/data-prep-menu/c-survey-preparation-menu/ii-create-survey-extent-polygon)→

------
<div align="center">
	<a class="hollow button" href="{{ site.baseurl }}/Help"><i class="fa fa-chevron-circle-left"></i>  Back to GCD Help </a>  
	<a class="hollow button" href="{{ site.baseurl }}/"><img src="{{ site.baseurl}}/assets/images/icons/GCDAddIn.png">  Back to GCD Home </a>  
</div>