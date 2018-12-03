---
title: Areas Of Interest
---

<div class="float-right">
<img src="{{ site.baseurl }} /assets/images/CommandRefs/00_ProjectExplorer/inputs/masks/aoi/aoi.png"></div>
Area of Interest (AOI) allow you to control the regions within your data that are considered during [change detection]() analyses. This is useful if your data have a larger extent than the area of change that you are particularly interested in. This is common in river channels where you might want to ignore out-of-channel changes and focus on specific areas within the channel itself. Alternatively you might have [LiDAR]() data in which you want to ignore the vegetation and focus instead on real topographic change.

The same The same basic requirements apply to AOIs that apply to all types of mask Polygon ShapeFiles:

* Must use the same spatial reference as the DEM Surveys in the project.
* Must contain at least one feature.
* Must not contain any multi-part features.
* Must not contain any features with null geometries.

# Add Existing AOI