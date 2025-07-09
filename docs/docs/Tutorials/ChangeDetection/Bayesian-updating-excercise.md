---
title: Spatial Coherence Filter & Bayesian Updating Exercise
sidebar_position: 4
slug: /Tutorials/ChangeDetection/Bayesian-updating-excercise
---

# Spatial Coherence Filter & Bayesian Updating Exercise

### Overview

This exercise is to show you that you can use Bayes theorem to update the probability that change is real if you have other lines of evidence. In this case, we will assume that there are no systematic errors or bias , and look at the spatial coherence of erosion and deposition patterns. Below shows the contrast in spatial coherence filter outputs based on moving window size.

![FIG_2007-2006_NbrHoodCompare0001](/img/tutorials/FIG_2007-2006_NbrHoodCompare0001.png)

### Data and Materials for Exercises

#### Datasets

[S_SpatialCoherence.zip](https://s3-us-west-2.amazonaws.com/etalweb.joewheaton.org/etal_workshops/GCD/2015_USU/S_SpatialCoherence.zip) File of Data for this Exercise

#### Prerequisite for this Exercise

- Some ArcGIS experience
- ArcGIS 10.X w/ Spatial Analyst Extension
- GCD Add-In

------

### Step by Step

####  **Exercise S: Bayesian Updating/Spatial Coherence**

C:\0_GCD\Exercises\S_SpatialCoherence

**1. **Start new ArcMap Document

**2. **Start a new GCD Project

**3. **Add two DEM

**4. **Do any type of error modeling for both DEM

**5. **Perform change detection using probabilistic thresholding at 80% confidence

**6. **Re-calculate probabilistic change detection by applying Bayesian updating with:

- 5 x 5 moving window
- 10 x 10 moving window

**7.** Compare outputs



import YouTube from "../../src/components/YouTube";

<YouTube embedId="n0wH63OWFFU" title="Bayesian Updating Exercise" />

### Related Online Help or Tutorials for this Topic

- [Parent Workshop Topic](/Help/Workshops/workshop-topics/versions/3-day-workshop/3-Day3/s-spatial-coherence-bayesian-updating)
- [GCD 6 Help](/)
- [06. Spatial Coherence & Bayesian Updating](/tutorials--how-to/vi-spatial-coherence-bayesian-updaing)
  - [Add Change Detection](/gcd-command-reference/gcd-analysis-menu/c-geomorphic-change-detection-submenu/change-detection)
  - [i. Add Change Detection](/gcd-command-reference/gcd-project-explorer/j-change-detection-context-menu/i-add-change-detection)

#### References:

- Wheaton JM, Brasington J, Darby SE and Sear D. 2010. [Accounting for Uncertainty in DEMs from Repeat Topographic Surveys: Improved Sediment Budgets](http://www.joewheaton.org/Home/research/paper-downloads/Wheaton_etal_ESPL_DoD.pdf). Earth Surface Processes and Landforms. 35 (2): 136-156. DOI: [10.1002/esp.1886](http://dx.doi.org/10.1002/esp.1886).
- Wheaton JM. 2008. [Uncertainty in Morphological Sediment Budgeting of Rivers](http://www.joewheaton.org/Home/research/projects-1/morphological-sediment-budgeting/phdthesis). Unpublished PhD Thesis, University of Southampton, Southampton, 412 pp. Chapters 4 & 5.

------

← [Back to Previous Tutorial](/tutorials--how-to/workshop-tutorials/q-running-a-change-detection-w-fis)        [Ahead to Next Tutorial](/tutorials--how-to/workshop-tutorials/v-morphodynamic-signatures-from-budget-segeregation) →



------
import { ToolsWrapper } from "../../src/components/ToolsWrapper/ToolsWrapper";

<ToolsWrapper
  cards={[
    {
      title: "Back to GCD Help",
      toolUrl: "/Help",
      logoUrl: "/img/icons/GCDAddIn.png",
      description: "Return to the main GCD Help page."
    },
    {
      title: "Back to GCD Home",
      toolUrl: "/",
      logoUrl: "/img/icons/GCDAddIn.png",
      description: "Go to the GCD Home page."
    }
  ]}
  cardsize="sm"
/>