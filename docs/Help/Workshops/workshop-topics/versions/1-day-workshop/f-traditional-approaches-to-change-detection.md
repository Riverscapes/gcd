---
title: Traditional Approaches to Change Detection
---

#### Synopsis of Topic

Traditional geomorphic change detection is based on some pretty basic concepts and simple assumptions that allow us to conduct very simple spatial analysis operations to show where change has occurred and quantify it. The major challenge of course comes in distinguishing the 'real' net change we're interested in from noise inherit in the data. The basic principles of error estimation, error propagation and how you can do this in ArcGIS are covered in this session.

![Fly_GCD_DoD_Cartoon]({{ site.baseurl }}/assets/images/Fly_GCD_DoD_Cartoon.png)

#### Why we're Covering it

You have to learn to crawl before you can walk. And in a lot of circumstances, simply crawling maybe good enough to get you there. Plus, it is important to understand what so many previous investigators have done to perform these types of analyses.

#### Learning Outcomes Supported

 This topic will help fulfill the following [primary learning outcome(s)](http://gcdworkshop.joewheaton.org/syllabus/primary-learning-outcomes) for the workshop: 

- A comprehensive overview of the theory underpinning geomorphic change detection
- The fundamental background necessary to design effective repeat topographic monitoring campaigns and distinguish geomorphic changes from noise (with particular focus on restoration applications)

------

### Resources

#### Slides and/or Handouts

- ​

  [![img](http://gcdworkshop.joewheaton.org/_/rsrc/1429979916927/workshop-topics/versions/3-day-workshop/1-Principles/g_traditionalGCD/pdfIcon.png) ](http://etal.usu.edu/GCD/Workshop/2012May/E_SimpleDoD.pdf)[Lecture](http://etal.usu.edu/GCD/Workshop/2015_RRNW/Lectures/F_SimpleDoD.pdf)  

#### 

#### Relevant or Cited Literature

- Brasington J, Rumsby BT and Mcvey RA. 2000. Monitoring and modelling morphological change in a braided gravel-bed river using high resolution GPS-based survey. Earth Surface Processes and Landforms. 25(9): 973-990. DOI: [10.1002/1096-9837(200008)25:9<973::AID-ESP111>3.0.CO;2-Y](http://dx.doi.org/10.1002/1096-9837%28200008%2925:9%3C973::AID-ESP111%3E3.0.CO;2-Y)
- Brasington J, Langham J and Rumsby B. 2003. Methodological sensitivity of morphometric estimates of coarse fluvial sediment transport. Geomorphology. 53(3-4): 299-316. DOI: [10.1016/S0169-555X(02)00320-3](http://dx.doi.org/10.1016/S0169-555X%2802%2900320-3)
- Lane SN, Chandler JH and Richards KS. 1994. Developments in Monitoring and Modeling Small-Scale River Bed Topography. Earth Surface Processes and Landforms. 19(4): 349-368. DOI: [10.1002/esp.3290190406](http://dx.doi.org/10.1002/esp.3290190406).
- Lane SN, Westaway RM and Hicks DM. 2003. Estimation of erosion and deposition volumes in a large, gravel-bed, braided river using synoptic remote sensing. Earth Surface Processes and Landforms. 28(3): 249-271. DOI: [10.1002/esp.483](http://dx.doi.org/10.1002/esp.483).
- Martin Y and Church M. 1995. Bed-Material Transport Estimated from Channel Surveys - Vedder River, British-Columbia. Earth Surface Processes and Landforms. 20(4): 347-361. DOI: [10.1002/esp.3290200405](http://dx.doi.org/10.1002/esp.3290200405).

### Exercise

#### Synopsis of Topic

In this hands on exercise we will learn how to do a basic change detection using geoprocessing in ArcGIS, vs. in GCD 6. 

#### Why we're Covering it

Before we get used to GCD 6 as a black box that works, its important to understand how to manually do the analysis. 

#### Datasets

- [F_SimpleDoD.zip](http://etal.usu.edu/GCD/Workshop/2015_RRNW/Excercises/F_SimpleDoD.zip) [![img](http://gcdworkshop.joewheaton.org/_/rsrc/1422836806362/workshop-topics/versions/1-day-workshop/f-traditional-approaches-to-change-detection/winzip_icon_16.gif)](http://gcdworkshop.joewheaton.org/workshop-topics/versions/1-day-workshop/f-traditional-approaches-to-change-detection/winzip_icon_16.gif?attredirects=0)

![GCD6_Form_ChangeDetectionConfiguration]({{ site.baseurl }}/assets/images/GCD6_Form_ChangeDetectionConfiguration.png)

#### Prerequisites for this Exercise

- GCD 6 and ArcGIS 10.1 w/ Spatial Analyst

#### Relevant Online Help or Tutorials for this Topic

- [Simple DoD in Raster Calculator Tutorial](http://gcd6help.joewheaton.org/tutorials--how-to/ii-simple-dod-in-raster-calculator)
- [Basic DoD Analysis in GCD 6](http://gcd6help.joewheaton.org/tutorials--how-to/iv-basic-dod-analysis-in-gcd)

------

← [Previous Topic](http://gcdworkshop.joewheaton.org/workshop-topics/versions/1-day-workshop/e-essential-best-practices-to-support-change-detection)            [Next Topic](http://gcdworkshop.joewheaton.org/workshop-topics/versions/1-day-workshop/g-introduction-to-gcd-software) →