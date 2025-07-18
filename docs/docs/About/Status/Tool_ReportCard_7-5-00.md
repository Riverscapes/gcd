---
title: Riverscapes Report Card
sidebar_position: 1
---
# Riverscapes Report Card

<!-- ![TRL 5](/img/TRL_5_128w.png) -->
![GCD Logo](/img/icons/GCD_Logo_White_wText.png)<br />

GCD is one of original tools developed by [North Arrow Research](https://northarrowresearch.com), and Utah State University's [Ecogeomorphology & Topographic Analysis Lab](https://etal.joewheaton.org) and the [Riverscapes Consortium](https://riverscapes.net). This report card communicates GCD's compliance with the Riverscape Consortium's published [tool standards](https://tools.riverscapes.net).



## Report Card Summary

| Key | Value |
| --- | --- |
| Tool | [GCD - Geomorphic Change Detection Tool](https://gcd.riverscapes.net) |
| Version | [7.5.00](https://github.com/Riverscapes/gcd/releases/tag/7.5.0) |
| Date | 2020-06-18 |
| Assessment Team | Wheaton |
| [Current Assessment](http://brat.riverscapes.net/Tools#tool-status) |[Professional Grade](https://tools.riverscapes.net) |
| Target Status |Commercial Grade|
| Riverscapes Compliance |[Riverscapes Compliant](https://tools.riverscapes.net) |
| Assessment Rationale | GCD has been applied extensively throughout the World. It has been used as a research tool and practitioner tool in both monitoring and design at the reach-scale. The developers have taught over 20 workshops from 2010 to 2019 and the tool has a large user base. It is well deserving of a Professional Grade. |

## Report Card Details

This tool's [discrimnation](https://tools.riverscapes.net) evaluation by the [Riverscapes Consortium's](https://riverscapes.net) is:

**Evaluation Key:**
None or Not Applicable: <i class="fa fa-battery-empty" aria-hidden="true"></i> •
Minimal or In Progress: <i class="fa fa-battery-quarter" aria-hidden="true"></i> •
Functional: <i class="fa fa-battery-half" aria-hidden="true"></i> •
Fully Developed: <i class="fa fa-battery-full" aria-hidden="true"></i>  

| Criteria | Value | Evaluation | Comments and/or Recommendations |
| --- | --- | --- | --- |
| Tool Interface(s) |  ArcGIS 10.x AddIn, <i class="fa fa-desktop" aria-hidden="true"></i> Stand Alone Windows Tool | <i class="fa fa-battery-full" aria-hidden="true"></i> | Tool is a powerful, professionally developed software interface. The stand-alone version has no map interactivity, but produces the exact same projects, which can be viewed in GIS. The ESRI AddIn has full map interactivity. |
| Scale | Reach (cell scale resolution, reach scale extent) | <i class="fa fa-battery-full" aria-hidden="true"></i> | This tool has been applied over hundreds of thousands of reach-scale analyses. Thousands of users have used it for change detection of their sites. As part of the [Columbia Habitat Monitoring Program](http://champmonitoring.org) the GCD command line version was made production grade and applied to over 5,000 visits at over 900 long term monitoring sites. The tool has been run on high resolution topography for 10s of kilometers of riverscape. |
| Language(s) and Dependencies | C# | <i class="fa fa-battery-three-quarters" aria-hidden="true"></i> | This is a professionally developed, highly scalable compiled code. However, because it is in C#, very few researchers are using the source-code that are not involved in the development (most researchers are more conversant with Python). There are a number of dependencies, but as this is compiled software, it is easy to deploy these on Windows through installers. Moreover, all the internal dependencies are open-source and there are no ArcObjects dependencies. We give this a 3/4 ranking for dependencies and languages because (i) the choice of C# is limiting uptake by researchers, and (ii) the map-based functionality is dependent on the proprietary ESRI ArcGIS 10.x software, which is 32 bit and [support for which will end in 2026](https://www.esri.com/arcgis-blog/products/arcgis-desktop/announcements/arcmap-continued-support/). |
| Vetted in Peer-Reviewed Literature | Yes.  [Wheaton et al. (2015)](http://brat.riverscapes.net/references.html#peer-reviewed-publication) | <i class="fa fa-battery-half" aria-hidden="true"></i> | The existing capacity model is vetted, and the historical capacity model is well described. The version in the publication is [2.0](https://github.com/Riverscapes/pyBRAT/releases/tag/v2.0.0), but the capacity model is basically the same in 3.0. Many of the risk, beaver management, conservation and restoration concepts have not yet been vetted in scholarly literature but have been applied, tested and vetted by many scientists and managers across the US and UK. |
| Source Code Documentation | Source code is clearly organized and documented | <i class="fa fa-battery-full" aria-hidden="true"></i> |  |
| Open Source | [open-source](https://github.com/Riverscapes/gcd) <i class="fa fa-github" aria-hidden="true"></i> with [GNU General Public License v 3.0](https://github.com/Riverscapes/gcd/blob/master/LICENSE) | <i class="fa fa-battery-three-quarters" aria-hidden="true"></i> | Open source code, but AddIn requires ArcGIS licenses to run. The standalone is more performant and requires no license to run, but provides no map-based interface. |
| Tool and Source Code Citeable | [![DOI](https://zenodo.org/badge/DOI/10.5281/zenodo.7248344.svg)](https://doi.org/10.5281/zenodo.7248344) | <i class="fa fa-battery-full" aria-hidden="true"></i> | Phlip Bailey, Joseph Wheaton, Matt Reimer, & James Brasington. (2020). Geomorphic Change Detection Software (7.5.0). Zenodo. DOI: [10.5281/zenodo.7248344](https://doi.org/10.5281/zenodo.7248344) |
| User Documentation | [Installation](/Download/), [Tutorials](/Tutorials/), [Software Help](/Help/) and [Workhsops](/Workshops/). | <i class="fa fa-battery-half" aria-hidden="true"></i> | Documentation is comprehensive, but could be cleaned up. There are over 120 broken links. Not all of the videos are for the latest versions. There is a command line version of the code, but it does not write GCD projects, and there is no documentation. |
| Easy User Interface | Tool is primarily accessed by most users as an ESRI AddIn. Both this and the standalone work in exactly the same way and have easy to use user interfaces. | <i class="fa fa-battery-full" aria-hidden="true"></i> | The user interface is well designed and at version 7 has the advantage of being refined extensively over time. The numerous workshops taught clearly helped improve the user interface. |
| Scalability | Computation engine | <i class="fa fa-battery-full" aria-hidden="true"></i> | The scalability is functional, but requires lots of custom scripting, has unnecessary hard-coding built in, and requires extensive manual pre-processing and preparation of data. |
| Produces [Riverscapes Projects](/Tools/Technical_Reference/Documentation_Standards/Riverscapes_Projects/)  | Tool is outputting to disk data in a Riverscapes Project that can be opened in [RV](http://rave.riverscapes.net). | <i class="fa fa-battery-half" aria-hidden="true"></i> | Unfortunately, the riverscapes projects do NOT expose the entire [GCD Project tree](/Concepts/projects) in [RV](https://rave.riverscapes.net) and only package up that there is a project, its name and location. This is fine as the GCD allows a pathway to open the projects. However, it is a limitation and seems inconsistent given that the tool was developed by the same developers of the [Riverscape Consortium warehouse](https://riverscapes.net/Data_Warehouses/) and [RV](https://rave.riverscapes.net). |



## F-A-I-R Assessment

 **F**-**A**-**I**-**R**, corresponds to the **f**indable, **a**ccessible, **i**nteroperable and **r**e-useable [Principles](https://force11.org/info/the-fair-data-principles/) (Wilkinson et al. [2016](https://www.nature.com/articles/sdata201618)), which the RC strives to follow and to help facilitate making it easier for the riverscapes community to follow. **F**-**A**-**I**-**R** can apply to metadata, data and the tool itself.




| FAIR Principle    | Value                                                        | Evaluation                                                   | Comments                                                     |
| ----------------- | ------------------------------------------------------------ | ------------------------------------------------------------ | ------------------------------------------------------------ |
| **METADATA**      |                                                              |                                                              |                                                              |
| **F**indable      | Metadata for GCD Projects is easy to find for the GCD project as well as for the individual layers in the project. | <i class="fa fa-battery-half" aria-hidden="true"></i>        | GCD does a good job of producing metadata. However, only the project metadata is made available in Riverscapes Projects, and only a few GCD projects exist in the warehouse. So it is *findable* if someone has the GCD project. Without GCD projects being hosted somewhere (e.g. Warehouse or Zeondo), it is not generally findable, hence the half-full score. |
| **A**vailable     | GCD makes its metadata available through both the UI and the `*.gcd` XML project files. | <i class="fa fa-battery-half" aria-hidden="true"></i>        | Same critique as above.                                      |
| **I**nteroperable | GCD metadata follows an internally consistent standard and the metadata ineteroperable with other tools in the Riverscapes Consortium. | <i class="fa fa-battery-three-quarters" aria-hidden="true"></i> | Only interoperable with other CHaMP tools.                   |
| **R**e-useable    | GCD Metadata is easily reuseable and any XML parser can read and scrape the GCD files and package up the metadata. | <i class="fa fa-battery-full" aria-hidden="true"></i>        | Lacks documentation for developers on XML standard GCD uses. Some explanation for users [here](/Concepts/projects). |
| **DATA**          |                                                              |                                                              |                                                              |
| **F**indable      | Riverscapes projects are produced in GCD 7.5.0 and can therefore be hosted in the Warehouse and made findable. | <i class="fa fa-battery-half" aria-hidden="true"></i>        | As discussed above, the riverscapes projects are NOT complete for GCD. They just have the project Metadata and wrap up the GCD project. They are not viewable. More to the point, because they are not that useful, very few are actually in the warehouse. |
| **A**vailable     | A small number of GCD projects are publicly available. For example, the '[Example Datasets](/Tutorials/example-data-sets)'. Roughly 900 GCD projects are also available from [CHaMP](https://www.champmonitoring.org/), but are difficult to find and require permision. | <i class="fa fa-battery-half" aria-hidden="true"></i>        | Relative to the thousands of GCD projects that could be made available (e.g. everything from CHaMP), a pathetically small number are available in the public domain. |
| **I**nteroperable | The data in GCD is highly interoperable with any GIS processing or geoprocessing. | <i class="fa fa-battery-three-quarters" aria-hidden="true"></i> | If the datasets from GCD were hosted and recognized in the warehouse, and API calls to the warehouse could be made from other tools and CyberCastor, it would dramatically improve the interoperability of GCD projects and data. |
| **R**e-useable    | GCD projects are highly re-useable through time and by multiple users. | <i class="fa fa-battery-full" aria-hidden="true"></i>        | The consistent standards by which GCD curates datasets and analyses makes it highly reuseable by others. It also has proved remarkably future-proof through release cycles, which have done a good job of honoring or upgrading old versions of GCD projects. |
| **TOOL**          |                                                              |                                                              |                                                              |
| **F**indable      | GCD is easy to find and download from the [website](/Download/),  [GitHub Repo](https://github.com/Riverscapes/gcd/releases/tag/7.5.0), and/or [Zeondo](https://doi.org/10.5281/zenodo.7248344) | <i class="fa fa-battery-full" aria-hidden="true"></i>        | Between  [GitHub Repo](https://github.com/Riverscapes/gcd/releases/tag/7.5.0), and/or [Zeondo](https://doi.org/10.5281/zenodo.7248344), and a Google Search, the GCD tool is very easy to find and access. |
| **A**vailable     | GCD is freely available and open Source!                     | <i class="fa fa-battery-full" aria-hidden="true"></i>        | Excellent.                                                   |
| **I**nteroperable | GCD can and has been incorporated into production-grade workflows and analyses with upstream and downstream tools demonstrating its interoprablity. | <i class="fa fa-battery-half" aria-hidden="true"></i>        | Although the tool is interoperable with some other Riverscapes Consortium tools, it does not work with the [Riverscapes Viewer](https://rave.riverscapes.net/) well. It also is not currently compatible with [QRiS](https://qris.riverscapes.net/). |
| **R**e-useable    | GCD code is all open source and re-useable by .net developers. | <i class="fa fa-battery-half" aria-hidden="true"></i>        | The irony of GCD being open-source is that virtually no high-end GCD researchers have used our code! Even those that have hired us (e.g. GCMRC) reinvent their own stuff. This is elaborated elsewhere, but really because when GCD was developed we started with C++ and C# for performance and it wasn't until mid 20-teens that Python (thanks to SciPy and NumPy) became a viable alternative that was nearly as performant. As long as GCD is not in Python, this will not change. |

Overall summary of GCD **FAIR**-ness <i class="fa fa-battery-quarter" aria-hidden="true"></i> : GCD does a remarkably good job for a Riverscapes tool of being set up for FAIRness but stops short of achieving FAIR mainly because the GCD projects are not fully baked (only partially) into Riverscapes Projects, and there is not a simple pathway to get GCD projects into the Riverscapes Warehouse.



## Tool Output Utility

| Criteria | Value | Evaluation | Comments |
| --- | --- | --- | --- |
| [RAVE](https://rave.riverscapes.net)- Compliant Riverscapes Projects? | Produces Riverscapes Project, but it is not a very useful project (you can't view the datasetes or results in RV) | :battery_quarter: | Refactoring needed and add Project Type registration with [`program.xml`](/Tools/Technical_Reference/Documentation_Standards/Riverscapes_Projects/Program/) in [Program Repo](https://github.com/Riverscapes/Program) and include all datasets and parameters in project file. |
| [RAVE](https://rave.riverscapes.net) Business Logic Defined? | Not for [3.1.00](https://github.com/Riverscapes/pyBRAT/releases/tag/3.1.00), but example exists for BETA [sqlBRAT](https://github.com/Riverscapes/sqlBRAT) that is functional | :battery_empty: | Simple to remedy. Projects do currently have ArcGIS layer packages following project structure and entirely symbolized. |
| Riverscapes Projects hosted in public-facing [Riverscapes Warehouse(s)](https://riverscapes.net/Data_Warehouses/#warehouse-explorer-concept) | Not consistently. 18 Riverscapes Projects are in the [Asotin IMW Warehouse Program](https://data.riverscapes.net/#/AsotinIMW). | :battery_empty: | GCD Data is easy to package up in GCD Projects and Share, but it is up to the [user to share these](/Concepts/projects). |
| Riverscapes Projects connected to [Web-Maps](https://riverscapes.net/Data_Warehouses#web-maps) 🗺️ | Not consistently. A proof of concept exist for a nice interactive webmap of results for Feshie.   | :battery_quarter: | All old data sets should be made Web Map accessible and clear about what version they were produced from and what years they correspond to (i.e. Riverscapes Project metadata) |
| Riverscapes Projects connected to Field [Apps](https://riverscapes.net//Data_Warehouses#apps---pwas) | None. | :battery_empty: | Since GCD data requires high resolution topography tied to robustly established ground control networks, there is no real need for a "connected" field app. Plenty of professional acquisition platforms exist for producting survey-grade topography. Although the geomorphic interpretation could benefit from some field notes or observations, it is a limited use case and other workflows exist.  |

## Developer Intent

The GCD [devleopment team](/About/who) are not actively seeking funding to build a **Commercial-Grade**  version of GCD, but they hope to do so in the next 2 to 5 years. A commercial grade version of GCD has been demo'd, and it  would:
- Reporduce the functionality of the GCD-AddIn entirely within a web-based interface, and do all the computation in the cloud. 
- Have an inviting [web-map interface](https://riverscapes.net/Data_Warehouses/#web-maps) so non GIS-users can discover GCD runs and explore them and interrogate them.
 - Making it easy for GIS users to download GCD projects for use in [Riverscapes Viewer (RV)](https://rave.riverscapes.net) with [Riverscapes Projects](https://tools.riverscapes.net/)
- Encourage more user-interaction with GCD outputs and crowd-sourcing of information to create ownership of outputs
  - Allow users to visualize dynamic outputs of GCD through time 
  - Allow users to upload their own GCD  projects
  - Allow discovery of past GCD runs in Warehouse
- Facilitate users paying modest prices (i.e. commercial) to have **production-grade** runs (similar to CHaMP) and more new runs or more carefully curated (validated, resolved, etc.) for a specific watershed and then share them with broader community


The development team has also speculated about refactoring a **profesional-grade** version for QGIS either as its own PlugIn, or as part of the [Riverscape Studio for QGIS - QRiS](http://qris.riverscapes.net). 

As part of either or both refactors, we would like to refactor the underlying source code to open-source Pyhton, to make it easier for researchers to fork and use in their own analyes (C# is above the skill level and familiarity of too many in the riverscapes research community).


The development team at this point has already produced a beta version of a **Production-Grade**  version of BRAT ([sqlBRAT](https://github.com/Riverscapes/sqlBRAT) with no release yet), which will be necessary to support the **Commercial-Grade**   product. 

If you share the above vision, get in touch with the developers to support/fund the effort. 

--------------------



[[Riverscapes Consortium](https://riverscapes.net)]

The [Riverscapes Consortium's](https://riverscapes.net) Techncial Committee provides report cards for tools either deemed as "[riverscapes-compliant](https://tools.riverscapes.net)" or "[pending riverscapes-compliance](https://tools.riverscapes.net)".
