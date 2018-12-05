---
title: New GCD Project
weight: 1
---

The `Create New GCD Project` dialog is where you create a new [project]({{ site.baseurl }}/Concepts/projects.html) by specifying the fields described below.

![New project]({{ site.baseurl}}/assets/images/CommandRefs/01_Project/new-project.png)

- **Name** (required): This is any descriptive name that you want to give to the project. Spaces and special characters can be used.
- **Parent directory** (required): This is where the project files will be stored. All inputs loaded through the [*Survey Library*]({{ site.baseurl }}/system/errors/NodeNotFound?suri=wuid:gx:3ed05905e41de6f6) will be copied and/or created in the '[`Inputs`]({{ site.baseurl }}/gcd-concepts/project/inputs-folder)' folder, whereas all outputs will be stored in the '[`Analyses`]({{ site.baseurl }}/gcd-concepts/project/analsyses-folder)' folder. Each parent folder can only contain a single GCD project.
- **GCD Project File**: The GCD will generate the project file inside the parent directory, using the project name without any spaces or other special characters. GCD project files have the `*.gcd` file suffix.
- **Description** (optional): This is an optional field where you can save any additional information about your project.
- **Metadata** (optional): A series of key value pairs of information that can be used to tag projects. These tags can be useful when you have lots of GCD projects and perhaps you are using a script to automate their batch processing using another software tool or script.
- **Raster Units**
	- **Horizontal** (required): The linear horizontal units of the rasters that will be used within the project. All rasters within a single GCD project must possess the same horizontal units. You can change this setting up until the first raster is imported into the project, after which this value becomes fixed. Leave this value with the default and GCD will automatically set this value as you import the first raster into the project.
	- **Vertical** (required): The linear vertical (elevation) raster units. The same limitations apply as to the horizontal units.
- **GCD Display Units**
	- **Area** (optional): The areal units that GCD will use to display analysis results. This setting can be changed at any time.
	- **Volume** (optional): The volumetric units that GCD will use to display analysis results. This setting can be changed at any time.

This video walks through the creation of a new project and shows what files and folders are created as a result of this process.

<div class="responsive-embed">
<iframe width="560" height="315" src="https://www.youtube.com/embed/YLMDF38R_8U?rel=0" frameborder="0" allow="autoplay; encrypted-media" allowfullscreen></iframe>
</div>

------
<div align="center">
	<a class="hollow button" href="{{ site.baseurl }}/Help"><i class="fa fa-chevron-circle-left"></i>  Back to GCD Help </a>  
	<a class="hollow button" href="{{ site.baseurl }}/"><img src="{{ site.baseurl}}/assets/images/icons/GCDAddIn.png">  Back to GCD Home </a>  
</div>