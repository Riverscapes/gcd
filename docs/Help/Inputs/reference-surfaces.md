---
title: Reference Surfaces
---

Reference surfaces are elevation rasters that originate from some other source than a topographic survey. They are very similar to [DEM Surveys]() in that they share the same vertical units, cell resolution and spatial reference. They can also be used as one or both surfaces in a [change detection ]() analysis.

The role of reference surfaces within the GCD software is intentionally flexible, enabling users to leverage them for several purposes:

* Uniform constant reference planes for comparision with DEM surveys over time (e.g. a reservoir spill surface).
* Statistical summary of several DEM surveys over time (e.g. max, min or standard deviation from a series of DEMs).
* User-specified raster than can represent any kind of reference imaginable (providing the cell values of the raster are in the same units at the DEM surveys within the GCD project).

Each reference surface can have multiple error rasters associated with it. These error surfaces are then used when performing a [change detection]() analysis that uses spatially variable uncertainty.

# Add Existing Reference Surface

# Calculating Statistical Reference Surfaces

# Uniform Reference Surfaces

# Context Menu

# Edit Properties

# Add To Map

# Delete
