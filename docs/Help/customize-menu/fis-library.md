---
title: FIS Library
---

The GCD maintains a library of fuzzy inference system rule files (`*.fis`) that are available when generating [error surface rasters](). The GCD ships with a set of *system* FIS rule files that define several typical FIS definitions. These system FIS are taken from the Utah State University [GitHub repository](https://github.com/Riverscapes/fis-dem-error). In addition, Users can add their own *user defined* FIS rule files. 

![FIS Library]({{ site.baseurl }}/assets/images/CommandRefs/03_Customize/fis-library.png)

# System FIS

System FIS represented a snapshot of the [GitHub repository](https://github.com/Riverscapes/fis-dem-error) that was taken at the last time that the GCD software was released. Changes to the repository that are newer than the date of the last GCD release will not be reflected in the system FIS that ship with the software. Click the button on the top right of the form to view the contents of the GitHub repository.

If you need to use the latest version of an FIS file from the repository then manually download the file in question and add it to the GCD software FIS library as a *user defined* FIS.

System FIS are **not** editable. All metadata describing system is read only. Should you need to update information about a system FIS then contact the [GCD development team]({{ site.baseurl }}/support.html).

# Adding User Defined FIS

Click the green plus button to browse to an existing FIS rule file and add it to the library. The FIS rule file will be copied into a folder in your Windows user profile. After browsing and selecting the `*.fis` rule file you will be presented with a metadata form where you can enter information describing the contents of the rule file. 

<div class="responsive-embed">
<iframe width="560" height="315" src="https://www.youtube.com/embed/84KgNRMQp2k" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>
</div>

# Editing User Defined FIS

Click the settings cog button to open the properties form for the selected FIS. The properties form will always be read only for system FIS and editable for user defined FIS.

# Deleting User Defined FIS

Select a user defined FIS and click the red delete button to delete the FIS from the library. The FIS will be completely removed and unavailable when generating [error surface rasters](). Deleting an FIS rule file does not affect any GCD projects that contain error surface rasters for which the rule file was used. These error rasters can still be viewed and used in change detection analyses.

------
<div align="center">
	<a class="hollow button" href="{{ site.baseurl }}/Help"><i class="fa fa-chevron-circle-left"></i>  Back to GCD Help </a>  
	<a class="hollow button" href="{{ site.baseurl }}/"><img src="{{ site.baseurl}}/assets/images/icons/GCDAddIn.png">  Back to GCD Home </a>  
</div>
