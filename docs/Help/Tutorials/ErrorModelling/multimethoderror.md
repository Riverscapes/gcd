---
title:  Multi-Method Error Estimation
weight: 3
---

### Overview

This exercise shows how GCD can be used to produce a multi-method error surface for a hybrid survey. A hybrid survey is simply a term we use for surveys and DEMs that are derived from topographic surveys that involved the use of multiple survey methods (e.g. total station, ariborne LiDaR and multi-beam SONAR). One of the problems with hybrid surveys is that modeling elevation uncertainty might require a different approach for each survey method. How do we mix and match different methods, yet keep all the analysis together in one clean package? The GCD software now supports this increasingly common dilema. In this segment we will cover how to do this.

### Background
The GCD supports the use of hybrid multi-method surveys in the [`Survey Library`]({{ site.baseurl }}/system/errors/NodeNotFound?suri=wuid:gx:3ed05905e41de6f6). GCD allows you to load a survey method mask, which then is used to calculate errors using different methods within the different masks. In this video tutorial, we illustrate how this is done for two examples from a reach called Pats Cabin in Bridge Creek, Oregon:

<iframe width="560" height="315" src="https://www.youtube.com/embed/3JXnCzlstBQ" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>



### Data and Materials for Exercises

#### Datasets

- [Multi-Method Error Modelling in GCD (Rees)](https://s3-us-west-2.amazonaws.com/etalweb.joewheaton.org/GCD/Workshop/2017/Bologna/Exercise/I_MultiMethod.zip) 
- [J_ErrorEstimation.zip](http://etalweb.joewheaton.org/etal_workshops/GCD/2015_USU/J_ErrorEstimation.zip) File of Data for this Exercise from Grand Canyon (courtesy of [GCMRC](https://www.gcmrc.gov/))



Data from: 

Kaplinski, M., Hazel, J.E., Grams, P.E. and Davis, P.A. (2014). [Monitoring Fine-Sediment Volume in the Colorado River Ecosystem, Arizona—Construction and Analysis of Digital Elevation Models](http://pubs.usgs.gov/of/2014/1052/). Open File Report 2014-1052. Flagstaff, AZ, US Geological Survey, Grand Canyon Monitoring Research Center: 29 pp.

  - #### Prerequisite for this Exercise

    - Some ArcGIS experience
    - ArcGIS 10.X w/ Spatial Analyst Extension
    - GCD Add-In

### Step by Step

![ExcerciseJ]({{ site.baseurl }}assets/images/ExcerciseJ.png)

<iframe width="560" height="315" src="https://www.youtube.com/embed/1MVNmbM99e4" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

### Related Online Help or Tutorials for this Topic

- [Parent Workshop Topic]({{ site.baseurl }}/Help/Workshops/workshop-topics/versions/3-day-workshop/2-errors-uncertainties/j-approaches-to-estimating-dem-errors)

- GCD Tutorial Topics:

- - [09. Working with Hybrid Surveys in GCD]({{ site.baseurl }}/tutorials--how-to/ix-working-with-hybrid-surveys-in-gcd)

------

← [Back to Previous Tutorial]({{ site.baseurl }}/tutorials--how-to/workshop-tutorials/i-dod-thresholding)        [Ahead to Next Tutorial]({{ site.baseurl }}/tutorials--how-to/workshop-tutorials/k-statistical-methods-of-estimating-error) →

------
<div align="center">
	<a class="hollow button" href="{{ site.baseurl }}/Help"><i class="fa fa-chevron-circle-left"></i>  Back to GCD Help </a>  
	<a class="hollow button" href="{{ site.baseurl }}/"><img src="{{ site.baseurl}}/assets/images/icons/GCDAddIn.png">  Back to GCD Home </a>  
</div>