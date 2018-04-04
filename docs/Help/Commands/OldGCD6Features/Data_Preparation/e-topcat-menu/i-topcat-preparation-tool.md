---
title: i. ToPCAT Preparation Tool
---

Currently in order to use ToPCAT to decimate a point cloud the input point cloud file needs to be in a space separated x y z file. To address this need, ToPCAT prep takes in a raw point cloud file and replaces its separator with spaces so that the file can be used in ToPCAT.

In addition to space separated files, files greater than 300 MB can occasionally fail to be processed by ToPCAT due to their large size. If you have a file larger than this or have run ToPCAT and it does not work you may want to consider subsetting your raw point cloud file with the *Subset Raw Point Cloud Tool*.

When the *ToPCAT Prep - Convert Point Cloud to Space Delimited* button is clicked the tool menu is shown:

![GCD6_Form_ToPCAT_Prep]({{ site.baseurl }}/assets/images/GCD6_Form_ToPCAT_Prep.png)

The inputs for this tool are:

- **Raw point cloud**

- - **formatted into columns of x, y, z.**

- **Point Cloud separator **(optional):

- - the symbol used to separate the x, y, z values. The default is comma.

The outputs for the tool are:

- **ToPCAT ready raw point cloud**

- - raw point cloud formatted into space delimited columns of x y z.

------
<div align="center">
	<a class="hollow button" href="{{ site.baseurl }}/Help"><i class="fa fa-chevron-circle-left"></i>  Back to GCD Help </a>  
	<a class="hollow button" href="{{ site.baseurl }}/"><img src="{{ site.baseurl}}/assets/images/icons/GCDAddIn.png">  Back to GCD Home </a>  
</div>