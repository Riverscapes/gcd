---
title: N. Running an FIS DEM Error Model
---

### Overview

This exercise will show you how to use GCD to build a spatially variable error model using fuzzy inference systems. Before we dive into the details of what an FIS is, [how they work]({{ site.baseurl }}/tutorials--how-to/workshop-tutorials/o-champ-fis-error-modelling) and [how to build your own]({{ site.baseurl }}/tutorials--how-to/workshop-tutorials/p-building-your-own-fis), we will simply treat an FIS as a black box that takes spatially variable inputs we can model and think relate to surface error, and it will spit out an FIS error surface. This is a powerful and flexible technique for modelling spatially variable error as illustrated below.

![Fig_2007-2006_FIS_Steps0001]({{ site.baseurl }}/assets/images/Fig_2007-2006_FIS_Steps0001.png)

 Figure illustrating independent estimate of spatially variable error for two surveys from Wheaton et al. (2010).

### Data and Materials for Exercises

#### Datasets

1. Feshie Example [N_FIS_Intro.zip](http://etalweb.joewheaton.org/etal_workshops/GCD/2015_USU/N_FIS_Intro.zip) File of Data for this Exercise 
2. CHaMP Example

Data for 1 (above) is from Wheaton et al. (2010) & for 2 (above) is from Bangen et al. (2016)

- 2016.  Bangen S‡ , Hensleigh J‡, McHugh P, and Wheaton JM.  [Error modeling of DEMs from topographic surveys of rivers using Fuzzy Inference Systems](https://www.researchgate.net/publication/292210478_Error_modeling_of_DEMs_from_topographic_surveys_of_rivers_using_fuzzy_inference_systems).  Water Resources Research. DOI: [10.1002/2015WR018299](http://dx.doi.org/10.1002/2015WR018299).
- Wheaton JM, Brasington J, Darby SE and Sear D. 2010. [Accounting for Uncertainty in DEMs from Repeat Topographic Surveys: Improved Sediment Budgets](https://www.researchgate.net/publication/227747150_Accounting_for_uncertainty_in_DEMs_from_repeat_topographic_surveys_Improved_sediment_budgets). Earth Surface Processes and Landforms. 35 (2): 136-156. DOI: [10.1002/esp.1886](http://dx.doi.org/10.1002/esp.1886).  

#### Prerequisite for this Exercise

- Some ArcGIS experience
- ArcGIS 10.X w/ Spatial Analyst Extension
- GCD Add-In

### Step by Step

**Exercise N - Part 1: RUNNING FIS ERROR MODELS**

Suggested Path: `C:\0_GCD\Exercises\N_FIS_Intro`

1. Start new ArcMap Document
2. Create New GCD Project - Feshie in N
3. Add survey DEM for 2006
4. To start, as reminder, derive a spatially uniform error raster
5. Load 2-input and 3-input FIS in FIS Library if not already present
6. Derive point density and slope associated surfaces
7. Run 2-Input point density, slope degrees FIS model to create new error surface

<iframe width="560" height="315" src="https://www.youtube.com/embed/Tp2wR20Z5aI" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

**Exercise N - Part 2: RUNNING FIS ERROR MODELS**

Suggested Path: `C:\0_GCD\Exercises\N_FIS_Intro`

1. In same ArcMap Document
2. Add a new point quality associated surface, PQ_2006.tif
3. Run FIS with 3-Input Model (point quality, point density, slope degrees)
4. Investigate difference between 2-Input and 3-Input FIS Error Surfaces
5. Perform Change Detection between 2006 DEM using its 3-Input FIS Error model and 2007 DEM using its spatially uniform error model
6. Compare results with a Change Detection between same DEM when both are using a spatially uniform error model. 

<iframe width="560" height="315" src="https://www.youtube.com/embed/gdJJ7K-xIh8" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

### Related Online Help or Tutorials for this Topic

- [Parent Workshop Topic]({{ site.baseurl }}/Help/Workshops/workshop-topics/versions/3-day-workshop/2-errors-uncertainties/n-fuzzy-inference-systems)

- [GCD 6 Help]({{ site.baseurl }}/)

- - [Fuzzy Inference Systems for Modeling DEM Error]({{ site.baseurl }}/gcd-concepts/fuzzy-inference-systems-for-modeling-dem-error)
  - [ii. Derive Error Surface]({{ site.baseurl }}/gcd-command-reference/gcd-project-explorer/g-error-surfaces-context-menu/ii-derive-error-surface)
  - [F. Individual Associated Surface Context Menu]({{ site.baseurl }}/gcd-command-reference/gcd-project-explorer/f-individual-associated-surface-context-menu)

------

← [Back to Previous Tutorial]({{ site.baseurl }}/tutorials--how-to/workshop-tutorials/m-deriving-roughness-with-topcat)        [Ahead to Next Tutorial]({{ site.baseurl }}/tutorials--how-to/workshop-tutorials/o-champ-fis-error-modelling) →

------
<div align="center">
	<a class="hollow button" href="{{ site.baseurl }}/Help"><i class="fa fa-chevron-circle-left"></i>  Back to GCD Help </a>  
	<a class="hollow button" href="{{ site.baseurl }}/"><img src="{{ site.baseurl}}/assets/images/icons/GCDAddIn.png">  Back to GCD Home </a>  
</div>