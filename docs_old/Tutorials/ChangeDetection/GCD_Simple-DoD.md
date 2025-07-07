---
title: Simple Change Detection in Raster Calculator vs. GCD
weight: 1
---


### Overview


This exercise is intended to contrast how to build a <a href="{{ site.baseurl }}/Concepts/dod.html">DoD</a> (DEM of Difference) with simple change detection minimum level of detection thresholding in ArcGIS's raster calculator versus using the GCD.



<div class="row">
    <div class="columns medium-4 small-12">
        <div class="card">
            <div style="text-align: center; padding: 5px 5px 0 5px">
                <img src="{{ site.baseurl }}/assets/images/datasets/sulphur_200h.png">
            </div>
            <div class="card-section">
                <h4>Sulphur Creek, CA</h4>
                <ul>
                    <li>300m of gravel bed river near <i class="fa fa-map-marker"></i> <a href="https://www.google.com/maps/place/38%C2%B029'44.0%22N+122%C2%B028'09.0%22W/@38.4958086,-122.4803136,4904m/data=!3m1!1e3!4m5!3m4!1s0x0:0x0!8m2!3d38.49555!4d-122.469166">St. Helena California</a> that underwent a flood in December 2005.</li>
                    <li>Two surveys</li>
                    <li>0.5m cell resolution</li>
                    <li>RTKGPS and Total Station</li>
                 </ul>
                 <a class="button" href="http://etalweb.joewheaton.org/etal_workshops/GCD/2015_USU/G_SimpleDoD.zip"><i class="fa fa-file-archive-o" aria-hidden="true"></i> Excercise Data for Part 1 & 2</a>
            </div>
        </div>     
    </div>
    <div class="columns medium-4 small-12">
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
                  </ul>
                  <a class="button" href="https://s3-us-west-2.amazonaws.com/etalweb.joewheaton.org/GCD/GCD7/Tutorials/GeoTERM_Feshie.zip"><i class="fa fa-file-archive-o" aria-hidden="true"></i> Excercise Data for Part 3</a>
            </div>
        </div>
    </div>
</div>


#### Prerequisite for this Exercise

- Some ArcGIS experience
- ArcGIS 10.X w/ Spatial Analyst Extension
- GCD Add-In

------

### Step by Step

#### Part 1A Raster Calculator

![ExcerciseG1A]({{ site.baseurl }}/assets/images/tutorials/ExcerciseG1A.png)

<iframe width="560" height="315" src="https://www.youtube.com/embed/rda6aVCPF9Q" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

In this video tutorial, we show you how to do a simple DoD Analysis in ArcGIS 10 just using the rater calculator.

<iframe width="560" height="315" src="https://www.youtube.com/embed/YHbDByz6HO4" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

The example data used in this tutorial are from Sulphur Creek, Napa County, California.

For corresponding lecture slides and more context, see [this topic]({{ site.baseurl }}/Help/Workshops/workshop-topics/1-Principles/e-traditional-approaches-to-change-detection) from [GCD Workshop]({{ site.baseurl }}).


#### Part 1B Threshold

![ExcerciseG1B]({{ site.baseurl }}/assets/images/tutorials/ExcerciseG1B.png)

![ExcerciseG1B_steps]({{ site.baseurl }}/assets/images/tutorials/ExcerciseG1B_steps.png)

<iframe width="560" height="315" src="https://www.youtube.com/embed/_lbqCraoi0U" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

#### Part 2 GCD

![ExcerciseG2]({{ site.baseurl }}/assets/images/tutorials/ExcerciseG2.png)

<iframe width="560" height="315" src="https://www.youtube.com/embed/8KrOMnpBATY" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

#### Part 3 Practice in GCD

![ExcerciseG3]({{ site.baseurl }}/assets/images/tutorials/ExcerciseG3.png)

##### GCD 7 Tutorial Video
<iframe width="560" height="315" src="https://www.youtube.com/embed/MI6p4DfT3Sk" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>
##### GCD 6 Tutorial Video
<iframe width="560" height="315" src="https://www.youtube.com/embed/khJE7dRsIKQ" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

### Related Online Help or Tutorials for this Topic

- [Parent Workshop Topic]({{ site.baseurl }}/Help/Workshops/workshop-topics/versions/3-day-workshop/1-Principles/f-essential-best-practices-to-support-change-detection)

- GCD Tutorial Topics:

- - [03. Simple DoD in Raster Calculator]({{ site.baseurl }}/tutorials--how-to/ii-simple-dod-in-raster-calculator)
  - [04. Basic DoD Analysis in GCD]({{ site.baseurl }}/tutorials--how-to/iv-basic-dod-analysis-in-gcd)
  - [05. Thresholding in GCD]({{ site.baseurl }}/tutorials--how-to/v-thresholding-w-raster-calculator)

- [Building DEMs](http://gis.joewheaton.org/assignments/labs/lab-07---building-dems) Lab - From my [Advanced GIS course](http://gis.joewheaton.org/)


------
<div align="center">  
<a class="button" href="{{ site.baseurl }}/tutorials--how-to/workshop-tutorials/f-essential-best-practices-to-support-change-detection"> ← Previous Tutorial </a>
<a class="button" href="{{ site.baseurl }}/tutorials--how-to/workshop-tutorials/i-dod-thresholding"> Next Tutorial →</a>  
</div>



------
<div align="center">
	<a class="hollow button" href="{{ site.baseurl }}/Help"><i class="fa fa-chevron-circle-left"></i>  Back to GCD Help </a>  
	<a class="hollow button" href="{{ site.baseurl }}/"><img src="{{ site.baseurl}}/assets/images/icons/GCDAddIn.png">  Back to GCD Home </a>  
</div>