---
title: Profile Routes
weight: 5
---

<div class="float-right">
<img src="{{ site.baseurl }} /assets/images/CommandRefs/00_ProjectExplorer/inputs/profile/profile_routes.png"></div>
Profile routes represent lines that can be used to extract values from the underlying GCD rasters via the [linear extraction]({{site.baseurl}}/Help/Inputs/linear-extractions.html) feature. The GCD uses two types of profile routes that are distinguished by their direction:

* **Transect** profile routes can be thought of as passing *across* the data. In the context of rivers, these might be thought of as cross sections or river station transects.
* **Longitudinal** profile routes pass *along* the data and, as the name suggests are typically used to extract river long profiles.

The fields required to generate these two types of profile routes differ slightly, but apart from that the way that they are used to [extract values]({{site.baseurl}}/Help/Inputs/linear-extractions.html) from underlying rasters, as well as the way that they are managed within the GCD software, is identical.

# Add Existing Transect Profile Route

To use an existing polyline ShapeFile as a transect based profile route right click on the Profile Routes node of the GCD Project Explorer and choose **Add Existing Transect Profile Route**.

![Add transect]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/profile/add_transect.png)

Provide a unique name for the profile route and then click the plus icon to browse and select the polyline ShapeFile that you want to use. The project path for the regular mask will be assigned automatically.

Select an attribute field that contains the station values. These represent the distance of each transect line from some arbitrary reference point. Typically they are river miles or kilometers but can in reality be any value. The field used must be of type **floating point**.

The optional label field can be used to override the strings in the value field with more appropriate text for display in the GCD software. For example the attribute value of "total_station" might have a label called "Total Station".

![Transect]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/profile/transect_details.png)

# Add Existing Longitudinal Profile Route

To use an existing polyline ShapeFile as a longitudinal based profile route right click on the Profile Routes node of the GCD Project Explorer and choose **Add Existing Longitudinal Profile Route**.

![Add longitudinal]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/profile/add_long.png)

Provide a unique name for the profile route and then click the plus icon to browse and select the polyline ShapeFile that you want to use. The project path for the regular mask will be assigned automatically.

Select an attribute field that contains the station offset values. This represents the distance from some reference point for the starting point of each longitudinal line feature. In the context of rivers, this typically represents the river mile/kilometer of the **upstream** most point of each line. If the ShapeFile has several lines, representing a mainstem with side channels and for example, then a river distance value is needed for each separate feature. Note that GCD does not attempt to capture or use any form of topology at this time.

The optional label field can be used to override the strings in the value field with more appropriate text for display in the GCD software. For example the attribute value of "total_station" might have a label called "Total Station".

![Transect]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/profile/long_details.png)

# Context Menu

Right clicking on a profile route of either type brings up the context menu that allows you to perform the three operations described below. Note that the Add To Map option is only available in the ArcGIS Addin version of GCD and not the Standalone.

![mask cms]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/profile/profile_cms.png)

# Edit Properties

Editing the properties of a profile route it is possible to change the name used to refer to it and change the station values or offsets field depending on whether it is a transect or longitudinal profile route.

# Add To Map

Profile routes are added to the current ArcMap document with ESRI's default symbology settings for vector ShapeFiles.

# Delete

Deleting a profile route removes it from the GCD project and permanently deletes the underlying ShpeFile within the GCD project.