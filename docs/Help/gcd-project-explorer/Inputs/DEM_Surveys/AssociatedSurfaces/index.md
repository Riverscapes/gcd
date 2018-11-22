---
title: Associated Surfaces Context Menu
weight: 2
---

![E_GCD6_ContextMenu_ProjectExplorer_AssocSurf]({{ site.baseurl }}/assets/images/E_GCD6_ContextMenu_ProjectExplorer_AssocSurf.png)

From the Associated Surfaces folder in the [GCD Project Explorer]({{ site.baseurl }}/gcd-command-reference/gcd-project-explorer), a right-click context menu reveals two commands:

#### Commands

- [i. Add Associated Surface]({{ site.baseurl }}/gcd-command-reference/gcd-project-explorer/e-associated-surface-context-menu/i-add-associated-surface)
- [ii. Add All Associated Surfaces to Map]({{ site.baseurl }}/gcd-command-reference/gcd-project-explorer/e-associated-surface-context-menu/ii-add-all-associated-surfaces-to-map)



Associated surfaces are not required, but can be derived and/or loaded when needed for deriving error surfaces using a [fuzzy inference system]({{ site.baseurl }}/gcd-concepts/fuzzy-inference-systems-for-modeling-dem-error).  Typical inputs include slope, point density, roughness and point quality, but the user can load any surface (assigning the [Undefined] type tag).  

![GCD6_Form_AddAssociatedSurface](../iv-add-associated-surface/%7B%7B%20site.baseurl%20%7D%7D/assets/images/GCD6_Form_AddAssociatedSurface.png)

By default, associated surfaces take on the name of the type of surface you derive or the name of the file you load. The *Original source* shows where the raster (if loaded) comes from, whereas the* Project raster path* shows where the associated surface will be stored within the [GCD Project]({{ site.baseurl }}/gcd-concepts/project). The type of associated surface is just a tag to make it helpful for identifying the correct input when deriving FIS error surfaces. There are five built-in types and one undefined type:

1. Undefined
2. 3D Point Quality
3. Point Density
4. Roughness
5. Slope Degrees
6. Slope Percent

The various ways to load an associated surface are shown in the sub commands:

#### Subcommands

- [1. Loading User Defined Associated Surface]({{ site.baseurl }}/gcd-command-reference/gcd-project-explorer/d-dem-context-menu/iv-add-associated-surface/1-loading-user-defined-associated-surface)
- [2. Deriving a Slope Analysis]({{ site.baseurl }}/gcd-command-reference/gcd-project-explorer/d-dem-context-menu/iv-add-associated-surface/2-deriving-a-slope-analysis)
- [3. Deriving Point Density]({{ site.baseurl }}/gcd-command-reference/gcd-project-explorer/d-dem-context-menu/iv-add-associated-surface/3-deriving-point-density)
- [4. Deriving Roughness]({{ site.baseurl }}/gcd-command-reference/gcd-project-explorer/d-dem-context-menu/iv-add-associated-surface/4-deriving-roughness)

------
<div align="center">
	<a class="hollow button" href="{{ site.baseurl }}/Help"><i class="fa fa-chevron-circle-left"></i>  Back to GCD Help </a>  
	<a class="hollow button" href="{{ site.baseurl }}/"><img src="{{ site.baseurl}}/assets/images/icons/GCDAddIn.png">  Back to GCD Home </a>  
</div>