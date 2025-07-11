---
title: Budget Segregation FAQ
slug: /Concepts/budget-seg
sidebar_position: 4
---

Here is a little description of how budget segregation works "under the hood". This is all internal workings that you don't need to know but it might help with the understanding:

### How does Rasterized budget segregation work?

1. By passing a string field name (Like "Method") to the rasterization method a temporary rasterized version of the ShapeFile is created. The values in the raster correspond to our special `GCDFID` field. 
2. The raster is stored in a special `VectorRaster` object which contains an open dataset and a Dictionary to create a relationship between our `GCDFID` field and the value of the field we want to use as a mask
3. This `VectorRaster` object is passed to the budget segregation app which iterates cell-by-cell and aggregates values into bins named after the string values of the fields specified in step 1.
4. The algorithm returns the dictionary of Stats objects.

### Why do I see `GCDFID` in my ShapeFile when I add it to a project?

The [ESRI ShapeFile specification](https://www.esri.com/library/whitepapers/pdfs/shapefile.pdf) allows for inconsistent handling of FID fields. Combining that with limitations in the GDaL Rasterization algorithm, in order to do Rasterization of an ESRI ShapeFile using the FID field we need an internally consistent (to GCD) unique ID field to identify shapes in our workflow.
