---
title: GCD Project Explorer
sidebar_position: 1
slug: /Help/GCD_Project_Explorer
---


The GCD Project Explorer helps you navigate your project. It is also the primary means by which you add data to your project, perform analyses, integrate settings, and access past results. Another very convenient feature of the project explorer is it allows you to Add to Map ![add to map](/img/icons/AddToMap.png) the map-based inputs and outputs that are part of your project to your current ArcMap document table of contents in ArcGIS.

![Project Explorer](/img/CommandRefs/00_ProjectExplorer/project_explorer.png)

The GCD project explorer is also accessible by clicking the GCD logo ![ProjectExplorerButton](/img/icons/GCDAddIn.png) on the left edge of the GCD toolbar in the AddIn version:

![Toolbar](/img/CommandRefs/addin_toolbar.png)

The main way to interact with the GCD Project Explorer is to right click on items and use the context menu that appears. Each section within this documentation describes the individual context menu items. The menu when you right click on the topo level project item is described below.

## Project Context Menu

Right click the GCD Project item in the Project Explorer to access the **Project Context Menu** that provides access to several commands pertaining to the project itself.

![ProjectExplorer_Menu](/img/CommandRefs/00_ProjectExplorer/project_explorer_menu.png)

## Edit Project Properties

Editing the project properties displays basic information about the project including the name, parent directory and project description etc. The name and parent directory cannot be edited once the project has been created but there are several properties that are always editable. Note that the raster units are only editable while there are no input DEM surveys in the project. You can change the raster units until the first raster is loaded, after which these values are no longer editable. Conversely, the GCD display units are always editable. See the section on adding new projects for information about each of the values on this form.

![Properties](/img/CommandRefs/00_ProjectExplorer/project_properties.png)

## Explore Project Folder

The Explore GCD Project Folder command simply opens windows explorer and points directly to the GCD Project Folder. Typically, the contents will include the `*.gcd` project file (an XML file), a `Inputs `folder and an `Analyses` folder.

![Explore Project](/img/CommandRefs/00_ProjectExplorer/explore_project.png)

## Export Project To Cross Section Viewer

GCD 7 introduced the ability to extract values from GCD rasters along vector [profile routes](/Help/Inputs/profile-routes) to produce what are called linear extractions. While the GCD software itself does not possess features to visualize these linear extractions, it is possible to export all linear extractions within a GCD project to the [Cross Section Viewer](http://xsviewer.northarrowresearch.com/Online_Help/File_Menu/import_gcd_project.html) software that possesses several powerful tools for analyzing such data.

![Cross Section Viewer](/img/CommandRefs/00_ProjectExplorer/cross_section_viewer.png)

You must download and install the [Cross Section Viewer](http://xsviewer.northarrowresearch.com/download.html) software before you attempt to export your GCD project. Once you have both products installed then clicking export from the GCD will create an `Exports` sub-folder in the root of the GCD project and populate a [SQLite](https://www.sqlite.org/index.html) database with the values from all linear extractions found within the project. Finally, the Cross Section Viewer software will be launched with the latest export database already opened and ready to use.

Each time that the GCD project export is used will produce a **new** Cross Section Viewer database. It does not attempt to update the last exported database. In other words, if you generate an export and then decide that you want to a new linear extraction, you need to click export and produce an entire new database that will contain all the latest linear extraction data.

## Refresh Project Tree

The entire project tree can be refreshed and read from the GCD project file. This can be useful if the file has been altered by another program, or should anything go wrong with the GCD operation and it's necessary to confirm the correct contents of the project.

<!--
# Add Entire Project To Map

Add Entire Project to the Map adds all project inputs and analyses to the map. is an extremely useful command. As most ArcGIS users are aware, Map Documents can become corrupted and if you forget to store relative paths, the table of contents can be populated with many red exclamation marks (indicating the path to the layers is no longer valid). This allows you to open any map, and add your entire GCD project (with symbology) to the map's table of contents in the current data frame. This is illustrated in the video below:

<YouTubeEmbed videoId="OHzY6dzilwA" title="Add Entire Project To Map Video" />
-->
