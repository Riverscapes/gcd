---
title: Budget Segregation
slug: /help/analyses/budget_segregation/budget_segregation_results
---

A GCD budget segregation is a way of grouping regions of a [change detection](/Help/Analyses/Change_Detection/change-detection) together into separated results. A polygon [mask](/Help/Inputs/Masks/regular-masks) is used to aggregate together all the change together within one or more polygons and then present them as if they are a separate analysis.

This might be useful if you already know categorical regions within your study area, or alternatively you might study the results of a regular change detection and then want to group together regions that have a similar signal. For example, geomorphic units within a river channel make for an ideal budget segregation. With this approach you can look at the change within pools separately from bars and the area outside the channel.

Within the GCD, each budget segregation is an operation that is performed on an existing change detection by applying a mask. 

## Creating a Budget Segregation

Right click on an individual change detection in the GCD project explorer and choose **Add Budget Segregation**.

![budget cms](/img/CommandRefs/05_Analyses/cd/budget/budget_cms.png)

Enter a name for the budget segregation in the form that appears. The GCD software suggests a default name that uses the selected mask name. 

The output folder is prescribed by the GCD and cannot be changed.

Choose a regular or directional mask. The mask can be a [regular](/Help/Inputs/Masks/regular-masks) or a [directional](/Help/Inputs/Masks/directional-masks), but note that if you want to continue and perform a [morphological analysis](/Help/Analyses/Change_Detection/morphological) on the budget segregation you must use a directional mask.

Click Create ![create](/img/icons/Save.png) to generate the budget segregation.

![budget cms](/img/CommandRefs/05_Analyses/cd/budget/budget_add.png)

## Context Menu

Right clicking on a budget segregation item in the GCD project explorer reveals a context menu containing several operations that are described below.

![budget cms](/img/CommandRefs/05_Analyses/cd/budget/budget_right.png)

## View Budget Segregation Results

The budget segregation results form is very similar to the [change detection results](/Help/Analyses/Change_Detection/change-detection-results) results form, albeit with the added ability to distinguish between each class within the budget segregation. 

See the [budget segregation results](/Help/Analyses/Budget_Segregation/budget_segregation_results) documentation for more information.

## Add Morphplogical Analysis

See the [morphological analysis](/Help/Analyses/Change_Detection/morphological) section on how the GCD can perform a simplified sediment transport analysis on the results of a budget segregation providing that you used a [directional mask](/Help/Inputs/Masks/directional-masks) to generate the budget segregation itself.

## Delete Budget Segregation

Deleting a budget segregation deletes all files associated with the analysis including dependent morphological analyses.

Note that no files related to the analysis being deleted should be open in other software when you attempt to delete the change detection or you will get a warning.

## Explorer Budget Segregations Folder

Exploring the budget segregations folder item will launch Windows Explorer at the location of the results on your hard drive. See the [Files and Folders](/Help/Analyses/Budget_Segregation/budget_segregation_results#files-and-folders) section of the budget segregation results documentation.
