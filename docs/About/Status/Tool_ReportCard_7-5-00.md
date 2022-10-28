---
title: Riverscapes Report Card
weight: 1
---

<img class="float-right" src="https://riverscapes.net/assets/images/tools/grade/TRL_5_128w.png"><img class="float-left" src="https://gcd.riverscapes.net/assets/images/icons/GCD_Logo_White_wText.png"> GCD is one of original tools developed by [North Arrow Research](https://northarrowresearch.com), and Utah State University's [Ecogeomorphology & Topographic Analysis Lab](https://etal.joewheaton.org) the [Riverscapes Consortium](https://riverscapes.net). This report card communicates GCD's compliance with the Riverscape Consortium's published [tool standards](https://riverscapes.net/Tools).

# Report Card Summary

| Tool | [GCD - Geomorphic Change Detection Tool](https://gcd.riverscapes.net) |
| Version | [7.5.00](https://github.com/Riverscapes/gcd/releases/tag/7.5.0) |
| Date | 2020-06-18 |
| Assessment Team | Wheaton |
| [Current Assessment](http://brat.riverscapes.net/Tools#tool-status) | ![professional](https://raw.githubusercontent.com/Riverscapes/riverscapes-website/master/assets/images/tools/grade/TRL_5_32p.png) [Professional Grade](https://riverscapes.net/Tools/discrimination.html#tool-grade) |
| Target Status | ![commercial](https://raw.githubusercontent.com/Riverscapes/riverscapes-website/master/assets/images/tools/grade/TRL_5_32p.png) Commercial Grade |
| Riverscapes Compliance | ![Pending](https://riverscapes.net/assets/images/rc/RiverscapesCompliantPending_28.png) [Pending](https://riverscapes.net/Tools/#tools-pending-riverscapes-compliance) |
| Assessment Rationale | GCD has been applied extensively throughout the world. It has been used extensively both as a research tool and practitioner tool in both monitoring and design at the reach-scale. The developers have taught over 20 workshops form 2010 to 2019 and the tool had a large user base. It is well deserving of a Professional Grade. |



# Report Card Details

This tool's [discrimnation](https://riverscapes.net/Tools/discrimination#model-discrimination) evaluation by the [Riverscapes Consortium's](https://riverscapes.net) is:

**Evaluation Key:**
None or Not Applicable: <i class="fa fa-battery-empty" aria-hidden="true"></i> •
Minimal or In Progress: <i class="fa fa-battery-quarter" aria-hidden="true"></i> •
Functional: <i class="fa fa-battery-half" aria-hidden="true"></i> •
Fully Developed: <i class="fa fa-battery-full" aria-hidden="true"></i>  

| Criteria | Value | Evaluation | Comments and/or Recommendations |
|------------------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|----------------------------------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| :----------------------------- | :----------------------------- |  | :----------------------------- |
| Tool Interface(s) | <img src="https://riverscapes.net/assets/images/tools/esri_icon.png">:  ArcGIS 10.x AddIn, i class="fa fa-desktop" aria-hidden="true"></i> Stand Alone Windows Tool   | <i class="fa fa-battery-full" aria-hidden="true"></i> | Tool is a powerful, professionally developed software interface. The stand-alone version has no map interactivity, but produces the exact same projects, which can be viewed in GIS. The ESRI AddIn has full map interactivity. |
| Scale | Reach (cell scale resolution, reach scale extent) | <i class="fa fa-battery-full" aria-hidden="true"></i> | This tool has been applied over hundreds of thousands of reach-scale anlyses. Thousands of users have used it for change detection of their sites. As part of the [Columbia Habitat Monitoring Program](htttp://champmonitoring.org) the GCD command line version was made production grade and applied to over 5000 visits at over 900 long term monitoring sites. The tool has been run on high resolution topography for 10's of kilometers of riverscape. |
| Language(s) and Dependencies | C# | <i class="fa fa-battery-three-quarters" aria-hidden="true"></i> | This is a professionally developed, highly scaleable complied code. However, because it is in C#, very few researchers are using the source-code that are not involved in the development (most researchers are more converseant with Python). There are a number of depencies, but as this is complied software, it is easy to deploy these on Windows through installers. Moreover, all the internal depencies are open-source and there are no ArcObjects depencies. We give this a 3/4 ranking for dependencies and languages because i) the choice of C# is limiting uptake by reseachers, and ii) the map-based functionality is dependent on the proprietary ESRI ArcGIS 10.x software, which is 32 bit and [support will end in 2026](https://www.esri.com/arcgis-blog/products/arcgis-desktop/announcements/arcmap-continued-support/) |
| Vetted in Peer-Reviewed Literature | Yes.  [Wheaton et al. (2015)](http://brat.riverscapes.net/references.html#peer-reviewed-publication) | <i class="fa fa-battery-half" aria-hidden="true"></i> | The existing capacity model is vetted, and the historical capacity model is well described. The version in the publication is [2.0](https://github.com/Riverscapes/pyBRAT/releases/tag/v2.0.0), but the capacity model is basically the same in 3.0. Many of the risk, beaver management, conservation and restoration concepts have not yet been vetted in scholarly literature but have been applied, tested and vetted by many scientists and managers across the US and UK. |
| Source Code Documentation | Source code is clearly organized and documented | <i class="fa fa-battery-full" aria-hidden="true"></i> |  |
| Open Source | [open-source](https://github.com/Riverscapes/gcd) <i class="fa fa-github" aria-hidden="true"></i> with [GNU General Public License v 3.0](https://github.com/Riverscapes/gcd/blob/master/LICENSE) | <i class="fa fa-battery-three-quarters" aria-hidden="true"></i> | Open source code, but AddIn requires ArcGIS licenses to run. The standalone is more performant and requires no license to run, but provides no map-based interface. |
| Tool and Source Code Citeable | [![DOI](https://zenodo.org/badge/DOI/10.5281/zenodo.7248344.svg)](https://doi.org/10.5281/zenodo.7248344) | <i class="fa fa-battery-full" aria-hidden="true"></i> | Phlip Bailey, Joseph Wheaton, Matt Reimer, & James Brasington. (2020). Geomorphic Change Detection Software (7.5.0). Zenodo. https://doi.org/10.5281/zenodo.7248344 |

| User Documentation | [Installation](https://gcd.riverscapes.net/Download/), [Tutorials](https://gcd.riverscapes.net/Tutorials/), [Software Help](https://gcd.riverscapes.net/Help/) and [Workhsops](https://gcd.riverscapes.net/Workshops/). | <i class="fa fa-battery-half" aria-hidden="true"></i> | Documentation is comprehensive, but could be cleaned up. There are over 120 broken links. Not all of the videos are for the latest verions. There is a command line version of the code, but it does not write GCD projects, and there is no documentation. |
| Easy User Interface | Tool is primarily accessed by most users via a ESRI AddIn. Both this and the standalone work in exactly the same way and have easy to use user interfaces. | <i class="fa fa-battery-full" aria-hidden="true"></i> | The user interface is well designed and at version 7 has the advantage of being refined extensively over time. The numerous workshops taught clearly helped improve the user interface.  |
| Scalability | Computation engi | <i class="fa fa-battery-full" aria-hidden="true"></i> | The scalability is functional, but requires lots of custom scripting, has unnecessary hard-coding built in, and requires extensive manual pre-processing and preparation of data. |
|  Produces [Riverscapes Projects]({{ site.baseurl }}/Tools/Technical_Reference/Documentation_Standards/Riverscapes_Projects/) <img  src="https://riverscapes.net/assets/images/data/RiverscapesProject_24.png"> | Tool is outputing to disk data in a Rivescapes Project that can be opened in [RV](http://rave.riverscapes.net). | <i class="fa fa-battery-half" aria-hidden="true"></i> | Unfortunately, the riverscapes projects do NOT expose the entire [GCD Project tree](https://gcd.riverscapes.net/Concepts/projects.html) in [RV](https://rave.riverscapes.net) and only package up that there is a project, its name and location. This is fine as the GCD allows a pathway to open the projects. However, it is a limitation and seems inconsistent given that the tool was developed by the same developers of the [Riverscape Consortium warehouse](https://riverscapes.net/Data_Warehouses/) and [RV](https://rave.riverscapes.net). |

## F-A-I-R Assessment


## Tool Output Utility

| Criteria | Value | Evaluation | Comments |
|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|--------------------------------|----------------------------------------------------------|--------------------------------|
| :----------------------------- | :----------------------------- | :----------------------------- | :----------------------------- |
| [RAVE](https://rave.riverscapes.net)- Compliant Riverscapes Projects <img  src="https://riverscapes.net/assets/images/data/RiverscapesProject_24.png">? | Produces Riverscapes Project, but it is not a very useful project (you can't view the datasetes or results in RV) | <i class="fa fa-battery-quarter" aria-hidden="true"></i> | Refactoring needed and add Project Type registration with [`program.xml`]({{ site.baseurl }}/Tools/Technical_Reference/Documentation_Standards/Riverscapes_Projects/Program/) in [Program Repo](https://github.com/Riverscapes/Program) and include all datasets and parameters in project file. |
| [RAVE](https://rave.riverscapes.net) Business Logic Defined? | Not for [3.1.00](https://github.com/Riverscapes/pyBRAT/releases/tag/3.1.00), but example exists for BETA [sqlBRAT](https://github.com/Riverscapes/sqlBRAT) that is functional | <i class="fa fa-battery-empty" aria-hidden="true"></i> | Simple to remedy. Projects do currently have ArcGIS layer packages following project structure and entirely symbolized. |
| Riverscapes Projects hosted in public-facing [Riverscapes Warehouse(s)](https://riverscapes.net/Data_Warehouses/#warehouse-explorer-concept) <img src="https://riverscapes.net/assets/images/data/RiverscapesWarehouseCloud_24.png"> | No. Data is primarily on USU Box Servers and some on DataBasin.org. Users are pointed to where publicly available data exists from [here]({{ site.baseurl }}/BRATData/). | <i class="fa fa-battery-empty" aria-hidden="true"></i> | The data is very, very difficult to find from the inconsistent and incomplete [data pages]({{ site.baseurl }}/BRATData/). Warehousing is the goal, but in the meantime this could be made easier. |
| Riverscapes Projects connected to [Web-Maps](https://riverscapes.net/Data_Warehouses#web-maps) <i class="fa fa-map-o" aria-hidden="true"></i> | Not consistently. A proof of concept exist for [Idaho BRAT](https://riverscapes.github.io/BratMap/#/), but has not been cartographically curated. Similarly, a [DataBasin](https://databasin.org/datasets/1420ffb7e9674753a5fb626e2b830c1f) entry exists for [Utah BRAT](http://brat.riverscapes.net/BRATData/USA/UDWR_Utah/) | <i class="fa fa-battery-quarter" aria-hidden="true"></i> | All old data sets should be made Web Map accessible and clear about what version they were produced from and what years they correspond to (i.e. Riverscapes Project metadata) |
| Riverscapes Projects connected to Field [Apps](https://riverscapes.net//Data_Warehouses#apps---pwas) <img src="{{ site.baseurl }}/assets/images/tools/PWA.png"> | Not publicly. Some simple Arc Data Collector field apps have been used, but they are not reliable, scalable or deployable to external audiences. | <i class="fa fa-battery-quarter" aria-hidden="true"></i> | Workflows and forms are well tested and vetted. This needs funding to develop as commercial, professional-grade reliable web app. |

## Developer Intent

The BRAT [devleopment team]({{ site.baseurl }}/support.html) are actively seeking funding to build a **Commercial-Grade** <img src="https://riverscapes.net/assets/images/tools/grade/TRL_7_32p.png"> version of BRAT, which would:
- Have an inviting [web-map interface](https://riverscapes.net/Data_Warehouses/#web-maps) so non GIS-users can discover BRAT runs and explore them and interrogate them.
- Making it easy for GIS users to download BRAT for use in [RAVE](https://rave.riverscapes.net) with [Riverscapes Projects](https://riverscapes.net/Tools/Technical_Reference/Documentation_Standards/Riverscapes_Projects/) <img  src="https://riverscapes.net/assets/images/data/RiverscapesProject_24.png">
- Encourage more user-interaction with BRAT outputs and crowd-sourcing of information to create ownership of outputs
  - Allow users to visualize dynamic outputs of BRAT through time 
  - Allow users to upload their own BRAT projects
  - Allow users to provide their own inputs locally (@ a reach) and produce local realizations.
  - Allow users to upload (or make) their own beaver dam and activity observations
  - Allow discovery of past BRAT runs in Warehouse
  - Present transparent ranking of level of BRAT model curation or [dataset rank](https://riverscapes.net/Data_Warehouses/#dataset-rank) and facilitate community commenting
  - Facilitate users paying modest prices (i.e. commercial) to have new runs or more carefully curated (validated, resolved, etc.) for a specific watershed and then share them with broader community

![reports_TRL_BRAT]({{https://riverscapes.net/assets\images\tools\TRL_badges_pngs\reports_TRL_BRAT.jpg)

The development team at this point has already produced a beta version of a **Production-Grade** <img  src="https://riverscapes.net/assets/images/tools/grade/TRL_6_32p.png"> version of BRAT ([sqlBRAT](https://github.com/Riverscapes/sqlBRAT) with no release yet), which will be necessary to support the **Commercial-Grade**  <img src="https://riverscapes.net/assets/images/tools/grade/TRL_7_32p.png"> product. 

If you share this [vision]({{ site.baseurl }}/Vision.html), get in touch with the developers to support/fund the effort. 




<a href="https://riverscapes.net"><img class="float-left" src="https://riverscapes.net/assets/images/rc/RiverscapesConsortium_Logo_Black_BHS_200w.png"></a>
The [Riverscapes Consortium's](https://riverscapes.net) Techncial Committee provides report cards for tools either deemed as "[riverscapes-compliant](https://riverscapes.net/Tools/#riverscapes-compliant)" <img  src="https://riverscapes.net/assets/images/rc/RiverscapesCompliant_24.png"> or "[pending riverscapes-compliance](https://riverscapes.net/Tools/#tools-pending-riverscapes-compliance)" <img  src="https://riverscapes.net/assets/images/rc/RiverscapesCompliantPending_28.png">. 
