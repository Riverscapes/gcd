---
title: DoD Thresholding
weight: 2
---

<div>
<h1> Overview</h1>

<p>This exercise contrasts the three most common forms of thresholding DoDs: i) Simple minimum level of detection, ii) propagated errors, and iii) probabilistic. We use the GCD Add-In to facilitate the analysis. </p>

<h1>Data and Materials for Exercises</h1>

<h2>Datasets</h2>
<p>
<ul>
	<li> Wheaton JM. 2008. <a href="http://www.joewheaton.org/Home/research/projects-1/morphological-sediment-budgeting/phdthesis">Uncertainty in Morphological Sediment Budgeting of Rivers</a>. Unpublished PhD Thesis, University of Southampton, Southampton, 412 pp. - *See Chapter 4 & Appendix D*</li>
</ul>
</p>


<div style="float: right;width:200px; margin:0 0 0 10px">
        <div class="card">
            <div style="text-align: center; padding: 5px 5px 0 5px">
                <img src="{{ site.baseurl }}/assets/images/datasets/feshie_200h.png">
            </div>
            <div class="card-section">
                <h4>River Feshie, Scotland, UK</h4>
                <ul>
                    <li>700m braided gravel bed river in the <i class="fa fa-map-marker"></i> <a href="https://www.google.com/maps/place/57%C2%B000'41.4%22N+3%C2%B054'16.1%22W/@57.0099348,-3.9000104,6821m/data=!3m1!1e3!4m5!3m4!1s0x0:0x0!8m2!3d57.01149!4d-3.90446">Scottish Cairngorm mountains</a>.</li>
                    <li>5 annual surveys</li>
                    <li>Mix of RTKGPS and Total Station</li>
                    <li>1m cell resolution</li>
                    <li><a href="https://s3-us-west-2.amazonaws.com/etalweb.joewheaton.org/GCD/GCD7/Tutorials/GeoTERM_Feshie.zip">Download</a></li>
                </ul>
            </div>
        </div>

</div>

</div>
-----
#### Prerequisite for this Exercise

[I_Thresholding.zip](http://etalweb.joewheaton.org/etal_workshops/GCD/2015_USU/I_Thresholding.zip) File of Data for this Exercise. Data from: 


- Some ArcGIS experience
- ArcGIS 10.X w/ Spatial Analyst Extension
- GCD Add-In

![GCD6_Form_ChangeDetectionConfiguration]({{ site.baseurl }}/assets/images/workshops/GCD6_Form_ChangeDetectionConfiguration.png)

# Overview of Thresholding

GCD Provides three primary ways for you to threshold data (all using [exclusion](http://forum.bluezone.usu.edu/gcd/viewtopic.php?f=40&t=117)):

#### Simple Minimum Level of Detection

In this video tutorial we explain how to use the GCD [`Change Detection`]({{ site.baseurl }}/gcd-command-reference/gcd-analysis-menu/c-geomorphic-change-detection-submenu/change-detection) panel to to a basic DoD Analysis with simple minimum level of detection thresholding. This is the equivalent of the raster calculator operation in the [03. Simple DoD in Raster Calculator]({{ site.baseurl }}/tutorials--how-to/ii-simple-dod-in-raster-calculator) tutorial.

<div  class="responsive-embed widescreen">
	<iframe width="560" height="315" src="https://www.youtube.com/embed/KFWfuaWPMuw" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>
</div>

#### Propagated Errors

In this video tutorial we explain how to use the GCD [`Change Detection`]({{ site.baseurl }}/gcd-command-reference/gcd-analysis-menu/c-geomorphic-change-detection-submenu/change-detection) panel to to a DoD Analysis with propagated errors (from error rasters in the [`Survey Library`]({{ site.baseurl }}/system/errors/NodeNotFound?suri=wuid:gx:3ed05905e41de6f6)) to establish the level of detection threshold. 
<div  class="responsive-embed widescreen">
<iframe width="560" height="315" src="https://www.youtube.com/embed/ZB3qrOZOyH8" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>
</div>

#### Probability

In this video tutorial we explain how to use the GCD [`Change Detection`]({{ site.baseurl }}/gcd-command-reference/gcd-analysis-menu/c-geomorphic-change-detection-submenu/change-detection) panel to to a DoD Analysis with probabilistic thresholding based on a confidence interval.

<div  class="responsive-embed widescreen">
<iframe width="560" height="315" src="https://www.youtube.com/embed/IUaicRVUsog" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>
</div>

This tutorial corresponds with topic [G. Thresholding Alternatives]({{ site.baseurl }}/Help/Workshops/workshop-topics/1-Principles/g-thresholding-alternatives) in the [GCD Workshop]({{ site.baseurl }}). See the lecture materials [there]({{ site.baseurl }}/Help/Workshops/workshop-topics/1-Principles/g-thresholding-alternatives) for more background on the underlying theories for these methods.

#### Extra:
[How to Make your own Elevation Change Distribtuions]({{ site.baseurl }}/tutorials--how-to/v-thresholding-w-raster-calculator/custom-elevation-change-distributions)



### Step by Step

#### Part 1 - Simple minLoD

![ExcerciseI1]({{ site.baseurl }}/assets/images/tutorials/ExcerciseI1.png)
How to do in GCD 7:
<div  class="responsive-embed widescreen">
<iframe width="560" height="315" src="https://www.youtube.com/embed/olZiDNeg8Q4" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>
</div>

How to do in GCD 6:
<div  class="responsive-embed widescreen">
<iframe width="560" height="315" src="https://www.youtube.com/embed/Lk5XHLasGZA" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>
</div>

#### Part 2 - Propagated Error

![ExcerciseI2]({{ site.baseurl }}/assets/images/tutorials/ExcerciseI2.png)

How to do in GCD 7:
<div  class="responsive-embed widescreen">
<iframe width="560" height="315" src="https://www.youtube.com/embed/nqkamJv2KQ0" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>
</div>

How to do in GCD 6:
<div  class="responsive-embed widescreen">
<iframe width="560" height="315" src="https://www.youtube.com/embed/_QQGBkFufvQ" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>
</div>

#### Part 3 - Probabilistic 

![ExcerciseI3]({{ site.baseurl }}/assets/images/tutorials/ExcerciseI3.png)
How to do in GCD 7:
<div  class="responsive-embed widescreen">
<iframe width="560" height="315" src="https://www.youtube.com/embed/HEJ8nOEwPiw" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>
</div>

How to do in GCD 6:
<div  class="responsive-embed widescreen">
<iframe width="560" height="315" src="https://www.youtube.com/embed/1D0KpUrdCT8" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>
</div>


------
<div align="center">
	<a class="hollow button" href="{{ site.baseurl }}/Help"><i class="fa fa-chevron-circle-left"></i>  Back to GCD Help </a>  
	<a class="hollow button" href="{{ site.baseurl }}/"><img src="{{ site.baseurl}}/assets/images/icons/GCDAddIn.png">  Back to GCD Home </a>  
</div>