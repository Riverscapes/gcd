---
title: New GCD Project
sidebar_position: 1
slug: /help/project-menu/new-project
---

The `Create New GCD Project` dialog is where you create a new [project](/Concepts/projects) by specifying the fields described below.

![New project](/img/CommandRefs/01_Project/new-project.png)

- **Name** (required): This is any descriptive name that you want to give to the project. Spaces and special characters can be used.
- **Parent directory** (required): This is where the project files will be stored. All inputs will be copied and/or created in the '[`Inputs`](/Help/Inputs/dem-surveys#add-existing-dem-survey)' folder, whereas all outputs will be stored in the '[`Analyses`](/Help/Analyses/Change_Detection/change-detection-results#files-and-folders)' folder. Each parent folder can only contain a single GCD project.
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

<YouTubeEmbed videoId="YLMDF38R_8U" title="New Project Video" />
