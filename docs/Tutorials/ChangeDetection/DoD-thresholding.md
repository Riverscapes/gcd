---
title: DoD Thresholding
weight: 2
---

### Overview

This exercise contrasts the three most common forms of thresholding DoDs: i) Simple minimum level of detection, ii) propagated errors, and iii) probabilistic. We use the GCD Add-In to facilitate the analysis.

### Data and Materials for Exercises

#### Datasets

[I_Thresholding.zip](http://etalweb.joewheaton.org/etal_workshops/GCD/2015_USU/I_Thresholding.zip) File of Data for this Exercise. Data from: 

- Wheaton JM. 2008. [Uncertainty in Morphological Sediment Budgeting of Rivers](http://www.joewheaton.org/Home/research/projects-1/morphological-sediment-budgeting/phdthesis). Unpublished PhD Thesis, University of Southampton, Southampton, 412 pp. - *See Chapter 4 & Appendix D*

#### Prerequisite for this Exercise

- Some ArcGIS experience
- ArcGIS 10.X w/ Spatial Analyst Extension
- GCD Add-In

![GCD6_Form_ChangeDetectionConfiguration]({{ site.baseurl }}/assets/images/GCD6_Form_ChangeDetectionConfiguration.png)

## Overview of Thresholding

GCD Provides three primary ways for you to threshold data (all using [exclusion](http://forum.bluezone.usu.edu/gcd/viewtopic.php?f=40&t=117)):

#### Simple Minimum Level of Detection

In this video tutorial we explain how to use the GCD [`Change Detection`]({{ site.baseurl }}/gcd-command-reference/gcd-analysis-menu/c-geomorphic-change-detection-submenu/change-detection) panel to to a basic DoD Analysis with simple minimum level of detection thresholding. This is the equivalent of the raster calculator operation in the [03. Simple DoD in Raster Calculator]({{ site.baseurl }}/tutorials--how-to/ii-simple-dod-in-raster-calculator) tutorial.

<iframe width="560" height="315" src="https://www.youtube.com/embed/KFWfuaWPMuw" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

#### Propagated Errors

In this video tutorial we explain how to use the GCD [`Change Detection`]({{ site.baseurl }}/gcd-command-reference/gcd-analysis-menu/c-geomorphic-change-detection-submenu/change-detection) panel to to a DoD Analysis with propagated errors (from error rasters in the [`Survey Library`]({{ site.baseurl }}/system/errors/NodeNotFound?suri=wuid:gx:3ed05905e41de6f6)) to establish the level of detection threshold. 

<iframe width="560" height="315" src="https://www.youtube.com/embed/ZB3qrOZOyH8" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

#### Probability

In this video tutorial we explain how to use the GCD [`Change Detection`]({{ site.baseurl }}/gcd-command-reference/gcd-analysis-menu/c-geomorphic-change-detection-submenu/change-detection) panel to to a DoD Analysis with probabilistic thresholding based on a confidence interval.

<iframe width="560" height="315" src="https://www.youtube.com/embed/IUaicRVUsog" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

This tutorial corresponds with topic [G. Thresholding Alternatives]({{ site.baseurl }}/Help/Workshops/workshop-topics/1-Principles/g-thresholding-alternatives) in the [GCD Workshop]({{ site.baseurl }}). See the lecture materials [there]({{ site.baseurl }}/Help/Workshops/workshop-topics/1-Principles/g-thresholding-alternatives) for more background on the underlying theories for these methods.

#### Extra:
[How to Make your own Elevation Change Distribtuions]({{ site.baseurl }}/tutorials--how-to/v-thresholding-w-raster-calculator/custom-elevation-change-distributions)



### Step by Step

#### Part 1 - Simple minLoD

![ExcerciseI1]({{ site.baseurl }}/assets/images/ExcerciseI1.png)
How to do in GCD 7:
<iframe width="560" height="315" src="https://www.youtube.com/embed/olZiDNeg8Q4" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

How to do in GCD 6:
<iframe width="560" height="315" src="https://www.youtube.com/embed/Lk5XHLasGZA" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

#### Part 2 - Propagated Error

![ExcerciseI2]({{ site.baseurl }}/assets/images/ExcerciseI2.png)

How to do in GCD 7:
<iframe width="560" height="315" src="https://www.youtube.com/embed/nqkamJv2KQ0" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

How to do in GCD 6:
<iframe width="560" height="315" src="https://www.youtube.com/embed/_QQGBkFufvQ" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

#### Part 3 - Probabilistic 

![ExcerciseI3]({{ site.baseurl }}/assets/images/ExcerciseI3.png)
How to do in GCD 7:
<iframe width="560" height="315" src="https://www.youtube.com/embed/HEJ8nOEwPiw" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

How to do in GCD 6:
<iframe width="560" height="315" src="https://www.youtube.com/embed/1D0KpUrdCT8" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

### Related Online Help or Tutorials for this Topic

- [Parent Workshop Topic]({{ site.baseurl }}/Help/Workshops/workshop-topics/versions/3-day-workshop/2-errors-uncertainties/i-thresholding-alternatives)

- GCD Tutorial Topics:

- - [05. Thresholding in GCD]({{ site.baseurl }}/tutorials--how-to/v-thresholding-w-raster-calculator)

------

← [Back to Previous Tutorial]({{ site.baseurl }}/tutorials--how-to/workshop-tutorials/g-simple-DoD)       [Ahead to Next Tutorial]({{ site.baseurl }}/tutorials--how-to/workshop-tutorials/j_multimethoderror) →

------
<div align="center">
	<a class="hollow button" href="{{ site.baseurl }}/Help"><i class="fa fa-chevron-circle-left"></i>  Back to GCD Help </a>  
	<a class="hollow button" href="{{ site.baseurl }}/"><img src="{{ site.baseurl}}/assets/images/icons/GCDAddIn.png">  Back to GCD Home </a>  
</div>