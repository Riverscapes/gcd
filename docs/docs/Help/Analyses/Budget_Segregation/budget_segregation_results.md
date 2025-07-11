---
title: Budget Segregation Results
slug: /Help/Analyses/Budget_Segregation/budget-segregation
---

The GCD software produces a rich set of results with each [budget segregation](/Help/Analyses/Budget_Segregation/budget-segregation). This section describes both the user interface for exploring results within the software as well as the files that are saved to the project folder structure as part of the analysis.

## User Interface

The budget segregation results user interface is displayed once the analysis has completed. They are also accessible by right clicking on an individual budget segregation in the project explorer:

![Same surfaces](/img/CommandRefs/05_Analyses/cd/budget/budget_right.png)

The results user interface is virtually identical to that for [change detections](/Help/Analyses/Change_Detection/change-detection-results#user-interface) with the primary difference being that budget segregation results are shown **per class**. This means that the tabular and charts displayed are for a set of polygons within the [mask](/Help/Inputs/Masks/regular-masks) that was used to generate the budget segregation.

![Same surfaces](/img/CommandRefs/05_Analyses/cd/budget/budget_results_table.png)

Below the class dropdown, the user can also change what the raw represents in each of the results tab. This is particularly useful for the histograms where this choice impacts what unthresholded data are shown as the gray backdrop data series. 

| Raw Representing | Description | Example Histogram |
|---|---|---|
| Raw DoD Area of Interest | The raw values in the table and gray area of the chart represent the unthresholded change in the original DEM of difference for the entire area of change, not just the current budget class. | ![r1](/img/CommandRefs/05_Analyses/cd/budget/budget_raw1.png) |
| Thresholded DoD Area of Detectable Change | The raw values in the table and the gray area of the cart represent the thresholded change in the original DEM of difference for the entire area of change, not just the current budget class. | ![r1](/img/CommandRefs/05_Analyses/cd/budget/budget_raw2.png) |
| Raw DoD Within Budget Class | The raw values in the table and the gray area in the chart represent the unthresholded change only within the currently selected budget class. | ![r1](/img/CommandRefs/05_Analyses/cd/budget/budget_raw3.png) |
| Thresholded DoD Within Budget Class | The raw values in the table and the gray area in the chart represent the thresholded change within the current budget class. This will always be the same as the thresholded values in the table and the colored areas of the chart. This included simply to demonstrate the concept of the "raw represents" input. | ![r1](/img/CommandRefs/05_Analyses/cd/budget/budget_raw4.png) |

## Longitudinal Breakdown

The longitudinal breakdown shows the volume of surface raising and lowering for each class together with the uncertainty associated with the values.

![longitudinal](/img/CommandRefs/05_Analyses/cd/budget/budget_results_long.png)

## Files and Folders

Budget segregation results are stored in a folder called `BS` under that of the parent change detection. Each budget segregation gets a separate sub folder with the same prefix:

```
<Change Detection Folder>\BS\BS0001
```

The files generated are virtually the same as [those produced for a change detection](/Help/Analyses/Change_Detection/change-detection-results#files-and-folders) with the primary exception being that a budget segregation does not possess any rasters. It is merely a mathematical summation of the change detection results and does not produce any geospatial results.