---
title: Riverscapes Report Card
sidebar_position: 1
---

<!-- ![TRL 5](/img/TRL_5_128w.png) -->
![GCD Logo](/img/icons/GCD_Logo_White_wText.png)<br />

GCD is one of original tools developed by [North Arrow Research](https://northarrowresearch.com), and Utah State University's [Ecogeomorphology & Topographic Analysis Lab](https://etal.joewheaton.org) and the [Riverscapes Consortium](https://riverscapes.net). This report card communicates GCD's compliance with the Riverscape Consortium's published [tool standards](https://riverscapes.net/Tools).



# Report Card Summary

| Key | Value |
| --- | --- |
| Tool | [GCD - Geomorphic Change Detection Tool](https://gcd.riverscapes.net) |
| Version | [7.5.00](https://github.com/Riverscapes/gcd/releases/tag/7.5.0) |
| Date | 2020-06-18 |
| Assessment Team | Wheaton |
| [Current Assessment](http://brat.riverscapes.net/Tools#tool-status) |[Professional Grade](https://riverscapes.net/Tools/discrimination.html#tool-grade) |
| Target Status |Commercial Grade|
| Riverscapes Compliance |[Riverscapes Compliant](https://riverscapes.net/Tools/#riverscapes-compliant-tools) |
| Assessment Rationale | GCD has been applied extensively throughout the World. It has been used as a research tool and practitioner tool in both monitoring and design at the reach-scale. The developers have taught over 20 workshops from 2010 to 2019 and the tool has a large user base. It is well deserving of a Professional Grade. |

# Report Card Details

This tool's [discrimnation](https://riverscapes.net/Tools/discrimination#model-discrimination) evaluation by the [Riverscapes Consortium's](https://riverscapes.net) is:

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
| User Documentation | [Installation](https://gcd.riverscapes.net/Download/), [Tutorials](https://gcd.riverscapes.net/Tutorials/), [Software Help](https://gcd.riverscapes.net/Help/) and [Workhsops](https://gcd.riverscapes.net/Workshops/). | <i class="fa fa-battery-half" aria-hidden="true"></i> | Documentation is comprehensive, but could be cleaned up. There are over 120 broken links. Not all of the videos are for the latest versions. There is a command line version of the code, but it does not write GCD projects, and there is no documentation. |
| Easy User Interface | Tool is primarily accessed by most users as an ESRI AddIn. Both this and the standalone work in exactly the same way and have easy to use user interfaces. | <i class="fa fa-battery-full" aria-hidden="true"></i> | The user interface is well designed and at version 7 has the advantage of being refined extensively over time. The numerous workshops taught clearly helped improve the user interface. |
| Scalability | Computation engine | <i class="fa fa-battery-full" aria-hidden="true"></i> | The scalability is functional, but requires lots of custom scripting, has unnecessary hard-coding built in, and requires extensive manual pre-processing and preparation of data. |
| Produces [Riverscapes Projects](/Tools/Technical_Reference/Documentation_Standards/Riverscapes_Projects/)  | Tool is outputting to disk data in a Riverscapes Project that can be opened in [RV](http://rave.riverscapes.net). | <i class="fa fa-battery-half" aria-hidden="true"></i> | Unfortunately, the riverscapes projects do NOT expose the entire [GCD Project tree](https://gcd.riverscapes.net/Concepts/projects.html) in [RV](https://rave.riverscapes.net) and only package up that there is a project, its name and location. This is fine as the GCD allows a pathway to open the projects. However, it is a limitation and seems inconsistent given that the tool was developed by the same developers of the [Riverscape Consortium warehouse](https://riverscapes.net/Data_Warehouses/) and [RV](https://rave.riverscapes.net). |
