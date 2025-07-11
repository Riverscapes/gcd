---
title: ToPCAT Decimation
sidebar_position: 8
slug: /Concepts/topcat-decimation
---

## Summary

Topographic Point Cloud Analysis Toolkit ([ToPCAT](http://tat.riverscapes.net/Help/Data_Preparation/topcat-menu/)) is a tool developed by Brasington et al. ([2012](#Relevant_Literature)) used to decimate (or thin) a point cloud and calculate meaningful statistics for grid defined areas. The process of decimation can reduce computation time and produce meaningful summary outputs (e.g. Z<sub>Min</sub> for modeling bare earth elevations; and detrended standard deviation for modelling surface roughness). 

## Concepts

### Point Cloud Decimation 

Point cloud decimation (A.K.A. thinning or binning) is common practice when using high resolution (i.e. > 1-5 points/m<sup>2</sup>) point cloud topographic data (e.g. from multibeam SONAR, ground-based LiDAR, structure from motion photogrammetry) to reduce the computation times and memory requirements of their large point clouds ([Kearns and Breman 2010](http://sites.google.com/a/joewheaton.org/mbs-gcd/z--old-crap/data-processing/quantifying-uncertainty/using-topcat-topcat-based-utilities#_ENREF_10), [Passalacqua et al. 2015](http://dx.doi.org/10.1016/j.earscirev.2015.05.012)). Point decimation is the process of setting a defined grid size and using the point values that are contained within those grids to calculate summary statistics from those points. 

### Locally Detrended Standard Deviation

One of the most useful statistics from a point cloud can be those that describe the variance of elevations within a cell. Brasington et al. ([2012](#Relevant_Literature)) developed an algorithm within ToPCAT that reports the variance from a locally detrended surface. This is ensures that only local variations in height are being modeled as is a reasonable proxy for roughness. 

Figure 4 from Brasington et al. ([2012](#Relevant_Literature)) illustrates this workflow (at left). On a cell by cell basis this operation is computationally expensive and is not a standard tool in popular GIS. ToPCAT uses the point values contained within the defined grid size to calculate the minimum, maximum, mean, detrended mean, range, standard deviation, detrended standard deviation, skew, detrended skew, kurtosis, detrended kurtosis, and point density. A minimum number of points is necessary, generally 4, to calculate the detrended standard deviation; the necessity to have more than one point within the cell resolution of analysis is in support of many of the concepts presented in Cell Resolution. Having these statistics in a regularly spaced grid is invaluable for developing a detrended standard deviation raster to represent surface roughness.


In [GCD 6](/Download/old_versions.html), you could run ToPCAT within GCD.

![ToPCAT_Workflow](/img/ToPCAT_Workflow.png)

### ToPCAT Outputs

 ToPCAT, the point cloud thinning toolset recommended by ET-AL, uses the point values contained within the defined grid size to calculate the minimum, maximum, mean, detrended mean, range, standard deviation, detrended standard deviation, skew, detrended skew, kurtosis, detrended kurtosis, and point density. A minimum number of points is necessary, generally 4, to calculate the detrended standard deviation; the necessity to have more than one point within the cell resolution of analysis is in support of many of the concepts presented [Determining Grid Resolution](http://sites.google.com/a/joewheaton.org/mbs-gcd/z--old-crap/data-processing/determining-grid-resolution). Having these statistics in a regularly spaced grid is invaluable for developing a detrended standard deviation raster to represent surface roughness.

The [command-line version of ToPCAT](https://code.google.com/archive/p/point-cloud-tools/) is straight forward to use, but requires a fair degree of pre and post processing. There are now three *other* ways to implement ToPCAT in a more familiar GIS environment:
1. Now, with GCD 7, you can run [ToPCAT](http://tat.riverscapes.net/Help/Data_Preparation/topcat-menu/topcat-point-cloud-decimation-tool.html) from the [`Data Preparation → ToPCAT`](http://tat.riverscapes.net/Help/Data_Preparation/topcat-menu/) menu.  A common workflow is to export just the locally detrended standard deviation as a proxy roughness surface and input this into GCD as an associated surface.
2. The [MBES Toolkit](http://sites.google.com/a/joewheaton.org/mbs-gcd/mbes-toolkit-download) (a standalone GUI), now makes these pre and post processing steps seamless and allows you to run ToPCAT from an easy to use user interface. The two main commands are [`ToPCAT Prep`](http://tat.riverscapes.net/Help/Data_Preparation/topcat-menu/topcat-preparation-tool.html)and [`ToPCAT Point Cloud Decimation`](http://tat.riverscapes.net/Help/Data_Preparation/topcat-menu/topcat-point-cloud-decimation-tool.html). 
3. These same commands are available in the GCD 6 Add-In. In [GCD 6](/Download/old_versions.html), you could run ToPCAT within GCD from the [`Data Preparation → ToPCAT`](http://gcd6help.joewheaton.org/gcd-command-reference/data-prep-menu/e-topcat-menu) menu. 

### Visualizing Decimated Point Clouds


<YouTubeEmbed videoId="yxz3NzRwDpA" title="Visualizing Decimated Point Clouds" />

-------

## Resources

### Relevant Literature

- Brasington, J., D. Vericat, and I. Rychkov. 2012. Modeling river bed morphology, roughness, and surface sedimentology using high resolution terrestrial laser scanning. Water Resources Research 48. DOI: [10.1029/2012wr012223](http://dx.doi.org/10.0.4.5/2012wr012223).
- Buscombe, D. 2016. [Spatially explicit spectral analysis of point clouds and geospatial data](https://static1.squarespace.com/static/582b5e07197aeac54fcdd6a8/t/59d92020e45a7c0ce23fb1c9/1507401777289/Buscombe2016_CompGeosciences.pdf). Computers and Geosciences 86, 92-108. 
- Passalacqua P, Belmont P, Staley D, Simley J, Arrowsmith JR, Bodee C, Crosby C, DeLongg S, Glenn N, Kelly S, Lague D, Sangireddy H, Schaffrath K, Tarboton D, Wasklewicz T, Wheaton J. 2015. [Analyzing high resolution topography for advancing the understanding of mass and energy transfer through ](http://www.joewheaton.org/Home/research/goog_1167543503)[landscapes: A review](https://www.researchgate.net/publication/277477904_Analyzing_high_resolution_topography_for_advancing_the_understanding_of_mass_and_energy_transfer_through_landscapes_A_review). Earth Science Reviews. 148. pp. 174-193. DOI: [10.1016/j.earscirev.2015.05.012](http://dx.doi.org/10.1016/j.earscirev.2015.05.012)
- Rychkov, I., J. Brasington, and D. Vericat. 2010. [Processing and Modelling on Terrestrial Point Clouds](http://code.google.com/p/point-cloud-tools/downloads/detail?name=1.5.pdf&can=2&q=). Institute of Geography and Earth Sciences, University of Wales, Aberystwyth, Wales.
- Rychkov, I., J. Brasington, and D. Vericat. 2012. Computational and methodological aspects of terrestrial surface analysis based on point clouds. Computers & Geosciences 42:64-70. DOI: [10.1016/j.cageo.2012.02.011](http://dx.doi.org/10.0.3.248/j.cageo.2012.02.011).

### Other Resources

- [PCTools 32 Bit Explanation](http://www.joewheaton.org/Home/research/unlisted-software/point-cloud-tools) on Joewheaton.org (former name of ToPCAT was PCTools)
- [PCTools ](http://code.google.com/p/point-cloud-tools/)(Point Cloud Tools - the documented alternative to TLS Decimation Utility)
  - [Software](http://code.google.com/p/point-cloud-tools/) (Python Script)
  - [PCTools Windows Console Version](http://www.google.com/url?q=http%3A%2F%2Fwww.gis.usu.edu%2F%257Ejwheaton%2Fet_al%2FWorkshops%2FGCD_IdahoPower%2FPcTools_0.1.2_x32.zip&sa=D&sntz=1&usg=AFrqEzdcM8EKFRdmahx17uA-1Au2wIu40g) (32-bit version, non PYTHON command prompt version; instructions below)
  - [Documentation](http://code.google.com/p/point-cloud-tools/downloads/detail?name=1.5.pdf&can=2&q=)
- A useful alternative to ToPCAT, is [Dan Buscombe's](https://www.danielbuscombe.com/) [PySESA](https://dbuscombe-usgs.github.io/pysesa/) - Python Program for Spatially Explicit Spectral Analysis. PySESA is an open-source project dedicated to provide a generic Python framework for spatially explicit statistical analyses of point clouds and other geospatial data, in the spatial and frequency domains, for use in the geosciences.
