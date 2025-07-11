---
title: M. Deriving Roughness with ToPCAT
sidebar_position: 1
slug: /Tutorials/ErrorModelling/StatisticalMethods/deriving-roughness-with-topcat
---

### Overview

This exercise shows how GCD can be used for coming up with statistical models of error. We will cover coincident point analysis, interpolation error, and using the ToPCAT algorithm to look at statistics from high resolution point clouds within a user-defined moving voxel. 

### Data and Materials for Exercises

#### Datasets

[M_ToPCATRoughness.zip](http://etalweb.joewheaton.org/etal_workshops/GCD/2015_USU/M_TopCATRoughness.zip) File of Data for this Exercise

#### Prerequisite for this Exercise

- Some ArcGIS experience
- ArcGIS 10.X w/ Spatial Analyst Extension
- GCD Add-In

### Step by Step

#### Run ToPCAT to get Roughness Surface (Simple)



![ExcerciseM1](/img/tutorials/ExcerciseM1.png)

#### Run ToPCAT as Associated Surface to Get Roughness

![ExcerciseM2](/img/tutorials/ExcerciseM2.png)

**Exercise M - Part 3: Use Associated Surface Panel to Create Roughness**

C:\0_GCD\Exercises\M_ToPCATRoughness\TLS_Flume\Exercise

1. Start new ArcMap Document

**2. **Open GCD Project  ToPCATRoughnessTLSFlume.gcd

**3. **For each DEM in GCD Project Select Add Associated Surface and from this menu choose to Calculate a Roughness Raster.

4. Use the *.pts file associated with each flume run as input to the Generate Surface Roughness Raster dialog.

5. Due to scale of flume data you may want to adjust symbology.


<YouTubeEmbed videoId="v5xd9-UQook" title="ToPCAT Roughness Video Tutorial" />

### Related Online Help or Tutorials for this Topic

- [Parent Workshop Topic](/Help/Workshops/workshop-topics/versions/3-day-workshop/2-errors-uncertainties/m-using-topcat-gcd-for-roughness-estimation-surface-construction)
- [GCD 6 Help](/)
  - [ToPCAT Submenu](/gcd-command-reference/data-prep-menu/e-topcat-menu)
  - [Simple ToPCAT Roughness](/gcd-command-reference/gcd-analysis-menu/b-roughness-analysis-submenu/i-simple-topcat-roughness)
  - [Create Roughness as Associated Surface](/gcd-command-reference/gcd-project-explorer/d-dem-context-menu/iv-add-associated-surface/4-deriving-roughness)

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