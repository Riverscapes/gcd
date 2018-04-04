---
title: Budget Segregation
---

The `Budget Segregation` command brings up a dialog for spatially segregating any of your DoD results. 

![GCD6_Form_AddBudgetSegregation]({{ site.baseurl }}/assets/images/GCD6_Form_AddBudgetSegregation.png)

The `Budget Segregation` tool allows you to choose an existing *DoD *(from the current project) to segregate, then specify your *spatial Mask* (a polygon feature class or ShapeFile, that must have at least one text field for the categories), and then choosing the *text Identifier Field* in the *Mask* feature class that you wish to segregate by. The tool then identifies all of the unique categories and summarizes statistics and elevation change distributions within those respective polygons by category. The elevation change distributions are shown in comparison to the thresholded elevation change distribution form the input *DoD. *

See the [Budget Segregation]({{ site.baseurl }}/tutorials--how-to/11-budget-segregation) tutorial for videos on how to undertake an analysis.

------
<div align="center">
	<a class="hollow button" href="{{ site.baseurl }}/Help"><i class="fa fa-chevron-circle-left"></i>  Back to GCD Help </a>  
	<a class="hollow button" href="{{ site.baseurl }}/"><img src="{{ site.baseurl}}/assets/images/icons/GCDAddIn.png">  Back to GCD Home </a>  
</div>