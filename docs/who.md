---
title: Who is the GCD Team?
weight: 3
---

## Project Inception & Vision

### The Early Days
<a href="http://joewheaton.org"><img class="float-left" src="{{ site.baseurl }}/assets/images/people/Wheaton_round.png"></a>
<a href="https://www.waikato.ac.nz/staff-profiles/people/jbrasing"><img class="float-right" src="{{ site.baseurl }}/assets/images/people/Brasington_round.png"></a>
The GCD Software ![Icon]({{ site.baseurl }}/assets/images/icons/GCDAddIn.png) was originally developed by [Joe Wheaton](http://www.joewheaton.org) ([Utah State University Department of Watershed Sciences](http://www.cnr.usu.edu/wats/)) and [James Brasington](https://www.waikato.ac.nz/staff-profiles/people/jbrasing) ([The University of Waikato (New Zealand)](https://www.waikato.ac.nz)). Joe and James started collaborating on the geomorphic change detection back in 2004 during Joe's PhD. During Joe's Masters with [Greg Pasternack](http://pasternack.ucdavis.edu/), he became familiar with James' early work on change detection and topographic surveying (Brasington et al. 2001, Brasington et al. 2003) and employed some of his methods in their work on the [Mokelumne](http://shira.lawr.ucdavis.edu/). James taught Joe how to code in Matlab, and they knocked up the first version (DoD 1) shortly thereafter. Joe coded up the Matlab code together simply to make his own life easier for processing DoDs and keeping a handle on the housekeeping associated with this sort of analysis. He soon realized that many in the geomorphology community were struggling with the same problems and decided to build an easier-to-use piece of software so folks could do this analysis in GIS and avoid making silly and costly mistakes.

### Evolving Vision

Joe was frustrated by how frequently researchers and practitioners were not doing even the most basic minimum level of detection analyses to convince their audiences the signals they were calculating were actually discernable from noise in the DEM data (a particularly pronounced problem in fluvial environments where elevation changes are low). Joe thought if you make it easy enough for people to do the analysis that they had no excuse not to, they might be able to incrementally raise the standard of practice. More to the point, if the tool could help with some of the common methodological hurdles, then it might help the community focus more on interpreting the outputs to test interesting hypotheses and develop new theories about morphodynamic evolution. 

<a href="http://northarrowresearch.com/#people"><img class="float-left" src="{{ site.baseurl }}/assets/images/people/Phlip_round.png"></a>
When Joe arrived at Utah State University in 2009, [Jack Scmidtt](https://qcnr.usu.edu/directory/schmidt_jack) quickly connected him with [Paul Grams](https://www.usgs.gov/staff-profiles/paul-grams) at USGS and some ICRRR funding, which enabled the GCD 4 development. Joe met [Philip Bailey](http://northarrowresearch.com/#people) (now of [North Arrow Research](http://northarrowresearch.com) then of ESSA Technologies) at a [NSF LiDAR Workshop](http://www.opentopography.org/index.php/resources/short_courses/lidar2_2010/) in 2010 in Boulder, Colorado. Joe was teaching a session on [DoD 3](https://github.com/joewheaton/DoD) and Philip was teaching a session on [the River Bathymetry Toolkit -RBT](https://www.fs.fed.us/rm/boise/AWAE/projects/river_bathymetry_toolkit.shtml). Like Joe, Philp also did his PhD in Geography from University of Southampton. The two started scheming at the workshop over ways to turn GCD into a more professional piece of software. Joe secured funding to make those schemes a reality, and the two have been working on GCD in various capacities ever since. In 2015, James,  Joe and [Damia Vericat](http://www.damiavericat.eu/) teamed back up to write a review article on the Morphological Approach for Gravel Bed Rivers ([Vericat et al., 2017](http://etal.joewheaton.org/new-fhc-publications/gravel-bed-rivers-chapter-on-revisiting-the-morphological-approach-finally-out)). That then laid the ground work for a 2016 NERC Proposal, which was funded and has involved a partnership with Regional Councils in New Zealand and the Scottish Environmental Protection Agency to refine GCD so that it can be used to support their management of gravel bed rivers.      


## Development Team

<a href="http://northarrowresearch.com/#people"><img class="float-right" src="{{ site.baseurl }}/assets/images/people/Matt_round.png"></a>
The newest version of the GCD is currently under development by [North Arrow Research](http://northarrowresearch.com/) and [ET-AL](http://etal.joewheaton.org/). Since [GCD 5]({{ site.baseurl }}/Download/old_versions.html#gcd-5),   [Philip Bailey](http://northarrowresearch.com/#people) (North Arrow Research) has been the lead developer and architect of GCD. These days, [Philip Bailey](http://northarrowresearch.com/#people) and [Matt Reimer](http://northarrowresearch.com/#people) ([North Arrow Research](http://northarrowresearch.com/)) are the primary developers of GCD.  Matt has really upped the game and exposed the teams to a wealth of new technologies and simpler, more elegant solutions. Matt, Philip and Joe envisioned and launced the [Riverscapes Consortium](http://riverscapes.xyz):

[![Riverscapes]({{ site.baseurl }}/assets/images/logos/RiverscapesConsortium_Logo_Black_BHS_200w.png)](http://riverscapes.xyz)


### Other Significant Contributors
- [James Hensleigh](http://etal.joewheaton.org/james-hensleigh.html) (now USGS, formerly USU [ETAL](http://etal.joewheaton.org)) was a major contributor to [GCD 5]({{ site.baseurl }}/Download/old_versions.html#gcd-5) & [6]({{ site.baseurl }}/Download/old_versions.html#gcd-6) and pushed the development of the FIS Development Assistant and MBES Tools..
- Frank Poulson (ESSA) has been instrumental in the development of GCD 5 through 7.
- [Sara Bangen](http://etal.joewheaton.org/sara-bangen1.html) (USU ETAL) has been instrumental in developing FIS models and the CHaMP GCD Testing.
- Nick Ochoski (ESSA) was part of the initial development of [GCD 5]({{ site.baseurl }}/Download/old_versions.html#gcd-5).
-  [ESSA Technologies](http://essa.com) generously invested in kind donations of development time in [GCD 5]({{ site.baseurl }}/Download/old_versions.html#gcd-5) development. 
-  [Chris Garrard](http://www.gis.usu.edu/~chrisg/) (USU [RSGIS Lab](http://www.gis.usu.edu/)) was the primary developer of GCD 4.
-  [Greg Pasternack](http://pasternack.ucdavis.edu/) has been an important sounding board throughout the development of GCD.

[![NAR]({{ site.baseurl }}/assets/images/logos/NA_Logo_150pxTall.png)](http://northarrowresearch.com/)
[![USU]({{ site.baseurl }}/assets/images/logos/etal.png)](http://etal.joewheaton.org)
[![ESS]({{ site.baseurl }}/assets/images/logos/essa_logo_blank.png)](http://essa.com)

## User Support and Testing

We are indebted to many helpful and patient Beta Testers, including all the participants of the GCD Workshops/Help/Workshops/), [Sara Bangen](http://etal.joewheaton.org/sara-bangen1.html), [Rocko Brown](http://www.fishsciences.net/rocko-brown-ph-d/),[Nicole Czarnomski](http://etal.joewheaton.org/nicole-czarnomski.html), [Kenny DeMeurichy](http://etal.joewheaton.org/kenny-demeurichy.html), [Steve Fortney](https://www.terraqua.biz/bios), [Paul Grams](https://www.usgs.gov/staff-profiles/paul-grams), [Andrew Hill](https://www.eco-logical-research.com/team), [Alan Kasprak](http://etal.joewheaton.org/alan-kasprak.html), Eric Larson, [Ryan Leary](https://digitalcommons.usu.edu/gradreports/351/), Chuck Podolack, Robert Ross, [Keelin Schaffrath](https://scholar.google.com/citations?user=IYnFqG0AAAAJ&hl=en), [Carol Volk](http://www.southforkresearch.org/), and [Cara Walter](http://oregonstate.edu/gradwater/cara-walter) (among many others). 

------
<div align="center">
    <a class="hollow button" href="{{ site.baseurl }}/"><i class="fa fa-chevron-circle-left"></i>  Back to Home </a>  
</div>