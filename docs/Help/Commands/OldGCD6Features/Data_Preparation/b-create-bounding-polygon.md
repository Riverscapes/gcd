---
title: B. Create Bounding Polygon
---

The `Create Bounding Polygon` tool is a simple tool for creating a boundary polygon around a point cloud of topographic data. We use a concave hull polygon ([not standard inside ArcGIS](http://forums.arcgis.com/threads/3151-Points-to-Polygon-quot-Footprint-quot?p=52952&viewfull=1#post52952) like the [convex hull](http://help.arcgis.com/en/arcgisdesktop/10.0/help/index.html#//00170000003q000000) tool) because it has the distinct property that it does not connect the dots across concavities creating over-interpolated zones when used as a bounding polygon when [creating a TIN or Terrain]({{ site.baseurl }}/tutorials--how-to/ii-building-dems). 

![GCD6_Form_CreateBoundingPolygon]({{ site.baseurl }}/assets/images/GCD6_Form_CreateBoundingPolygon.png)

The main input to the tool is a point feature class that represents your topographic point cloud. The aggregation distance controls how 'tight' the polygon is drawn around the points and how jagged the boundary will be. If you choose the default of 10 meters, any points closer together than 10m will not force the boundary to connect the dots between them.

The video below illustrates how this works.

<iframe width="560" height="315" src="https://www.youtube.com/embed/fGRpn_nZM0Y" frameborder="0" gesture="media" allow="encrypted-media" allowfullscreen></iframe>



------
<div align="center">
	<a class="hollow button" href="{{ site.baseurl }}/Help"><i class="fa fa-chevron-circle-left"></i>  Back to GCD Help </a>  
	<a class="hollow button" href="{{ site.baseurl }}/"><img src="{{ site.baseurl}}/assets/images/icons/GCDAddIn.png">  Back to GCD Home </a>  
</div>