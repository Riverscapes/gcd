---
title: Batch Change Detection
---

It can be time consuming to set up and perform several change detection analyses. The GCD software simplifies this process with two batch change detection features. This section describes the generic batch intended for quickly adding multiple analyses with different surfaces and thresholding. See the [multi-epoch batch] documentation for information on the other kind.

![Add mask]({{ site.baseurl }}/assets/images/CommandRefs/05_Analyses/cd/batch/batch_cms.png)

# Creating a Batch Analysis

Right click on the change detection item in the project explorer and choose "Generic Batch Change Detection" (see above).

The form shows an empty table where each row represents a separate change detection analysis. It is initially empty. Clicking the ![plus]({{ site.baseurl }}/assets/images/icons/Add.png) plus button reveals a menu for adding different types of analyses described below.

![Add mask]({{ site.baseurl }}/assets/images/CommandRefs/05_Analyses/cd/batch/batch_add.png)

## Single Minimum Level of Detection

Use this feature to add just a single change detection between two surfaces using MinLoD thresholding. The controls are very similar to the [manual change detection]({{ site.baseurl}}/Help/Analyses/Change_Detection/change-detection.html#calculating-a-change-detection) user interface.

![Add mask]({{ site.baseurl }}/assets/images/CommandRefs/05_Analyses/cd/batch/batch_single_minlod.png)

## Multiple Minimum Level of Detection

Use this feature to add multiple change detections **between the same two surfaces** but with varying minimum levels of detection.

Specify a minimum, maximum and interval threshold. When the batch is run, the GCD will iterate starting from the minimum threshold, increasing each time by the interval until the maximum threshold is reached. Note that a change detection with the maximum threshold will only be produced if the range specified (maximum - minimum) is evenly divisible by the interval.

![Add mask]({{ site.baseurl }}/assets/images/CommandRefs/05_Analyses/cd/batch/batch_multi_minlod.png)

## Propagated Error 

Specifying a propagated error change detection only requires the new and old surfaces with corresponding error surfaces. Again, see the [manual change detection]({{ site.baseurl}}/Help/Analyses/Change_Detection/change-detection.html#calculating-a-change-detection) documentation for how to use these controls.

## Single Probabilistic

Much like the single MinLoD described above, this approach requires the surfaces to be specified together with a single probabilistic thresholding value.  Again, see the [manual change detection]({{ site.baseurl}}/Help/Analyses/Change_Detection/change-detection.html#calculating-a-change-detection) documentation for how to use these controls.

## Multiple Probabilistic

The multiple probabilistic batch requires a range of confidence intervals to be specified, analogous to the multiple MinLoD approach described above. In this case, Bayesian updating can also be applied to all the change detections that will be added to the batch. Should you want to explore the impact of this Bayesian updating simply repeat adding multiple probabilistic items to the batch, once with and once without the Bayesian updating turned on.

![Add mask]({{ site.baseurl }}/assets/images/CommandRefs/05_Analyses/cd/batch/batch_multi_prob.png)

# Running the Batch

Once you have populated the table with the desired batch of change detection analyses click the **Run Batch** button. Large batches might take several minutes to process. Watch the status bar for an indication of progress.

Batch change detection results are stored and displayed in the GCD project explorer just like any other, manual, change detection. Click on any individual analysis to view the results or use the [inter-comparison]({{site.baseurl}}/Help/Analyses/Change_Detection/intercomparison.html) feature to compare several analyses to each other.