---
title: A. Uncertainty Analysis Submenu
---

There are three sub-submenus under Uncertainty Analysis: These include i. FIS Development Tools, ii. Point Cloud Based, and iii. Raster Based. These stand-alone tools are not tied to the GCD project and can be used whether or not a GCD project is opened. Unlike most commands from the [GCD Project Explorer]({{ site.baseurl }}/gcd-command-reference/gcd-project-explorer), these commands require you to specify where the outputs will be saved instead of automatically saving them within the project structure.

These commands are intended to help you explore various ways of either developing FIS Error models tailored to your data, indepently estimate elevation uncertainty from point clouds or rasters. These tools were developed by James Hensleigh as part of the MBES Tools project for Idaho Power, and have undergone some testing. They have not yet undergone extensive testing in the GCD environment and running from within ArcGIS, and are still missing some basic features the user might expect (e.g. add to map automatically). As such, they are currently made available in a BETA form.

#### Uncertainty Analysis Commands

- a. [FIS Development Tools]({{ site.baseurl}}/gcd-command-reference/gcd-analysis-menu/a-uncertainty-analysis-submenu/a-fis-development-tools)
  - [i. FIS Development Assistant]({{ site.baseurl }}/gcd-command-reference/gcd-analysis-menu/a-uncertainty-analysis-submenu/a-fis-development-tools/i-fis-development-assistant)
- [b. Point Cloud Based]({{ site.baseurl }}/gcd-command-reference/gcd-analysis-menu/a-uncertainty-analysis-submenu/b-point-cloud-based)
  - [i. Coincident Points Tool]({{ site.baseurl }}/gcd-command-reference/gcd-analysis-menu/a-uncertainty-analysis-submenu/b-point-cloud-based/i-coincident-points-tool)
- [c. Raster Based]({{ site.baseurl }}/gcd-command-reference/gcd-analysis-menu/a-uncertainty-analysis-submenu/c-raster-ba)
  - [i. Create Interpolation Error Surface]({{ site.baseurl }}/gcd-command-reference/gcd-analysis-menu/a-uncertainty-analysis-submenu/c-raster-ba/i-create-interpolation-error-surface)

The FIS Development Tools, currently include the FIS Development Assistant and will eventually include an FIS Development Wizard.

![GCD6_Menu_Analysis_Uncertainty_FIS_FISAssist]({{ site.baseurl }}/assets/images/GCD6_Menu_Analysis_Uncertainty_FIS_FISAssist.png)

The Point Cloud Based tools include two tools: the Coincident Point Tools and Brasington et al (2013)'s ToPCAT Detrended Standard Deviation tool. These tools both require high density (circa. > 5 points/m2) point clouds. 

![GCD6_Menu_Analysis_Uncertainty_PtCloud]({{ site.baseurl }}/assets/images/GCD6_Menu_Analysis_Uncertainty_PtCloud.png)

The Raster Based tools include Creating an FIS Error Surface (works the same as within the Project Explorer, except you specify where to store the output), and the Create Interpolation Error Surface.

![GCD6_Menu_Analysis_Uncertainty_RasterBasedt]({{ site.baseurl }}/assets/images/GCD6_Menu_Analysis_Uncertainty_RasterBasedt.png)

------
<div align="center">
	<a class="hollow button" href="{{ site.baseurl }}/Help"><i class="fa fa-chevron-circle-left"></i>  Back to GCD Help </a>  
	<a class="hollow button" href="{{ site.baseurl }}/"><img src="{{ site.baseurl}}/assets/images/icons/GCDAddIn.png">  Back to GCD Home </a>  
</div>