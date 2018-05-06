---
title: Delete DEM Survey
weight: 5
---

![DEMSurvey_Delete]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/DEMSurvey_Delete.png)

The **Delete DEM Survey** command deletes a DEM Survey from the GCD Project. This does several things:

- Checks to see if there are any Analyses depdendent on this DEM Survey. If there are, GCD will not allow you to  delete it from map.
  - If any Change Detection analyses have been performed already, you will get a warning that you need to delete those first. GCD works on the principle that any analysis should be transparent and reproducible in the context of its inputs. Therefore, GCD prevents you from deleting inputs that were used in any analysis. 
![DEMSurvey_InUse]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/DEMSurvey_InUse.png)

- Checks if there are any childern associated surfaces or error surfaces. If the there are, it will warn you and ask you if you want to delete this anway.
- If user proceeds, it removes the DEM, any associated surfaces or error surfaces from the table of contents (Arc AddIN version only).
- After, it deletes the `Inputs\Surveys\DEM000n` folder associated with the selected DEM Survey from the project directory (note this does not impact where you loaded or copied the DEM survey from originally). This physcially deletes the files from your system.
- It then deletes all the project information about this DEM Survey from the GCD project by removing the contents inside the `<DEM></DEM>` xml tags for this survey in the GCD Project.

![DEMSurvey_Expanded]({{ site.baseurl }}/assets/images/CommandRefs/00_ProjectExplorer/DEMSurvey_Expanded.png)

As an example, a DEM Survey named `DEM_2003`, might have an entry in the GCD project as follows:

``` xml
<DEMSurveys>
    <DEM>
      <Name>DEM_2003</Name>
      <Path>Inputs\Surveys\DEM0001\DEM2003.tif</Path>
      <Hillshade>Inputs\Surveys\DEM0001\DEM2003_HS.tif</Hillshade>
      <ErrorSurfaces>
        <ErrorSurface>
          <Name>6 cm Uniform</Name>
          <Path>Inputs\Surveys\DEM0001\ErrorSurfaces\Err0001\6cmUniform.tif</Path>
          <IsDefault>True</IsDefault>
          <ErrorSurfaceProperties>
            <ErrorSurfaceProperty>
              <Name>Entire DEM Extent</Name>
              <UniformValue>0.06</UniformValue>
            </ErrorSurfaceProperty>
          </ErrorSurfaceProperties>
        </ErrorSurface>
        <ErrorSurface>
          <Name>2003_FIS_2In</Name>
          <Path>Inputs\Surveys\DEM0001\ErrorSurfaces\Err0002\2003FIS2In.tif</Path>
          <IsDefault>False</IsDefault>
          <ErrorSurfaceProperties />
        </ErrorSurface>
      </ErrorSurfaces>
      <SurveyDate>
        <Year>2003</Year>
        <Month />
        <Day />
        <Hour />
        <Minute />
      </SurveyDate>
    </DEM>
```
The entire contents of `<DEM></DEM>` are then removed.

### Video Tutorial
A Video to Illustrate what is going on behind the scenes

<iframe width="560" height="315" src="https://www.youtube.com/embed/xYvGK5uZgjY?rel=0" frameborder="0" allow="autoplay; encrypted-media" allowfullscreen></iframe>

------
<div align="center">
	<a class="hollow button" href="{{ site.baseurl }}/Help"><i class="fa fa-chevron-circle-left"></i>  Back to GCD Help </a>  
	<a class="hollow button" href="{{ site.baseurl }}/"><img src="{{ site.baseurl}}/assets/images/icons/GCDAddIn.png">  Back to GCD Home </a>  
</div>