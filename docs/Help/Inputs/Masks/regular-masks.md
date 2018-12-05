---
title: Regular Masks
---

<div class="float-right">
<img src="{{ site.baseurl }} /assets/images/CommandRefs/00_ProjectExplorer/inputs/masks/regular.png"></div>
Regular masks group together regions of a GCD project into different categories. Each regular mask is represented as a polygon ShapeFile. An attribute field in the ShapeFile is used to distinguish the different regions. Muliple polygons can have the same region category.

Regular masks are used during [budget segregations]({{site.baseurl}}/Help/Analyses/Budget_Segregation/budget-segregation.html) to summarise [change detection analyses]().

The same basic requirements apply to all types of mask Polygon ShapeFiles:

* Must use the same spatial reference as the DEM Surveys in the project.
* Must contain at least one feature.
* Must not contain any multi-part features.
* Must not contain any features with null geometries.

The polygons in a regular mask can overlap but this is not advisable since it will produce double accounting during [budget segregations]().

# Add Existing Regular Mask

To use an existing polygon ShapeFile as a regular mask right click on the Masks node of the GCD Project Explorer and choose **Add Existing Regular Mask**.

![Add mask]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/masks/regular_add.png)

Provide a unique name for the mask and then click the plus icon to browse and select the polygon ShapeFile that you want to use. The project path for the regular mask will be assigned automatically.

Pick the **string** attribute **field** in the ShapeFile that you want to use to distinguish the mask regions. A list of the unique values in this field is then displayed below. Checking or unchecking the box beside each of the unique values in this field determines whether GCD includes the polgons when it is used.

The righthand label column is editable and can be used to override the field value for the purposes of displaying results within the GCD. For example a field value of "total_station" might have the label "Total Station" applied so as to make tables and figures easier to read. All labels must be unique and cannot be blank, empty strings.

Note that changing the status and labels of fields can also be done after the mask has been added to the GCD project, during editing. Any budget segregations that use this mask will get updated to reflect this change.

![Add mask]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/masks/regular_mask.png)

# Context Menu

Right clicking on any regular mask brings up the context menu that allows you to perform the three operations described below. Note that the Add To Map option is only available in the ArcGIS Addin version of GCD and not the Standalone.

![mask cms]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/masks/regular_cms.png)

## Edit Properties

Editing the properties of a regular mask it is possible to change the name used to refer to it and also which field values are active. However, it is not possible to change which attribute field is used.

## Add To Map

Regular masks are added to the current ArcMap document with a  semi-transparent symbology and labels that identify each region.

## Delete

Deleting a regular mask removes it from the GCD project and permanently deletes the underlying ShpeFile within the GCD project. Note that you cannot delete a particular mask until all [budget segregations]({{site.baseurl}}/Help/Analyses/Budget_Segregation/budget-segregation.html) that refer to it have also been deleted.

![mask cms]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/inputs/masks/regular_inuse.png)
