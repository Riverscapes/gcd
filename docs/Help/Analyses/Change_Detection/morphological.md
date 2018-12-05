---
title: Morphological Analysis
---

<div class="float-right">
<img src="{{ site.baseurl }}/assets/images/CommandRefs/05_Analyses/cd/morph/morph.png"></div>
A GCD morphological analysis is a special sediment budgeting tool designed for fluvial environments. It takes a [budget segragation]({{site.baseurl}}/Help/Analyses/Budget_Segregation/budget-segregation.html) that was generated using a [directional mask]({{site.baseurl}}/Help/Inputs/Masks/directional-masks.html) of **channel spanning** polygons and calculates the accumulation or loss of bed material down the channel. This is a fairly simply accounting operation and does not involve any additional geospatial operations.

Users can interact with morphological analyses and adjust the density, porosity and boundary conditions. This provides a simple but powerful way to quickly assess the movement of bedload within each channel "cell".

Given the simplified nature of this tool it should be noted that each polygon must represent the full channel width as in the illustration to the right. Each cell in this highly braided channel spans the entire valley floor. No attempt is made to track the movement of sediment between annabranches.

# Create a Morphological Analysis

right click on a budget segregation to create a morphological analysis. Note that the context menu item is only available for budget segregations that were generated using a [directional mask](). The menu item is grayed out if you used a regular mask.

![Change Detection]({{ site.baseurl }}/assets/images/CommandRefs/05_Analyses/cd/morph/morph_cms.png)

In the form that appears, provide a unique name for the new analysis. All other inputs are not editable and only provided for context. Click Create ![Change Detection]({{ site.baseurl }}/assets/images/icons/Save.png) to perform the analysis. The morphological analysis results form will appear when it is complete.

![Change Detection]({{ site.baseurl }}/assets/images/CommandRefs/05_Analyses/cd/morph/morph_config.png)

# Context Menu

Right click on an individual morphological analysis to view it's results or delete it altogether.

![Change Detection]({{ site.baseurl }}/assets/images/CommandRefs/05_Analyses/cd/morph/morph_cms2.png)

## View Morphological Analysis Results

TODO

## Delete

Before attempting to delete a morphological analysis be sure to close any related files to prevent file lock issues. Then right click on an individual analysis and choose **Delete** to remove it from the GCD project.