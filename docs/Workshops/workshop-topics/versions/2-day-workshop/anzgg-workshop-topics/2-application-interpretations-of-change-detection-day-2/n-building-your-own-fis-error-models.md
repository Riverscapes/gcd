---
title: Building your own FIS Error Models
---

#### Synopsis of Topic

 An FIS can be constructed and implemented in a variety of ways. In the GCD software, we have adopted a standard format for fuzzy inference systems (*.fis) that is defined in Matlab's [Fuzzy Logic Toolbox](http://www.mathworks.com/products/fuzzylogic/). In this session, we show you how to build and edit your own FIS and emphasize the thought process behind choosing inputs, defining categories and their membership functions, building the rule table, calibrating the output, and verifying the behavior and performance of your FIS.

![aggregation_2]({{ site.baseurl }}/assets/images/aggregation_2.png)

An example from Matlab's [Fuzzy Logic Toolbox Support Documentation](http://www.mathworks.com/help/toolbox/fuzzy/fp351dup8.html) of an FIS for determining a tip.

#### Why we're Covering it

Fuzzy inference systems are supported in the GCD Software and the appropriate construction, calibration and applications of FIS is a critical skill to develop. In this section we focus on the construction and calibration aspects of building an FIS. 

#### Learning Outcomes Supported

This topic will help fulfill the following [primary learning outcome(s)]({{ site.baseurl }}/Help/Workshops/syllabus/primary-learning-outcomes) for the workshop:

- A comprehensive overview of the theory underpinning geomorphic change detection
- The fundamental background necessary to design effective repeat topographic monitoring campaigns and distinguish geomorphic changes from noise (with particular focus on restoration applications)
- Hands-on instruction on use of the [GCD software](http://www.joewheaton.org/Home/research/software/GCD) through group-led and self-paced exercises

------

### Data and Materials for Exercises

#### Data for this Excercise

- [N_BuildFIS.zip](http://etal.usu.edu/GCD/Workshop/2014_ANZGG/Excercises/N_BuildFIS.zip)

#### Prerequisites for this Exercise

- Completed Exercises in Topics [C]({{ site.baseurl }}/Help/Workshops/workshop-topics/versions/2-day-workshop/anzgg-workshop-topics/1-surveying-principles-change-detection/c-review-of-building-surfaces-from-raw-topographic-data), [E]({{ site.baseurl }}/Help/Workshops/workshop-topics/versions/2-day-workshop/anzgg-workshop-topics/1-surveying-principles-change-detection/e-traditional-approaches-to-change-detection), [H]({{ site.baseurl }}/Help/Workshops/workshop-topics/versions/2-day-workshop/anzgg-workshop-topics/1-surveying-principles-change-detection/h-thresholding-alternatives) , [I]({{ site.baseurl }}/Help/Workshops/workshop-topics/versions/2-day-workshop/anzgg-workshop-topics/1-surveying-principles-change-detection/i-approaches-to-estimating-dem-error) & [J]({{ site.baseurl }}/Help/Workshops/workshop-topics/versions/2-day-workshop/anzgg-workshop-topics/1-surveying-principles-change-detection/j-statistical-methods-for-error-modelling)

#### Relevant Online Help or Tutorials for this Topic

- [07. Applying Fuzzy Inference Systems in GCD](http://gcd5help.joewheaton.org/tutorials--how-to/vii-fuzzy-inference-systems-in-gcd) - GCD 5 Online Help Tutorial
- [Fuzzy Inference Systems for Modeling DEM Error](http://gcd5help.joewheaton.org/gcd-concepts/fuzzy-inference-systems-for-modeling-dem-error) - GCD 5 Online Help Concept Reference
- [Survey Library](http://gcd5help.joewheaton.org/gcd-command-reference/data-prep-menu/survey-library) - GCD 5 Online Help Command Reference
- [Matlab's Documenation on Fuzzy Inference Systems](http://www.mathworks.com/help/toolbox/fuzzy/fp351dup8.html)
- [Sets: An R package for simple Fuzzy Inference Systems](http://cran.r-project.org/web/packages/sets/sets.pdf)
- [FuzzyToolkitUoN: An R package for TypeII Fuzzy Inference Systems](http://ima.ac.uk/papers/wagner2011a.pdf)
- [FRBS: Fuzzy rule based systems for deriving fuzzy inference systems in R](http://dicits.ugr.es/software/FRBS/index.php?view=Introduction)

------

### Resources

#### Slides and/or Handouts

- ​

  [![img](http://gcdworkshop.joewheaton.org/_/rsrc/1429979102347/workshop-topics/versions/3-day-workshop/2-errors-uncertainties/p-building-your-own-fis/pdfIcon.png)]({{ site.baseurl }}/Help/Workshops/workshop-topics/versions/3-day-workshop/2-errors-uncertainties/p-building-your-own-fis/pdfIcon.png?attredirects=0) [Lecture Slides](http://etal.usu.edu/GCD/Workshop/2014_ANZGG/M_FuzzyInferenceSystems.pdf)

#### 

#### Relevant or Cited Literature

- Jang JSR and Gulley N. 2009. [Fuzzy Logic Toolbox 2: User Guide](http://www.mathworks.com/access/helpdesk/help/pdf_doc/fuzzy/fuzzy.pdf), Matlab, Matlab, Natick, MA, 343 pp. 
- Milan DJ, Heritage GL, Large ARG and Fuller IC. 2011. [Filtering spatial error from DEMs: Implications for morphological change estimation](http://etal.usu.edu/ICRRR/GCD/Milan_Filtering%20Spatial%20Error%20from%20DEM%27s.pdf). Geomorphology. 125(1): 160-171. DOI: [10.1016/j.geomorph.2010.09.012](http://dx.doi.org/10.1016/j.geomorph.2010.09.012).
- Wheaton JM, Brasington J, Darby SE and Sear D. 2010. [Accounting for Uncertainty in DEMs from Repeat Topographic Surveys: Improved Sediment Budgets](http://www.joewheaton.org/Home/research/paper-downloads/Wheaton_etal_ESPL_DoD.pdf). Earth Surface Processes and Landforms. 35 (2): 136-156. DOI: [10.1002/esp.1886](http://dx.doi.org/10.1002/esp.1886).
- Wheaton JM. 2008. [Uncertainty in Morphological Sediment Budgeting of Rivers](http://www.joewheaton.org/Home/research/projects-1/morphological-sediment-budgeting/phdthesis). Unpublished PhD Thesis, University of Southampton, Southampton, 412 pp. Chapters 4 & 5.

------

← [Previous Topic]({{ site.baseurl }}/Help/Workshops/workshop-topics/versions/2-day-workshop/anzgg-workshop-topics/2-application-interpretations-of-change-detection-day-2/m-introduction-to-fuzzy-error-modelling)            [Next Topic]({{ site.baseurl }}/Help/Workshops/workshop-topics/versions/2-day-workshop/anzgg-workshop-topics/2-application-interpretations-of-change-detection-day-2/o-self--paced-exercise--running-change-detection-in-gcd) →