---
title: O. CHaMP FIS Error Modelling
---

### Overview

This exercise shows the impact on error model predictions of using different combinations of inputs. By way of example, we use the Columbia Habitat Monitoring Program (CHaMP) data and examples. The exercise helps reinforce the concepts of how FIS error modelling works in GCD. 

![Figure_05]({{ site.baseurl }}assets/images/Figure_05.png)

Figure from Bangen et al. (In Prep). 

### Data and Materials for Exercises

#### Datasets

[O_CHaMP_FIS.zip](http://etalweb.joewheaton.org/etal_workshops/GCD/2015_USU/O_CHaMP_FIS.zip) 

- `File of Data for this Exercise `

#### Prerequisite for this Exercise

- Some ArcGIS experience
- ArcGIS 10.X w/ Spatial Analyst Extension
- GCD Add-In

### Step by Step

####  **Exercise O: CHaMP FIS**

C:\0_GCD\Exercises\O_CHaMP_FIS

1.Start new ArcMap Document

**2. **Create New GCD Project - CHaMP_FIS in O

**3. **Add survey DEM for 2013

**4. **Load and/or Investigate 3-input, 4-input, and 5-input FIS in FIS Library

**5. **Load associated surfaces for 2013 DEM

**6. **Run 3-Input point density, slope degrees, interpolation error FIS model to create new error surface

**7. **Run 4-Input point density, slope degrees, interpolation error, 3D point quality FIS model to create new error surface

**8. **Run 5-Input point density, slope degrees, interpolation error, 3D point quality, roughness FIS model to create new error surface

**9. **Compare outputs from different error models 

<iframe width="560" height="315" src="https://www.youtube.com/embed/t7kLfLr-iTU" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

### Related Online Help or Tutorials for this Topic

- [Parent Workshop Topic](http://gcdworkshop.joewheaton.org/workshop-topics/versions/3-day-workshop/2-errors-uncertainties/o-fis-error-modelling-in-champ-bitbucket-repository)

- [GCD 6 Help]({{ site.baseurl }}/)

- - [Fuzzy Inference Systems for Modeling DEM Error]({{ site.baseurl }}/gcd-concepts/fuzzy-inference-systems-for-modeling-dem-error)
  - [ii. Derive Error Surface]({{ site.baseurl }}/gcd-command-reference/gcd-project-explorer/g-error-surfaces-context-menu/ii-derive-error-surface)
  - [F. Individual Associated Surface Context Menu]({{ site.baseurl }}/gcd-command-reference/gcd-project-explorer/f-individual-associated-surface-context-menu)

------

← [Back to Previous Tutorial]({{ site.baseurl }}/tutorials--how-to/workshop-tutorials/n-running-an-fis-dem-error-model)        [Ahead to Next Tutorial]({{ site.baseurl }}/tutorials--how-to/workshop-tutorials/p-building-your-own-fis) →