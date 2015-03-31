<?php
function top_mag_theme_options_init(){
 register_setting( 'top_mag_options', 'topmag_theme_options','top_mag_options_validate');
} 
add_action( 'admin_init', 'top_mag_theme_options_init' );
function top_mag_options_validate($input)
{
	$input['breaking-news'] = sanitize_text_field( $input['breaking-news'] );
	$input['breaking-news-category'] = sanitize_text_field( $input['breaking-news-category'] );
 	$input['logo'] = top_mag_image_validation(esc_url_raw( $input['logo'] ));
	$input['logo-tagline'] = sanitize_text_field( $input['logo-tagline'] );
	$input['favicon'] = top_mag_image_validation(esc_url_raw( $input['favicon'] ));
	
	$input['footertext'] = sanitize_text_field( $input['footertext'] );
	
	$input['post-slider-category'] = sanitize_text_field( $input['post-slider-category'] );
	$input['recent-post-number'] = sanitize_text_field( $input['recent-post-number'] );
	$input['home-post-category'] = sanitize_text_field( $input['home-post-category'] );
	$input['post-number'] = sanitize_text_field( $input['post-number'] );	
	
	$input['banner-ads'] = esc_url_raw( $input['banner-ads'] );
	$input['banneradslink'] = esc_url_raw( $input['banneradslink'] ); 
    return $input;
}
function top_mag_image_validation($top_mag_imge_url) {
  $top_mag_filetype = wp_check_filetype($top_mag_imge_url);

  $top_mag_supported_image = array('gif', 'jpg', 'jpeg', 'png', 'ico');

  if (in_array($top_mag_filetype['ext'], $top_mag_supported_image)) {
    return $top_mag_imge_url;
  } else {
    return '';
  }
}

function top_mag_theme_framework_load_scripts(){
	wp_enqueue_media();
	wp_enqueue_style( 'topmagtheme_framework', get_template_directory_uri(). '/theme-options/css/topmagtheme_framework.css' ,false, '1.0.0');
	// Enqueue custom option panel JS
	wp_enqueue_script( 'options-custom', get_template_directory_uri(). '/theme-options/js/topmagtheme-custom.js', array( 'jquery' ) );
	wp_enqueue_script( 'media-uploader', get_template_directory_uri(). '/theme-options/js/media-uploader.js', array( 'jquery') );		
}
add_action( 'admin_enqueue_scripts', 'top_mag_theme_framework_load_scripts' );
function top_mag_theme_framework_menu_settings() {
	$top_mag_menu = array(
				'page_title' => __('topmagtheme Options', 'top-mag'),
				'menu_title' => __('Theme Options', 'top-mag'),
				'capability' => 'edit_theme_options',
				'menu_slug' => 'top_mag_theme_framework',
				'callback' => 'top_mag_framework_page'
				);
	return apply_filters( 'top_mag_theme_framework_menu', $top_mag_menu );
}
add_action( 'admin_menu', 'theme_options_add_page' ); 
function theme_options_add_page() {
	$top_mag_menu = top_mag_theme_framework_menu_settings();
   	add_theme_page($top_mag_menu['page_title'],$top_mag_menu['menu_title'],$top_mag_menu['capability'],$top_mag_menu['menu_slug'],$top_mag_menu['callback']);
} 
function top_mag_framework_page(){ 
		global $select_options; 
		if ( ! isset( $_REQUEST['settings-updated'] ) ) 
		$_REQUEST['settings-updated'] = false;		
		$top_mag_image=get_template_directory_uri().'/theme-options/images/logo.png';		
?>
<div class="topmagtheme-themes">
	<form method="post" action="options.php" id="form-option" class="theme_option_ft">
  <div class="topmagtheme-header">
    <div class="logo">
      <?php
		$top_mag_image=get_template_directory_uri().'/theme-options/images/logo.png';
		echo "<a href='http://fasterthemes.com' target='_blank'><img src='".$top_mag_image."' alt='FasterThemes' /></a>";
		?>
    </div>
    <div class="header-right">
      <?php
			echo "<h1>". __( 'Theme Options', 'top-mag' ) . "</h1>"; 			
			echo "<div class='btn-save'><input type='submit' class='button-primary' value='".__('Save Options','top-mag')."' /></div>";			
			?>
    </div>
  </div>
  <div class="topmagtheme-details">
    <div class="topmagtheme-options">
      <div class="right-box">
        <div class="nav-tab-wrapper">
          <ul>
            <li><a id="options-group-1-tab" class="nav-tab headersettings-tab" title="<?php _e('Header Settings','top-mag') ?>" href="#options-group-1"><?php _e('Header Settings','top-mag') ?></a></li>
            <li><a id="options-group-2-tab" class="nav-tab footersettings-tab" title="<?php _e('Footer Settings','top-mag') ?>" href="#options-group-2"><?php _e('Footer Settings','top-mag') ?></a></li>
            <li><a id="options-group-3-tab" class="nav-tab homepagesettings-tab" title="<?php _e('Home Page Settings','top-mag') ?>" href="#options-group-3"><?php _e('Home Page Settings','top-mag') ?></a></li>
            <li><a id="options-group-4-tab" class="nav-tab bannersettings-tab" title="<?php _e('Banner Settings','top-mag') ?>" href="#options-group-4"><?php _e('Banner Settings','top-mag') ?></a></li>
            <li><a id="options-group-5-tab" class="nav-tab prosettings-tab" title="<?php _e('PRO Theme Features','top-mag') ?>" href="#options-group-5"><?php _e('PRO Theme Features','top-mag') ?></a></li>
  		  </ul>
        </div>
      </div>
      <div class="right-box-bg"></div>
      <div class="postbox left-box"> 
        <!--======================== F I N A L - - T H E M E - - O P T I O N ===================-->
          <?php settings_fields( 'top_mag_options' );  
		$top_mag_options = get_option( 'topmag_theme_options' ); ?>
          <!-------------- First group ----------------->
          <div id="options-group-1" class="group faster-inner-tabs">
            <div class="section theme-tabs theme-colors theme-fonts">
            	<a href="javascript:void(0)" class="heading faster-inner-tab active"><?php _e('Display Breaking News','top-mag') ?></a>
              <div class="faster-inner-tab-group active">
                <div class="option-group">                  
                 	<div class="ft-control">
              <input type="checkbox" id="credit-news" name="topmag_theme_options[breaking-news]" <?php if(!empty($top_mag_options['breaking-news'])) { ?> checked="checked" <?php } ?> value="yes">
               <label class="breaking-news" for="credit-news"><?php _e('Please check checkbox to display Breaking News.','top-mag') ?></label>
                </div>
                </div>                
              </div>
            </div>
            <div class="section theme-tabs theme-email">
            	<a class="heading faster-inner-tab" href="javascript:void(0)"><?php _e('Select Breaking News Category','top-mag') ?></a>
              <div class="faster-inner-tab-group">
              	<div class="ft-control">
                <div class="explain"><?php _e('Please Select Category for Breaking News.','top-mag') ?></div>
                <?php $top_mag_terms = get_terms('category'); ?>
                <select class="of-input" name="topmag_theme_options[breaking-news-category]">
                    <option value="">--<?php _e('Select Category','top-mag') ?>--</option>
					<?php foreach($top_mag_terms as $top_mag_news_term) { ?>
                    <option value="<?php echo $top_mag_news_term->name; ?>" <?php if(!empty($top_mag_options['breaking-news-category']) && $top_mag_options['breaking-news-category'] == $top_mag_news_term->name) { ?> selected="selected" <?php } ?>><?php echo $top_mag_news_term->name; ?></option>
                    <?php } ?>
                </select>
                </div>                
              </div>
            </div>
            <div class="section theme-tabs theme-logo">
            <a class="heading faster-inner-tab" href="javascript:void(0)"><?php _e('Site Logo','top-mag') ?></a>
            <div class="faster-inner-tab-group">
              	<div class="ft-control">
                <input id="logo-img" class="upload" type="text" name="topmag_theme_options[logo]" 
                            value="<?php if(!empty($top_mag_options['logo'])) { echo esc_url($top_mag_options['logo']); } ?>" placeholder="<?php _e('No file chosen','top-mag') ?>" />
                <input id="1upload_image_button" class="upload-button button" type="button" value="<?php _e('Upload','top-mag') ?>" />
                <div class="screenshot" id="logo-image">
                  <?php if(!empty($top_mag_options['logo'])) { echo "<img src='".esc_url($top_mag_options['logo'])."' /><a class='remove-image'></a>"; } ?>
                </div>
              </div>
              
            </div>
          </div>
          	<div class="section theme-tabs theme-colors theme-fonts">
            	<a class="heading faster-inner-tab" href="javascript:void(0)"><?php _e('Display Tagline','top-mag') ?></a>
              <div class="faster-inner-tab-group">
                <div class="option-group">                  
                 	<div class="ft-control">
              <input type="checkbox" id="credit-tagline" name="topmag_theme_options[logo-tagline]" <?php if(!empty($top_mag_options['logo-tagline'])) { ?> checked="checked" <?php } ?> value="yes">
               <label class="tagline" for="credit-tagline"><?php _e('Please check checkbox to display tagline in header.','top-mag') ?></label>
                </div>
                </div>                
              </div>
            </div>
            <div class="section theme-tabs theme-favicon">
              <a class="heading faster-inner-tab" href="javascript:void(0)"><?php _e('Favicon','top-mag') ?></a>
              <div class="faster-inner-tab-group">
              	<div class="explain"><?php _e('Size of favicon should be exactly 32x32px for best results.','top-mag') ?></div>
                <div class="ft-control">
                  <input id="favicon-img" class="upload" type="text" name="topmag_theme_options[favicon]" 
                            value="<?php if(!empty($top_mag_options['favicon'])) { echo esc_url($top_mag_options['favicon']); } ?>" placeholder="<?php _e('No file chosen','top-mag') ?>" />
                  <input id="upload_image_button1" class="upload-button button" type="button" value="<?php _e('Upload','top-mag') ?>" />
                  <div class="screenshot" id="favicon-image">
                    <?php  if(!empty($top_mag_options['favicon'])) { echo "<img src='".esc_url($top_mag_options['favicon'])."' /><a class='remove-image'></a>"; } ?>
                  </div>
                </div>
                
              </div>
            </div>
          </div>          
          <!-------------- Second group ----------------->
          <div id="options-group-2" class="group faster-inner-tabs">   
            <div id="section-footertext2" class="section theme-tabs">
            	<a class="heading faster-inner-tab" href="javascript:void(0)"><?php _e('Copyright Text','top-mag') ?></a>
              <div class="faster-inner-tab-group active">
              	<div class="ft-control">
              		<div class="explain"><?php _e('Some text regarding copyright of your site, you would like to display in the footer.','top-mag') ?></div>                
                  	<input type="text" id="footertext2" class="of-input" name="topmag_theme_options[footertext]" size="32"  value="<?php if(!empty($top_mag_options['footertext'])) { echo esc_attr($top_mag_options['footertext']); } ?>">
                </div>                
              </div>
            </div>
          </div>          
          <!-------------- Third group ----------------->
          <div id="options-group-3" class="group faster-inner-tabs">            
            <div class="section theme-tabs theme-email">
            	<a class="heading faster-inner-tab active" href="javascript:void(0)"><?php _e('Select Post Slider Category','top-mag') ?></a>
              <div class="faster-inner-tab-group active">
              	<div class="ft-control">
                <div class="explain"><?php _e('Select Post Slider Category','top-mag') ?></div>
                <?php $top_mag_terms = get_terms('category'); ?>
                <select class="of-input" name="topmag_theme_options[post-slider-category]">
                    
				<?php 
				$top_mag_args = array(
				'meta_query' => array(
									array(
									'key' => '_thumbnail_id',
									'compare' => 'EXISTS'
										),
									)
								);  
				$top_mag_post = new WP_Query( $top_mag_args );
				$top_mag_cat_id=array();
				while($top_mag_post->have_posts()){
				$top_mag_post->the_post();
				$top_mag_post_categories = wp_get_post_categories( get_the_id());   
				$top_mag_cat_id[]=$top_mag_post_categories[0];
				}
				$top_mag_cat_id=array_unique($top_mag_cat_id);
				$top_mag_args = array(
				'orderby' => 'name',
				'parent' => 0,
				'include'=>$top_mag_cat_id
				);
				
				$top_mag_categories = get_categories($top_mag_args); 
			
                  foreach ($top_mag_categories as $top_mag_category) {
					  if($top_mag_category->term_id == $top_mag_options['post-category'])
					  	$top_mag_selected="selected=selected";
					  else
					  	$top_mag_selected='';
                    $top_mag_option = '<option value="'.$top_mag_category->term_id .'" '.$top_mag_selected.'>';
                    $top_mag_option .= $top_mag_category->cat_name;
                    $top_mag_option .= '</option>';
                    echo $top_mag_option;
                  }
                 ?>
                </select>
                </div>                
              </div>
            </div>
            <div class="section theme-tabs section-recent-post-slider">
            	<a class="heading faster-inner-tab" href="javascript:void(0)"><?php _e('Select recent post slider number','top-mag') ?></a>
              <div class="faster-inner-tab-group">
              	<div class="ft-control">
                <div class="explain"><?php _e('Please select number of post','top-mag') ?></div>
                 <select id="recent-post-number" class="of-input"  name="topmag_theme_options[recent-post-number]">
              		<option value="">--<?php _e('select number','top-mag') ?>--</option>
                	<?php for($recent_post_number = 1; $recent_post_number <= 20; $recent_post_number++ ) { ?>
                	<option value="<?php echo $recent_post_number; ?>"<?php if($top_mag_options['recent-post-number'] == $recent_post_number) { echo 'selected="selected"'; } ?>><?php echo $recent_post_number; ?></option>
					<?php } ?>
              </select>
                </div>                
              </div>
            </div>
            <div class="section theme-tabs theme-post">
            	<a class="heading faster-inner-tab" href="javascript:void(0)"><?php _e('Select post category','top-mag') ?></a>
              <div class="faster-inner-tab-group">
              	<div class="ft-control">
                <div class="explain"><?php _e('Please select category for post slider.','top-mag') ?></div>
                <?php $top_mag_terms = get_terms('category'); ?>
                <select class="of-input" name="topmag_theme_options[home-post-category]">
                    <option value="">--<?php _e('Select Category','top-mag') ?>--</option>
					<?php foreach($top_mag_terms as $top_mag_home_term) { ?>
                    <option value="<?php echo $top_mag_home_term->name; ?>" <?php if(!empty($top_mag_options['home-post-category']) && $top_mag_options['home-post-category'] == $top_mag_home_term->name) { ?> selected="selected" <?php } ?>><?php echo $top_mag_home_term->name; ?></option>
                    <?php } ?>
                </select>
                </div>                
              </div>
            </div>
            <div class="section theme-tabs section-recent-post-slider">
            	<a class="heading faster-inner-tab" href="javascript:void(0)"><?php _e('Select category post number','top-mag') ?></a>
              <div class="faster-inner-tab-group">
              	<div class="ft-control">
                <div class="explain"><?php _e('Please select number of post','top-mag') ?></div>
                 <select id="post-number" class="of-input"  name="topmag_theme_options[post-number]">
              		<option value="">--<?php _e('select number','top-mag') ?>--</option>
                	<?php for($post_number = 1; $post_number <= 20; $post_number++ ) { ?>
                	<option value="<?php echo $post_number; ?>"<?php if($top_mag_options['post-number'] == $post_number) { echo 'selected="selected"'; } ?>><?php echo $post_number; ?></option>
					<?php } ?>
              </select>
                </div>                
              </div>
            </div>
          </div>    
          <!-------------- Fourth group ----------------->
          <div id="options-group-4" class="group faster-inner-tabs">  
			  <div class="section theme-tabs theme-colors theme-fonts">
            	<a href="javascript:void(0)" class="heading faster-inner-tab active"><?php _e('Settings of Top Banner header (Right Hand Side of the Logo)','top-mag') ?></a>
              <div class="faster-inner-tab-group active">
                <div class="option-group">                  
                 	<div class="ft-control">
              <input type="checkbox" id="display-banner" name="topmag_theme_options[display-banner]" <?php if(!empty($top_mag_options['display-banner'])) { ?> checked="checked" <?php } ?> value="yes">
               <label class="display-banner" for="display-banner"><?php _e('Please check this checkbox if you want to display banner in the right hand side of the logo.','top-mag') ?></label>
                </div>
                </div>                
              </div>
            </div>
            <div id="section-banneradd" class="section theme-tabs theme-logo">
            	<a class="heading faster-inner-tab" href="javascript:void(0)"><?php _e('HTML Code for the Banner ad','top-mag') ?></a>
              <div class="faster-inner-tab-group">
              	<div class="ft-control">
              		 <textarea name="topmag_theme_options[banner-html]" class="of-input" id="banner-html"><?php if(!empty($top_mag_options['banner-html'])) { echo $top_mag_options['banner-html']; } ?></textarea>
                </div>  
                <h4 class="fasterthemes-or sub-heading"><?php _e('OR','top-mag') ?></h4>
               <h4 class="sub-heading"><?php _e('Image upload for the Banner ad (860px x 90px)','top-mag') ?></h4> 
                <div class="ft-control">
              		 <input id="banner-ads-image" class="upload" type="text" name="topmag_theme_options[banner-ads]" value="<?php if(!empty($top_mag_options['banner-ads'])) { echo esc_url($top_mag_options['banner-ads']); } ?>" placeholder="<?php _e('No file chosen','top-mag') ?>" />
               <input id="upload_image_button" class="upload-button button" type="button" value="<?php _e('Upload','top-mag') ?>" />
                    <div class="screenshot" id="logo-img">
                      <?php if(!empty($top_mag_options['banner-ads'])) {  echo "<img src='".esc_url($top_mag_options['banner-ads'])."' /><a class='remove-image'>Remove</a>"; } ?></div>
                </div>
                <div class="ft-control">      
               <input type="text" id="bannerlink" class="of-input" name="topmag_theme_options[banneradslink]" size="32" value="<?php if(!empty($top_mag_options['banneradslink'])) { echo esc_url($top_mag_options['banneradslink']); } ?>" placeholder="<?php _e('Banner Link','top-mag') ?>">
                </div>              
              </div>
            </div> 
		  </div>
          <!-------------- Fifth group ----------------->
          <div id="options-group-5" class="group faster-inner-tabs">            
			<div class="topmagtheme-pro-header">
              <img src="<?php echo get_template_directory_uri(); ?>/theme-options/images/theme-logo.png" class="topmagtheme-pro-logo" />
              <a href="http://fasterthemes.com/checkout/get_checkout_details?theme=TopMag" target="_blank"><img src="<?php echo get_template_directory_uri(); ?>/theme-options/images/buy-now.png" class="topmagtheme-pro-buynow" /></a>
              </div>
          	<img src="<?php echo get_template_directory_uri(); ?>/theme-options/images/pro_features.jpg" />
          </div>   
        <!--======================== F I N A L - - T H E M E - - O P T I O N S ===================--> 
      </div>
     </div>
	</div>
	<div class="topmagtheme-footer">
      	<ul>
            <li class="btn-save"><input type="submit" class="button-primary" value="<?php _e('Save Options','top-mag') ?>" /></li>
        </ul>
    </div>
    </form>    
</div>
<div class="save-options"><h2><?php _e('Options saved successfully.','top-mag') ?></h2></div>
<?php } ?>
