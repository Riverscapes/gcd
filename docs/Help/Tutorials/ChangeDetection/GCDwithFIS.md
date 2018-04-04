---
title: Running GCD w/ FIS
weight: 3
---

### Overview

This exercise is simply about putting together what you've done in [M. Deriving Roughness with ToPCAT]({{ site.baseurl }}/tutorials--how-to/workshop-tutorials/m-deriving-roughness-with-topcat), [P. Building your own FIS]({{ site.baseurl }}/tutorials--how-to/workshop-tutorials/p-building-your-own-fis) and the data we collected in the flume and do a complete change detection. 

### Data and Materials for Exercises

#### Datasets

[Q_FlumeFIS.zip](http://etalweb.joewheaton.org/etal_workshops/GCD/2015_USU/Q_FlumeFIS.zip) File of Data for this Exercise 

[Q_BridgeCreek.zip](http://etalweb.joewheaton.org/etal_workshops/GCD/2015_USU/Q_BridgeCreek.zip) File of Data for this Exercise 

#### Prerequisite for this Exercise

- Some ArcGIS experience
- ArcGIS 10.X w/ Spatial Analyst Extension
- GCD Add-In

------

### Step by Step

#### **Exercise R: Running a Change Detection with FIS - Bridge Creek**

C:\0_GCD\Exercises\R_BridgeCreek

1. Start new ArcMap Document
2. Create New GCD Project - "Lower Owens D" in R
3. Add two DEM Surveys (2010, 2014)
4. Generate Associated Surfaces
  - Slope degrees
  - Point density
  - use Topo_Points.shp
  - Interpolation Error
  - use provided surface or generate it with interpolation error tool
5. Generate 3 Input FIS error surfaces
  - Using CHaMP_TLS_ZError_PD_SLPdeg_IntErr
6. Perform change detection using probabilistic thresholding at 80% confidence

<iframe width="560" height="315" src="https://www.youtube.com/embed/vzsDEEwVVMk" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

### Related Online Help or Tutorials for this Topic

- [Parent Workshop Topic](http://gcdworkshop.joewheaton.org/workshop-topics/versions/3-day-workshop/2-errors-uncertainties/q-changedetection)

- [GCD 6 Help]({{ site.baseurl }}/)
  - [Fuzzy Inference Systems for Modeling DEM Error]({{ site.baseurl }}/gcd-concepts/fuzzy-inference-systems-for-modeling-dem-error)
  - [ii. Derive Error Surface]({{ site.baseurl }}/gcd-command-reference/gcd-project-explorer/g-error-surfaces-context-menu/ii-derive-error-surface)
  - [F. Individual Associated Surface Context Menu]({{ site.baseurl }}/gcd-command-reference/gcd-project-explorer/f-individual-associated-surface-context-menu)
  - [Add Change Detection]({{ site.baseurl }}/gcd-command-reference/gcd-analysis-menu/c-geomorphic-change-detection-submenu/change-detection)

------

← [Back to Previous Tutorial]({{ site.baseurl }}/tutorials--how-to/workshop-tutorials/p-building-your-own-fis)        [Ahead to Next Tutorial]({{ site.baseurl }}/tutorials--how-to/workshop-tutorials/s-bayesian-updating-excercise) →

------
<div align="center">
	<a class="hollow button" href="{{ site.baseurl }}/Help"><i class="fa fa-chevron-circle-left"></i>  Back to GCD Help </a>  
	<a class="hollow button" href="{{ site.baseurl }}/"><img src="{{ site.baseurl}}/assets/images/icons/GCDAddIn.png">  Back to GCD Home </a>  
</div>