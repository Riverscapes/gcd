---
title: Data Preparation - Best Practices
---

The GCD software does what it can to help you through the process of preparing your data for analysis. However, there are a number of concepts and best practices you can undertake to exert more explicit control on these factors as well as help ensure that your processing goes smoother.

Although it might seem to be a simple task, producing concurrent grids in ArcGIS is a significant challenge. If you don't know what [concurrency]({{ site.baseurl }}/gcd-concepts/data-preparation---best-practices/concurrency) even is, then you should watch the video. If you know what concurrency is, the best advice I can give you is to round your raster extents to whole numbers and make sure that your raster width and height are evenly divisible by your cell resolution. If that does not make sense to you, you can watch this rather long-winded (50 minute) video lecture and tutorial that goes over all the necessary background and mechanics of how you do this. You can cut to the last 10 minutes if you just want the pointers. The lecture slides can be found here:

[The Raster Concurrency Nightmare in ArcGIS](http://www.gis.usu.edu/~jwheaton/et_al/GCD/GCD5/GCD_GridConcurrency.pdf)

<div class="responsive-embed">
<iframe width="560" height="315" src="https://www.youtube.com/embed/UpiIo8XVEUw" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>
</div>

To avoid the problems, [Philip Bailey](http://northarrowresearch.com/people/) (North Arrow Research) has built an extremely simple spreadsheet extents calculator to help you ensure that your raster extents are orthogonal and concurrent:

[Concurrency Calculator.xlsx](http://etal.usu.edu/GCD/Concurrency%20Calculator.xltx)
