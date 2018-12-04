---
title: Inter Comparisons
weight: 3
---

Intercomparisons qucikly and easily summarize multiple [change detections]({{ site.baseurl}}/Help/Analyses/Change_Detection/change-detection.html) together. This does not involve any geospatial analysis but merely involves an accounting exercise to sum up the various results from the relevant analyses.

Each GCD project can have multiple intercomparisons that can be added, viewed and deleted.

# Creating an Inter-Comparison

Right click on the Intercomparisons items in the GCD project explorer and choose "Create Change Detection Inter-Comparision":

![Intercomparison CMS]({{ site.baseurl }}/assets/images/CommandRefs/05_Analyses/cd/inter/inter_cms.png)

In the form that appears provide a unique name for the new inter-comparison and then check the box beside each change detection that you want to include. Right click on the list to quickly select all or none of the items.

![Intercomparison CMS]({{ site.baseurl }}/assets/images/CommandRefs/05_Analyses/cd/inter/inter_detail.png)

Click Save ![Intercomparison CMS]({{ site.baseurl }}/assets/images/icons/Save.png) to run the analysis.

# View Inter-Comparison Results

Right click on an individual inter-comparison in the GCD project explorer to view the folder in which it was generated.

![Intercomparison CMS]({{ site.baseurl }}/assets/images/CommandRefs/05_Analyses/cd/inter/inter_view.png)

This will open Windows Explorer at the relevant location. The inter-comparison itself is represented by a single Microsoft Excel Spreadshet XML file. 

![Intercomparison CMS]({{ site.baseurl }}/assets/images/CommandRefs/05_Analyses/cd/inter/inter_browse.png)

Use Microsoft Excel to open the inter-comparison Spreadsheet. The layout is an adaption of the change detection [tabular results](). Each row represents one of the change detections selected for inclusion and the columns the GCD change statistics. Additional columns are included that show the proportion of the total that each analysis represents.

![Intercomparison CMS]({{ site.baseurl }}/assets/images/CommandRefs/05_Analyses/cd/inter/inter_results.png)

# Delete An Inter-Comparison

Before attempting to delete an inter-comparison, be sure to close the corresponding Excel Spreadsheet to prevent file lock issues. Then right click on an individual inter-comparison and choose **Delete** to remove it from the GCD project. 