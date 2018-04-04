---
title: Add Change Detection
---

The `Add Change Detection` command [![img]({{ site.baseurl }}/_/rsrc/1472842988678/gcd-command-reference/gcd-analysis-menu/c-geomorphic-change-detection-submenu/change-detection/Add.png)]({{ site.baseurl }}/gcd-command-reference/gcd-analysis-menu/c-geomorphic-change-detection-submenu/change-detection/Add.png?attredirects=0)  brings up a dialog, which allows you to specify how you wish to perform a change detection analysis.

![GCD6_Form_ChangeDetectionConfiguration]({{ site.baseurl }}/assets/images/GCD6_Form_ChangeDetectionConfiguration.png)

The command is only accessible when you are in a `*.gcd `[project]({{ site.baseurl }}/gcd-concepts/project) and you can only perform analyses when you have at least two overlapping surveys loaded in the [GCD Project Explorer]({{ site.baseurl }}/gcd-command-reference/gcd-project-explorer). Because the [GCD Project Explorer]({{ site.baseurl }}/gcd-command-reference/gcd-project-explorer)  enforces [orthogonality]({{ site.baseurl }}/gcd-concepts/data-preparation---best-practices) of all rasters loaded, we know that any two surveys will be compatible for analysis without requiring resampling. However, the rasters may have different extents, and the surveys may have different coverages (i.e. the NoData extents) within those rasters. As such, at the time of analysis, the GCD Software creates two temporary concurrent rasters, and only performs the DoD analysis where their data intersect. 

- [04. Basic DoD Analysis in GCD]({{ site.baseurl }}/tutorials--how-to/iv-basic-dod-analysis-in-gcd)

- [05. Thresholding w/ GCD]({{ site.baseurl }}/tutorials--how-to/v-thresholding-w-raster-calculator)

- [07. Applying Fuzzy Inference Systems in GCD]({{ site.baseurl }}/tutorials--how-to/vii-fuzzy-inference-systems-in-gcd)

- [06. Spatial Coherence & Bayesian Updating]({{ site.baseurl }}/tutorials--how-to/vi-spatial-coherence-bayesian-updaing)

  ​

-------
<div align="center">
  	<a class="hollow button" href="{{ site.baseurl }}/Help"><i class="fa fa-chevron-circle-left"></i>  Back to GCD Help </a>  
  	<a class="hollow button" href="{{ site.baseurl }}/"><img src="{{ site.baseurl}}/assets/images/icons/GCDAddIn.png">  Back to GCD Home </a>  
</div>