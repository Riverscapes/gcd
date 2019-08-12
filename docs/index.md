---
title: GCD
---

<div class="float-right">
<img src="{{ site.baseurl }}/assets/images/GCD_SplashLogo_200.png">
<br>
<a href="https://riverscapes.xyz"><img src="{{ site.baseurl }}/assets/images/logos/RiverscapesConsortium_Logo_Black_BHS_200w.png"></a></div>

Welcome to the Geomorphic Change Detection (GCD) Software website. Here you will find [downloads]({{ site.baseurl }}/Download), [help]({{ site.baseurl }}/Help) and general information on the GCD software. GCD 7 is the current version of the software.

GCD is part of the [Riverscapes Consortium's](https://riverscapes.xyz) much larger family of tools for analyzing riverscapes.  GCD is the Riverscapes Consortium's  longest-standing, best developed software with the largest user base. GCD has users all over the world. 

<a class="button large" href="{{ site.baseurl }}/Download">
​        <img src="{{ site.baseurl}}/assets/images/icons/GCDAddIn.png">
​        &nbsp;&nbsp;Download GCD 7</a>
<a class="button large" href="{{ site.baseurl}}/Help"> <i class="fa fa-question-circle"></i>&nbsp;&nbsp;GCD 7 Help</a>
------
<div class="responsive-embed">
<iframe width="560" height="315" src="https://www.youtube.com/embed/JjbXDDThqIg" frameborder="0" allow="autoplay; encrypted-media" allowfullscreen></iframe></div>

<p class="caption">Animated GCD results produced by <a href="https://www.waikato.ac.nz/staff-profiles/people/jbrasing/">Prof. James Brasington</a> of the <a href="https://www.waikato.ac.nz/">University of Waikato</a>, New Zealand, illustrating changes on the <a href="https://en.wikipedia.org/wiki/Waiho_River">Waiho River</a>, New Zealand.</p>
------

## Background

The GCD software was developed primarily for topographic change detection in rivers, but will work for simple, raster-based change detection of any two surfaces. The volumetric change in storage is calculated from the difference in surface elevations from digital elevation models (DEMs) derived from repeat topographic surveys. 

![StandAlone_500w]({{ site.baseurl }}/assets/images/screen/StandAlone_500w.png)

As each DEM has an uncertain surface representation (which might vary in space and time), our ability to detect changes between surveys is highly dependent on surface representation uncertainties inherent in the individual DEMs. The fundamental problem is separating out the changes between the surveys that are due to topographic change as opposed to noise in the survey data. GCD provides a suite of tools for quantifying those uncertainties independently in each DEM and propagating them through to the DEM of difference. The program also provides ways for segregating the best estimates of change spatially using different types of masks. The overall suite of tools is more generically applicable to many different spatial raster-based change detection problems.

## GCD Research

The methodological development is described in ([Wheaton et al., 2010a](http://dx.doi.org/10.1002/esp.1886)), the Wheaton ([2008](http://sites.google.com/a/joewheaton.org/www/Home/research/projects-1/morphological-sediment-budgeting/phdthesis)) thesis, and the Wheaton et al. ([2010b](http://dx.doi.org/10.1002/rra.1305)) RRA paper. The [Matlab version of the code]({{ site.baseurl}}/Download/old_versions.html#dod-3-matlab--gcd-3) (DoD 3) is provided as supplemental information with the [ESPL paper](http://dx.doi.org/10.1002/esp.1886) so that readers can test or extend the code as they see fit for their purposes.

<div align="center">
	<a class="hollow button" href="https://www.researchgate.net/project/Geomorphic-Change-Detection" ><img src="{{ site.baseurl }}/assets/images/icons/ResearchGate_Icon.png">&nbsp;&nbsp; Check out GCD ResearchGate Project for Publications</a>
</div>

## What's Next for GCD

GCD 7 is always in development. How active that development is depends on our [funding levels]({{ site.baseurl }}/acknowledgements). We post our wish list of enhancements on our [GCD GitHub Repository](https://github.com/Riverscapes/gcd/issues?q=is%3Aopen+is%3Aissue+label%3Aenhancement).  If you have ideas of your own, or would like to support these efforts click below:
<div align="center">
	<a class="button success" href="{{ site.baseurl}}/Download/future-feature-request#want-to-donate-to-the-cause" ><i class="fa fa-paypal"></i>&nbsp;&nbsp;Donate Now</a>
	<a class="hollow button" href="{{ site.baseurl}}/Download/future-feature-request#making-feature-requests" ><i class="fa fa-lightbulb-o"></i>&nbsp;&nbsp;GCD Enhancements</a>
</div>
------

### Note on Terminology

**DoD** is an acronym for DEM (digital elevation model) of Difference (not [Department of Defense](https://www.defense.gov/)). DoDs are derived from repeat topographic surveys and used in change detection studies and morphological sediment budgeting. We use **GCD** to mean geomorphic change detection, not [Glen Canyon Dam](https://www.usbr.gov/uc/rm/crsp/gc).
