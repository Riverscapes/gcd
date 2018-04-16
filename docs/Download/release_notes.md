---
title: Release Notes
weight: 4
---

# GCD 7

Note, see the [Commit History](https://github.com/Riverscapes/gcd/commits) in Github for fuller documentation of changes (by [release](https://github.com/Riverscapes/gcd/releases).


<a class="button" href="https://github.com/Riverscapes/gcd/commits"><i class="fa fa-github"/> GCD Github Repository Commit History</a>

## 7.0.14 - 16 Apr 2018

* Fixed y axis label when switching between area and volume DoD histogram plots.
* Enforces zero as the minimum of the elevation change DoD bar charts.
* HTML encoding GCD project file path when exporting to the Cross Section Viewer (allows for spaces in the path).

## 7.0.13 - 13 Apr 2018

* AnyCPU architecture.

## 7.0.12 - 12 Apr 2018

* Click Once deployment fixed for StandAlone.
* Launching Cross Section Viewer now works correctly.
* Web-based acknowledgements form.
* Online help buttons.
* Exceptions now have GCD and ArcMap versions.
* Deleting morphological analyses and linear extractions implemented.
* Deleting DoDs and budget segregations checks for file locks first.
* Viewing FIS error surface properties now works.

## 7.0.11 - 4 Apr 2018

* Fixed clicking on DoD tabular results table header.
* Revised morphological analysis user interface.

## 7.0.10 - 28 Mar 2018

* Fixed FIS calculations
* Fixed row indexing in the budget segregation stat % totals column.

## 7.0.09 - 22 Mar 2018

* Error Surfaces:
  * Fixed default checkbox.
  * Fixed bug when error surface has no value for cells. 
  * FIS file copied next to error surface raster.
  where there is valid change between two surfaces.
* Linear Extractions from DoDs implemented.
* GCD now handles "US Survey Foot" linear units.
* Disabled opening of last project in GCD standalone.
* Morphological analysis now produces a spreadsheet.
* Fixed multi-epoch bug when sorting DEMs.
* Regular masks added to map with 30% transparency.
* New FIS library features:
  * System and user FIS shown in library.
  * ET-AL FIS repo included in deployment.
* DoD elevation change bars y axis accounts for error bars.
* Change Detection:
  * Produces thresholded error raster.
  * Adding all DoDs to map.
  * Adding all DoDs with the same surfaces to the map.
  * User can edit DoD name.
* GCD does not copy `Deploy` folder to `AppData` folder at start-up.

*Known Issues*

* FIS calculation is wrong.
* Thresholded error raster for probabilistic thresholding is wrong.
* Editing two items (of any type) and giving them the same name breaks uniqueness constraint.

## 7.0.08 - 8 Mar 2018

* Including the Morphological and InterComparison spreadsheets in the deployments.
* Raw and thresholded layers in the map now have the name of the DoD.
* Fixed DoD results - clicking the reset button.
* Changed order or raw and threshdolded add to map menu items on DoD right click menu
* Changed order of create new error surface and create from mask.
* Removed test "for entire DEM extent" from create error surface menu.
* Bug fixing serialization of morphological spreadsheet into XML.
* Fixing creating regular mask when the user changes field.
* Fixed bug adding existing error surface.
* Fixing editing both single and multi-method error surfaces.
* Slope and point density associated surfaces getting default names.

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

------

# Prior Versions

* [GCD 6 Release Notes](/_releasenotes/gcd6_releasenotes.html)
* [GCD 5 Release Notes](http://gcd.joewheaton.org/downloads/older-versions)


------
<div align="center">
	<a class="hollow button" href="{{ site.baseurl }}/Download"><i class="fa fa-chevron-circle-left"></i>  Back to GCD Downloads </a>  
	<a class="hollow button" href="{{ site.baseurl }}/"><img src="{{ site.baseurl}}/assets/images/icons/GCDAddIn.png">  Back to GCD Home </a>  
</div>