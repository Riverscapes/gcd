---
title: FIS Library
---

The Fuzzy Inference System Library is where you load `*.fis` files so that they are available for use in the `Error C``alculations` tab of the `DEM Survey Properties` dialog. In the FIS library, you can `Add new FIS`, `Edit `or configure an existing FIS and `Remove` items from the library. The topics at left discuss each of these operations.

![GCD6_Form_FIS_Libraqry]({{ site.baseurl }}/assets/images/GCD6_Form_FIS_Libraqry.png)

### Adding an FIS to the Library

To add an existing FIS is easy. Note that by default, when you install GCD no FIS files are loaded in the library, but 

 two example FIS files are included in the user's roaming folder. These can be found at:

```
c:\Users\<YourUsername>\AppData\Roaming\GCD\FIS Sample Files
```

The two FIS files provided are examples of two input FIS where the inputs are slope and point density, and the output is elevation uncertainty. We do not automtically load FIS files into GCD's Survey library as users should excercise judgement and caution in applying, calibrating and developing appropriate fuzzy inference sytems. 

<iframe width="560" height="315" src="https://www.youtube.com/embed/84KgNRMQp2k" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

## How it Actually Works

When you add a FIS to the library, this video shows what actually happens to the 

```
FISLibraryXML.xml
```

 file, which is stored in your 

```
c:\Users\<YourUsername>\AppData\Roaming\GCD\FISLibrary 
```

folder. 

<iframe width="560" height="315" src="https://www.youtube.com/embed/6COkQrzL7_c" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

Every time a FIS is added, another <FISTable> entry gets added to the 

```
FISLibraryXML.xml
```

 file:

`  <FISTable>`
`    <FISID>3</FISID>`
`    <Name>GPS_2Input_PD_SLP</Name>`
`    <Path>C:\Users\Joe Wheaton\AppData\Roaming\GCD\FIS Sample Files\GPS_2Input_PD_SLP.fis</Path>`
`  </FISTable>`

### Removing a FIS from the Library

Removing a FIS from the library is as simple as highlighting it and clicking remove. When you remove a FIS from the library, it only removes it from the listing here (i.e. it deletes the corresponding 

```
<FISTable>...</FISTable>
```

entry in the 

```
FISLibraryXML.xml
```

 file. It does not delete the actual *.fis file from your computer.

### Editing a FIS in FIS Library

FIS files (*.fis) are an ASCII text format file that is compatible with the [Matlab's Fuzzy Logic Toolbox](http://www.mathworks.com/help/toolbox/fuzzy/fp351dup8.html) and can be edited in a text editor (e.g. [Notepad ++](http://notepad-plus-plus.org/)), in Matlab's Fuzzy Logic Toolobx using their [FIS Editor](http://www.mathworks.com/help/toolbox/fuzzy/fp243dup9.html#FP28385), or in GCD using the `Fuzzy Inference System Editor`:

![Dialog_FISLibrary_FIS_Editor]({{ site.baseurl }}/assets/images/Dialog_FISLibrary_FIS_Editor.png)

This video shows you how to edit an FIS using the Fuzzy Inference System Editor:

<iframe width="560" height="315" src="https://www.youtube.com/embed/fnvsyBBCFwc" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>

For more information on how fuzzy inference systems for elevation uncertainty work and can be created and modified, see the GCD Concept Reference documentation [here]({{ site.baseurl }}/gcd-concepts/fuzzy-inference-systems-for-modeling-dem-error).

