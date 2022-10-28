---
title: Riverscapes Report Card
weight: 1
---

<img class="float-right" src="https://riverscapes.net/assets/images/tools/grade/TRL_5_128w.png"><img class="float-left" src="https://gcd.riverscapes.net/assets/images/screen/GCD_Logo_White_wText.png"> GCD is one of originall tools developed by [North Arrow Research](https://northarrowresearch.com), and Utah State University's [Ecogeomorphology & Topographic Analysis Lab](https://etal.joewheaton.org) the [Riverscapes Consortium](https://riverscapes.net). This report card communicates GCD's compliance with the Riverscape Consortium's published [tool standards](https://riverscapes.net/Tools).

# Report Card Summary

| Tool | [BRAT - Beaver Restoration Assessment Tool](https://gcd.riverscapes.net) |
| Version | [3.1.00](https://github.com/Riverscapes/gcd/releases/tag/7.5.0) |
| Date | 2020-06-18 |
| Assessment Team | Wheaton |
| [Current Assessment](http://brat.riverscapes.net/Tools#tool-status) | ![professional](https://raw.githubusercontent.com/Riverscapes/riverscapes-website/master/assets/images/tools/grade/TRL_5_32p.png) [Professional Grade](https://riverscapes.net/Tools/discrimination.html#tool-grade) |
| Target Status | ![commercial](https://raw.githubusercontent.com/Riverscapes/riverscapes-website/master/assets/images/tools/grade/TRL_5_32p.png) Commercial Grade |
| Riverscapes Compliance | ![Pending](https://riverscapes.net/assets/images/rc/RiverscapesCompliantPending_28.png) [Pending](https://riverscapes.net/Tools/#tools-pending-riverscapes-compliance) |
| Assessment Rationale | GCD has been applied extensively throughout the world. It has been used extensively both as a research tool and practitoner tool in both monitoring and design at the reach-scale. The developers have taught over 20 workshops form 2010 to 2019 and the tool had a large user base. It is well deserving of a Professional Grade. |



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
| Tool Interface(s) | <img src="https://riverscapes.net/assets/images/tools/ArcPyToolbox.png">:  [ArcPy Toolbox in ArcGIS](https://desktop.arcgis.com/en/arcmap/10.7/analyze/creating-tools/a-quick-tour-of-python-toolboxes.htm) | <i class="fa fa-battery-half" aria-hidden="true"></i> | Tool is a series of steps (wizard-driven) that is very specific to freely available US data. |
| Scale | Network (reach scale resolution, watershed extent) | <i class="fa fa-battery-full" aria-hidden="true"></i> | This tool has been applied across entire states, regions and watersheds, resolving detail down to 250 m to 300 m length reaches of riverscape. |
| Language(s) and Dependencies | Python with ArcPy | <i class="fa fa-battery-half" aria-hidden="true"></i> | [Dependencies are well documented](http://brat.riverscapes.net/Documentation/Download/ComputerSetup.html#python-prerequisites). ArcPy, [NumPy_mkl](https://www.lfd.uci.edu/~gohlke/pythonlibs/#numpy) and <br>[scikit-fuzzy dependencies](https://pypi.python.org/pypi/scikit-fuzzy). |
| Vetted in Peer-Reviewed Literature | Yes.  [Macfarlane et al. (2015)](http://brat.riverscapes.net/references.html#peer-reviewed-publication) | <i class="fa fa-battery-half" aria-hidden="true"></i> | The existing capacity model is vetted, and the historical capacity model is well described. The version in the publication is [2.0](https://github.com/Riverscapes/pyBRAT/releases/tag/v2.0.0), but the capacity model is basically the same in 3.0. Many of the risk, beaver management, conservation and restoration concepts have not yet been vetted in scholarly literature but have been applied, tested and vetted by many scientists and managers across the US and UK. |
| Source Code Documentation | Source code is clearly organized and documented | <i class="fa fa-battery-full" aria-hidden="true"></i> |  |
| Open Source | [open-source](https://github.com/Riverscapes/pyBRAT) <i class="fa fa-github" aria-hidden="true"></i> with [GNU General Public License v 3.0](https://github.com/Riverscapes/pyBRAT/blob/master/LICENSE) | <i class="fa fa-battery-half" aria-hidden="true"></i> | Open source code, but code requires ArcGIS licenses to run. |
| User Documentation | [Installation](http://brat.riverscapes.net/Documentation/Download/), [Tutorials](http://brat.riverscapes.net/Documentation/Tutorials/) with videos, and implementation tips | <i class="fa fa-battery-half" aria-hidden="true"></i> | Documentation is comprehensive, but could be streamlined, contains many out of date components (e.g. applies to V 2.0). It would be helpful to separate tutorials from a command reference. Also it would be helpful to have conceptual references. |
| Easy User Interface | Tool is primarily accessed via ArcToolbox custom tools or command prompt. | <i class="fa fa-battery-quarter" aria-hidden="true"></i> | For a power ArcGIS user that is familiar with geoprocessing and really understands the model, they can make this work. However, the user experience is fragile, not very flexible and this is not an easy-to-use tool for end user. Most "users" or consumers of BRAT, do not run pyBRAT, but instead think of the "tool" as the outputs of the model. |
| Scalability | Tool can be batched, and development team made major strides in implementing batch-processing (typically applied to HUC-8s). | <i class="fa fa-battery-half" aria-hidden="true"></i> | The scalability is functional, but requires lots of custom scripting, has unnecessary hard-coding built in, and requires extensive manual pre-processing and preparation of data. |
|  Produces [Riverscapes Projects]({{ site.baseurl }}/Tools/Technical_Reference/Documentation_Standards/Riverscapes_Projects/) <img  src="https://riverscapes.net/assets/images/data/RiverscapesProject_24.png"> | Tool is outputing to disk data in a Rivescapes Project. | <i class="fa fa-battery-half" aria-hidden="true"></i> | Unfortunately, [3.1.00](https://github.com/Riverscapes/pyBRAT/releases/tag/3.1.00) is not producing Riverscapes Projects that are fully-compatible or registered with [RAVE](https://rave.riverscapes.net) |

## F-A-I-R Assessment


## Tool Output Utility

| Criteria | Value | Evaluation | Comments |
|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|--------------------------------|----------------------------------------------------------|--------------------------------|
| :----------------------------- | :----------------------------- | :----------------------------- | :----------------------------- |
| [RAVE](https://rave.riverscapes.net)- Compliant Riverscapes Projects <img  src="https://riverscapes.net/assets/images/data/RiverscapesProject_24.png">? | Produces Riverscapes Project, but not RAVE-compliant | <i class="fa fa-battery-quarter" aria-hidden="true"></i> | Refactoring needed and add Project Type registration with [`program.xml`]({{ site.baseurl }}/Tools/Technical_Reference/Documentation_Standards/Riverscapes_Projects/Program/) in [Program Repo](https://github.com/Riverscapes/Program) and include all datasets and parameters in project file. |
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

![reports_TRL_BRAT]({{ site.baseurl }}\assets\images\tools\TRL_badges_pngs\reports_TRL_BRAT.jpg)

The development team at this point has already produced a beta version of a **Production-Grade** <img  src="https://riverscapes.net/assets/images/tools/grade/TRL_6_32p.png"> version of BRAT ([sqlBRAT](https://github.com/Riverscapes/sqlBRAT) with no release yet), which will be necessary to support the **Commercial-Grade**  <img src="https://riverscapes.net/assets/images/tools/grade/TRL_7_32p.png"> product. 

If you share this [vision]({{ site.baseurl }}/Vision.html), get in touch with the developers to support/fund the effort. 




<a href="https://riverscapes.net"><img class="float-left" src="{{ site.baseurl }}/assets/images/rc/RiverscapesConsortium_Logo_Black_BHS_200w.png"></a>
The [Riverscapes Consortium's](https://riverscapes.net) Techncial Committee provides report cards for tools either deemed as "[riverscapes-compliant](https://riverscapes.net/Tools/#riverscapes-compliant)" <img  src="{{ site.baseurl }}/assets/images/rc/RiverscapesCompliant_24.png"> or "[pending riverscapes-compliance](https://riverscapes.net/Tools/#tools-pending-riverscapes-compliance)" <img  src="{{ site.baseurl }}/assets/images/rc/RiverscapesCompliantPending_28.png">. 
