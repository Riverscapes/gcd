---
title: Introduction to Fuzzy Error Modelling
---

#### Synopsis of Topic

Fuzzy inference systems are a powerful and flexible tool that have a lot of utility in their application to estimating spatially variable elevation uncertainty. In this session we introduce the concepts of fuzzy logic, and fuzzy inference systems and show how Wheaton et al. (2010) applied these to producing robust, spatially variable estimates of DEM elevation uncertainty.  The cartoon below  (click on for larger version), shows an FIS with three inputs used to estimate elevation uncertainty in meters. 

![FIS_EuclidianDistance]({{ site.baseurl }}/assets/images/workshops/FIS_EuclidianDistance.png)

#### Why we're Covering it

Fuzzy inference systems are supported in the GCD Software and the appropriate construction, calibration and applications of FIS is a critical skill to develop. In this section we focus on understanding how they work and their application; whereas in the [next topic]({{ site.baseurl }}/Help/Workshops/workshop-topics/versions/3-day-workshop/2-errors-uncertainties/p-building-your-own-fis), we work on the construction and calibration aspects.

#### Learning Outcomes Supported

This topic will help fulfill the following [primary learning outcome(s)]({{ site.baseurl }}/Help/Workshops/syllabus/primary-learning-outcomes) for the workshop:

- A comprehensive overview of the theory underpinning geomorphic change detection
- The fundamental background necessary to design effective repeat topographic monitoring campaigns and distinguish geomorphic changes from noise (with particular focus on restoration applications)
- Hands-on instruction on use of the [GCD software](http://www.joewheaton.org/Home/research/software/GCD) through group-led and self-paced exercises

------

### Data and Materials for Exercises

#### Datasets

- [Dataset](http://etal.usu.edu/GCD/Workshop/2014_ANZGG/Excercises/M_FIS_Intro.zip)

#### Prerequisites for this Exercise

- Completed Exercises in Topics  [C]({{ site.baseurl }}/Help/Workshops/workshop-topics/versions/2-day-workshop/anzgg-workshop-topics/1-surveying-principles-change-detection/c-review-of-building-surfaces-from-raw-topographic-data), [E]({{ site.baseurl }}/Help/Workshops/workshop-topics/versions/2-day-workshop/anzgg-workshop-topics/1-surveying-principles-change-detection/e-traditional-approaches-to-change-detection), [H]({{ site.baseurl }}/Help/Workshops/workshop-topics/versions/2-day-workshop/anzgg-workshop-topics/1-surveying-principles-change-detection/h-thresholding-alternatives) , [I]({{ site.baseurl }}/Help/Workshops/workshop-topics/versions/2-day-workshop/anzgg-workshop-topics/1-surveying-principles-change-detection/i-approaches-to-estimating-dem-error) & [J]({{ site.baseurl }}/Help/Workshops/workshop-topics/versions/2-day-workshop/anzgg-workshop-topics/1-surveying-principles-change-detection/j-statistical-methods-for-error-modelling)

#### Relevant Online Help or Tutorials for this Topic

- [07. Applying Fuzzy Inference Systems in GCD](http://gcd5help.joewheaton.org/tutorials--how-to/vii-fuzzy-inference-systems-in-gcd) - GCD 5 Online Help Tutorial
- [Fuzzy Inference Systems for Modeling DEM Error](http://gcd5help.joewheaton.org/gcd-concepts/fuzzy-inference-systems-for-modeling-dem-error) - GCD 5 Online Help Concept Reference
- [Survey Library](http://gcd5help.joewheaton.org/gcd-command-reference/data-prep-menu/survey-library) - GCD 5 Online Help Command Reference

------

### Resources

#### Slides and/or Handouts

- This topic will help fulfill the following [primary learning outcome(s)]({{ site.baseurl }}/Help/Workshops/syllabus/primary-learning-outcomes) for the workshop:
- [![img](http://gcdworkshop.joewheaton.org/_/rsrc/1429979231746/workshop-topics/versions/3-day-workshop/2-errors-uncertainties/n-fuzzy-inference-systems/pdfIcon.png)]({{ site.baseurl }}/Help/Workshops/workshop-topics/versions/3-day-workshop/2-errors-uncertainties/n-fuzzy-inference-systems/pdfIcon.png?attredirects=0) [Lecture Slides](http://etal.usu.edu/GCD/Workshop/2014_ANZGG/M_FuzzyInferenceSystems.pdf)

#### Relevant or Cited Literature

- Jang JSR and Gulley N. 2009. [Fuzzy Logic Toolbox 2: User Guide](http://www.mathworks.com/access/helpdesk/help/pdf_doc/fuzzy/fuzzy.pdf), Matlab, Matlab, Natick, MA, 343 pp. 
- Milan DJ, Heritage GL, Large ARG and Fuller IC. 2011. [Filtering spatial error from DEMs: Implications for morphological change estimation](http://etal.usu.edu/ICRRR/GCD/Milan_Filtering%20Spatial%20Error%20from%20DEM%27s.pdf). Geomorphology. 125(1): 160-171. DOI: [10.1016/j.geomorph.2010.09.012](http://dx.doi.org/10.1016/j.geomorph.2010.09.012).
- Wheaton JM, Brasington J, Darby SE and Sear D. 2010. [Accounting for Uncertainty in DEMs from Repeat Topographic Surveys: Improved Sediment Budgets](http://www.joewheaton.org/Home/research/paper-downloads/Wheaton_etal_ESPL_DoD.pdf). Earth Surface Processes and Landforms. 35 (2): 136-156. DOI: [10.1002/esp.1886](http://dx.doi.org/10.1002/esp.1886).
- Wheaton JM. 2008. [Uncertainty in Morphological Sediment Budgeting of Rivers](http://www.joewheaton.org/Home/research/projects-1/morphological-sediment-budgeting/phdthesis). Unpublished PhD Thesis, University of Southampton, Southampton, 412 pp. Chapters 4 & 5.

------

← [Previous Topic]({{ site.baseurl }}/Help/Workshops/workshop-topics/versions/2-day-workshop/anzgg-workshop-topics/1-surveying-principles-change-detection/l-recap-of-day-preview-of-day-2)            [Next Topic]({{ site.baseurl }}/Help/Workshops/workshop-topics/versions/2-day-workshop/anzgg-workshop-topics/2-application-interpretations-of-change-detection-day-2/n-building-your-own-fis-error-models) →