---
title: Release Notes
weight: 4
---

# GCD 7

See the [commit history](https://github.com/Riverscapes/gcd/commits) in GitHub for fuller documentation of changes. Alternatively you can view the commits organized by each product [release](https://github.com/Riverscapes/gcd/releases). The release notes for [prior versions](#prior-versions) are linked at the bottom of this page.

## 7.4.4 - 27 Nov 2019

* The GCD now works consistently regardless of the User's Windows regionalization settings. i.e. whether they use commas or decimals as separators when reading and writing the GCD project file.
* Ability to control the order of DoDs in the inter-comparison tool.
* Directional budget segregations always write to the inter-comparision spreadsheet in the correct order.
* Changing DoD result units is now reflected in the longitudinal screen.
* Fixed mis-labelled DoD rasters in DoD properties screen.

## 7.4.3 - 5 Sep 2019

* Fixe issue when GCD project units are mixed ([#358](https://github.com/Riverscapes/gcd/issues/358)). i.e. the horizontal and vertical units are in one family of units (say feet) but the area and volume units are in another (e.g. yards). Note that although this wasn't explicitly a metric/imperial problem, this was not an issue for most users whose projects were entirely based in metres (linear, squared and cubed). It is recommended that users with projects using mixed units and that that were created with GCD version 7.4.2 and earlier should delete all DoDs and budget segregations and recreate them using version 7.4.3 or newer).

## 7.4.2 - 14 Aug 2019

* Fixed issue for projects that use non-metric units whereby DoD and budget segregation volumes were changed to be incorrect with subsequent changes to the GCD project ([#355](https://github.com/Riverscapes/gcd/issues/355)). This issue only affected projects using non-metric units. Projects that used metric units were unaffected. It is recommended that users with projects using non-metric units and that that were created with GCD version 7.4.1 and earlier should delete all DoDs and budget segregations and recreate them using version 7.4.2 or newer).

## 7.4.1 - 11 Feb 2019

* Fixed correctly using first DEM surveys cell size when adding subsequent DEMs to project ([#341](https://github.com/Riverscapes/gcd/issues/341)).
* Fixed correctly reading associated surface type from GCD project XML file ([#343](https://github.com/Riverscapes/gcd/issues/343)).
* Deleting inter-comparisons now working ([#332](https://github.com/Riverscapes/gcd/issues/332)).
* Removing the ability to use an associated surface raster as an error surface ([#329](https://github.com/Riverscapes/gcd/issues/329)). Users should simply add the existing raster as an error surface directly.

## 7.4.0 - 6 Nov 2018

* Fixed issue when attempting a linear extraction with ShapeFiles containing [null](https://github.com/Riverscapes/gcd/issues/326) and [multipart](https://github.com/Riverscapes/gcd/issues/324) geometries.
* Raster properties now appear in the correct raster units instead of always being converted to meters.
* `foot_us` units now recognized correctly as [US Survey Foot](https://en.wikipedia.org/wiki/Foot_(unit)#US_survey_foot) units.
* Import raster form improvements.
* Removed Data Preparation menu from both ArcGIS AddIn and Standalone.
* Renamed the `Tools` menu in the Standalone to `Customize` to be consistent within the ArcGIS AddIn.

## 7.3.0 - 14 Sep 2018

* Bug fix to user FIS library loading

## 7.2.0 - 27 Aug 2018

* [Operational progress](https://github.com/Riverscapes/gcd/issues/41). Both the Stand alone and ArcGIS versions now display progress bar information during GCD operations.
* [Profile route improvements](https://github.com/Riverscapes/gcd/issues/121). Profile routes have been broken out into *longitudinal* and *transect* profile routes to distinguish between different polyline orientations.
* Budget Segregation
  * Budget segregation results screen now includes [graphs showing the overall breakdown of surface raising and lowering](https://github.com/Riverscapes/gcd/issues/194) for all classes.
  * Fixed [order that classes are displayed](https://github.com/Riverscapes/gcd/issues/235) when the budget segregation is performed using a directional mask.
* [Generic batch tool](https://github.com/Riverscapes/gcd/issues/147) now allows user to select both surfaces and uncertainty method for each batch.
* [Improved raster properties](https://github.com/Riverscapes/gcd/issues/232) dialog implemented throughout GCD.
* [Morphological analysis has additional column](https://github.com/Riverscapes/gcd/issues/251) for material augmentation or extraction.
* [FIS rule file and metadata are now packaged](https://github.com/Riverscapes/gcd/issues/234) with the GCD project.
* Users can control the [default DoD symbology range](https://github.com/Riverscapes/gcd/issues/286).
* GCD standalone can now be launched by [double clicking GCD project file](https://github.com/Riverscapes/gcd/issues/292).
* Bug Fixes
  * Fixed [duplicate zero station value](https://github.com/Riverscapes/gcd/issues/297) for some linear extraction results.
  * Fixed crash importing rasters with [linear units that are in decimal degrees](https://github.com/Riverscapes/gcd/issues/300) (not recommended).
  * Fixed error when attempting to use the [clean raster tool](https://github.com/Riverscapes/gcd/issues/301).

## 7.1.1 - 29 Jun 2018

* Fixed GCD calling the Cross Section Viewer by changing the publisher to `North Arrow Research Ltd.`
* Fixed budget segregation issue when the thresholded histogram possesses no values.

## 7.1.0 - 11 May 2018

This one is a big release that changes the platform of the Standalone so if you have the Standalone installed you will need to uninstall it and re-install it. Further updates should happen as usual.

#### Nearly 50 bug fixes and enhancements including:

* **64-bit support!** *(Standalone only)* We're thrilled to announce the Standalone product is now working in full 64-bit.
* Memory leaks: Fixed a nasty memory leak issue which should free up more RAM and allow for larger rasters to be processed.
* Significant performance increases (in addition to the 64-bit) when doing windowed operations like Hillshading.
* Lots of workflow issues to smooth out user experience.
* Many file locking issues.
* Dozens of little UI glitches and typos.
* Better help and tooltips overall.
* Improved integrity checking when deleting project items.

#### Known Issues

* Morphological analysis results and user interface still being finalized.

-----------------------------


## 7.0.16 - 17 Apr 2018

* Probabilistic thresholded error raster.
* Fixing DoD and budget segregation analysis inputs property grid to reference correct new and old surfaces.
* Links to [TAT](http://tat.riverscapes.xyz) and [Cross Section Viewer](http://xsviewer.northarrowresearch.com) online help web sites.

## 7.0.15 - 17 Apr 2018

* Projection mismatches presented as warning instead of error.
* Browsing to *.tiff files.

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

## 7.0.02 - 22 Jan 2018

* Fixing AddIn so that `Deploy` files are considered part of the installation and not `AddInContent`.

## 7.0.01 - 19 Jan 2018

* First version of GCD standalone and GCD AddIn for ArcGIS
* All core GCD 6 functionality
* Multiple uncertainty analysis batch change detection

------

# Prior Versions

* [GCD 6 Release Notes](/releasenotes/gcd6_releasenotes)
* [GCD 5 Release Notes](old_versions)
