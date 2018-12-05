---
title: Thresholding Alternatives
---

#### Synopsis of Topic

In the morning, we introduced simple techniques for estimating errors and propagating errors. Once you have an uncertainty estimate for your DoD, how do you use this information to distinguish real change from noise? Typically some sort of threshold is established, such as a minimum level of detection or a confidence interval on a probability that change is real. Then there is the question of what to do with data below that threshold. Exclusion is the most common method, but subtraction and weighted sums are other possibilities. In this session, we cover these alternatives and show you how to interact with these in the GCD software. 

![Fig_2007-2006_PW2_threshold]({{site.baseurl }}/assets/images/tutorials/Fig_2007-2006_PW2_threshold.jpg)

#### Why we're Covering it

Thresholding is a key concept in developing estimates of net change and volumetric estimates of erosion and deposition. 

#### Learning Outcomes Supported

This topic will help fulfill the following [primary learning outcome(s)]({{ site.baseurl }}/Help/Workshops/syllabus/primary-learning-outcomes) for the workshop:

- A comprehensive overview of the theory underpinning geomorphic change detection
- The fundamental background necessary to design effective repeat topographic monitoring campaigns and distinguish geomorphic changes from noise (with particular focus on restoration applications)
- Hands-on instruction on use of the [GCD software](http://www.joewheaton.org/Home/research/software/GCD) through group-led and self-paced exercises

------

### Data and Materials for Exercises

#### Datasets

- [`H_Thresholding.zip`](http://etal.usu.edu/GCD/Workshop/2015_RRNW/Excercises/H_Thresholding.zip) [![img](http://gcdworkshop.joewheaton.org/_/rsrc/1422837060873/workshop-topics/versions/1-day-workshop/h-thresholding-alternatives/winzip_icon_16.gif)]({{ site.baseurl }}/Help/Workshops/workshop-topics/versions/1-day-workshop/h-thresholding-alternatives/winzip_icon_16.gif?attredirects=0)

#### Prerequisite for this Exercise

- Completed Exercise in Topics [C]({{ site.baseurl }}/Help/Workshops/workshop-topics/versions/3-day-workshop/1-Principles/c-review-of-building-surfaces-from-raw-data),  [E]({{ site.baseurl }}/Help/Workshops/workshop-topics/versions/3-day-workshop/1-Principles/g_traditionalGCD), & [F](http://gcdworkshop.joewheaton.org/system/errors/NodeNotFound?suri=wuid:gx:27d471080239b4a)

#### Relevant Online Help or Tutorials for this Topic

- [05. Thresholding with GCD](http://gcd5help.joewheaton.org/tutorials--how-to/v-thresholding-w-raster-calculator) - GCD 5 Online Help Tutorial
- [Change Detection](http://gcd5help.joewheaton.org/gcd-command-reference/gcd-analysis-menu/change-detection) - GCD 5 Online Help Command Reference

------

### Resources

#### Slides and/or Handouts

- ![img](http://gcdworkshop.joewheaton.org/_/rsrc/1429929134873/workshop-topics/versions/3-day-workshop/2-errors-uncertainties/i-thresholding-alternatives/pdfIcon.png)  [Lecture](http://etal.usu.edu/GCD/Workshop/2015_RRNW/Lectures/H_Thresholding.pdf) 

#### 

#### Relevant Links

- See [this forum post](http://forum.bluezone.usu.edu/gcd/viewtopic.php?f=40&t=117) on new GCD features for Thresholding
- See Greg Pasternack's Lecture - [M. Segment Scale GCD for the Lower Yuba River](http://borlecture.joewheaton.org/lecture-topics/4-case-studies-thu) from the[ USBoR Lecture Series](http://borlecture.joewheaton.org/)

#### Relevant or Cited Literature

#### 

- Brasington J, Langham J and Rumsby B. 2003. Methodological sensitivity of morphometric estimates of coarse fluvial sediment transport. Geomorphology. 53(3-4): 299-316. DOI: [10.1016/S0169-555X(02)00320-3](http://dx.doi.org/10.1016/S0169-555X%2802%2900320-3)
- James LA, Hodgson ME, Ghoshal S and Latiolais MM.  2012. Geomorphic change detection using historic maps and DEM differencing: The temporal dimension of geospatial analysis. Geomorphology. 137(1): 181-198. DOI: [10.1016/j.geomorph.2010.10.039](http://dx.doi.org/10.1016/j.geomorph.2010.10.039).
- Lane SN, Westaway RM and Hicks DM. 2003. Estimation of erosion and deposition volumes in a large, gravel-bed, braided river using synoptic remote sensing. Earth Surface Processes and Landforms. 28(3): 249-271. DOI: [10.1002/esp.483](http://dx.doi.org/10.1002/esp.483).
- Milan DJ, Heritage GL, Large ARG and Fuller IC. 2011. [Filtering spatial error from DEMs: Implications for morphological change estimation](http://etal.usu.edu/ICRRR/GCD/Milan_Filtering%20Spatial%20Error%20from%20DEM%27s.pdf). Geomorphology. 125(1): 160-171. DOI: [10.1016/j.geomorph.2010.09.012](http://dx.doi.org/10.1016/j.geomorph.2010.09.012).
- Wheaton JM. 2008. [Uncertainty in Morphological Sediment Budgeting of Rivers](http://www.joewheaton.org/Home/research/projects-1/morphological-sediment-budgeting/phdthesis). Unpublished PhD Thesis, University of Southampton, Southampton, 412 pp. Chapters 4 & 5.

------

← [Previous Topic]({{ site.baseurl }}/Help/Workshops/workshop-topics/versions/1-day-workshop/g-introduction-to-gcd-software)            [Next Topic]({{ site.baseurl }}/Help/Workshops/workshop-topics/versions/1-day-workshop/i-approaches-to-estimating-dem-error) →