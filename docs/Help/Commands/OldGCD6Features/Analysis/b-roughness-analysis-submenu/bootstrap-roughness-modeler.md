---
title: ii. Bootstrap Roughness Modeler
---

The Bootstrap Roughness Modeler Tool* *is located under the *Analysis* menu and further the *Roughness Analysis* menu:

![GCD_MenuLocationBootstrapRoughnessModeler]({{ site.baseurl }}/assets/images/GCD_MenuLocationBootstrapRoughnessModeler.png)

A video tutorial for how to use the *Bootstrap Roughness Modeler* can be seen below:

<iframe width="560" height="315" src="https://www.youtube.com/embed/U05upegsMlI" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

When the *Bootstrap Roughness Modeler* is run, the following dialog appears:

![ScreenshotBootstrapRoughnessModeler]({{ site.baseurl }}/assets/images/ScreenshotBootstrapRoughnessModeler.png)

#### INPUTS:

The inputs for this tool are:

- **Input Survey Points**

- - point feature class of the points used to create the Digital elevation model (DEM) used as input below.

- **Survey Extent**

- - Polygon feature class representing the extent of the survey.

- **Channel Extent (optional)**

- - Polygon feature class representing areas not to be included in the bootstrapping. For example if a channel unit polygon feature class is used as input then the bootstrap will only be performed using points that exist outside of the channel unit feature class.

- **DEM**

- - Digital elevation model (DEM) that was created from the input survey points.

- **Convert to millimeters **(optional)

- - if this check box is selected then the output results will be in millimeters

- **Vertical units of DEM **(optional, only enabled if Convert to millimeters is checked)

- - grain size metrics are generally reported in millimeters so a conversion based on this value is performed to get values in millimeters.

- **Number of Iterations**

- - controls the number of times that points should be randomly sampled and differenced from the DEM.

- **Percent of data to keep as subset each iteration**

- - controls the percent of data that should be kept during each iteration to make a DEM to compare with the DEM provided above.

#### OUTPUTS:

The outputs for the tool are:

- **Output Raster Path**

- - path to save the output bootstrap roughness raster to. The output raster is comprised of the mean delta Z between each iteration of sub-setting points and the original DEM.

------
<div align="center">
	<a class="hollow button" href="{{ site.baseurl }}/Help"><i class="fa fa-chevron-circle-left"></i>  Back to GCD Help </a>  
	<a class="hollow button" href="{{ site.baseurl }}/"><img src="{{ site.baseurl}}/assets/images/icons/GCDAddIn.png">  Back to GCD Home </a>  
</div>