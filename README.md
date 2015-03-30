# FunnelWeb

FunnelWeb is an open source blog engine, built by developers for developers. Instead of fancy quotes and oodles of widgets, we focus on letting you post beautiful code samples, keeping your markup clean and valid, and encouraging collaboration with rich comments. FunnelWeb is easy to install, and has an active community.

# Markdown

FunnelWeb is built on top of [Markdown](http://daringfireball.net/projects/markdown/) which is a HTML abstraction layer allowing you to author your articles in a clean text format which will be transformed to HTML for serving on your site.

For editing we recommend that you use [Downmarker](https://github.com/Code52/DownmarkerWPF) which is just like Windows Live Writer but for Markdown.

# Installing

Really we recommend that you grab the code from this repository and run the `build.bat` file. This will generate you an output that you can publish onto your server and run through the installer.

If you're not a fan of running bleeding edge software you can [download a stable build](http://hg.funnelweblog.com/release/downloads).

Lastly you need to modify the `my.config` file which contains your login information and SQL connection information.

# Be involved

* Found a bug? [Let us know](http://hg.funnelweblog.com/release/issues?status=new&status=open)!
* Got a question? [Drop us a line](https://groups.google.com/forum/?fromgroups#!forum/funnelweblog)!
* Fixed something? Send us a pull request on [Bitbucket](http://hg.funnelweblog.com/dev) or [GitHub](https://github.com/funnelweblog/FunnelWeb)!

# License

FunnelWeb is licensed under the New BSD license.

==============================================================================

Copyright (c) 2009, FunnelWeb Contributors
All rights reserved.

Redistribution and use in source and binary forms, with or without 
modification, are permitted provided that the following conditions are met:

 - Redistributions of source code must retain the above copyright notice, 
   this list of conditions and the following disclaimer.
 - Redistributions in binary form must reproduce the above copyright notice, 
   this list of conditions and the following disclaimer in the documentation 
   and/or other materials provided with the distribution.
 - Neither the name of FunnelWeb nor the names of its contributors may be used 
   to endorse or promote products derived from this software without specific 
   prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE 
FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR 
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER 
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, 
OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE 
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.


New features in 2.5 (2015-03-30)
==========================

* Upgraded the project file to work with Visual Studio 2013 Community Editon
* The HTML structure of the site mirrors the default WordPress structure, so we can apply WordPress themes to FunnelWeb now
* Theming engine now supports both WordPress and Bootstrap themes
* Updated the MVC framework to version 5
* Included Bootstrap 3 and it's responsive layout (so it's mobile-friendly now)
* Redesigned the administration page
* ShowDown and WMD is replaced by a more recent PageDown
* The HTML is more HTML5-compliant
* Tested with both SQL and SQL CE databases

Screenshots
==========

Admin
--------
![admin-1](https://cloud.githubusercontent.com/assets/910321/6892236/b2069014-d6c2-11e4-87fe-b5d308f95f4d.png)
![admin-2](https://cloud.githubusercontent.com/assets/910321/6892238/b5c2900e-d6c2-11e4-8443-54b873c623f1.png)

WP-Flower theme
-----------------------
![wp_flower-1](https://cloud.githubusercontent.com/assets/910321/6892245/cfb7a7d8-d6c2-11e4-8db7-66aaec7a5703.png)
![wp_flower-2](https://cloud.githubusercontent.com/assets/910321/6892251/d6a09faa-d6c2-11e4-81ab-7c9269136650.png)
![wp_flower-post-1](https://cloud.githubusercontent.com/assets/910321/6892254/d8a4194e-d6c2-11e4-8f3c-175fcc958e31.png)
![wp_flower-post-2](https://cloud.githubusercontent.com/assets/910321/6892255/da13f3da-d6c2-11e4-931e-3c1be425e0a1.png)
![wp_flower-post-3](https://cloud.githubusercontent.com/assets/910321/6892256/e02ea68e-d6c2-11e4-8260-5308c756cf3b.png)
![wp_flower-post-4](https://cloud.githubusercontent.com/assets/910321/6892257/e1b6f75e-d6c2-11e4-9b9a-867be464ecf1.png)

WP-Hostmarks theme
----------------------------
![wp_hostmarks-1](https://cloud.githubusercontent.com/assets/910321/6892261/ed74b5f4-d6c2-11e4-93d9-2bd360e644d8.png)
![wp_hostmarks-2](https://cloud.githubusercontent.com/assets/910321/6892262/eedd02c0-d6c2-11e4-9380-a79bbf3aa4f9.png)
![wp_hostmarks-post-1](https://cloud.githubusercontent.com/assets/910321/6892265/f04c9486-d6c2-11e4-8443-2725c394eb0f.png)
![wp_hostmarks-post-2](https://cloud.githubusercontent.com/assets/910321/6892267/f5221490-d6c2-11e4-8203-29cd55e64a6e.png)
![wp_hostmarks-post-3](https://cloud.githubusercontent.com/assets/910321/6892268/f73c6a28-d6c2-11e4-8e68-126014796a4d.png)
![wp_hostmarks-post-4](https://cloud.githubusercontent.com/assets/910321/6892272/ff8e1c26-d6c2-11e4-83c2-d7d9c6e359dc.png)

WP-Quark theme
----------------------
![wp_quark-1](https://cloud.githubusercontent.com/assets/910321/6892290/1941fff2-d6c3-11e4-9383-44dc0045e8bc.png)
![wp_quark-2](https://cloud.githubusercontent.com/assets/910321/6892294/1c6c6f32-d6c3-11e4-9a30-b878f9a0034e.png)
![wp_quark-post-1](https://cloud.githubusercontent.com/assets/910321/6892295/1f6508d4-d6c3-11e4-9652-0a44c8ee66e0.png)
![wp_quark-post-2](https://cloud.githubusercontent.com/assets/910321/6892296/218c30ba-d6c3-11e4-9a8c-946993cbd72d.png)
![wp_quark-post-3](https://cloud.githubusercontent.com/assets/910321/6892298/25f43fda-d6c3-11e4-8695-6210116c5d25.png)
![wp_quark-post-4](https://cloud.githubusercontent.com/assets/910321/6892299/2866b4be-d6c3-11e4-94b6-d279b02dc0b5.png)

WP-Topmag theme
-------------------------
![wp_topmag-1](https://cloud.githubusercontent.com/assets/910321/6892305/34b42594-d6c3-11e4-8659-629ef586d6a6.png)
![wp_topmag-2](https://cloud.githubusercontent.com/assets/910321/6892307/36a82a8a-d6c3-11e4-9913-44e7d4f26566.png)
![wp_topmag-post-1](https://cloud.githubusercontent.com/assets/910321/6892309/3bce8f0e-d6c3-11e4-9adb-826c7aba42e4.png)
![wp_topmag-post-2](https://cloud.githubusercontent.com/assets/910321/6892310/3da19d12-d6c3-11e4-83a8-0a7410c6a23a.png)
![wp_topmag-post-3](https://cloud.githubusercontent.com/assets/910321/6892311/3f780a9a-d6c3-11e4-96b3-24ca8f420da0.png)
![wp_topmag-post-4](https://cloud.githubusercontent.com/assets/910321/6892313/44a3fc54-d6c3-11e4-99c4-7c3d6370bacf.png)

Further Information
===============
If you need some help with .NET, C#, MVC, Entity Framework, HTML5, CSS, JavaScript, jQuery or Bootstrap, you can contact me for one-on-one consultancy or live training on the [Kevin Sharp Traning website](http://www.kevinsharp.net).
