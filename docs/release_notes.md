---
title: Release Notes
---

## 7.0.07 - 6 Mar 2018

* Internal test release.
* Refactored project tree with more consistent menus.
  * Simplified associated surface user interface
  * Refactored error surface generation.
* Profile routes.
* Linear extractions.
* Inter-comparison spreadsheets
* Morphological change identifying minimum flux cell
* Customizable change detection plots
  * Error bars on elevation change bar plots.
  * Simplified tabular change detection results.
* Add to map for all new project items.

## 7.0.06 - 22 Feb 2018

* Reference surfaces and reference error surfaces
    * From a set of DEMs the error surface is derived from the min, max from the corresponding error surface value, or the root mean square of the error surface values.
* Regular, directional and area of interest masks
* Change detection with AOI
* Vector based operations use GDAL to convert the polygons into a temporary raster layer
    * Budget segregation
    * Multi-method error
    * AOI change detection
* Morphological approach
* Budget segregation % totals column added to tabular display
* Simplified DEM properties form

*Known Issues*

* Multi-method error surfaces not working
* FIS error surfaces not working
* Morphological approach min flux cell identification not working

## 7.0.03 - 30 Jan 2018

* Fixed change detection with probabilistic thresholding when no spatial coherence in use ([issue #30](https://github.com/Riverscapes/gcd/issues/30)).

[Download](https://github.com/Riverscapes/gcd/releases)

## 7.0.02 - 22 Jan 2018

* Fixing AddIn so that `Deploy` files are considered part of the installation and not `AddInContent`.

## 7.0.01 - 19 Jan 2018

* First version of GCD standalone and GCD AddIn for ArcGIS
* All core GCD 6 functionality
* Multiple uncertainty analysis batch change detection

# Prior Versions

* [GCD 6 Release Notes](/_releasenotes/gcd6_releasenotes.html)
* [GCD 5 Release Notes]()

