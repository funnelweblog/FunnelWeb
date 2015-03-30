=== Redux Framework ===
Contributors: section214, dovyp, kprovance
Donate link: https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=N5AD7TSH8YA5U
Tags: admin, admin interface, options, theme options, plugin options, options framework, settings, web fonts, google fonts
Requires at least: 3.5.1
Tested up to: 4.0
Stable tag: 3.3.9.4
License: GPLv3 or later
License URI: http://www.gnu.org/licenses/gpl-3.0.html

Redux is a simple, truly extensible and fully responsive options framework for WordPress themes and plugins. Ships with an integrated demo.

== Description ==

Redux is a simple, truly extensible and fully responsive options framework for WordPress themes and plugins. Built on the WordPress Settings API, Redux supports a multitude of field types as well as custom error handling, custom field & validation types, and import/export functionality.

But what does Redux actually DO? We don't believe that theme and plugin
developers should have to reinvent the wheel every time they start work on a
project. Redux is designed to simplify the development cycle by providing a
streamlined, extensible framework for developers to build on. Through a
simple, well-documented config file, third-party developers can build out an
options panel limited only by their own imagination in a fraction of the time
it would take to build from the ground up!

= Online Demo =
Don't take our word for it, check out our online demo and try Redux without installing a thing!
[**http://demo.reduxframework.com/wp-admin/**](http://demo.reduxframework.com/wp-admin/)

= Use Our Custom Generator to Get Started =
Want to use Redux, but not sure what to do? Use our [generator](http://generate.reduxframework.com/)! It will allow you to make
a custom theme based on [_s](http://underscores.me), [TGM](http://tgmpluginactivation.com), and [Redux](http://reduxframework.com), and any Redux arguments you want to set.
Don't want to make your own theme? Then output a custom admin folder that you can place
in a theme or plugin. Oh and did we mention it's free? Try it today at:
[**http://generate.reduxframework.com/**](http://generate.reduxframework.com/)


= Docs & Support =
We have extremely extensive docs. Please visit [http://docs.reduxframework.com/](http://docs.reduxframework.com/) If that doesn’t solve your concern, you should search [the issue tracker on Github](https://github.com/ReduxFramework/ReduxFramework/issues). If you can't locate any topics that pertain to your particular issue, [post a new issue](https://github.com/ReduxFramework/ReduxFramework/issues/new) for it. Before you submit an issue, please read [our contributing requirements](https://github.com/redux-framework/redux-framework/blob/master/CONTRIBUTING.md). We build off of the dev version and push to WordPress.org when all is confirmed stable and ready for release.


= Redux Framework Needs Your Support =
It is hard to continue development and support for this free plugin without contributions from users like you. If you enjoy using Redux Framework, and find it useful, please consider [making a donation](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=N5AD7TSH8YA5U). Your donation will help encourage and support the plugin's continued development and better user support.

= Fields Types =

* Background
* Border
* Button Set
* Checkbox / Multi-Check
* Color (WordPress Native)
* Color Gradient
* Color RGBA
* Date
* Dimensions (Height/Width)
* Divide (Divider)
* Editor (WordPress Native)
* Gallery (WordPress Native)
* Image Select (Patterns/Presets)
* Import/Export
* Info (Header/Notice)
* Link Color
* Media (WordPress Native)
* Multi-Text
* Password
* Radio (w/ WordPress Data)
* Raw (HTML/PHP/MarkDown)
* Section (Indent and Group Fields)
* Select (Select/Multi-Select w/ Select2 & WordPress Data)
* Select Image
* Slider (Drag a Handle)
* Slides (Multiple Images, Titles, and Descriptions)
* Sortable (Drag/Drop Checkbox/Input Fields)
* Sorter (Drag/Drop Manager - Works great for content blocks)
* Spacing (Margin/Padding/Absolute)
* Spinner
* Switch
* Text
* Textarea
* Typography 
 * The most advanced typography module complete with preview, Google fonts, and auto-css output!

= Additional Features =

* Field Validation
* MANY translations. (See below)
* Full value escaping
* Required - Link visibility from parent fields. Set this to affect the visibility of the field on the parent's value. Fully nested with multiple required parents possible.
* Output CSS Automatically - Redux generates CSS and the appropriate Google Fonts stylesheets for you on select fields. You need only specify the CSS selector to apply the CSS to (limited to certain fields).
* Compiler integration! A custom hook runs when any fields with the argument `compile => true` are changed.
* Oh, and did we mention a fully integrated Google Webfonts setup that will make you so happy you'll want to cry?

  
= Translators & Non-English Speakers =
We need your help to translate Redux into your language! Redux is part of the WP-Translations.org team. To help us translate Redux create a few account here: <a href="https://www.transifex.com/organization/wp-translations">https://www.transifex.com/organization/wp-translations</a>. Once you're in, you can head over to the <a href="https://www.transifex.com/projects/p/redux-framework/">Redux sub-project</a> and translate away. Thank you for your assistance.

= Get Involved =
Redux is an ever-changing, living system. Want to stay up to date or
contribute? Subscribe to one of our mailing lists or join us on [Twitter](https://twitter.com/reduxframework) or [Github](https://github.com/ReduxFramework/ReduxFramework)!

NOTE: Redux is not intended to be used on its own. It requires a config file
provided by a third-party theme or plugin developer to actual do anything
cool!

== Installation ==

= For Complete Documentation and Examples =
Visit: [http://docs.reduxframework.com/](http://docs.reduxframework.com/)

== Frequently Asked Questions ==

= Why doesn't this plugin do anything? =

Redux is an options framework... in other words, it's not designed to do anything on its own! You can however activate a demo mode to see how it works. 

= How can I learn more about Redux? =

Visit our website at [http://reduxframework.com/](http://reduxframework.com/)

= You don't have much content in this FAQ section =
That's because the real FAQ section is on our site! Please visit [http://docs.reduxframework.com/faq/](http://docs.reduxframework.com/faq/)

== Screenshots ==

1. This is the demo mode of Redux Framework. Activate it and you will find a fully-functional admin panel that you can play with. On the Plugins page, beneath the description and an activated Redux Framework, you will find a Demo Mode link. Click that link to activate or deactivate the sample-config file Redux ships with.  Don't take our word for it, check out our online demo and try Redux without installing a thing! [**http://demo.reduxframework.com/wp-admin/**](http://demo.reduxframework.com/wp-admin/)

== Changelog ==

## 3.3.9.4
* Added:    Customizer now supports PANEL! Yay 4.0.
* Fixed:    #1789 - Customizer now properly working again with WP 4.0. Odd bug.

= 3.3.9.2 =
* Fixed:    #1670 - Fixed some extra themecheck and customizer issues.
* Fixed:    #1782 - Media field not showing files after upload? Hopefully this fixes it.

= 3.3.9 =
* Fixed:    #1775 - Call to undefined function is_customize_preview() in pre WP 4.0.
* Fixed:    Issue where in some cases tracking still occuring after opt-out.
* Modified: Documentation URL.
* Fixed:    #1742 - Sidebar subsections don't always expand.
* Fixed:    #1758 - Thanks @echo1consulting!
* Added:    'hidden' to menu_type argument to allow for hidden menus until available.
* Fixed:    #1749 - Remove font-wight and font-style from css output when not in use.
* Modified: Added the "redux/options/{$this->args['opt_name']}/compiler/advanced" hook for more advanced compiling.
* Added:    Suggestions as per #1709. Thanks @echo1consulting.
* Modified: Removed a cURL instance from the core and fixed the developer ad resizing.
* Fixed: PHP 5.2 issues. *sigh*

= 3.3.8.3 =
* Added:   #1593 - Great pull request by @JonasDoebertin. Now you can enqueue dynamic output to the login screen or admin backend.
* Fixed:   Customizer wasn't saving at all! That's been like 4 months. No one's reported it. Hmm.
* Fixed: #1702 - Customizer only fields were being erased on panel save.
* Fixed:   Various Theme-Check errors with languages.
* Added: Theme-Check class to help devs know what is what.
* Fixed: The way we include files from include_once to require_once everywhere.
* Modified: Language files to reflect new strings.
* Modified: Formatted a bunch of old class files.
* Added: Notice on the updates for non-devs to use the new dev_mode disabler plugin and notify their developer.  ;)

= 3.3.8 =
* Modified:   Updated potomo, thanks @shivapoudel.
* Added: Grunt checktextdomain and made improvements. Thanks @shivapoudel. 
* Modified:   #1685 - Specifying no default argument for image_select caused errors on reset.
* Fixed:      #1667 - Slides Upload button causing JS error.
* Fixed: #1670 - Fix for Theme Check -> `add_setting() method needs to have a sanitization callback function passed.`
* Fixed:  #1661 - Fix for undefined index in some versions of PHP. Thanks @gianbalex!
* Modified: #1658 - Improvements from @shivapoudel, including:
  * Removed makepot and used grunt-wp-i18n instead.
  * Added a jshintrc file
  * Added a `grunt addtextdomain` to correct any bad textdomains in the core.
  * Updated .gitignore for better readability
  * Updates to a few other files including package.json.
  * Updated language files.
  * Update codestyles/.editorconfig to reflect the project's standards. 
* Modified:  #1653 - Better admin bar with external links: Admin bar menu priority, icon, and external links. Thanks @shivapoudel!
* Added:      #1651 - `library_filter` argument.  Allows specification of what files to display in the media library.
* Modified:   #1651 - `mode` argument accepts either file type or mime type (but not both).
* Fixed:      #1650 - Toogle error with responsive CSS.
* Fixed:      #1643 - Slight border issue (2px) on sticky footer.
* Fixed:      #1642 - Added `font_family_clear` arg, enabling the clear option for font-family.
* Fixed:      #1638 - Spacing field not outputting when units values attached to default values.
* Modified    #1644 - `import_icon` argument now accepts wordpress dashicons
* Fixed:      #1634 - Double border for sections field. Thanks @AlexandruDoda
* Modified:   Changelog location to now Changelog.md.
* Fixed:      #1632 - Sortable with no defaults set revert to false (instead of options values).
* Fixed:      Labels for sortable in text mode updated to match framework.

= 3.3.7 =
* Added:      #1586 - Class-level declaration for callbacks and validation. Thanks @echo1consulting.
* Modified:   Typography field now fully dynamic.
* Modified:   No longer require a google_api_key for the typography module.  :)
* Fixed:      FTP credentials screen giving a "undefined submit_button function". Resolved.
* Fixed:      #1623 - Registered older noUISlider JS under a new name to avoid conflicts.
* Modified:   #1622 - Removed googlefonts.js dependency.
* Modified:   #1628 - Spacing and dimensions now only output 0 if the entry is a 0, not empty.
              Thanks @Webcreations907
* Modified:   CSS for menu items when active (no hover).
* Added:      Visual feedback to left menu if active.

= 3.3.6.8 =
* Fixed:      #1600 - ACE Editor bombing in PHP 5.2 environments.
* Fixed:      #1591 - Erroneous outputting of font-weight and font-style when no font-family selected.
* Updated:    #1569 - Improved submenu highlighting.
* Added:      #1487 - Added `get_default_value` function into the framework.php
* Fixed:      Framework URI errors when using child themes. Some restructuring.
* Fixed:      Framework URI errors when embedded in theme with Windows.
* Added:      image_size as an option for the data argument. Thanks @Gyroscopic!
* Modified:   How Redux paths are run. Should cover all use cases now. Child themes can also embed
              Redux properly now. Thanks @cfoellmann for the suggestions. Fix for issue #1566.
* Modified:   How we declare the uploads directory and URL. Using core WP functions now.
* Modified:   Now if a section is empty, but has subsections, that section will be "skipped" when
              clicked and the first subsection will then be shown.

= 3.3.6 =
* Fixed:      #1560 - IE8 RGBA fallack
* Modified:   Language files.
* Fixed:      #1543 - Hint icon not changing when set in args.
* Fixed:      #1537 - Media field not accepting images with mode set to false.
* Fixed:      #1529 - ACE Editor conflict with Visual Composer.
* Added:      #1530 - Added argument to specify admin bar icon, `admin_bar_icon`.  Thanks Ninos!
* Fixed:      #1532 - Media field not accepting any mime type when `'mode' => false`.
* Fixed:      #1520 - Checkbox field not displaying Wordpress data when using data argument.
* Fixed:      #1516 - Invalid index and foreach when using compiler and async_typography.
* Fixed:      #1509 - Sorter adding unnecessary bits on some items.
* Fixed:      #1514 - Customizer and multisite not getting on properly.
* Fixed:      #1512 - Slides 'Upload' button not showing or saving selected image.
* Fixed:      Checkboxes with required were working in reverse.
* Fixed:      ASync Typography now works! No more flashing fonts.
* Fixed:      #1489 - Color picker UI lining up improperly.
* Fixed:      #1497 - dev_mode spinner issue.
* Fixed:      Spelling error in tracking dialog.
* Modified:   Updated ace_editor.
* Modified:   Many MANY fields for the group field.
* Fixed:      Some CSS bugs.
* Fixed:      #1481 - Custom fonts loading in google font CSS.
* Fixed:      #1485 - Customizer 'invalid argument' error.  Thanks @rnlmedia.
* Fixed:      #1472 - font style not displaying saved valie with no font-family argument set.
* Fixed:      #1471 - raw field and required not playing nice together.

= 3.3.5 =
* Added:      An annoying notice at the top so our devs don't ship with dev_mode on.  ;)

= 3.3.4.9 =
* Fixed:      #1462 - Google fonts not loading in font drop down.

= 3.3.4.8 =
* Fixed:      More WP FileSystem tanking. Did PHP fallback before FTP. Works 99.9% of the time without credentials.

= 3.3.4.7 =
* Fixed:      Incorrect folder CHMOD in filesystem class.

= 3.3.4.6 =
* Fixed:      #1454 - Chmod permissions for redux folder.

= 3.3.4.5 =
* Fixed:      #1451 - Googlefonts not loading due to failing copy function.

= 3.3.4.4 =
* Fixed:      #1450 - Saves witch values with no `on` or `off` args make the core unhappy.

= 3.3.4.3 =
* Fixed:      #1444, again, due to filesystem growing pains.
* Fixed:      #1449 - Restoring `options` argument over a lousy attempt to fix placeholder.

= 3.3.4.2 =
* Fixed:      More file permission issues.

= 3.3.4.1 =
* Fixed:      Font debug was left from last commit. Sorry all.

= 3.3.4 =
* Fixed:      Issues with file writing. Basically many users don't install WordPress with all the permissions
              correct. So... Had to move it back to ~/uploads/. Sorry Otto, that's just how it is.
* Fixed:      #1444 - output of typography all_styles when font_style UI was hidden.              
* Fixed:      #1440 - flaw in new cleanFilePath logic.
* Fixed:      #1432 - Theme check failing when double-slashes existed in get_template_directory() return.
* Removed:    curlRead from helper class.
* Fixed:      #1426 - menu_name not appearing on front end admin bar.
* Added:      #1427 - button_set added to customizer UI.  Thanks @wpexplorer.
* Fixed:      #1429 - ACE Editor erroring with no default value set.
* Fixed:      wp_filesystem now initialized with credentials in an effort to combat the tmp file issue.
* Modified:   Code purification.
* Modified:   How section tabs work. Isolated within the redux-container class.
* Modified:   #1412 - Redesigned text label, placeholder fix.

= 3.3.3 =
* Fixed:      #1408 & #1357 - Typography subsets losing value after multiple saves on other panels.
* Fixed:      #1403 - unit value no longer prints after empty typography values
* Modified:   Typography: Backup font no longer appends to `font-family` variable.  Please use the
              `backup-font` variable to specify backup fonts.  This does not apply to output/compiler strings.
* Fixed:      #1403 - Backup font not appearing in font-family variable.
* Modified:   Customizer now supports section and field `permissions` argument.
* Fixed:      #1399 - Customizer respects `page_permissions` argument.
* Fixed:      #1400 - output/compiler string incomplete using multiple selectors.
* Fixed:      #1396 - Custom fonts cutting off multiple families in selector, after save.
* Fixed:      Typography attempting to queue up non google fonts on backend.
* Added:      #1395 - Display of child theme status in sysinfo, thanks @SiR-DanieL.
* Fixed:      #1387 - Page jump when clicking "Options Object".  Thanks @rrikesh.
* Added:      #1392 - Filters to change the following localized strings:
              redux/{opt_name}/localize/reset
              redux/{opt_name}/localize/reset_all
              redux/{opt_name}/localize/save_pending
              redux/{opt_name}/localize/preset
* Fixed:      #1376 - checkbox.min.js missing.
* Fixed:      Static variable changes for instances and basic comment cleanup
* Fixed:      #1361 - Raw field not hiding with required.
* Fixed:      Datepicker not formatting properly.  Still needs some work.


= 3.3.2 =
* Fixed:      #1357 - Preview not rendering font on page load.
* Fixed:      #1356 - Color fields and transparency not syncing due to new JS.
* Fixed:      #1354 - Add class check for W3_ObjectCache.
* Fixed:      #1341 - JS not initializing properly in import_export.
* Fixed:      #1339 - Typography would lose Font Weight and Style. value was named val in the
              HTML, so it would be destroyed on the next save if not initialized.
* Fixed:      #1226 - W3 Total Cache was affecting validation and compiler hooks.
* Fixed:      Menu errors weren't showing properly for non-subsectioned items.
* Fixed:      #1341 - Import/Export buttons not functioning. Also fixed sortable somehow.
* Fixed:      Slides not initializing with the last fix.
* Fixed:      Slides field was not properly initialized for the media elements. Fixed.

= 3.3.1 =
* Fixed:      #1337 - `redux` JS dependency loading issue.  Many thanks @tpaksu
* Modified:   Drastically changed the way JavaScript is used in the panel. Forced as-needed
              initialization of fields. Thus reducing dramatically the overall load time of
              the panel. The effects have been seen up to 300% speed improvement. The only
              time a field will be initialized is if it's visible, thus reducing the processing
              needed in DOM overall.
* Fixed:      #1336 - fixed default font in preview.
* Fixed:      #1334 - Typography not un-saving italics.
* Added:      #1332 - New validation: numeric_not_empty.
* Fixed:      #1330 - Required not working on all fields.

= 3.3.0 =
* Fixed:      #1322 - Sections not folding with required argument.
* Fixed:      #1270 - Editor field compiler hook not firing in visual mode.
* Fixed:      select2 dependency in select_image, and other fields.
* Fixed:      Filter out `@eaDir` directories in extensions folder.
* Fixed:      Fixed the image_select presets to work again. Also now will function even if import/export is disabled.
* Fixed:      Minor tweaks for metabox update.
* Fixed:      #1297 - Missing space in image_select class.
* Fixed:      Slider field tweaked for metaboxes.
* Fixed:      #1291 - Change of font-family would not trigger preview, or show in open preview.  
* Fixed:      #1289 - Typography not retaining size/height/spacing/word/letter spacing settings.
* Fixed:      #1288 - Background color-picker dependency missing.  Thanks @farhanwazir.
* Fixed:      Search extension failed do to dependency issue from the core.
* Fixed:      #1281 - color field output/compiler outputting incorrect selector when only one array present.
* Fixed:      Update check only appears once if multiple instances of Redux are loaded in the same wordpress instance.
* Fixed:      Changing font-family in typography didn't trigger 'save changes' notification.
* Fixed:      More typography: Back up font appearing in font-family when opening selector.
* Fixed:      Typography: undefined message when NOT using google fonts.  Thanks @farhanwazir
* Fixed:      Typography font backup not in sync with font-family.
* Fixed:      Typography not saving font-family after switching back and forth between standard and google fonts.
* Fixed:      Background field selects not properly aligned.
* Fixed:      Removed select field dependency from background field.
* Fixed:      #1264 - Color-picker/transparent checkbox functionality.
* Fixed:      Typography fine-tuning.
* Fixed:      All typography select fields render as select2.
* Fixed:      Switching between transparency on and off now restores the last chosen color in all color fields. 
* Fixed:      Redux uploads dir should NOT be ~/wp-content/uploads, but just wp-content. As per Otto.
* Fixed:      Navigation no longer has that annoying outline around the links. Yuk.
* Fixed:      #1218 - Select2 multi select not accepting any keyboard input.
* Fixed:      #1228 - CSS fixes
* Added:      `hide_reset` argument, to hide the Reset All and Reset Section buttons.        
* Added:      `content_title` argument to slides field.  Thanks @psaikali!
* Added:      `customizer_only` argument for fields & sections, contributed by @andreilupu.
* Added:      select2 args for spacing field.
* Added:      select2 args for the following fields: typography, background, border, dimensions and slider.
* Added:      #1329 - `'preview' = array('always_display' => true)` argument to typography, to determine if
              preview field show always be shown.
* Modified:   Portions of core javascript rewritten into object code.
* Modified:   All field javascript rewritten using jQuery objects (versus standard function).
* Modified:   Typography field rewritten to fill out font-family field dynamically, versus on page load.

= 3.2.9.13 =
* Modified    data => taxonomies now has a little more power behind it.
* Fixed:      #1255 - button_set multi field not saving when all buttons not selected.
* Fixed:      #1254 - Border field with 0px not outputting properly.
* Fixed:      #1250 - Typography preview font-size not set in preview.
* Fixed:      #1247 - Spacing field not outputting properly in `absolute` mode.
* Modified:   Typography previewing hidden until font inputs are changed.
* Fixed:      Vendor js not loading properly when dev_mode = true
* Fixed:      Border field not outputting properly.
* Modified:   Centralized import/export code in anticipation of new builder features.
* Fixed:      Removed rogue echo statement.
* Modified:   select2 loads only when a field requires it.
* Modified:   More code to load JS on demand for fields require it.
* Modified:   Field specific JS only loads with active field.
* Fixed:      Hints stopped working due to classname change.
* Fixed:      Permissions argument on section array not filtering out raw field.
* Fixed:      Too many CSS tweaks to list, due to last build.
* Fixed:      Sortable and Sorter fields now sort without page scroll when page size is under 782px.
* Fixed:      Hint icon defaults to left position when screen size is under 782px.
* Fixed:      `permissions` argument for fields and sections erasing saved field data.  See #1231
* Modified:   Woohoo! Nearly fully responsive. Yanked out all SMOF and NHP field customizations. Lots of little
              fixes on all browser screens. This will also greatly benefit Metaboxes and other areas of Redux.
* Fixed:      In dev_mode panel CSS was being loaded 2x.
* Fixed:      Typography color picker bleeding under other elements.  #1225
* Fixed:      Hint icon_color index error from builder.  #1222

= 3.2.9 =
* Added:      Network admin support! Set argument 'database' to network and data will be saved site-wide. Also
              two new arguments: network_admin & network_sites for where to show the panel.
* Added:      Customizer hook that can be used to simulate the customizer for live preview in the customizer.
              `redux/customizer/live_preview`
* Added:      `output` argument for `color` and `color_rgba` fields accepts key/pairs for different modes.
* Added:      `class` argument to the Redux Arguments, section array, and metabox array. If set, a class will
              be appended to whichever level is used. This allows further customization for our users.
* Added:      disable_save_warn flags to the arguments to disable the "you should save" slidedown.
* Added:      Actions hooks for errors and warnings.
* Fixed:      Redux now ignores any directories that begin with `.` in the extension folder.  See #1213.
* Fixed:      Redux not saving when validating uploads.
* Fixed:      Border field output/compiler formatting.  Removed 'inherit' in place of default values.  See #1208.
* Fixed:      Trim() warning in framework.php when saving.  See #1209, #1201.
* Fixed:      Typography not outputting all styles when `all_styles` set to true.
* Fixed:      'Cannot send header' issues with typography.
* Fixed:      Small fix for validation if subsection parent is free of errors, remove the red highlight when not
              expanded.
* Fixed:      Small CSS classes for flashing fonts where web-font-loader.
* Fixed:      ASync Flash on fonts. FINALLY. What a pain.
* Fixed:      3+ JavaScript errors found in the background field. Now works flawlessly.
* Fixed:      PHP warnings in background field.  #1173.  Thanks, @abossola.
* Fixed:      CSS validation not respecting child selector symbol. #1162
* Fixed:      Extra check for typography bug.
* Fixed:      Error css alignment issue with subsections.
* Fixed:      javascript error in typography field.
* Fixed:      Added a title to the google fonts stylesheet to fix validation errors.
* Fixed:      One more slides field error check, and an extra JS goodie for an extension.
* Fixed:      Leftover debug code messing up slides field.
* Fixed:      More reliable saved action hook.
* Fixed:      Removed erroneous debug output in link_color field.
* Modified:   Dimension field default now accepts either `units` or `unit`.
* Modified:   Google CSS moved into HEAD via WP enqueue.
* Modified:   Now do a trim on all fields before validating. No need to alert because of a space...
* Modified:   Typography field CSS completely rewritten. All thanks to @eplanetdesign!
* Modified:   Validation now works in metaboxes as well as updates numbers as changes occur. Validation for
              subsections is SO hot now.
* Modified:   Various CSS fixes and improvements.
* Modified:   Turned of mod_rewrite check.
* Modified:   How errors are displayed, no longer dependent on the ID, now proper classes.
* Modified:   Error notice stays until all errors are gone. Also updates it's number as errors fixed!
* Modified:   Moved google font files to proprietary folder in upload to help with permission issues.

= 3.2.8 =
* Fixed:        Formatting of field files. Normalizing headers.
* Added:        is_empty / empty / !isset    AND    not_empty / !empty / isset as required operations
* Fixed:        Reset defaults error.
* Added:        `show` argument to turn on and off input boxes in slider.
* Fixed:        Required now works with muti-check fields and button set when set to multi.

= 3.2.7 =
* Fixed:        Import works again. A single line was missed...
* Fixed:        link_color field not outputting CSS properly via compiler or output.  Thanks @vertigo7x
* Fixed:        Sorter field CSS.  Buttons were all smushed together.
* Fixed:        'undefined' error in typography.js.  Thanks @ksere.

= 3.2.6 =
* Fixed:        Another stray undefined index. Oy.
* Added:        `open_expanded` argument to start the panel completely expanded initially.

= 3.2.5 =
* Fixed:        Various bad mistakes. Oy.

= 3.2.4 =
* Fixed:        Slight typography speed improvement. Less HTML hopefully faster page loads.
* Fixed:        Unload error on first load if the typography defaults are not set.
* Fixed:        Errors pertaining to mod_rewrite check.
* Fixed:        All those headers already set errors.
* Added:        $changed_values variable to save hooks denoting the old values on a save.
* Added:        Pointers to Extensions on load.
* Modified:     CSS Output for the background field.
* Fixed:        Validation error messages not appearing on save.
* Modified:     Speed boost on validation types.
* Added:        Apache mod_rewrite check.  This should solve many issues we've been seeing regarding mod_rewrite
                not being enabled.
* Fixed:        Sortable field not saving properly.
* Fixed:        Erroneous data in admin.less
* Updated:      sample-config.php.  Sortable checkbox field example now uses true/false instead of text meant for
                textbox example.

= 3.2.3 =
* Fixed:        Responsive issues with spacing and dimension fields.
* Fixed:        Style conflicts with WP 3.9. Added register filter to fields via id.
* Fixed:        Metaboxes issues.
* Fixed:        Compiler hook in the customizer now passes the CSS.
* Fixed:        Compiler hook now properly fires in the customizer.
* Fixed:        Validation error with headers already being set.
* Fixed:        Added mode for width/height to override dimensions css output.
* Fixed:        Restoring lost formatting from multiple merges.
* Fixed:        New sorter default values get set properly now.  ;)
* Fixed:        Removed erroneous 's' character from HTML.
* Fixed:        Info field didn't intend within section.
* Fixed:        Compiler hook wasn't running.
* Modified:     Some admin panel stylings. Now perfect with mobile hover. Also fixed an issue with the slidedown
                width for sections. No more 2 empty pixels.
* Added:        `data` and `args` can now be set to sorter! Just make sure to have it be a key based on what you
                want it to display as. IE: `array('Main'=>'sidebars')`
* Added:        Prevent Redux from firing on AJAX heartbeat, but added hook for it 'redux/ajax/heartbeat'.
* Added:        Tick mark if section has sub sections. Hidden when subsections expanded.
* Added:        Check to make sure a field isn't empty after the filter. If it is empty, skip over it.
* Added:        Subsections now show icon if they have it. Show text only (without indent) if they do not.
* Added:        Set a section or field argument of `'panel' => false` to skip over that field or panel and hide it.
                It will still be registered with defaults saved, but not display. This can be useful for things
                like the customizer.
* Added:        SUBSECTIONS! Just add `'subsection' => true` to any section that isn't a divide/callback and isn't
                the first section in your panel.  ;)

= 3.2.1 =
* Fixed:      Small bug in image_select javascript.
* Added:      Import hook, just because we can.  :)
* Fixed:      Customizer preview now TRULY outputs CSS even if output_tag is set to false;
* Fixed:      Reset section, etc. Discovered an odd WordPress thing.
* Fixed:      Image_select size override.
* Fixed:      Customizer save not firing the compiler hook.
* Fixed:      Customizer not outputting CSS if output_tag is set to false.
* Fixed:      Small empty variable check. Undefined index in the defaults generating function.
* Fixed:      WP 3.9 update made editor field button look ugly.
* Fixed:      Save hook not firing when save_default set to false.
* Fixed:      Reset section anomalies.  Maybe.
* Fixed:      Array of values in required not recognized.
* Fixed:      Updated hint defaults to prevent index warning.
* Fixed:      Removed leftover debug code.
* Added:      New readonly argument for text field.
* Fixed:      Reset/Reset section actions hooks now fire properly.
* Fixed:      When developer uses section field but does not specify an indent argument.
* Fixed:      Dynamic URL for slides
* Fixed:      Accidently removed reset action on section reset. Restored.
* Fixed:      Section defaults bug for certain field types.
* Fixed:      Dynamic URL if site URL changed now updates media properly if attachement exists.
* Fixed:      Customizer now correctly does live preview.
* Fixed:      Special enqueue case fix.
* Added:      A few more hooks for defaults and options.
* Fixed:      Small undefined index error.
* Added:      Section key generation via title.
* Modified:   File intending.
* Fixed:      Custom menus not displaying options panel.
* Fixed:      Single checkbox option not retaining checked value.
* Fixed:      Border field returning bad CSS in CSS compiler.
* Fixed:      Import/Export fix.  Thanks, @CGlingener!

= 3.2.0 =
* Added:      Save warning now is sticky to the top and responsive.
* Fixed:      Mobile fixes for Redux. Looks great on small screens how.
* Fixed:      Slight CSS fixes.
* Fixed:      Compiler fixes and added notices.
* Added:      Import/Export more reasonable text.
* Added:      `force_output` on the field level to bypass the required check that removes the output if the field is hidden. Thanks @rffaguiar.
* Fixed:      Fully compatible with WordPress 3.9. Now it just works.  ;)
* Fixed:      Info and divide field now work with required.
* Added:      Fallback. Now if the media, slides, or background URL doesn't match the site URL, but the attachment ID is present, the data is updated.
* Fixed:      Last tab not properly set.  Slow rendering.
* Modified:   Replaced transients with cookies. Less DB queries.
* Fixed:      Undefined variable issues for new required methods.
* Fixed:      Default_show display error with a non-array being steralized.
* Added:      Multiple required parent value checking! Booya!
* Fixed:      Sections now fold with required.
* Fixed:      select2 not rendering properly when dev_mode = false, because of ace_editor fix.
* Fixed:      Removed mistakenly compiled test code from redux.js.
* Fixed:      ace_editor not rendering properly in certain instances.
* Modified:   Small change to import_export field in checking for existing instance of itself.
* Fixed:      import_export not rendering when the menutype argument was set to menu
* Fixed:      Ace_editor not enqueued unless used. MEMORY HOG.
* Fixed:      Color_Gradient transparency to was being auto-selected if from way transparent.
* Fixed:	  Enqueue select with slider for local dev.
* Modified:   removed add_submenu_page when creating a submenu for us in the WP admin area.  WP approved API is used in it's place to being Redux up to wp.org theme check standards.
* Fixed:      Massive speed issue with button_set. Resolved.
* Fixed:      Issue where default values throws an error if ID is not set.
* Fixed:      Continuing effort to ensure proper loading of config from child themes.
* Fixed:      Import/Export array search bug if section['fields'] is not defined.
* Fixed:      Inconsistencies in import/export across different versions of PHP.
* Fixed:      Redux checks for child or parent theme exclusively before loading.

= 3.1.9 =
* Fixed:      Typography custom preview text/size not outputting.
* Fixed:      No font selected in typography would default to 'inherit'.
* Fixed:      Hint feature kicking back a notice if no title was specified.
* Fixed:      Sortable field, when used a checkboxes, were all checked by default, even when set not to be.
* Fixed:      button_set field not setting properly in multi mode.
* Fixed:      Javascript console object not printing options object.
* Fixed:      Load errors from child themes no longer occur.
* Fixed:      Compiler output for slider field.
* Fixed:      update_check produced a fatal error on a local install with no internet connection.
* Fixed:      Compiler hook failing on slider.
* Fixed:      Error on update_check when the response code was something other than 200.
* Fixed:      image_select images not resizing properly in FF and IE.
* Fixed:      Layout for the typography field, so everything isn't smushed together.  The new layout is as follows:
* Fixed:      link_color field showing notice on default, if user enters no defaults.
* Fixed:      Fixed tab notice in framework.php if no tab parameter is set in URL.
* Fixed:      Hide demo hook wasn't hiding demo links.
* Added:      Admin notice for new builds of Redux on Github as they become available.  This feature is available on in dev_mode, and may be turned off by setting the `update_notice` argument to false.  See the Arguments page of the wiki for more details.
* Added:      text-transform option for the typography field.
* Added:      Newsletter sign-up popup at first load of the Redux options panel.
* Added:      Added PHP 5.2 support for import/export.
* Added:      Action hooks for options reset and options reset section.
* Added:      Theme responsive for date picker.
* Added:      New slider.  Better looking UI, double handles and support for floating point values.  See the wiki for more info.
* Added:      Typography improvements.
* Added:      Hints!  More info:  https://github.com/ReduxFramework/ReduxFramework/wiki/Using-Hints-in-Fields
* Added:      Complete Wordpress admin color styles. Blessed LESS/SCSS mixins.  ;)
* Added:      Font family not required for the typography module any longer.
* Added:      Support for using the divide field in folding.
* Added:      Error trapping in typography.js for those still attempting to use typography with no font-family.
* Added:      Full asynchronous font loading.
* Added:      email_not_empty validation field.
* Modified:   Typography word and letter spacing now accept negative values.
* Modified:   Typography preview shows spaces between upper and lower case groupings.
* Modified:   Google font CSS moved to header so pages will pass HTML5 validation.
* Modified:   Removed Google font CSS line from header (because it's in the footer via wp_enqueue_style.
* Modified:   RGBA Field stability.  Thank you, @SilverKenn.
* Modified:   Separated Import/Export from the core.  It can now be used as a field.
              [family-font] [backup-font]
              [style] [script] [align] [transform]
              [size] [height] [word space] [letter space]
              [color]
* Reverted:   email validation field only checks for valid email.  not_empty check moved to new validation field.

= 3.1.8 =
* Fixed:    Improper enqueue in tracking class.
* Fixed:    Few classes missed for various fields.
* Fixed:    Spacing field kicking back notices and warnings when 'output' wasn't set.
* Modified: Added file_exists check to all include lines in framework.php
* Fixed:    Background field now works with dynamic preview as it should.
* Fixed:    Extension fields now enqueueing properly.
* Added:    Text-align to typography field.
* Fixed:    Servers returning forwards slashes in TEMPLATEPATH, while Redux is installed embedded would not show options menu.
* Fixed:    On and Off for switch field not displaying language translation.
* Fixed:    email validation allowing a blank field.
* Fixed:    Now allow for empty values as valid keys.
* Added:    Dismiss option to admin notices (internal function)

= 3.1.7 =
* Fixed:    Servers returning forwards slashes in TEMPLATEPATH, while Redux is installed embedded would not show options menu.
* Fixed:    On and Off for switch field not displaying language translation.
* Fixed:    email validation allowing a blank field.
* Added:    Dismiss option to admin notices (internal function)
* Fixed:    On and Off for switch field not displaying language translation.
* Fixed:    email validation allowing a blank field.
* Added:    Dismiss option to admin notices (internal function)

= 3.1.6 =
* Fixed:    CSS spacing issue
* Fixed:    Customizer now works and doesn't break other customizer fields outside of Redux.
* Fixed:    Several minor bug fixes
* Added:    Metabox support via extension http://reduxframework.com/extensions/
* Added:    Admin-bar menu
* Fixed:    Section field now folds.
* Fixed:    wp_content_dir path now handles double forward slashes.
* Fixed:    Typography field missing italics in Google fonts.
* Fixed:    Default color in border field not saving properly.
* Fixed:    hex2rgba in class.redux_helpers.php changed to static.
* Fixed:    'sortable' field type not saving options as default.
* Fixed:    Specified default color not set when clicking the color box default button.
* Fixed:    Sorter field options are now saved as default in database.
* Fixed:    Issues with checkboxes displaying default values instead of labels.
* Fixed:    Outstanding render issues with spacing field.
* Fixed:    Plugins using Redux from load failure.
* Fixed:    'not_empty' field validation.
* Fixed:    Media field.
* Added:    'read-only' option for media text field.
* Added:    'mode' option to image_select, so CSS output element may be specified.
* Added:    Admin Bar menu for option panel.
* Modified: media field 'read-only' to 'readonly' to vonform to HTML standards.
* Modified: Removed raw_align field and added align option to raw field. See wiki for more info.
* Removed:  EDD extension. It never belonged in Core and will be re-released as a downloadable extension shortly
* Removed:  Group field, temporarily.
* Removed:  wp_get_current_user check.  See https://github.com/ReduxFramework/ReduxFramework/wiki/How-to-fix-%22Fatal-error%3A-Call-to-undefined-function-wp_get_current_user%28%29-%22

= 3.1.5 =
* Typography font arrays may not contain comma spaces.
* Merge in pull request - 542, code cleanup and better readability
* Change how HTML is output to support metaboxes
* CSS only on pages that matter, better checks.
* font-backup in typography now appends to font-family in output and compiler.
* More fixes for Google font css outputting.
* Addded output and compiler to field_image_select.  Images will be output as 'background-image'.
* Fixed output in field_background.
* Prevent standard fonts from outputting to Google fonts CSS call.
* class_exists in field_section checking for incorrect classname.
* sample_config fix.
* Compiler not outputting CSS without output set to comthing other than false.
* Google fonts not rendering on frontend.
* Rewrote sample_config as a class

= 3.1.4 =
* Fixed error in redux-framework.php.
* Added select_image field.

= 3.1.3 =
* Fixed a few undefined variables
* Removed old code from the repo.
* Fix for validation.
* Remove the compiler hook by default.
* Fix to sortable field.
* Added an extra check for link color. Removes user error.
* Localization updates.
* Error in slides.
* Fixed the info box bug with spacing and padding.
* Fixed the first item in each section having WAY too much padding.  ;)
* Fixed section reset issue where values weren't being saved to the db properly.

= 3.1.2 =
* Feature - Sortable select boxes!
* Feature - Reset a section only or the whole panel!
* New Field - RGBA Color Field!
* Improvement - Use of REM throughout.
* Fixed Typography - Fix output option and various small bugs.
* Fixed Border - Fix output option and various small bugs.
* Fixed Dimensions - Fix output option and various small bugs.
* Fixed Image_select - Various small bugs.
* Fixed Slides - Various small bugs.
* Fixed Sortable - Using native jQuery UI library same as within WordPress.
* Fixed Slider and Spinner Input Field - Values now move to the closest valid value in regards to the step, automatically.
* Fixed Ace Editor
* FEATURE - All CSS/JS files are compiled into a single file now! Speed improvements for the backend. 
* Fix in how WordPress data is received, improved some output.
* Fix for various fields not triggering fold/compiler/save.
* Fixed elusive icons to use the new version and classes.
* Fixed media thumb to only be the thumbnail version.
* Fixed admin https error with WordPress core not renaming URL.
* Placeholders throughout the framework are now properly there.
* Feature - Setting to not save defaults to database on load.
* Fixed - Computability issue with GT3 builder.
* Fixed localization issue with default values.
* Language - Added Russian
* Feature - Media now can have any content type passed in to limit content types.
* Allow negative values in typography and other fields.
* WordPress 3.8 computability.
* CSS validation issue.
* Feature - User contributed text direction feature.
* EDD Extension now fully function for plugins or themes.
* Removed get_theme_data() fallbacks, we're well pass WordPress 3.4 now.  ;)
* A ton of other small updates and improvements.


= 3.1.0 =
* Fix Issue 224 - Image Select width was breaking the panel.
* Fix Issue 181 - Broken panel in firefox
* Fix Issue 225 - 0px typography bug. Thanks @partnuz.
* Fix Issue 228 - Resolved a duplicated enqueue on color_link field. Thanks @vertigo7x.
* Fix Issue 231 - Field spacing bug fixes.
* Fix Issue 232 & 233 - Dimensions: bug fix with units and multiple units. Thanks @kpodemski
* Fix Issue 234 - Pass options as a ref so validating actions can modify/sanitize them. Thanks @ZeroBeeOne
* Fix Issue 222 - Tab cookie function wasn't working.
* Feature - Pass params to Select2. Thanks @andreilupu
* Fix Issue 238 - Fix for conditional output. Thanks @partnuz.
* Fix Issue 211 - Google Web font wasn't loading at first init of theme.
* Fix Issue 210 - Elusive Icons update. Changed classes to force use of full elusive name.
* Fix Issue 247 - Media thumbnails were not showing. Also fixed media to keep the largest file, but display the small version in the panel as a thumb. Thanks @kwayyinfotech.
* Fix Issue 144 - JS error when no item found in slider.
* Fix Issue 246 - Typography output errors.
* Feature & Issue 259 - Multi-Text now support validation!
* Fix Issue 248/261 - Links color issue. Also fixed color validation.
* Feature & Issue 262 - Now registered sidebars can be used as a data type.
* Fix Issue 194/276 - Custom taxonomy terms now passing properly. Thanks @kprovance.
* Feature & Issue 273 - Argument save_defaults: Disable the auto-save of the default options to the database if not set.
* Feature - Docs now being moved to the wiki for community participation.
* Issue 283 - Date placeholder. Thanks @kprovance.
* Issue 285 - HTTPS errors on admin. Known WordPress bug. Resolved.
* Fix Issue 288 - Float values now possible for border, dimensions, and spacing.
* Feature - Media field can now accept non-image files with a argument being set.
* Fix Issue 252 - Post Type data wasn't working properly. Thanks @Abu-Taymiyyah.
* Fix Issue 213 - Radio and Button Set wasn't folding.

= 3.0.9 =
* Feature - Added possibility to set default icon class for all sections and tabs.
* Feature - Make is to the WP dir can be moved elsewhere and Redux still function.
* Added Spanish Language. Thanks @vertigo7x.
* Fix Issue 5 - Small RGBA validation fix.
* Fix Issue 176 - Fold by Image Select. Thanks @andreilupu.
* Fix Issue 194 - Custom taxonomy terms in select field.
* Fix Issue 195 - Border defaults not working.
* Fix Issue 197 - Hidden elements were showing up on a small screen. Thanks @ThinkUpThemes.
* Fix issue 200 - Compiler not working with media field.
* Fix Issue 201 - Spacing field not using default values.
* Fix Issue 202 - Dimensions field not using units.
* Fix Issue 208 - Checkbox + Required issue.
* Fix Issue 211 - Google Font default not working on page load.
* Fix Issue 214 - Validation notice not working for fields.
* Fix Issue 181/224 - Firefox 24 image resize errors.
* Fix Issue 223 - Slides were losing the url input field for the image link.
* Fix - Various issues in the password field.
* Fixed various spelling issues and typos in sample-config file.
* Initialize vars before extract() - to shut down undefined vars wargnings.
* Various other fixes.

= 3.0.8 =
* Version push to ensure all bugs fixes were deployed to users. Various.

= 3.0.7 =
* Feature - Completely redone spacing field. Choose to apply to sides or all at once with CSS output!
* Feature - Completely redone border field. Choose to apply to sides or all at once with CSS output!
* Feature - Added opt-in anonymous tracking, allowing us to further analyze usage.
* Feature - Enable weekly updates of the Google Webfonts cache is desired. Also remove the Google Webfont files from shipping with Redux. Will re-download at first panel run to ensure users always have the most recent copy.
* Language translation of german updated alone with ReduxFramework pot file.
* Fix Issue 146 - Spacing field not storing data.
* Fix - Firefox field description rendering bug.
* Fix - Small issue where themes without tags were getting errors from the sample data.

= 3.0.6 =
* Hide customizer fields by default while still under development.
* Fix Issue 123 - Language translations to actually function properly embedded as well as in the plugin.
* Fix Issue 151 - Media field uses thumbnail not full image for preview. Also now storing the thumbnail URL. Uses the smallest available size as the thumb regardless of the name.
* Fix Issue 147 - Option to pass params to select2. Contributed by @andreilupu. Thanks!
* Added trim function to ace editor value to prevent whitespace before and after value keep being added
* htmlspecialchars() value in pre editor for ace. to prevent html tags being hidden in editor and rendered in dom
* Feature: Added optional 'add_text' argument for multi_text field so users can define button text.
* Added consistent remove button on multi text, and used sanitize function for section id
* Feature: Added roles as data for field data
* Feature: Adding data layout options for multi checkbox and radio, we now have quarter, third, half, and full column layouts for these fields.
* Feature: Eliminate REDUX_DIR and REDUX_URL constants and instead created static ReduxFramework::$_url and ReduxFramework::$_dir for cleaner code.
Feature: Code at bottom of sample-config.php to hide plugin activation text about a demo plugin as well as code to demo how to hide the plugin demo_mode link.
* Started work on class definitions of each field and class. Preparing for the panel builder we are planning to make.

= 3.0.5 =
* Fixed how Redux is initialised so it works in any and all files without hooking into the init function.
* Issue #151: Added thumbnails to media and displayed those instead of full image.
* Issue #144: Slides had error if last slide was deleted.
* Color field was outputting hex in the wrong location.
* Added ACE Editor field, allowing for better inline editing.

= 3.0.4 =
* Fixed an odd saving issue.
* Fixed link issues in the framework
* Issue #135: jQuery UI wasn't being properly queued
* Issue #140: Admin notice glitch. See http://reduxframework.com/2013/10/wordpress-notifications-custom-options-panels/
* Use hooks instead of custom variable for custom admin CSS
* Added "raw" field that allows PHP or a hook to embed anything in the panel.
* Submenus in Admin now change the tabs without reloading the page.
* Small fix for multi-text.
* Added IT_it and RO_ro languages.
* Updated readme file for languages.

= 3.0.3 =
* Fixed Issue #129: Spacing field giving an undefined.
* Fixed Issue #131: Google Fonts stylesheet appending to body and also to the top of the header. Now properly placed both at the end of the head tag as to overload any theme stylesheets.
* Fixed issue #132 (See #134, thanks @andreilupu): Could not have multiple WordPress Editors (wp_editor) as the same ID was shared. Also fixed various styles to match WordPress for this field.
* Fixed Issue #133: Issue when custom admin stylesheet was used, a JS error resulted.

= 3.0.2 =
* Improvements to slides, various field fixes and improvements. Also fixed a few user submitted issues.

= 3.0.1 =
* Backing out a bit of submitted code that caused the input field to not properly break.

= 3.0.0 =
* Initial WordPress.org plugin release.

= 3.0 =
Redux is now hosted on WordPress.org! Update in order to get proper, stable updates.


== Attribution ==

Redux is was originally based off the following frameworks:

* [NHP](https://github.com/leemason/NHP-Theme-Options-Framework)
* [SMOF](https://github.com/syamilmj/Options-Framework "Slightly Modified Options Framework")

It has now a completely different code base. If you like what you see, realize this is a labor of love. Please [donate to the Redux Framework](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=N5AD7TSH8YA5U) if you are able.
