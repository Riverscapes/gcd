---
title: Known Bugs
---

If you find a bug, please report it to the development team at gcd@joewheaton.org. 

GCD 5 was built on a combination of our own algorithms, ArcGIS functionality and ArcObjects. The advantages to providing this tool within ArcGIS are that it is a familiar analysis environment for many users, and users can then combine and leverage the many other capabilities of ArcGIS. The major disadvantage is that the GCD extension inherits a large number of unreliablities from ArcGIS (and some are our fault too).  Before reporting a bug, we ask that you browse through this list to see if the problem you are experiencing is already logged. 

With GCD 6, we have moved away from a relying on ArcObjects and Arc Geoprocessing as much as possible. We are slowing moving all of the tool functionality down into our own C++ libraries, and making every GCD command accessible from a command line interface. This will allow users to use the tools of GCD without necessarily ever having to be in ArcGIS. However, for mapping display functionality, we will still rely on ArcGIS. 

![bug]({{ site.baseurl }}assets/images/bug.jpg)