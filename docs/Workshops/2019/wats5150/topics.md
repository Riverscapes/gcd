---
title: GCD Topics
weight: 2
---

During the semester, we will cover the following GCD topics in fluvial geomorphology during our extended Monday afternoon sessions:

- Topography & Change Detection Applications
- Change Detection Principles
- Error Modelling & Uncertainty Analyis- Introduction
- Error Modelling Fuzzy Methods

As we cover them, I will post and add slides below for your reference.

## Inroduction & Setup

We covered some very basic background, and made sure we had [GCD properly installed]({{ site.baseurl }}/Download/install.html).

<div class="responsive-embed widescreen">
	<iframe src="https://docs.google.com/presentation/d/e/2PACX-1vRtePz767JoiprO5s7Y-Y62nNq19ZtBfYuTmHpcBXnBeiesobpXS4VlwJQuL8G4nbYRoGagzqmUnY8p/embed?start=false&loop=false&delayms=3000" frameborder="0" width="600" height="400" allowfullscreen="true" mozallowfullscreen="true" webkitallowfullscreen="true"></iframe>
</div>

### Related Resources

- <i class="fa fa-lightbulb-o" aria-hidden="true"></i> [**CONCEPT REFERENCE**: DEM of Difference (DoD)]({{ site.baseurl }}/Concepts/dod.html)

-----

## Change Detection Applications

To motivate the interst in change detection, it is helpful to review a variety of applications (this is a very fluvially biased sample... sorry).

<div  class="responsive-embed widescreen">
	<iframe src="https://docs.google.com/presentation/d/e/2PACX-1vRclLYWDizQsMj-96AFMFE3FQVmbsNyD5W0IrwXLMO49VKwyNoVWLJAThJ4p2kHHdTEaQfeMFHAfwXK/embed?start=false&loop=false&delayms=3000" frameborder="0" width="960" height="749" allowfullscreen="true" mozallowfullscreen="true" webkitallowfullscreen="true"></iframe>
</div>



-----

## Traditional Geomorphic Change Detection

As to 'dive right in', and get your hands dirty with GCD, we did a quick demo of basic change detection techniques in ArcGIS with Raster Calculator, then did the same thing in GCD. 

<div  class="responsive-embed widescreen">
	<iframe src="https://docs.google.com/presentation/d/e/2PACX-1vQ2Q60S_nR7e6KDSPpAMtlFu9VF3gbFKiYXA9Sfbpj1A3-XFj0lnoNGGvYMPuBrbN7AkVo_VJDtFFD3/embed?start=false&loop=false&delayms=3000" frameborder="0" width="960" height="749" allowfullscreen="true" mozallowfullscreen="true" webkitallowfullscreen="true"></iframe>
</div>

### Related Resources

- <i class="fa fa-graduation-cap" aria-hidden="true"></i>
[**TUTORIAL** Simple Change Detection in Raster Calculator vs. GCD]({{ site.baseurl }}/Tutorials/ChangeDetection/GCD_Simple-DoD)

-----

## High Resolution Repeat Topography to Support Change Detection

Backing up, to do geomorphic change detection requires high quality, high resolution repeat topography. We need to review how you get that.

<div  class="responsive-embed widescreen">
	<iframe src="https://docs.google.com/presentation/d/e/2PACX-1vQGSNQ3pGAiI1yxzS3_BdZZEIIK30jhtQquuOXTHX6HfIp-Z-ySqAFm_iruHXJ-alMu51sQEuJFwJq-/embed?start=false&loop=false&delayms=3000" frameborder="0" width="960" height="749" allowfullscreen="true" mozallowfullscreen="true" webkitallowfullscreen="true"></iframe>
</div>

### Related Resources

-<i class="fa fa-graduation-cap" aria-hidden="true"></i>
[**TUTORIAL** Building DEMs from Topographic Survey Data]({{ site.baseurl }}/Tutorials/Building_DEMs/building-dems.html)

<div  class="responsive-embed widescreen">
	<iframe src="https://docs.google.com/presentation/d/e/2PACX-1vTso96X4e_DmjjslEcJKNDsQ5cG9qcrZebShuekKqG8eJTORaFzSTQ6ZHVFDzJaupbslVCF1hCfje7d/embed?start=false&loop=false&delayms=3000" frameborder="0" width="960" height="749" allowfullscreen="true" mozallowfullscreen="true" webkitallowfullscreen="true"></iframe>
</div>


#### Background Reading on Change Detection & High Resolution Topography

- <i class="fa fa-book" aria-hidden="true"></i> [**READING ASSIGNMENT**: Canvas Prep Reading Assignment](https://usu.instructure.com/courses/532404/assignments/2616300)

To do geomorphic change detection, one needs to have repeat topographic survey. There are many ways to acquire topographic data and this paper by Bangen et al. (2014) reviews some of the most common methods to acquire topography for wadeable streams. Please skim Bangen et al. (2014) to get an appreciation of common ways to survey topography and what are there limitations. 

- Bangen SG, Wheaton JM and Bouwes N. 2014. [A methodological intercomparison of topographic survey techniques for characterizing wadeable streams and rivers](https://www.researchgate.net/publication/260043955_A_methodological_intercomparison_of_topographic_survey_techniques_for_characterizing_wadeable_streams_and_rivers)  Geomorphology. DOI:  [10.1016/j.geomorph.2013.10.010](http://dx.doi.org/10.1016/j.geomorph.2013.10.010)

High resolution topography (HRT) has become increasingly common from a variety of platforms. This review by Passalacqua et al. (2015) discusses some of the most common challenges associated with producing and analyzing HRT to do meaningful change detection (among other analyses). Please skim to get an appreciation of the diversity off HRT applications, but focus on the sections that talk about change detection applications. 

- Passalacqua P, Belmont P, Staley D, Simley J, Arrowsmith JR, Bodee C, Crosby C, DeLongg S, Glenn N, Kelly S, Lague D, Sangireddy H, Schaffrath K, Tarboton D, Wasklewicz T, Wheaton J. 2015. [Analyzing high resolution topography for advancing the understanding of mass and energy transfer through  landscapes: A review](https://www.researchgate.net/publication/277477904_Analyzing_high_resolution_topography_for_advancing_the_understanding_of_mass_and_energy_transfer_through_landscapes_A_review). Earth Science Reviews. 148. pp. 174-193. DOI: [10.1016/j.earscirev.2015.05.012](http://dx.doi.org/10.1016/j.earscirev.2015.05.012).

-----

## Managing Raster Data for GCD

Unfortunately, simple raster based change detection requires doing a really good job of preparing and manaigng  your raster data. We review some concepts and best practices to avoid the common pitfalls and make that simple subtraction problem work. 
<div  class="responsive-embed widescreen">
	<iframe src="https://docs.google.com/presentation/d/e/2PACX-1vRsx-80zgEMSGKnJAgy1sZ0kQJ8QpxzhzAUE3fXIMI8TzzBrOEEwcU-wrQuOuSKBCysSI2rKEfgcs2m/embed?start=false&loop=false&delayms=3000" frameborder="0" width="960" height="749" allowfullscreen="true" mozallowfullscreen="true" webkitallowfullscreen="true"></iframe>
</div>

### Related Resources

- <i class="fa fa-graduation-cap" aria-hidden="true"></i>
[**TUTORIAL** : Essential Best Practices to Support Change Detection]({{ site.baseurl }}/Tutorials/Building_DEMs/Building_DEMs/bestpractices.html)
- <i class="fa fa-lightbulb-o" aria-hidden="true"></i> [**CONCEPT REFERENCE**: Data Preparation - Best Practices ]( {{ site.baseurl }}/Concepts/data-preparation---best-practices/)

------
<div align="center">
    <a class="hollow button" href="{{ site.baseurl }}/Workshops/2019/wats5150/"><i class="fa fa-chevron-circle-left"></i>  Back to Course Home </a>  

        <a class="hollow button" href="{{ site.baseurl }}/Workshops/"><i class="fa fa-graduation-cap"></i>  Back to Workshops </a>  

</div>