---
title: V. Morphodynamic Signatures from Budget Segeregation
---

### Overview

This exercise is to show you how DoDs can be easily interrogated to support . In GIS, this is basically a zonal statistics exercise, but in GCD we call it budget segregation and in addition to summary statistics you can derive all the standard GCD outputs for any polygon class (e.g. elevation change distributions, summary.xml, etc.). This provides an excellent means of hypothesis testing.

![Fig_16]({{ site.baseurl }}assets/images/Fig_16.png)

Example from Wheaton et al. (2013) of morphodynamic signatures derived from four DoDs that help in separating braiding mechanisms from other mechanisms of change. 

### Data and Materials for Exercises

#### Datasets

[V_BudgetSeg.zip](http://etalweb.joewheaton.org/etal_workshops/GCD/2015_USU/V_BudgetSeg.zip) File of Data for this Exercise 

#### Prerequisite for this Exercise

- Some ArcGIS experience
- ArcGIS 10.X w/ Spatial Analyst Extension
- GCD Add-In

------

### Step by Step

#### **Exercise V: Part 1: Perform Budget Segregation**

C:\0_GCD\Exercises\V_BudgetSeg

**1. **Start new ArcMap Document

**2. **Start a new GCD Project - Sulphur Creek in V

**3. **Add 2005 and 2006 DEM, perform DoD with threholding of your choosing

**4. **Add a budget segregation using provided shapefile

**5. **Explore results

<iframe width="560" height="315" src="https://www.youtube.com/embed/IYUyBzTGMAA" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

#### **Exercise V: Part 2: Derive GI Budget Segregation**

C:\0_GCD\Exercises\V_BudgetSeg

#### **1.**Choose a GCD Project or Create a new one

#### 2. **Run raster calculator on thresholded DoD to get erosion and deposition areas**

#### **3. Convert integer raster output into polygon**

#### 4. **Add text field(s) to polygon**

#### 5. Start classifying...

#### 6. Use classified polygon in a budget segregation

#### 7. Interrogate results

<iframe width="560" height="315" src="https://www.youtube.com/embed/W_zJNJ85dmc" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

### Related Online Help or Tutorials for this Topic

- [Parent Workshop Topic](http://gcdworkshop.joewheaton.org/workshop-topics/versions/3-day-workshop/3-Day3/v-budget-segregation)

- [GCD 6 Help]({{ site.baseurl }}/)

- - [11. Budget Segregation]({{ site.baseurl }}/tutorials--how-to/11-budget-segregation) - GCD 6 Online Help Tutorial
  - [Add Budget Segregation]({{ site.baseurl }}/gcd-command-reference/gcd-project-explorer/l-individual-change-detection-context-menu/v-add-budget-segregation)  - GCD 6 Online Help Command Reference
  - [View Budget Segregation Results]({{ site.baseurl }}/gcd-command-reference/gcd-project-explorer/n-individual-budget-segregation-context-menu/i-view-budget-segregation-results) - GCD 6 Online Help Command Reference

------

← [Back to Previous Tutorial]({{ site.baseurl }}/tutorials--how-to/workshop-tutorials/s-bayesian-updating-excercise)

