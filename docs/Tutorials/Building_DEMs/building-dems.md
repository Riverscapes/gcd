---
title: Building DEMs from Topographic Survey Data
weight: 1
---

<div style="float: right;width:200px; margin:0 0 0 10px">
    <div class="card">
        <div style="text-align: center; padding: 5px 5px 0 5px">
            <img src="{{ site.baseurl }}/assets/images/datasets/pats_cabin_200t.png">
        </div>
        <div class="card-section">
            <h4>Pats Cabin</h4>
            <p>500m channel in <i class="fa fa-map-marker"></i> <a href="https://www.google.com/maps/place/44%C2%B041'23.7%22N+120%C2%B017'00.4%22W/@44.6899308,-120.285625,776m/data=!3m1!1e3!4m5!3m4!1s0x0:0x0!8m2!3d44.689927!4d-120.283431">Bridge Creek, Oregon</a>
            </p>
            <a class="button" href="http://etalweb.joewheaton.org/etal_workshops/GCD/2015_USU/D_DEM.zip"><i class="fas fa-file-archive"></i> Exercise Data</a>
        </div>
    </div> 
</div>

<div>
<p>Having good DEMs is critical for effective change detection. In this series of video tutorials, we lay out how to build DEMs from raw topographic data (e.g. rtkGPS or total station data). A similar procedure is <A href="http://gis.joewheaton.org/assignments/labs/lab-07---building-dems/task-4">available for airborne LiDaR data</a>.</p>

<h1>Overview</h1>

<p>This exercise is intended to highlight the fundamentals of building a DEM from raw topographic data. In this tutorial we are provided with some raw topographic survey data. Your job is to build a DEM from that data (of appropriate resolution), show the water depth overlaid on the DEM, and pull a longitudinal profile and some cross sections off the DEM. You will use a triangular irregular network (TIN) to interpolate between your raw topographic survey data and produce a continuous surface that you will later convert to a raster DEM.</p>

<p>NOTE: These instructions are for ArcGIS 10 and are primarily provided in the form of video tutorials. As an additional reference, you might find the 'Using ArcGIS 9.3.X to Construct and Manipulate DEMs' tutorial listed in the <a href="http://gis.joewheaton.org/assignments/labs/lab06-1#TOC-Follow-up-Activities:">main Lab 6 page</a> helpful (it uses different data, and is for the old version of ArcGIS, but goes through a similar sequence of steps to arrive at the same end point in Part I of the document; one significant difference is the absence of TIN editing in ArcGIS 9.3.X).</p>
</div>

# Data

Everything you need to create a TIN can be found in the  `PatsCabinSurveyPoints.csv` file and the zip file link above. This video tutorial goes through what is included in the data and how to import it into ArcGIS.

Although not covered in the video tutorial, I would suggest exporting the `PatsCabinSurveyPoints.csv` table you imported as X-Y points to a ShapeFile or feature class to use in the construction of the TIN (I named mine `TopoSurveyPoints`).

<div class="responsive-embed">
    <iframe width="560" height="315" src="https://www.youtube.com/embed/jb-UY6S6r8I" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>
</div>

## Prerequisite for this Exercise

- Some ArcGIS experience
- ArcGIS 10.x with 3D Analyst Extension
- Some familiarity with DEMs (e.g. [Topic C]({{ site.baseurl }}/Help/Workshops/workshop-topics/versions/3-day-workshop/1-Principles/b-review-of-topographic-data-sources-surveys).)

------

# Building TINS

The following steps lead you through the process of creating an initial TIN and then how to refine it.

### Your First TIN

In this video tutorial, we go through the simplest form of constructing a TIN from just raw topographic survey point data. We also highlight some of the potential pitfalls to this approach.

<iframe width="560" height="315" src="https://www.youtube.com/embed/FrXqYazKuAk" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

### A Better TIN

In this video tutorial, we talk about how you can build a TIN using a polygon hardclip boundary as well as how to manually edit your tin with the TIN Editing Tools.

<iframe width="560" height="315" src="https://www.youtube.com/embed/LoK6lazwGqM" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

### A Better TIN yet

In this video tutorial, we will use the same data used in our first two TINs as well as the some 3D breakline data provided to you:

<iframe width="560" height="315" src="https://www.youtube.com/embed/x3iCJ9xBocE" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

## Building DEM

Building the DEM is a simple matter of [converting the TIN to a Raster](http://help.arcgis.com/en/arcgisdesktop/10.0/help/index.html#//00q900000077000000.htm). This video tutorial walks you through the considerations. 

<iframe width="560" height="315" src="https://www.youtube.com/embed/cORkIYUp3_U" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

For this exercise, we have not been careful with the creation of this raster (see [best practices]({{ site.baseurl }}/gcd-concepts/data-preparation---best-practices) for a follow up).

------

## OPTIONAL: Building Water Depth DEM

Since we have a DEM that includes shots above the water and below the water, and because the survey data denoted where the water's edge shots are, we can derive a bathymetric water depth DEM. The way in which we will do this is to:

1. Build a water surface TIN from the water edge data
2. Convert that water surface TIN into a DEM of the same resolution and extent (i.e. concurrent) as our original DEM
3. Use Raster Calculator to derive water depths by subtracting the lower elevation DEM from the higher elevation water surface.

The resulting water depth map can be overlaid on the DEM and provides a more intuitive context of the physcial habitat within the reach as the pools, bars, and beaver ponds jump out more clearly then from the topography alone. This video walks you through the sequence:

<iframe width="560" height="315" src="https://www.youtube.com/embed/hSGW9p570Y8" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

## OPTIONAL: Pulling Some Cross Sections and a Long Profile Off DEM

Using the 3D Analyst interpolate line and profile graph features, you can easily pull off a longitudinal profile and cross section(s) from the DEM. This video shows you how:

<iframe width="560" height="315" src="https://www.youtube.com/embed/Kv263FBGJnE" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

## References

The above exercise comes from a [Lab](http://gis.joewheaton.org/assignments/labs/lab06-1) in my [Advanced GIS Class](http://gis.joewheaton.org/assignments/labs/lab08). If you want more information on how to work with DEMs and topographic data, you may find Labs [6](http://gis.joewheaton.org/assignments/labs/lab06-1), [7](http://gis.joewheaton.org/assignments/labs/lab-07---building-dems) and [8](http://gis.joewheaton.org/assignments/labs/lab08) helpful references/refreshers.

### Related Online Help or Tutorials for this Topic

- [Parent Workshop Topic]({{ site.baseurl }}/Help/Workshops/workshop-topics/versions/3-day-workshop/1-Principles/c-review-of-building-surfaces-from-raw-data)
- [Tutorial Topic: ii. Building DEMs]({{ site.baseurl }}/tutorials--how-to/ii-building-dems)
- [Working with DEMs](http://gis.joewheaton.org/assignments/labs/lab06-1) Lab - From my [Advanced GIS course](http://gis.joewheaton.org/)
- [Building DEMs](http://gis.joewheaton.org/assignments/labs/lab-07---building-dems) Lab - From my [Advanced GIS course](http://gis.joewheaton.org/)
- Using ArcGIS to Manipulate DEMs and build Grading Plans - *The proper place to design grading plans is in a CAD environment, but it can be done in ArcGIS for those so inclined:*
- [Lecture](http://etal.usu.edu/ICRRR/PartII/2010/Part_II/D1_JMW.pdf)
- [Tutorial](http://etal.usu.edu/ICRRR/PartII/2010/Part_II/ICRRR_D2_Topo_Excercise.pdf)
- [Dataset](http://etal.usu.edu/ICRRR/PartII/2010/Part_II/ProvoTopoData.zip)


------
<div align="center">
	<a class="hollow button" href="{{ site.baseurl }}/Help"><i class="fa fa-chevron-circle-left"></i>  Back to GCD Help </a>  
	<a class="hollow button" href="{{ site.baseurl }}/"><img src="{{ site.baseurl}}/assets/images/icons/GCDAddIn.png">  Back to GCD Home </a>  
</div>