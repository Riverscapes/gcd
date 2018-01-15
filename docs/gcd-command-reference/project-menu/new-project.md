---
title: A. New GCD Project
---

The `Create New Project` dialog is where you create a new [project]({{ site.baseurl }}/gcd-concepts/project) by specifying the following information: 

- **Name** (required): This is any descriptive name that you want to give to the project. Spaces and special characters can be used.
- **Parent directory** (required): This is where the project files will be stored. All inputs loaded through the [*Survey Library*]({{ site.baseurl }}/system/errors/NodeNotFound?suri=wuid:gx:3ed05905e41de6f6) will be copied and/or created in the '[`Inputs`]({{ site.baseurl }}/gcd-concepts/project/inputs-folder)' folder, whereas all outputs will be stored in the '[`Analyses`]({{ site.baseurl }}/gcd-concepts/project/analsyses-folder)' folder.
- **GCD Project File** (optional): By default, this is the same as the Output file directory (recommended), but you can optionally choose where the `*.gcd` [project XML file]({{ site.baseurl }}/gcd-concepts/project/-gcd-files) is stored.
- **Description** (optional): This is an optional field where you can save any additional information about your project.
- **Horizontal decimal precision**: The horizontal decimal precision refers to the number of decimals places used to define the raster extent and cell size. This value should be the minimum number of decimal places required to accurately store the raster cell size. e.g. a cell size of 0.01m (1cm) should use a precision of 2.

![GCD6_Form_CreateNewGCDProject_HorizontalDecimalPrecision]({{ site.baseurl}}/assets/images/GCD6_Form_CreateNewGCDProject_HorizontalDecimalPrecision.png)

![GCD6_Form_CreateNewGCDProject]({{ site.baseurl}}/assets/images/GCD6_Form_CreateNewGCDProject.png)

This video walks through the creation of a new project using the `Create New Project` dialog and shows what files and folders are created as a result of this process.

<iframe width="560" height="315" src="https://www.youtube.com/embed/YPeVRjoq0Y0" frameborder="0" allowfullscreen></iframe>

When [project XML file]({{ site.baseurl }}/gcd-concepts/project/-gcd-files) (*.gcd) gets created, these are the XML tags that get populated in the project (example below is what gets saved when the dialog is filled out as specified above:

![GCDSnipett]({{ site.baseurl}}/assets/images/GCDSnipett.png)