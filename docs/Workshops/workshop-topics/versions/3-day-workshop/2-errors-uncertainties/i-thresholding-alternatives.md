---
title: Thresholding Alternatives
---

### Background

#### Synopsis of Topic

Today we will introduce a variety of techniques for estimating errors and propagating errors. Once you have an uncertainty estimate for your DoD, how do you use this information to distinguish real change from noise? Typically some sort of threshold is established, such as a minimum level of detection or a confidence interval on a probability that change is real. Then there is the question of what to do with data below that threshold. Exclusion is the most common method, but subtraction and weighted sums are other possibilities. In this session, we cover these alternatives and show you how to interact with these in the GCD software. 

![Fig_2007-2006_PW2_threshold]({{ site.baseurl }}/assets/images/tutorials/Fig_2007-2006_PW2_threshold.jpg)

Simple MinLoD

![SimpleMinLoD_Slide]({{ site.baseurl }}/assets/images/workshops/SimpleMinLoD_Slide.png)

Propagated Error

![Propagated_Slide]({{ site.baseurl }}/assets/images/workshops/Propagated_Slide.png)

Probabilistic

![Probabilistic_Slide]({{ site.baseurl }}/assets/images/workshops/Probabilistic_Slide.png)

#### Why we're Covering it

Thresholding is a key concept in developing estimates of net change and volumetric estimates of erosion and deposition. 

#### Learning Outcomes Supported

This topic will help fulfill the following [primary learning outcome(s)]({{ site.baseurl }}/Help/Workshops/syllabus/primary-learning-outcomes) for the workshop:

- A comprehensive overview of the theory underpinning geomorphic change detection
- The fundamental background necessary to design effective repeat topographic monitoring campaigns and distinguish geomorphic changes from noise (with particular focus on restoration applications)
- Hands-on instruction on use of the [GCD software](http://www.joewheaton.org/Home/research/software/GCD) through group-led and self-paced exercises

------

### Resources

#### Slides and/or Handouts

- [2015 Lecture](http://etalweb.joewheaton.org/etal_workshops/GCD/2015_USU/I_Thresholding.pdf)
- [2014 Lecture](http://etal.usu.edu/GCD/Workshop/2014/Lectures/I_Thresholding.pdf) 

#### Exercise

- [Link to Exercise](http://gcd6help.joewheaton.org/tutorials--how-to/workshop-tutorials/i-dod-thresholding)

#### Relevant Links

- See Greg Pasternack's Lecture - [M. Segment Scale GCD for the Lower Yuba River](http://borlecture.joewheaton.org/lecture-topics/4-case-studies-thu) from the[ USBoR Lecture Series](http://borlecture.joewheaton.org/)
- Nice review slides on [Error and Statistics](http://www.slu.edu/~rmccull2/resources/statistics_for_data.pdf) - *From Saint Louis University*

#### Relevant or Cited Literature

- Brasington J, Langham J and Rumsby B. 2003. Methodological sensitivity of morphometric estimates of coarse fluvial sediment transport. Geomorphology. 53(3-4): 299-316. DOI: [10.1016/S0169-555X(02)00320-3](http://dx.doi.org/10.1016/S0169-555X%2802%2900320-3)
- James LA, Hodgson ME, Ghoshal S and Latiolais MM.  2012. Geomorphic change detection using historic maps and DEM differencing: The temporal dimension of geospatial analysis. Geomorphology. 137(1): 181-198. DOI: [10.1016/j.geomorph.2010.10.039](http://dx.doi.org/10.1016/j.geomorph.2010.10.039).
- Lane SN, Westaway RM and Hicks DM. 2003. Estimation of erosion and deposition volumes in a large, gravel-bed, braided river using synoptic remote sensing. Earth Surface Processes and Landforms. 28(3): 249-271. DOI: [10.1002/esp.483](http://dx.doi.org/10.1002/esp.483).
- Milan DJ, Heritage GL, Large ARG and Fuller IC. 2011. [Filtering spatial error from DEMs: Implications for morphological change estimation](http://etal.usu.edu/ICRRR/GCD/Milan_Filtering%20Spatial%20Error%20from%20DEM%27s.pdf). Geomorphology. 125(1): 160-171. DOI: [10.1016/j.geomorph.2010.09.012](http://dx.doi.org/10.1016/j.geomorph.2010.09.012).
- Wheaton JM. 2008. [Uncertainty in Morphological Sediment Budgeting of Rivers](http://www.joewheaton.org/Home/research/projects-1/morphological-sediment-budgeting/phdthesis). Unpublished PhD Thesis, University of Southampton, Southampton, 412 pp. Chapters 4 & 5.

------

← [Previous Topic]({{ site.baseurl }}/Help/Workshops/workshop-topics/versions/3-day-workshop/1-Principles/h-recap-of-day-preview-of-tomorrow)            [Next Topic]({{ site.baseurl }}/Help/Workshops/workshop-topics/versions/3-day-workshop/2-errors-uncertainties/j-approaches-to-estimating-dem-errors) →