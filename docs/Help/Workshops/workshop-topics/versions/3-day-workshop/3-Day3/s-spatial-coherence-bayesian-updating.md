---
title: Spatial Coherence & Bayesian Updating
---

#### Synopsis of Topic

In Wheaton et al. (2010), a new method for looking at the spatial coherence of erosion and deposition was presented, which proposed that the probability that DoD predicted change is real depends in part on what is going on around you. In other words, if you are in a cell experiencing minor erosion (perhaps below the minimum level of detection), but every cell around you is also erosional, there is a higher probability that you actually are erosional. By contrast, if you are in a cell experiencing minor erosion and everything around you is depositional, then there is a lower probability that you are actually erosional. This simple concept was used to develop a 'spatial coherence filter', which is then converted into a conditional probability. Bayes theorem can then be evoked to modify the a priori probability and calculate a new probability (posterior) that change is real.

![FIG_2007-2006_NbrHoodCompare0001]({{ site.baseurl }}/assets/images/FIG_2007-2006_NbrHoodCompare0001.png)

Although this is a powerful concept, it can be misapplied. If your dataset exhibits systematic errors and bias, this filter can be problematic.

#### Why we're Covering it

This is one of the options in the GCD Change Detection panel and it is important to understand how it works and when it is appropriate to apply.

#### Learning Outcomes Supported

This topic will help fulfill the following [primary learning outcome(s)]({{ site.baseurl }}/Help/Workshops/syllabus/primary-learning-outcomes) for the workshop:

- A comprehensive overview of the theory underpinning geomorphic change detection
- The fundamental background necessary to design effective repeat topographic monitoring campaigns and distinguish geomorphic changes from noise (with particular focus on restoration applications)
- Methods for interpreting and segregating morphological sediment budgets quantitatively in terms of both geomorphic processes and changes in physical habitat
- Hands-on instruction on use of the [GCD software](http://www.joewheaton.org/Home/research/software/GCD) through group-led and self-paced exercises
- An opportunity to interact with experts on geomorphic monitoring and the software developers of GCD to help you make better use of your own data

------

### Resources

#### Exercise

- [Link to Exercise](http://gcd6help.joewheaton.org/tutorials--how-to/workshop-tutorials/s-bayesian-updating-excercise)

#### Slides and/or Handouts

- [2015 Lecture Slides](http://etalweb.joewheaton.org/etal_workshops/GCD/2015_USU/S_SpatialCoherenceBayesian.pdf)
- [2014 Lecture Slides](http://etal.usu.edu/GCD/Workshop/2014/Lectures/U_SpatialCoherenceBayesian.pdf)

#### Relevant or Cited Literature

- Milan DJ, Heritage GL, Large ARG and Fuller IC. 2011. [Filtering spatial error from DEMs: Implications for morphological change estimation](http://etal.usu.edu/ICRRR/GCD/Milan_Filtering%20Spatial%20Error%20from%20DEM%27s.pdf). Geomorphology. 125(1): 160-171. DOI: [10.1016/j.geomorph.2010.09.012](http://dx.doi.org/10.1016/j.geomorph.2010.09.012).
- Wheaton JM, Brasington J, Darby SE and Sear D. 2010. [Accounting for Uncertainty in DEMs from Repeat Topographic Surveys: Improved Sediment Budgets](http://www.joewheaton.org/Home/research/paper-downloads/Wheaton_etal_ESPL_DoD.pdf). Earth Surface Processes and Landforms. 35 (2): 136-156. DOI: [10.1002/esp.1886](http://dx.doi.org/10.1002/esp.1886).
- Wheaton JM. 2008. [Uncertainty in Morphological Sediment Budgeting of Rivers](http://www.joewheaton.org/Home/research/projects-1/morphological-sediment-budgeting/phdthesis). Unpublished PhD Thesis, University of Southampton, Southampton, 412 pp. Chapters 4 & 5.

------

← [Previous Topic]({{ site.baseurl }}/Help/Workshops/workshop-topics/versions/3-day-workshop/2-errors-uncertainties/v-recap-of-day-2-preview-of-day-3)            [Next Topic]({{ site.baseurl }}/Help/Workshops/workshop-topics/versions/3-day-workshop/3-Day3/t-interpreting-outputs-of-gcd) →