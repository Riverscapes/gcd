---
title: Batch Change Detection - DISABLED
---

The batch change detection automates performing multiple change detection analyses.

The batch change dialog allows you to load an XML file (currently this must be manually populated), that specifies the parameters required to run multiple change detection scenarios at once. You can create an XML file in any text editor (e.g. [Notepad++](http://notepad-plus-plus.org/)). The current functionality is primitive in its functionality and user friendliness and intended for power users. Future releases will look at ways of expanding on this.

## Batch Change Detection Parameters

The input to the Batch Change Detection tools is provided in an XML file. Several examples are provided in the next section. This section describes the input parameters (XML tags).

| **Input Name**       | **Comment**                              |
| -------------------- | ---------------------------------------- |
| **Name**             | Name of the analysis. The name must be unique, i.e. it cannot previously have been used to do a Change Detection Analysis. |
| **Type**             | The type describes the type of analysis to run and must be either “`simple`”, “`propagated`” or “`probability`” (case sensitive). |
| **NewSurvey**        | Name of new survey. The survey must have previously been loaded in Survey Library |
| **OldSurvey**        | Name of old survey. The survey must have previously been loaded in Survey Library |
| **Threshold**        | Threshold value for analysis (see examples by type below) |
| **NewSurveyError\*** | Name of error calculation for new survey. The error calculation must have previously been specified in Survey Library |
| **OldSurveyError\*** | Name of error calculation for old survey. The error calculation must have previously been specified in Survey Library |
| **Baysian\***        | Baysian must be either “`TRUE`” or “`FALSE`”. Enables Baysian updating if “`TRUE`” |
| **WindowSize\***     | Size of window for spatial coherence filter. |
| **PercentLess\***    | Setting for spatial coherence filter. If percent of cells is less than this value, then probability is 0%. |
| **PercentGreater\*** | Setting for spatial coherence filter. If percent of cells is greater than this value, then probability is 100%. |

 * Optional tags

## Examples

### Simple change detection analysis

`<?xml version="1.0" standalone="yes"?>`

`<BatchChangeDetectionDS xmlns="http://tempuri.org/BatchChangeDetectionDS.xsd">`

`  ``<BatchChangeDetection>`

`    ``<Name>`**BatchA**`</Name>`

`    ``<Type>`**simple**`</Type>`

`    ``<NewSurvey>`**DEM_2005Projected**`</NewSurvey>`

`    ``<OldSurvey>`**DEM_2004Projected**`</OldSurvey>`

`    ``<Threshold>`**0.10**`</Threshold>`

`  ``</BatchChangeDetection>`

`</BatchChangeDetectionDS>`

Download this [XML example](http://www.gis.usu.edu/~jwheaton/et_al/GCD/GCD5/XML_BatchExamples/SimpleChangeDetectionAnalysisTemplate.xml) as a template. 

### Propagated change detection analysis

`<?xml version="1.0" standalone="yes"?>`

`<BatchChangeDetectionDS xmlns="http://tempuri.org/BatchChangeDetectionDS.xsd">`

`  ``<BatchChangeDetection>`

`    ``<Name>`**BatchA**`</Name>`

`    ``<Type>`**simple**`</Type>`

`    ``<NewSurvey>`**DEM_2005Projected**`</NewSurvey>`

`    ``<OldSurvey>`**DEM_2004Projected**`</OldSurvey>`

`    ``<Threshold>`**0.10**`</Threshold>`

`  ``</BatchChangeDetection>`

`</BatchChangeDetectionDS>`

Download this [XML example](http://www.gis.usu.edu/~jwheaton/et_al/GCD/GCD5/XML_BatchExamples/PropagatedChangeDetectionAnalysisTemplate.xml) as a template. 

### Probability change detection analysis without Baysian updating

`<?xml version="1.0" standalone="yes"?>`

`<BatchChangeDetectionDS xmlns="http://tempuri.org/BatchChangeDetectionDS.xsd">`

`  ``<BatchChangeDetection>`

`    ``<Name>`**BatchC**`</Name>`

`    ``<Type>`**probability**`</Type>`

`    ``<NewSurvey>`**DEM_2005Projected**`</NewSurvey>`

`    ``<OldSurvey>`**DEM_2004Projected**`</OldSurvey>`

`    ``<NewSurveyError>`**TS**`</NewSurveyError>`

`    ``<OldSurveyError>`**rtkGPS**`</OldSurveyError>`

`    ``<Threshold>`**0.95**`</Threshold>`

`  ``</BatchChangeDetection>`

`</BatchChangeDetectionDS>`

Download this [XML example](http://www.gis.usu.edu/~jwheaton/et_al/GCD/GCD5/XML_BatchExamples/Probablistic_NoBayesian_ChangeDetectionAnalysisTemplate.xml) as a template.

### Probability change detection analysis with Baysian updating

`<?xml version="1.0" standalone="yes"?>`

`<BatchChangeDetectionDS xmlns="http://tempuri.org/BatchChangeDetectionDS.xsd">`

`  ``<BatchChangeDetection>`

`    ``<Name>`**BatchC**`</Name>`

`    ``<Type>`**probability**`</Type>`

`    ``<NewSurvey>`**DEM_2005Projected**`</NewSurvey>`

`    ``<OldSurvey>`**DEM_2004Projected**`</OldSurvey>`

`    ``<NewSurveyError>`**TS**`</NewSurveyError>`

`    ``<OldSurveyError>`**rtkGPS**`</OldSurveyError>`

`    ``<Threshold>`**0.95**`</Threshold>`

`    ``<Baysian>`**TRUE**`</Baysian>`

`    ``<WindowSize>`**5**`</WindowSize>`

`    ``<PercentLess>`**60**`</PercentLess>`

`    ``<PercentGreater>`**100**`</PercentGreater>`

`  ``</BatchChangeDetection>`

`</BatchChangeDetectionDS>`

 Download this [XML example](http://www.gis.usu.edu/~jwheaton/et_al/GCD/GCD5/XML_BatchExamples/Probablistic_BayesianChangeDetectionAnalysisTemplate.xml) as a template. 

### Multiple change detection analysis

`<?xml version="1.0" standalone="yes"?>`

`<BatchChangeDetectionDS xmlns="http://tempuri.org/BatchChangeDetectionDS.xsd">`

`  ``<BatchChangeDetection>`

`    ``<Name>BatchF</Name>`

`    ``<Type>probability</Type>`

`    ``<NewSurvey>DEM_2005Projected</NewSurvey>`

`    ``<OldSurvey>DEM_2004Projected</OldSurvey>`

`    ``<NewSurveyError>TS</NewSurveyError>`

`    ``<OldSurveyError>rtkGPS</OldSurveyError>`

`    ``<Threshold>`**0.95**`</Threshold>`

`  ``</BatchChangeDetection>`

`  ``<BatchChangeDetection>`

`    ``<Name>BatchG</Name>`

`    ``<Type>probability</Type>`

`    ``<NewSurvey>DEM_2005Projected</NewSurvey>`

`    ``<OldSurvey>DEM_2004Projected</OldSurvey>`

`    ``<NewSurveyError>TS</NewSurveyError>`

`    ``<OldSurveyError>rtkGPS</OldSurveyError>`

`    ``<Threshold>`**0.90**`</Threshold>`

`  ``</BatchChangeDetection>`

`</BatchChangeDetectionDS>`

Download this [XML example](http://www.gis.usu.edu/~jwheaton/et_al/GCD/GCD5/XML_BatchExamples/Multiple_ChangeDetectionAnalysisTemplate.xml) as a template.



------
<div align="center">
	<a class="hollow button" href="{{ site.baseurl }}/Help"><i class="fa fa-chevron-circle-left"></i>  Back to GCD Help </a>  
	<a class="hollow button" href="{{ site.baseurl }}/"><img src="{{ site.baseurl}}/assets/images/icons/GCDAddIn.png">  Back to GCD Home </a>  
</div>