<?php

/**
  ReduxFramework Sample Config File
  For full documentation, please visit: https://docs.reduxframework.com
 * */

if (!class_exists('flower_Redux_Framework_config')) {

    class flower_Redux_Framework_config {

        public $args        = array();
        public $sections    = array();
        public $theme;
        public $ReduxFramework;

        public function __construct() {

            if (!class_exists('ReduxFramework')) {
                return;
            }

            // This is needed. Bah WordPress bugs.  ;)
            if ( true == Redux_Helpers::isTheme( __FILE__ ) ) {
                $this->initSettings();
            } else {
                add_action('plugins_loaded', array($this, 'initSettings'), 10);
            }

        }

        public function initSettings() {

            // Just for demo purposes. Not needed per say.
            $this->theme = wp_get_theme();

            // Set the default arguments
            $this->setArguments();

            // Set a few help tabs so you can see how it's done
            $this->setHelpTabs();

            // Create the sections and fields
            $this->setSections();

            if (!isset($this->args['opt_name'])) { // No errors please
                return;
            }

            // If Redux is running as a plugin, this will remove the demo notice and links
            add_action( 'redux/loaded', array( $this, 'remove_demo' ) );
            
            // Function to test the compiler hook and demo CSS output.
            // Above 10 is a priority, but 2 in necessary to include the dynamically generated CSS to be sent to the function.
            //add_filter('redux/options/'.$this->args['opt_name'].'/compiler', array( $this, 'compiler_action' ), 10, 2);
            
            // Change the arguments after they've been declared, but before the panel is created
            //add_filter('redux/options/'.$this->args['opt_name'].'/args', array( $this, 'change_arguments' ) );
            
            // Change the default value of a field after it's been set, but before it's been useds
            //add_filter('redux/options/'.$this->args['opt_name'].'/defaults', array( $this,'change_defaults' ) );
            
            // Dynamically add a section. Can be also used to modify sections/fields
            //add_filter('redux/options/' . $this->args['opt_name'] . '/sections', array($this, 'dynamic_section'));

            $this->ReduxFramework = new ReduxFramework($this->sections, $this->args);
        }

        /**

          This is a test function that will let you see when the compiler hook occurs.
          It only runs if a field	set with compiler=>true is changed.

         * */
        function compiler_action($options, $css) {
            //echo '<h1>The compiler hook has run!';
            //print_r($options); //Option values
            //print_r($css); // Compiler selector CSS values  compiler => array( CSS SELECTORS )

            /*
              // Demo of how to use the dynamic CSS and write your own static CSS file
              $filename = dirname(__FILE__) . '/style' . '.css';
              global $wp_filesystem;
              if( empty( $wp_filesystem ) ) {
                require_once( ABSPATH .'/wp-admin/includes/file.php' );
              WP_Filesystem();
              }

              if( $wp_filesystem ) {
                $wp_filesystem->put_contents(
                    $filename,
                    $css,
                    FS_CHMOD_FILE // predefined mode settings for WP files
                );
              }
             */
        }

        /**

          Custom function for filtering the sections array. Good for child themes to override or add to the sections.
          Simply include this function in the child themes functions.php file.

          NOTE: the defined constants for URLs, and directories will NOT be available at this point in a child theme,
          so you must use get_template_directory_uri() if you want to use any of the built in icons

         * */
        function dynamic_section($sections) {
            //$sections = array();
            $sections[] = array(
                'title' => __('Section via hook', 'redux-framework-demo'),
                'desc' => __('<p class="description">This is a section created by adding a filter to the sections array. Can be used by child themes to add/remove sections from the options.</p>', 'redux-framework-demo'),
                'icon' => 'el-icon-paper-clip',
                // Leave this as a blank section, no options just some intro text set above.
                'fields' => array()
            );

            return $sections;
        }

        /**

          Filter hook for filtering the args. Good for child themes to override or add to the args array. Can also be used in other functions.

         * */
        function change_arguments($args) {
            //$args['dev_mode'] = true;

            return $args;
        }

        /**

          Filter hook for filtering the default value of any given field. Very useful in development mode.

         * */
        function change_defaults($defaults) {
            $defaults['str_replace'] = 'Testing filter hook!';

            return $defaults;
        }

        // Remove the demo link and the notice of integrated demo from the redux-framework plugin
        function remove_demo() {

            // Used to hide the demo mode link from the plugin page. Only used when Redux is a plugin.
            if (class_exists('ReduxFrameworkPlugin')) {
                remove_filter('plugin_row_meta', array(ReduxFrameworkPlugin::instance(), 'plugin_metalinks'), null, 2);

                // Used to hide the activation notice informing users of the demo panel. Only used when Redux is a plugin.
                remove_action('admin_notices', array(ReduxFrameworkPlugin::instance(), 'admin_notices'));
            }
        }

        public function setSections() {

            /**
              Used within different fields. Simply examples. Search for ACTUAL DECLARATION for field examples
             * */
            // Background Patterns Reader
            $sample_patterns_path   = ReduxFramework::$_dir . '../sample/patterns/';
            $sample_patterns_url    = ReduxFramework::$_url . '../sample/patterns/';
            $sample_patterns        = array();

            if (is_dir($sample_patterns_path)) :

                if ($sample_patterns_dir = opendir($sample_patterns_path)) :
                    $sample_patterns = array();

                    while (( $sample_patterns_file = readdir($sample_patterns_dir) ) !== false) {

                        if (stristr($sample_patterns_file, '.png') !== false || stristr($sample_patterns_file, '.jpg') !== false) {
                            $name = explode('.', $sample_patterns_file);
                            $name = str_replace('.' . end($name), '', $sample_patterns_file);
                            $sample_patterns[]  = array('alt' => $name, 'img' => $sample_patterns_url . $sample_patterns_file);
                        }
                    }
                endif;
            endif;

            ob_start();

            $ct             = wp_get_theme();
            $this->theme    = $ct;
            $item_name      = $this->theme->get('Name');
            $tags           = $this->theme->Tags;
            $screenshot     = $this->theme->get_screenshot();
            $class          = $screenshot ? 'has-screenshot' : '';

            $customize_title = sprintf(__('Customize &#8220;%s&#8221;', 'redux-framework-demo'), $this->theme->display('Name'));
            
            ?>
            <div id="current-theme" class="<?php echo esc_attr($class); ?>">
            <?php if ($screenshot) : ?>
                <?php if (current_user_can('edit_theme_options')) : ?>
                        <a href="<?php echo wp_customize_url(); ?>" class="load-customize hide-if-no-customize" title="<?php echo esc_attr($customize_title); ?>">
                            <img src="<?php echo esc_url($screenshot); ?>" alt="<?php esc_attr_e('Current theme preview'); ?>" />
                        </a>
                <?php endif; ?>
                    <img class="hide-if-customize" src="<?php echo esc_url($screenshot); ?>" alt="<?php esc_attr_e('Current theme preview'); ?>" />
                <?php endif; ?>

                <h4><?php echo $this->theme->display('Name'); ?></h4>

                <div>
                    <ul class="theme-info">
                        <li><?php printf(__('By %s', 'redux-framework-demo'), $this->theme->display('Author')); ?></li>
                        <li><?php printf(__('Version %s', 'redux-framework-demo'), $this->theme->display('Version')); ?></li>
                        <li><?php echo '<strong>' . __('Tags', 'redux-framework-demo') . ':</strong> '; ?><?php printf($this->theme->display('Tags')); ?></li>
                    </ul>
                    <p class="theme-description"><?php echo $this->theme->display('Description'); ?></p>
            <?php
            if ($this->theme->parent()) {
                printf(' <p class="howto">' . __('This <a href="%1$s">child theme</a> requires its parent theme, %2$s.') . '</p>', __('http://codex.wordpress.org/Child_Themes', 'redux-framework-demo'), $this->theme->parent()->display('Name'));
            }
            ?>

                </div>
            </div>

            <?php
            $item_info = ob_get_contents();

            ob_end_clean();

            $sampleHTML = '';
            if (file_exists(dirname(__FILE__) . '/info-html.html')) {
                /** @global WP_Filesystem_Direct $wp_filesystem  */
                global $wp_filesystem;
                if (empty($wp_filesystem)) {
                    require_once(ABSPATH . '/wp-admin/includes/file.php');
                    WP_Filesystem();
                }
                $sampleHTML = $wp_filesystem->get_contents(dirname(__FILE__) . '/info-html.html');
            }

                        

            // ACTUAL DECLARATION OF SECTIONS
            $this->sections[] = array(
                'icon'      => 'el-icon-cogs',
                'title'     => __('General Settings', 'redux-framework'),
                'fields'    => array(

                    array(
                        'id'        => 'favicon',
                        'type'      => 'media',
                        'desc'      => __('Image should be ico format and 16x16 size ( Exp: favicon.ico ) ','redux-framework'),
                        'title'     => __('Favicon', 'redux-framework'),                       
                        'subtitle'  => __('Upload your custom favicon.', 'redux-framework'),
                    ),
                    
                    array(
                        'id'        => 'opt-ace-editor-css',
                        'type'      => 'ace_editor',
                        'title'     => __('CSS Code', 'redux-framework'),
                        'subtitle'  => __('Paste your CSS code here.', 'redux-framework'),
                        'mode'      => 'css',
                        'theme'     => 'monokai',
                        'desc'      => 'Possible modes can be found at <a href="http://ace.c9.io" target="_blank">http://ace.c9.io/</a>.',
                        'default'   => ""
                    ),
                                        
                    array(
                        'id'        => 'opt-editor',
                        'type'      => 'editor',
                        'title'     => __('Footer Text', 'redux-framework'),
                        'subtitle'  => __('Text will be displayed left side of footer', 'redux-framework'),
                        'default'  => get_bloginfo('name'),
                    ),

                    array(
                        'id'       => 'opt-switch',
                        'type'     => 'switch', 
                        'title'    => __('Credit Links', 'redux-framework-demo'),
                        'subtitle' => __('Links : wordpress.org and burak-aydin.com', 'redux-framework-demo'),
                        'default'  => false,
                    )


                )
            );

             $this->sections[] = array(
                            'type' => 'divide',
                        );

            $this->sections[] = array(
                'title'     => __('Home Settings', 'redux-framework'),
                
                'icon'      => 'el-icon-home',
                // 'submenu' => false, // Setting submenu to false on a given section will hide it from the WordPress sidebar menu!
                'fields'    => array(

                    array(
                        'id'        => 'logo',
                        'type'      => 'media',                        
                        'title'     => __('Logo', 'redux-framework'),   
                        'desc'      => __('Theme title and description will disappear when uploaded','redux-framework'),                  
                        'subtitle'  => __('Size should be 180x50 ( It can be 360x100 for retina screens too )', 'redux-framework'),

                    ),

                     array(
                        'id' => 'categories-1',
                        'type' => 'select',
                        'data' => 'categories',
                        'title' => __('Featured Category 1', 'redux-framework'),
                       
                    ),
                     array(
                        'id' => 'categories-2',
                        'type' => 'select',
                        'data' => 'categories',
                        'title' => __('Featured Category 2', 'redux-framework'),
                       
                    ), 

                     array(
                        'id' => 'categories-3',
                        'type' => 'select',
                        'data' => 'categories',
                        'title' => __('Featured Category 3', 'redux-framework'),
                       
                    ), 

                     array(
                        'id' => 'categories-4',
                        'type' => 'select',
                        'data' => 'categories',
                        'title' => __('Featured Category 4', 'redux-framework'),
                       
                    ),               
                   
                ),
            );

           

           


            $theme_info  = '<div class="redux-framework-section-desc">';
            $theme_info .= '<p class="redux-framework-theme-data description theme-uri">' . __('<strong>Theme URL:</strong> ', 'redux-framework') . '<a href="' . $this->theme->get('ThemeURI') . '" target="_blank">' . $this->theme->get('ThemeURI') . '</a></p>';
            $theme_info .= '<p class="redux-framework-theme-data description theme-author">' . __('<strong>Author:</strong> ', 'redux-framework') . $this->theme->get('Author') . '</p>';
            $theme_info .= '<p class="redux-framework-theme-data description theme-version">' . __('<strong>Version:</strong> ', 'redux-framework') . $this->theme->get('Version') . '</p>';
            $theme_info .= '<p class="redux-framework-theme-data description theme-description">' . $this->theme->get('Description') . '</p>';
            $tabs = $this->theme->get('Tags');
            if (!empty($tabs)) {
                $theme_info .= '<p class="redux-framework-theme-data description theme-tags">' . __('<strong>Tags:</strong> ', 'redux-framework') . implode(', ', $tabs) . '</p>';
            }
            $theme_info .= '</div>';

           
            
          
            $this->sections[] = array(
                'title'     => __('Import / Export', 'redux-framework'),
                'desc'      => __('Import and Export your framework settings from file, text or URL.', 'redux-framework'),
                'icon'      => 'el-icon-refresh',
                'fields'    => array(
                    array(
                        'id'            => 'opt-import-export',
                        'type'          => 'import_export',
                        'title'         => 'Import Export',
                        'subtitle'      => 'Save and restore your Redux options',
                        'full_width'    => false,
                    ),
                ),
            );                     
                    
            $this->sections[] = array(
                'type' => 'divide',
            );

            $this->sections[] = array(
                'icon'      => 'el-icon-info-sign',
                'title'     => __('Theme Information', 'redux-framework'),
                'desc'      => __('<p class="description">This is the Description. Again HTML is allowed</p>', 'redux-framework'),
                'fields'    => array(
                    array(
                        'id'        => 'opt-raw-info',
                        'type'      => 'raw',
                        'content'   => $item_info,
                    )
                ),
            );

            
        }

        public function setHelpTabs() {

            // Custom page help tabs, displayed using the help API. Tabs are shown in order of definition.
            $this->args['help_tabs'][] = array(
                'id'        => 'redux-help-tab-1',
                'title'     => __('Flower', 'redux-framework-demo'),
                'content'   => __('<p>If you have any issue about the theme, please contact with me.</p>', 'redux-framework-demo')
            );
          
        }

        /**

          All the possible arguments for Redux.
          For full documentation on arguments, please refer to: https://github.com/ReduxFramework/ReduxFramework/wiki/Arguments

         * */
        public function setArguments() {

            $theme = wp_get_theme(); // For use with some settings. Not necessary.

            $this->args = array (
  'opt_name' => 'flower',
  'global_variable' => 'flower',
  'admin_bar' => '1',
  'allow_sub_menu' => '1',
  'default_mark' => '*',
  'footer_text' => 'Theme by Burak Aydin',
  'hints' => 
  array (
    'icon' => 'el-icon-question-sign',
    'icon_position' => 'right',
    'icon_color' => '#848484',
    'icon_size' => 'normal',
    'tip_style' => 
    array (
      'color' => 'dark',
      'style' => 'youtube',
    ),
    'tip_position' => 
    array (
      'my' => 'top left',
      'at' => 'bottom right',
    ),
    'tip_effect' => 
    array (
      'show' => 
      array (
        'effect' => 'slide',
        'duration' => '100',
        'event' => 'mouseover',
      ),
      'hide' => 
      array (
        'effect' => 'slide',
        'duration' => '100',
        'event' => 'mouseleave unfocus',
      ),
    ),
  ),
  'intro_text' => 'Theme by Burak Aydin',
  'last_tab' => '1',
  'menu_title' => 'Flower Options',
  'menu_type' => 'submenu',
  'output' => '1',
  'output_tag' => '1',
  'page_icon' => 'icon-themes',
  'page_parent_post_type' => 'your_post_type',
  'page_priority' => '61',
  'page_permissions' => 'manage_options',
  'page_slug' => '_options',
  'page_title' => 'Flower Options',
  'save_defaults' => '1',
);

            $theme = wp_get_theme(); // For use with some settings. Not necessary.
            $this->args["display_name"] = $theme->get("Name");
            $this->args["display_version"] = $theme->get("Version");

            // SOCIAL ICONS -> Setup custom links in the footer for quick links in your panel footer icons.

            $this->args['share_icons'][] = array(
                'url'   => 'http://burak-aydin.com/',
                'title' => 'My Personal Page',
                'icon'  => 'el-icon-website-alt'
            );            
            $this->args['share_icons'][] = array(
                'url'   => 'https://www.behance.net/buraksdu',
                'title' => 'Follow me on Behance',
                'icon'  => 'el-icon-behance'
            );
            $this->args['share_icons'][] = array(
                'url'   => 'https://twitter.com/buraksdu',
                'title' => 'Follow me on Twitter',
                'icon'  => 'el-icon-twitter'
            );
            

            }

    }
    
    global $reduxConfig;
    $reduxConfig = new flower_Redux_Framework_config();
}

/**
  Custom function for the callback referenced above
 */
if (!function_exists('flower_my_custom_field')):
    function flower_my_custom_field($field, $value) {
        print_r($field);
        echo '<br/>';
        print_r($value);
    }
endif;

?>