---
title: Essential Best Practices to Support Change Detection
---

#### Synopsis of Topic

Aside from surveying, there are some very basic problems that have to do with raster compatibility, which can wreak havoc in change detection analyses.  A DEM of Difference is a simple subtraction operation. To do the simple math on a cell-by-cell basis, the cells must line up (i.e. their grids must be orthogonal). Here we discuss the concepts of dimensional divisibility, orthogonality, the special case of concurrency, and data extents. We also explain how problems typically arise and what you can do to avoid them.

![NeedOrthogonalConcurrent]({{ site.baseurl }}/assets/images/NeedOrthogonalConcurrent.png)

#### Why we're Covering it

Even though the GCD Software and ArcGIS will 'take care' of the problem of compatible rasters so that you can do analyses, their doing so unnecessarily potentially introduces interpolation errors during re-sampling that you can (and should) avoid.

#### Learning Outcome Supported

This topic will help fulfill the following [primary learning outcome](http://gcdworkshop.joewheaton.org/syllabus/primary-learning-outcomes) for the workshop

- The fundamental background necessary to design effective repeat topographic monitoring campaigns and distinguish geomorphic changes from noise (with particular focus on restoration applications)

------

### Data and Materials for Exercises

#### Datasets

### `E_BestPractices.zip` File of Data for this Exercise [![img](http://gcdworkshop.joewheaton.org/_/rsrc/1422836703269/workshop-topics/versions/1-day-workshop/e-essential-best-practices-to-support-change-detection/winzip_icon_16.gif)](http://gcdworkshop.joewheaton.org/workshop-topics/versions/1-day-workshop/e-essential-best-practices-to-support-change-detection/winzip_icon_16.gif?attredirects=0)

#### Prerequisite for this Exercise

### Some ArcGIS experienceTopic [C](http://gcdworkshop.joewheaton.org/workshop-topics/versions/2-day-workshop/anzgg-workshop-topics/1-surveying-principles-change-detection/c-review-of-building-surfaces-from-raw-topographic-data)

#### Relevant Online Help or Tutorials for this Topic

### [GCD 6 Concept Reference: Best Practices](http://gcd6help.joewheaton.org/gcd-concepts/data-preparation---best-practices)

------

### Resources

#### Slides and/or Handouts

- ![img](http://gcdworkshop.joewheaton.org/_/rsrc/1429928387073/workshop-topics/versions/3-day-workshop/1-Principles/f-essential-best-practices-to-support-change-detection/pdfIcon.png)  [Lecture](http://etal.usu.edu/GCD/Workshop/2015_RRNW/Lectures/E_BestPractices.pdf)  

#### 

#### Relevant Links

- [Data Preparation Best Practices](http://gcd6help.joewheaton.org/gcd-concepts/data-preparation---best-practices) (includes video of a version of this lecture & an exercise) - GCD 5 Online Help

------

← [Previous Topic](http://gcdworkshop.joewheaton.org/workshop-topics/versions/1-day-workshop/c-review-of-building-surfaces-from-raw-topographic-data)            [Next Topic](http://gcdworkshop.joewheaton.org/workshop-topics/versions/1-day-workshop/f-traditional-approaches-to-change-detection) →