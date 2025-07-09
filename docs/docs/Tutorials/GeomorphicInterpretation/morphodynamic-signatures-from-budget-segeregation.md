---
title: Morphodynamic Signatures from Budget Segregation
sidebar_position: 4
slug: /Tutorials/GeomorphicInterpretation/morphodynamic-signatures-from-budget-segeregation
---
# Morphodynamic Signatures from Budget Segregation

### Overview

This exercise is to show you how DoDs can be easily interrogated to support . In GIS, this is basically a zonal statistics exercise, but in GCD we call it budget segregation and in addition to summary statistics you can derive all the standard GCD outputs for any polygon class (e.g. elevation change distributions, summary.xml, etc.). This provides an excellent means of hypothesis testing.

![Fig_16](/img/tutorials/Fig_16.png)

Example from Wheaton et al. (2013) of morphodynamic signatures derived from four DoDs that help in separating braiding mechanisms from other mechanisms of change. 

### Data and Materials for Exercises

#### Datasets

- [V_BudgetSeg.zip](https://s3-us-west-2.amazonaws.com/etalweb.joewheaton.org/GCD/GCD7/Tutorials/V_BudgetSeg.zip) File of Data for this Exercise 

#### Prerequisite for this Exercise

- Some ArcGIS experience
- ArcGIS 10.X w/ Spatial Analyst Extension
- GCD Add-In

------

### Step by Step

#### Exercise V: Part 1: Perform Budget Segregation

Suggested path: `C:\0_GCD\Exercises\V_BudgetSeg`

1. Start new ArcMap Document
2. Start a new GCD Project - Sulphur Creek in V
3. Add 2005 and 2006 DEM, perform DoD with threholding of your choosing
4. Add a budget segregation using provided shapefile
5. Explore results


How to do in gCD 7

import YouTube from "@site/docs/src/components/YouTube";

<YouTube embedId="2A2R4L8yiq8" title="How to do in GCD 7" />

How to do in GCD 6:
<YouTube embedId="IYUyBzTGMAA" title="How to do in GCD 6" />

#### Exercise V: Part 2: Derive GI Budget Segregation

C:\0_GCD\Exercises\V_BudgetSeg

1. Choose a GCD Project or Create a new one
2. Run raster calculator on thresholded DoD to get erosion and deposition areas
3. Convert integer raster output into polygon
4. Add text field(s) to polygon
5. Start classifying...
6. Use classified polygon in a budget segregation
7. Interrogate results

<YouTube embedId="W_zJNJ85dmc" title="Derive GI Budget Segregation" />

### Related Online Help or Tutorials for this Topic

- [Parent Workshop Topic](/Help/Workshops/workshop-topics/versions/3-day-workshop/3-Day3/v-budget-segregation)
- [GCD 6 Help](/)
  - [11. Budget Segregation](/tutorials--how-to/11-budget-segregation) - GCD 6 Online Help Tutorial
  - [Add Budget Segregation](/gcd-command-reference/gcd-project-explorer/l-individual-change-detection-context-menu/v-add-budget-segregation)  - GCD 6 Online Help Command Reference
  - [View Budget Segregation Results](/gcd-command-reference/gcd-project-explorer/n-individual-budget-segregation-context-menu/i-view-budget-segregation-results) - GCD 6 Online Help Command Reference

------

import { ToolsWrapper } from "@site/docs/src/components/ToolsWrapper/ToolsWrapper";

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
