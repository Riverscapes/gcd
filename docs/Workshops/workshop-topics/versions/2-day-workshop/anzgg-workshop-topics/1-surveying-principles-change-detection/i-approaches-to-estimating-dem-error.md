---
title: Approaches to Estimating DEM Error
---

### Background

![GCD6_Form_AddAssociatedSurface_ALT]({{ site.baseurl }}/assets/images/GCD6_Form_AddAssociatedSurface_ALT.png)

#### Synopsis of Topic

There are many different techniques for estimating error, ranging from spatially uniform, simple classification (e.g. differentiating between wet, dry and vegetated), and various statistical methods. In this topic we will discuss several of these methods and techniques and then leverage ToPCAT (Topographic Point Cloud Analysis Toolkit) to derive roughness using a locally detrended standard deviation from a high resolution point cloud. 

#### Why we're Covering it

Estimating DEM uncertainty (typically as a vertical DEM error) independently for each DEM survey is a critical step in change detection. In this topic and corresponding exercise (I) we will get experience doing just this. 

#### Learning Outcomes Supported

This topic will help fulfill the following [primary learning outcome]({{ site.baseurl }}/Help/Workshops/syllabus/primary-learning-outcomes) for the workshop: 

- A comprehensive overview of the theory underpinning geomorphic change detection
- The fundamental background necessary to design effective repeat topographic monitoring campaigns and distinguish geomorphic changes from noise (with particular focus on restoration applications)
- Hands-on instruction on use of the [GCD software](http://www.joewheaton.org/Home/research/software/GCD) through group-led and self-paced exercises

------

### Data and Materials for Exercises

In this exercise we will look at using the ToPCAT tool to derive roughness from high resolution point clouds and add then how to use the derived roughness service as a GCD error model. We will use some multi-beam echo sounding (i.e. SONAR) survey. 

#### Datasets

- [I_ToPCAT.zip](http://etal.usu.edu/GCD/Workshop/2014_ANZGG/Excercises/I_ToPCAT.zip)

#### Relevant Online Help or Tutorials for this Topic

- [ToPCAT Preparation Tool - GCD6 Command Reference](http://gcd6help.joewheaton.org/gcd-command-reference/data-prep-menu/e-topcat-menu/i-topcat-preparation-tool)
- [ToPCAT - GCD6 Command Reference](http://gcd6help.joewheaton.org/gcd-command-reference/data-prep-menu/e-topcat-menu/ii-topcat-point-cloud-decimation-tool)
- [Simple ToPCAT Roughness Tool - GCD 6 Command Reference](http://gcd6help.joewheaton.org/gcd-command-reference/gcd-analysis-menu/b-roughness-analysis-submenu/i-simple-topcat-roughness)
- [Deriving Roughness - GCD 6 Command Reference](http://gcd6help.joewheaton.org/gcd-command-reference/gcd-project-explorer/d-dem-context-menu/iv-add-associated-surface/4-deriving-roughness)

------

### Resources

#### Slides and/or Handouts

-  [Lecture](http://etal.usu.edu/GCD/Workshop/2014_ANZGG/I_SpatiallyVariableDEMError.pdf)

#### Relevant or Cited Literature

- Brasington J, Vericat D and Rychkov I. 2012. Modeling river bed morphology, roughness, and surface sedimentology using high resolution terrestrial laser scanning. Water Resources Research. 48(11). DOI: 10.1029/2012wr012223.
- Milan DJ, Heritage GL, Large ARG and Fuller IC. 2011. Filtering spatial error from DEMs: Implications for morphological change estimation. Geomorphology. 125(1): 160-171. DOI: 10.1016/j.geomorph.2010.09.012.

------

← Back to [Previous Topic]({{ site.baseurl }}/Help/Workshops/workshop-topics/versions/2-day-workshop/anzgg-workshop-topics/1-surveying-principles-change-detection/h-thresholding-alternatives)         Ahead to [Next Topic]({{ site.baseurl }}/Help/Workshops/workshop-topics/versions/2-day-workshop/anzgg-workshop-topics/1-surveying-principles-change-detection/j-statistical-methods-for-error-modelling) →