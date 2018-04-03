---
title: Essential Best Practices to Support Change Detection
---

#### Synopsis of Topic

Aside from surveying, there are some very basic problems that have to do with raster compatibility, which can wreak havoc in change detection analyses.  A DEM of Difference is a simple subtraction operation. To do the simple math on a cell-by-cell basis, the cells must line up (i.e. their grids must be orthogonal). Here we discuss the concepts of dimensional divisibility, orthogonality, the special case of concurrency, and data extents. We also explain how problems typically arise and what you can do to avoid them.

#### Why we're Covering it

Even though the GCD Software and ArcGIS will 'take care' of the problem of compatible rasters so that you can do analyses, their doing so unnecessarily potentially introduces interpolation errors during re-sampling that you can (and should) avoid.

#### Learning Outcome Supported

This topic will help fulfill the following [primary learning outcome](http://gcdworkshop.joewheaton.org/syllabus/primary-learning-outcomes) for the workshop:

- The fundamental background necessary to design effective repeat topographic monitoring campaigns and distinguish geomorphic changes from noise (with particular focus on restoration applications)

![NeedOrthogonalConcurrent]({{ site.baseurl }}/assets/images/NeedOrthogonalConcurrent.png)

------

### Resources

#### Slides and/or Handouts

- [2015 Lecture](http://etalweb.joewheaton.org/etal_workshops/GCD/2015_USU/F_BestPractices.pdf)
- [2014 Lecture](http://etal.usu.edu/GCD/Workshop/2015_RRNW/Lectures/E_BestPractices.pdf)  

#### Exercise

- [Link to Exercise](http://gcd6help.joewheaton.org/tutorials--how-to/workshop-tutorials/f-essential-best-practices-to-support-change-detection)

#### Relevant Links

- [Data Preparation Best Practices](http://gcd6help.joewheaton.org/gcd-concepts/data-preparation---best-practices) (includes video of a version of this lecture & an exercise) - GCD 5 Online Help

------

← [Previous Topic](http://gcdworkshop.joewheaton.org/workshop-topics/versions/3-day-workshop/1-Principles/e-field-trip-to-logan-river)            [Next Topic](http://gcdworkshop.joewheaton.org/workshop-topics/versions/3-day-workshop/1-Principles/g_traditionalGCD) →