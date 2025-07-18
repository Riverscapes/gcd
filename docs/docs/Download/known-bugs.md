---
title: Reporting Bugs & Getting Support
sidebar_position: 10
slug: /Download/known-bugs
---

GCD is under constant development and we are eager to resolve all known issues.  However, please search [existing discussion posts](https://github.com/Riverscapes/gcd/issues),   [existing issue  posts](https://github.com/Riverscapes/gcd/issues)and our help pages first before [posting an issue](#posting-an-issue).

## Questions or Help

You can explore or search [past user questions here](https://github.com/Riverscapes/gcd/discussions).

## Bugs

Before logging a suspected bug, please search [known open bugs](https://github.com/Riverscapes/gcd/labels/bug). If you have more information to report about this bug, please post a comment to the appropriate issue. Otherwise, you can post a new bug to the ["I need help please" Discussion](https://github.com/Riverscapes/gcd/discussions/categories/i-need-help-please). If we deem your problem to be a software bug, we will tranfer it to our [known bugs](https://github.com/Riverscapes/gcd/labels/bug) (note we sometimes fix simple bugs, but we often just put them in the development queue for when funding is avaialable to fix them).

* [GCD Bugs](https://github.com/Riverscapes/gcd/labels/bug)

## Posting an Issue

If you find a bug, or simply want to report an issue with the software, please log a GitHub Discussion. [GitHub GCD Discussion](https://github.com/Riverscapes/gcd/discussions/categories/i-need-help-please).

Anyone in the GCD community and a member of GitHub  can respond, and the development team will receive a notification. Please be patient in your response. We don't have any budget for [supporting users](/Download/future-feature-request), but we do try and respond to most issues when we can. Just bare in mind that the [development team](/About/who), all have full-time jobs and any support we do of GCD users is usually on donated time. 


This video shows you how to post an issue and some helpful things to include:

<YouTubeEmbed videoId="EFAQgvZQY0s-xs"/>

### Please always include following information:

> ## The Problem
>
> What went wrong?
>
> ## Reproduction steps
> 
> 1. I did this
> 2. Then I did that...
> 
> ## Exception message
> 
> ```text
> 
> Paste the exception message you are getting from the app here. It really helps us. 
> 
> ```
> 
> ## Anything else?
> 
> You can provide links to datasets.

If you didn't get an Exception Message, please also include the:
- Version of GCD you were using (e.g. `GCD 7.0.10`)
- Version of ArcGIS  (e.g. `ArcGIS 10.5.1`)
- What you were trying to do.
- Context

The issue posting in GitHub uses [markdown syntax](https://guides.github.com/features/mastering-markdown/). Below is a template you can copy and paste if helpful:

> I was using GCD 7.0.10 in ArcGIS 10.5.2, and I performed a *morphological analysis*, and attempted to delete it from my project. The following handled exception was thrown:

```
deleting morphological analysis is not implemented.
 --- Stacktrace --- 
   at GCDCore.Project.Morphological.MorphologicalAnalysis.Delete() in D:\Code\gcd\gcd\GCDCore\Project\Morphological\MorphologicalAnalysis.cs:line 283
   at GCDCore.UserInterface.Project.TreeNodeTypes.TreeNodeItem.OnDelete(Object sender, EventArgs e) in D:\Code\gcd\gcd\GCDCore\UserInterface\Project\TreeNodeTypes\TreeNodeItem.cs:line 147
Windows: Microsoft Windows NT 6.2.9200.0
Date: 4/3/2018 2:34:05 PM
```

> It looks like it actually deleted the information form the file folder `D:\0_GCD\GCD7.0.10\Rees\Analyses\CD\DoD0001\BS\BS0001\Morph\MA0001`, but it is still in the project. 
[Rees.gcd.txt](https://github.com/Riverscapes/gcd/files/1873277/Rees.gcd.txt)

### Optional Information:
- [Zipping up your `*.gcd` project](/Concepts/projects) and sending us a link of the complete project can really make it easier for someone to help you troubleshoot (e.g. this [Rees.GCD Project](https://drive.google.com/file/d/1OOcZBeE3TFKOFaLKh2l-rBsdeOQNV2-H/view?usp=sharing) ). 
- Information about the file paths where the problem occurred (e.g. `D:\0_GCD\GCD7.0.10\Rees\Analyses\CD\DoD0001\BS\BS0001\Morph\MA0001`) You'd be surprised how often problems are simply do to spaces or unsafe characters in a folder name or file name. 
- If you don't want to provide a complete copy of the project, just making a copy of the `*.gcd` project file and changing the file extension to text and uploading it to your post can help a lot (see [video](https://www.youtube.com/watch?v=EFAQgvZQY0s&feature=youtu.be&t=5m14s))
- Record a video of what is going wrong (we recommend [FastStone Capture](http://etal.joewheaton.org/faststone-capture.html)), post it to YouTube or Vimeo, and share the link (DO NOT SEND VIDEOS as attachments! No one wants to download your video, just stream it). Videos can be really helpful for helping us understand what you're doing and what is going wrong.
