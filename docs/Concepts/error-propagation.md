---
title: Error Propagation
weight: 2
---

## Background Theory on Error Propagation

### Error Propagation Theory Based on Minimum Level of Detection Logic

<div class="responsive-embed">
<iframe width="560" height="315" src="https://www.youtube.com/embed/boXszBr0RHQ" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>
</div>

<-- #### Representing Propagated Errors as Probabilities -->

### How does GCD calculate +/- Error Volumes?

 The +/- error volumes are very conservatively estimated with error propagation on a cell-by-cell basis. For every change detection that is done based on error surfaces in the inputs, a properror.tif is calculated. This is nothing more than:
 
	PropError = √((ErrorDEM New)^2 + (ErrorDEM Old)^2)

This is just error propagation on a cell by cell basis to get an estimate of propagated vertical error. The error volume is estimated just like change detection values are estimated: by multiplying vertical value (in this case propagated DEM error instead of elevation change) by the cell area, which is just the square of cell resolution.

	Cell Error Volume = Cell-Resolution^2 * PropError

For the summary tabular statistics, these are simply summed up for erosion cells and deposition cells independently the same way that volumetric estimates of change are. In the video below, illustrate how GCD calculates error volumes under different thresholding techniques:

<div class ="responsive-embed">
<iframe width="560" height="315" src="https://www.youtube.com/embed/FHBcCf2Nx5k" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>
</div>


--------------

### Further Reading on Error Propagation

- See pages 250-256 of:
  - Lane, S.N., Westaway, R.M. and Hicks, D.M., 2003. Estimation of erosion and deposition volumes in a large, gravel-bed, braided river using synoptic remote sensing. Earth Surface Processes and Landforms, 28(3): 249-271. DOI: [10.1002/esp.483](http://dx.doi.org/10.1002/esp.483).

- See pages 306-314 of:
  - Brasington, J., Langham, J. and Rumsby, B., 2003. Methodological sensitivity of morphometric estimates of coarse fluvial sediment transport. Geomorphology, 53(3-4): 299-316. DOI: [10.1016/S0169-555X(02)00320-3](http://dx.doi.org/10.1016/S0169-555X%2802%2900320-3) 

- See pages 78-90 of: 
  - Chapter 4 of Wheaton JM. 2008. [Uncertainty in Morphological Sediment Budgeting of Rivers](http://www.joewheaton.org/Home/research/projects-1/morphological-sediment-budgeting/phdthesis). Unpublished PhD Thesis, University of Southampton, Southampton, 412 pp.

- See page 140 of:
  - Wheaton JM, Brasington J, Darby SE and Sear D. 2010. [Accounting for Uncertainty in DEMs from Repeat Topographic Surveys: Improved Sediment Budgets](http://dx.doi.org/10.1002/esp.1886). Earth Surface Processes and Landforms. 35 (2): 136-156. DOI: [10.1002/esp.1886](http://dx.doi.org/10.1002/esp.1886).
