---
title: Using ToPCAT & GCD for Roughness Estimation & Surface Construction
---

### Background

#### Synopsis of Topic

Surface roughness is a primary driver in DEM error or uncertainty. In cases where , roughness alone may be the primary factor limiting in surface representation uncertainty for extremely dense point clouds (e.g. below) in which the individual grains are represented by 10's to 1000's of points. In other cases, roughness is at least a useful input into more complicated error models. 

![PointCloudGrainscale]({{ site.baseurl }}/assets/images/PointCloudGrainscale.png)

#### Why we're Covering it

High resolution point clouds may capture roughness (e.g. above), but it has been difficult for many to make use of these point clouds (see [zCloud](http://zcloudtools.boisestate.edu/)). With the advent of algorithms like [ToPCAT](http://gcd6help.joewheaton.org/gcd-concepts/topcat-decimation) and [PySESA](https://dbuscombe-usgs.github.io/pysesa/index.html), it is now possible to extract highly accurate roughness models based either on detrended variance of the point clouds (e.g. topographic amplitude as illustrated below) and even perform spectral analyses with programs like PySESA. 

![Fig2]({{ site.baseurl }}/assets/images/Fig2.png)

#### Learning Outcomes Supported

This topic will help fulfill the following [primary learning outcome(s)](http://gcdworkshop.joewheaton.org/syllabus/primary-learning-outcomes) for the workshop:

- A comprehensive overview of the theory underpinning geomorphic change detection
- The fundamental background necessary to design effective repeat topographic monitoring campaigns and distinguish geomorphic changes from noise (with particular focus on restoration applications)
- Hands-on instruction on use of the [GCD software](http://www.joewheaton.org/Home/research/software/GCD) through group-led and self-paced exercises

------

### Resources

#### Slides and/or Handouts

- [2015 Lecture Slides](http://etalweb.joewheaton.org/etal_workshops/GCD/2015_USU/M_ToPCATRoughness.pdf)
- [2014 Lecture Slides](http://etal.usu.edu/GCD/Workshop/2014/Lectures/O_ToPCAT_AndRoughnessEstimation_GCD_Workshop.pdf)

#### Exercises

- [Link to Exercise](http://gcd6help.joewheaton.org/tutorials--how-to/workshop-tutorials/m-deriving-roughness-with-topcat)

#### Relevant or Cited Literature

- Brasington, J., D. Vericat, and I. Rychkov. 2012. Modeling river bed morphology, roughness, and surface sedimentology using high resolution terrestrial laser scanning. Water Resources Research 48. DOI:[10.1029/2012wr012223](http://dx.doi.org/10.0.4.5/2012wr012223).
- Rychkov, I., J. Brasington, and D. Vericat. 2010. [Processing and Modelling on Terrestrial Point Clouds](http://code.google.com/p/point-cloud-tools/downloads/detail?name=1.5.pdf&can=2&q=). Institute of Geography and Earth Sciences, University of Wales, Aberystwyth, Wales.
- Rychkov, I., J. Brasington, and D. Vericat. 2012. Computational and methodological aspects of terrestrial surface analysis based on point clouds. Computers & Geosciences 42:64-70. DOI:[10.1016/j.cageo.2012.02.011](http://dx.doi.org/10.0.3.248/j.cageo.2012.02.011).

#### Relevant Links

- [ToPCAT](http://gcd6help.joewheaton.org/gcd-concepts/topcat-decimation) - Topographic Point Cloud Analysis Toolkit - *Developed by James Brasington, Damia Vericat & Igor Rychkov*
- [PySESA](https://dbuscombe-usgs.github.io/pysesa/index.html) - Python program for Spatially Explicit Spectral Analysis -*Developed by Daniel Buscombe*

------

← Back to [Previous Topic](http://gcdworkshop.joewheaton.org/workshop-topics/versions/3-day-workshop/2-errors-uncertainties/l-ground-based-lidar-survey-of-emriver-flume)             Ahead to [Next Topic](http://gcdworkshop.joewheaton.org/workshop-topics/versions/3-day-workshop/2-errors-uncertainties/n-fuzzy-inference-systems) →