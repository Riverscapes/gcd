# Geomorphic Change Detection Software

This repository contains the source code for the Geomorphic Change Detection (GCD) Software. The code is written in C# using Visual Studio 2015 and consists of several code projects within a single solution:

* GCD Visual Studio 2015 Solution
    * GCDConsole - core raster processing
    * GCDConsoleTest - unit testing for GCDConsole
    * GCDAddin - ArcGIS 10.4 addin
    * GCDCore - GCD user interface and project
    * GCDCoreTests1 - unit tests for the user interface and project
    * GCDStandalone - Windows desktop (non-GIS) user interface
    * NARU - [North Arrow Research Utilities](https://github.com/NorthArrowResearch/naru)

## Getting Started

The code is written using Visual Studio 2015 but you need the "Visual Studio 2012 Shell" to compile the AddIn. Weirdly, the 2012 Shell actually comes with the "Visual Studio 2013 Isolated Shell" and not the 2012 version. Here are [ESRI's instructions](https://support.esri.com/en/technical-article/000017857).

Also make sure that the AddIn is not "registered for COM" on the Assembly Information tab of the project properties.

## License

[GNU Public License Version 3](https://raw.githubusercontent.com/Riverscapes/gcd/master/LICENSE)

To cite the software:

> Phlip Bailey, Joseph Wheaton, Matt Reimer, & James Brasington. (2020). Geomorphic Change Detection Software (7.5.0). Zenodo. https://doi.org/10.5281/zenodo.7248344.

[![DOI](https://zenodo.org/badge/DOI/10.5281/zenodo.7248344.svg)](https://doi.org/10.5281/zenodo.7248344)
