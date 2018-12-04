---
title: Change Detection
---

<div class="float-right">
<img src="{{ site.baseurl }}/assets/images/Fly_GCD_DoD_Cartoon.png"></div>

The ability to perform a change detection by subtracting two topographic surveys is the fundamental reason that the GCD Software exists.

While at it's heart this appears to be a simple operation, it's the process of calculating and then visualizing the uncertainty that is the focus of the GCD software.

This section of the GCD help describes the basic process of performing a change detection analysis and then visualizing the results. The following sections expand on this information with more advanced topics:

* Exploring [Change Detection Results]()
* [Batch change detection]()
* [Multi-epoch change detection]()
* [Budget segregating]() a change detection
* [Morphological analysis]() of a change detection
* [Intercomparing]() change detections

# Calculating a Change Detection

Before performing any kind of change detection you must have at least two [DEM Surveys]() in your GCD project and depending on the way that you want to represent uncertainty, each DEM might require an [error surface]().

Expand the Analyses section of the [GCD Explorer]() and right click on Change Detection. 

![Change Detection]({{ site.baseurl }}/assets/images/CommandRefs/05_Analyses/cd/add_change_detection_cms.png)

The change detection form requires the following inputs:

* Choose a unique **analysis name** that does not already exists in the GCD project. The GCD software will suggest a name based on the inputs selected below. You can override this should you choose.
* The output folder is defined by the GCD software and cannot be changed.
* **Surfaces** - both [DEM Surveys]() and [reference surfaces]() can be used as the new and old rasters in a change detection analysis.
* **Error** surfaces are mandatory if choose the propagated error or probabilistic thresholding method, but are not required if you are using the minimum level of detection method.
* **Area of Interest** - leave this option as the intersection of the new and old surface data extents to calculate change everywhere that both surfaces possess data. This is the default. Alternatively you can select an [area of interset]() to ignore certain areas of your data if you want to restrict the calculation.
* **Uncertainty Analysis Method**

![Change Detection]({{ site.baseurl }}/assets/images/CommandRefs/05_Analyses/cd/change_config.png)
