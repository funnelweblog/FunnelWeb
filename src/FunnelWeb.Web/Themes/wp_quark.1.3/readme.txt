=== Quark ===
Contributors: ahortin
Donate link: http://quarktheme.com
Tags: black, gray, dark, light, one-column, two-columns, right-sidebar, fluid-layout, responsive-layout, custom-background, custom-header, custom-menu, editor-style, featured-image-header, featured-images, full-width-template, microformats, post-formats, sticky-post, theme-options, threaded-comments, translation-ready
Requires at least: 3.6
Tested up to: 4.1
Stable tag: 1.3
License: GPLv2 or later
License URI: http://www.gnu.org/licenses/gpl-2.0.html

Quark is your basic building block for creating beautiful, responsive custom themes. It's not a convoluted or confusing framework that's hard to learn or even harder to modify. It's a simple and elegant starter theme built on HTML5 & CSS3. Its base is a responsive, 12 column grid. It incorporates custom theme options that are easy to modify, a set of common templates, support for WordPress Post Formats and the gorgeous, retina friendly Font Awesome icon font. Quark is WooCommerce compatible, Multilingual Ready (WPML) and translated into Spanish, German and French.


== Description ==

Quark is your basic building block for creating beautiful, responsive custom themes. It's not a convoluted or confusing framework that's hard to learn or even harder to modify. It's a simple and elegant starter theme built on HTML5 & CSS3. It's based on the Underscores (_s) and TwentyTwelve themes, so that means not only is it flexible, it's extremely easy to customise. There's no need to make a child theme (unless you really want to), just dig in to the code & use it to give yourself a kickstart in creating your next awesome theme.

Its base is a responsive, 12 column grid. It uses Normalize to make sure that browsers render all elements more consistently and Mordernizr for detecting HTML5 and CSS3 browser capabilities along with some default stylings from HTML5 Boilerplate.

It incorporates the [Options Framework](http://wptheming.com/options-framework-theme/) by Devin Price to make it super easy to add custom Theme Options as well as the gorgeous [Font Awesome](http://fortawesome.github.io/Font-Awesome/) icon font by Dave Gandy.

The main navigation uses the standard WordPress menu. Support for dropdown menus is inluded by default. If you'd like to envoke a button toggle for the main navigation menu on small screens, simply uncomment the two lines from the quark_scripts_styles() function within functions.php to register and enqueue the necessary javascript file, and BAM! You're done!

If you're looking to build an eCommerce website, Quark now supports WooCommerce. When WooCommerce is activated, an extra tab is displayed within the Theme Options page that provides options to show or hide the sidebar on the default WooCommerce templates, along with the choice to remove the built-in WooCommerce breadcrumbs.

Templates

Quark includes a set of your most common theme templates, including templates for Full-Width pages, Left Sidebar, Right Sidebar (default), Front-Page, Tag, Categories, Authors, Search, Posts Archive and 404.

Post Formats

All the standard WordPress Post Formats are supported. These include; Aside, Gallery, Link, Image, Quote, Status, Video, Audio, Chat and of course, your standard post.

Widgets

Widgets are a great way of adding extra content to your site and Quark has a whole assortment of them.

Main Sidebar: Appears in the sidebar on posts and pages except the optional Homepage template, which has its own widgets
Blog Sidebar: Appears in the sidebar on the blog and archive pages only
Single Post Sidebar: Appears in the sidebar on single posts only
Page Sidebar: Appears in the sidebar on pages only

The Front Page Banner Widget areas are dynamic! You can use up to two of these and they'll magically space themselves out evenly. For example, if you only add widgets into the First Front Page Banner Widget Area, then it will expand the full width of the page. However, if you add widgets to both Front Page Banner Widget areas, they'll magically space themselves out over two equal columns.
First Front Page Banner Widget: Appears in the banner area on the Front Page
Second Front Page Banner Widget: Appears in the banner area on the Front Page

The Front Page Widget areas are dynamic! You can use up to four of these and they'll magically space themselves out evenly. For example, if you only add widgets into the First Front Page Widget Area, then it will expand the full width of the page. However, if you add widgets to all four Front Page Widget Areas, they'll magically space themselves out over four equal columns.
First Homepage Widget Area: Appears when using the optional homepage template with a page set as Static Front Page
Second Homepage Widget Area: Appears when using the optional homepage template with a page set as Static Front Page
Third Homepage Widget Area: Appears when using the optional homepage template with a page set as Static Front Page
Fourth Homepage Widget Area: Appears when using the optional homepage template with a page set as Static Front Page

The Footer Widget areas are dynamic! You can use up to four of these and they'll magically space themselves out evenly. For example, if you only add widgets into the First Footer Widget Area, then it will expand the full width of the page. However, if you add widgets to all four Footer Widget Areas, they'll magically space themselves out over four equal columns.
First Footer Widget Area: Appears in the footer sidebar
Second Footer Widget Area: Appears in the footer sidebar
Third Footer Widget Area: Appears in the footer sidebar
Fourth Footer Widget Area: Appears in the footer sidebar

Custom Header

The Default logo can be easily changed using the Custom Header feature. You change this in the Appearance > Header menu option

Custom Background

The background pattern can be changed using the Custom Background feature. You change this in the Appearance > Background menu option

Theme Options

Additional Theme Options can be found in the Appearance > Theme Options menu option. These include options for:
Specifying the URL's for various social media networks
Specifying the banner background image & color
Specifying the footer color
Changing the footer credit text
Hiding the sidebar on WooCommerce templates
Hiding the WooCommerce breadcrumbs

Multilingual Ready (WPML)

Using the WordPress Multilingual Plugin (WPML) it's now easy to build multilingual sites. With WPML you can translate pages, posts, custom types, taxonomy, menus and even the theme’s texts.

WooCommerce Support

WooCommerce is a WordPress eCommerce toolkit that helps you sell anything. Beautifully. Turn your website into a powerful eCommerce site by installing the WooCommerce plugin by WooThemes.

== Installation ==

There are three ways to install your theme. It can be installed by manually uploading the files to the themes folder using an FTP application,
it can be installed by downloading from the WordPress Theme Directory within the Dashboard or it can be installed by uploading the theme zip
file that you downloaded.

Use the following instructions to install & activate Quark using your preferred method.

Manual installation:

1. Unzip the files from the Quark zip file that you downloaded
2. Upload the Quark folder to your /wp-content/themes/ directory
3. Click on the Appearance > Themes menu option in the WordPress Dashboard
4. Click the Activate link below the Quark preview thumbnail

Install from the WordPress Theme Directory:

1. Click on the Appearance > Themes menu option in the WordPress Dashboard
2. Click the Install Themes tab at the top of the page
3. Type 'Quark' in the search field, without the quotes, and then click the Search button
4. Click the Install Now link below the Quark preview thumbnail
5. Once the theme has been installed, click the Activate link

Install by uploading the theme zip file:

1. Click on the Appearance > Themes menu option in the WordPress Dashboard
2. Click the Install Themes tab at the top of the page
3. Click on the Upload link just below the two tabs at the top of the page
4. Click the Browse button, browse to the folder that contains the theme zip file, select it and then click the Open button
5. Click the Install Now button
6. Once the theme has been installed, click the Activate link


== Getting Started ==

Since Quark is a starter theme to kick off your own awesome theme, the first thing you want to do is copy the quark theme folder 
and change the name to something else. You'll then need to do a three-step find and replace on the name in all the templates.

1. Search for quark inside single quotations to capture the text domain.
2. Search for quark_ to capture all the function names.
3. Search for quark with a space before it to replace all the occurrences of it in comments.
   (You'd replace this with the capitalized version of your theme name.)

or, to put it another way...

Search for:'quark'
 Replace with:'yourawesomethemename'
Search for:quark_
 Replace with:yourawesomethemename_
Search for: quark
 Replace with: YourAwesomeThemeName

Lastly, update the stylesheet header in style.css and either update or delete this readme.txt file.


== License ==

Quark is licensed under the [GNU General Public License version 2](http://www.gnu.org/licenses/old-licenses/gpl-2.0.html).

This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the
Free Software Foundation; either version 2 of the License, or (at your option) any later version.


== Credits ==

Quark utilises the following awesomeness:

[Options Framework](http://wptheming.com/options-framework-theme), which is licensed under the GPLv2 License
[Modernizr](http://modernizr.com), which is licensed under the MIT license
[Normalize.css](https://github.com/necolas/normalize.css), which is licensed under the MIT license
[jQuery Validation](http://bassistance.de/jquery-plugins/jquery-plugin-validation) which is dual licensed under the MIT license and GPL licenses
[Font Awesome](http://fortawesome.github.io/Font-Awesome) icon font, which is licensed under SIL Open Font License and MIT License
[PT Sans font](http://www.google.com/fonts/specimen/PT+Sans), which is licensed under SIL Open Font License 1.1
[Arvo font](http://www.google.com/fonts/specimen/Arvo), which is licensed under SIL Open Font License 1.1


== Changelog ==

= 1.3 =
- Updated normalize.css to v3.0.2
- Updated Options Framework to v1.9.1
- Fixed focus on footer links so they're visible
- Added French translation. Props @arpinfo
- Added support for new title-tag
- Added support for WooCommerce

= 1.2.12 =
- Updated Modernizr to v2.8.3
- Updated Font Awesome icon font to v4.2
- Updated jQuery Validation to v1.13.0
- Added SlideShare icon to the theme options

= 1.2.11 =
- Updated Modernizr to v2.8.2
- Updated Font Awesome icon font to v4.1
- Added German translation. Props Tino Groteloh
- Added WPML compatibility

= 1.2.10 =
- Updated normalize.css to v3.0.1
- Updated Modernizr to v2.7.2
- Fixed grid percentages
- Added RSS icon to the theme options
- Added Spanish translation. Props @amieiro
- Updated img element to add vertical-align so images are better aligned

= 1.2.9 =
- Removed Google Analytics script as requested by theme reviewer. This is best left for plugins so please ensure you add one if you were using this feature

= 1.2.8 =
- Fixed undefined function error on sanitization methods that were introduced due to Options Framework changing to class based code

= 1.2.7 =
- Updated Font Awesome icon font to v4.0.3
- Updated Options Framework to v1.7.1
- Updated Modernizr to v2.7.1
- Updated comments to be enclosed in <section> rather than <div>. Props @gnotaras
- Removed pubdate from post/comment meta. Replaced with itemprop
- Removed invalid attribute from email input box. Props @gnotaras

= 1.2.6 =
- Updated normalize.css to v2.1.3
- Updated Font Awesome icon font to v4.0.0 (incl. renaming font classes as per their new naming convention)
- Removed Font Awesome More font as it's now outdated and no longer needed 
- Removed minimum-scale & maximum-scale from viewport meta tag
- Fixed extra period in blockquote style. Props @angeliquejw
- Fixed 'Skip to main content' accessibility link
- Added extra theme option to allow social media links to open in another browser tab/window
- Added extra social media profiles in the theme options for Dribbble, Tumblr, Bitbucket and Foursquare
- Added check for 'Comment author must fill out name and e-mail' setting when validating comments

= 1.2.5 =
- Updated normalize.css to v2.1.2
- Updated Font Awesome icon font to v3.2.1
- Updated theme short description
- Updated Post Format templates to contain Author bio
- Updated _e() references to esc_html_e() to ensure any html added into translation file is neutralised
- Updated __() references to esc_html__() to ensure any html added into translation file is neutralised
- Added template for displaying Author bios
- Added extra use of wp_kses() to ensure only authorised HTML is allowed in translations
- Added loading of Google Fonts in TinyMCE Editor
- Added display of Featured Image on Pages, if used
- Added extra styling to make sure non-breaking text in the title, content, and comments doesn't break layout
- Removed login_errors filter. This is best left for plugins
- Removed audio.js since audio functionality is now part of core
- Removed use of clearfix class as containers will now automatically clear

= 1.2.4 =
- Updated strings that weren't wrapped in gettext functions for translation purposes
- Updated Text Domain in Options Framework
- Added esc_url() when using site URL in header
- Added sanitation when outputting theme options
- Fixed bottom margin on blog articles on homepage
- Fixed text colour in homepage banner

= 1.2.2 =
- Updated blockquote.pull-right style
- Updated footer smallprint link colour
- Fixed display of site name in header area if no Custom Header is specified (ie. no logo image)
- Removed wp_head hook that removes the WP version number. This is best left for plugins
- Updated enqueing of scripts. Scripts that are being depended on, will automatically get enqueued so no need to enqueue them manually.
- Added max-with of 100% to select form fields. Field no longer extends past container in sidebar
- Fixed padding in main content area when homepage is a blog, so pagination doesn't touch footer
- Changed fonts so they're called from Google Fonts rather than local
- Removed unrequired font files from fonts folder

= 1.2.1 =
- Fixed sidebars
- Updated description in stylesheet
- Updated IE filters in btn class 
- Added extra class when styling frontpage widgets

= 1.2 =
- Updated Options Framework to version 1.5.2
- Replaced Museo font with Arvo font
- Replaced background images
- Replaced Responsive Grid System with own custom grid
- Replaced IcoMoon icon font with Font Awesome icon font
- Added GitHub social icon theme option

= 1.1 =
- Changed margin and removed padding on .row class and consolidated html to remove extra container elements from templates
- Removed unnecessary comments from style.css
- Updated navigation margins in media queries
- Updated margin, padding & font-size with matching rem values, where missing
- Updated readme.txt with Getting Started information
- Removed Google Analytics code from footer and enqueued with other scripts
- Initial Repository Release

= 1.0 =
- Initial version
