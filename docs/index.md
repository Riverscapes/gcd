---
title: GCD
---

<img class="float-right" src="{{ site.baseurl }}/assets/images/GCD_SplashLogo_200.png">

Welcome to the GCD Software website. Here you will find downloads, help and background on the GCD software. GCD 6 is currently under development and will be released in late April, 2014.

[Download the latest version of the GCD Software]({{ site.baseurl }}/download)

# Background

The GCD software was developed primarily for [morphological sediment budgeting](http://sites.google.com/a/joewheaton.org/www/Home/research/projects-1/morphological-sediment-budgeting) in rivers. The volumetric change in storage is calculated from the difference in surface elevations from digital elevation models (DEMs) derived from repeat topographic surveys. As each DEM has an uncertain surface representation (which might vary in space and time), our ability to detect changes between surveys is highly dependent on surface representation uncertainties inherent in the individual DEMs. The fundamental problem is separating out the changes between the surveys that are due to geomorphic change as opposed to noise in the survey data. GCD provides a suite of tools for quantifying those uncertainties independently in each DEM and propagating them through to the DEM of difference. The program also provides ways for segregating the best estimates of change spatially using different types of masks. The overall suite of tools is more generically applicable to many different spatial raster-based change detection problems.

The methodological development is described in ([Wheaton et al., 2010a](http://dx.doi.org/10.1002/esp.1886)), the Wheaton ([2008](http://sites.google.com/a/joewheaton.org/www/Home/research/projects-1/morphological-sediment-budgeting/phdthesis)) thesis, and the Wheaton et al. ([2010b](http://dx.doi.org/10.1002/rra.1305)) RRA paper. The [Matlab version of the code](http://gcd.joewheaton.org/downloads/older-versions/dod-3-0) (DoD 3) is provided as supplemental information with the [ESPL paper](http://dx.doi.org/10.1002/esp.1886) so that readers can test or extend the code as they see fit for their purposes.

# Funding

Current funding for the Geomorphic Change Detection Software development (GCD 6) is being provided by:

* The National Science Foundation (Geoinformatics: [Award #1226127](http://www.nsf.gov/awardsearch/showAward?AWD_ID=1226127) - '[Making Point Clouds Useful for Earth Science](http://etal.joewheaton.org/projects/current-projects/development-of-integrated-airborne-and-ground-based-lidar-tools-for-earth-sciences)')  - [ZCloudTools](http://zcloudtools.boisestate.edu/)
* Idaho Power Company
* Bonneville Power Administration via [Eco Logical Research](http://ecologicalresearch.net)

![NSF]({{ site.baseurl }}/assets/images/logos/nsf1.gif)
![IPC]({{ site.baseurl }}/assets/images/logos/IPC_GreenOnTransparent.png)
![BPA]({{ site.baseurl }}/assets/images/logos/bpaTransparent.png)
![ELR]({{ site.baseurl }}/assets/images/logos/ELRLogo.png)

Previous funding for GCD Software Development was provided by:

* The USGS's [Grand Canyon Monitoring & Research Center](http://www.gcmrc.gov/gcmrc.aspx)
* The [US Army Corps of Engineers Kansas City District](http://www.nwk.usace.army.mil/)
* Utah State University [ICRRR](https://www.cnr.usu.edu/icrrr/) (Intermountain Center for River Rehabilitation and Restoration)

![USGS]({{ site.baseurl }}/assets/images/logos/USGS_logo.png)
![USACE]({{ site.baseurl }}/assets/images/logos/612px-US-ArmyCorpsOfEngineers-Logo.svg.png)
![ICRRR]({{ site.baseurl }}/assets/images/logos/ICRR_Logo.png)

# Software Development

The GCD Software was originally developed by [Joe Wheaton](http://www.joewheaton.org) ([Utah State University Department of Watershed Sciences](http://www.cnr.usu.edu/wats/)) and [James Brasington](http://www.reesscan.org/meet-the-team/brasington) ([Queen Mary University](http://www.geog.qmul.ac.uk/staff/brasingtonj.html)). The newest version of the GCD is currently under development by [North Arrow Research](http://northarrowresearch.com/) and [ET-AL](http://etal.joewheaton.org/). The current development team consists of [Philip Bailey](http://www.essa.com/team/index.html#pb) (North Arrow Research), Matt Reimer ([North Arrow Research](http://northarrowresearch.com/)), and Joe Wheaton (USU). Nick Ochoski (ESSA) and [Frank Poulsen](http://www.essa.com/team/index.html#fp) (ESSA) were instrumental in the initial development of GCD 5. [ESSA Technologies](http://essa.com) generously invested in kind donations of development time in GCD 5 development. [Chris Garrard](http://www.gis.usu.edu/~chrisg/) (USU [RSGIS Lab](http://www.gis.usu.edu/)) was the primary developer of GCD 4.

![NAR]({{ site.baseurl }}/assets/images/logos/NA_Logo_150pxTall.png)
![ESS]({{ site.baseurl }}/assets/images/logos/ESSATechnologies_500x500.png)
![USU]({{ site.baseurl }}/assets/images/logos/etal.png)

We are indebted to many helpful and patient Beta Testers, including all the participants of the GCD Workshops, Sara Bangen, Rocko Brown, Nicole Czarnomski, Kenny DeMeurichy, Paul Grams, Alan Kasprak, Eric Larson, Ryan Leary, Chuck Podolack, Robert Ross, Keelin Schaffrath, and Cara Walter. 

# What's Next

GCD 7 is currently in development. See [Extending GCD Software]() for latest developments (GCD 6).

### Note:

DoD is an acronym for DEM (digital elevation model) of Difference (not Department of Defense). DoDs are derived from repeat topographic surveys and used in change detection studies and [morphological sediment budgeting](http://www.joewheaton.org/Home/research/projects-1/morphological-sediment-budgeting).
