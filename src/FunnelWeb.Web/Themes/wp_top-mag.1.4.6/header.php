<?php
/**
 * The Header template.
 */
$top_mag_options = get_option( 'topmag_theme_options' );
?>
<!DOCTYPE html>
<!--[if IE 7]>
<html class="ie ie7" <?php language_attributes(); ?>>
<![endif]-->
<!--[if IE 8]>
<html class="ie ie8" <?php language_attributes(); ?>>
<![endif]-->
<!--[if !(IE 7) | !(IE 8)  ]><!-->
<html <?php language_attributes(); ?>>
<!--<![endif]-->
<head>
<meta charset="<?php bloginfo( 'charset' ); ?>">
<meta name="viewport" content="width=device-width">
<title>
<?php wp_title(); ?>
</title>
<link rel="profile" href="http://gmpg.org/xfn/11">
<link rel="pingback" href="<?php bloginfo( 'pingback_url' ); ?>">
<?php if(!empty($top_mag_options['favicon'])) { ?>
<link rel="shortcut icon" href="<?php echo esc_url($top_mag_options['favicon']);?>">
<?php } ?>
<?php wp_head(); ?>
</head>

<body <?php body_class(); ?>>
<!-- header -->
<header>
  <div class="container container-magzemine"> 
    <!-- LOGO AND TEXT -->
    <div class="col-md-12 no-padding ">
      <div class="col-md-4 header-logo">
        <?php if(empty($top_mag_options['logo'])) { echo '<div class="top-mag-site-name"><a href="'.esc_url( home_url( '/' ) ).'">'.get_bloginfo('name').'</a></div>';  } else { ?>
        <a href="<?php echo esc_url( home_url( '/' ) ); ?>" rel="home"><img src="<?php echo esc_url($top_mag_options['logo']); ?>" alt="<?php _e('site logo','top-mag'); ?>" class="img-responsive" /></a>
        <?php } ?>
        <p class="top-mag-tagline">
          <?php if(!empty($top_mag_options['logo-tagline'])) { echo get_bloginfo('description'); } ?>
        </p>
      </div>
     <div class="col-md-8 header-text"> 
		<?php if(!empty($top_mag_options['display-banner'])) { ?>
          <div class="custom-header-img">
            	<?php if(empty($top_mag_options['banner-html']) && !empty($top_mag_options['banner-ads'])) { 
						if(!empty($top_mag_options['banneradslink'])) {	?>
                		<a href="<?php echo esc_url($top_mag_options['banneradslink']); ?>" target="_blank"><img width="860" height="90" src="<?php echo $top_mag_options['banner-ads']; ?>" class="img-responsive" /></a>
                <?php } else { ?>
                		<img width="860" height="90" src="<?php echo esc_url($top_mag_options['banner-ads']); ?>" class="img-responsive" />
				<?php } 
				} else { 
					echo esc_html($top_mag_options['banner-html']); 
		  } ?>
          </div>
        <?php } ?>
      </div>
    </div>
    <!-- END LOGO AND TEXT --> 
    <!-- MENU -->
    <div class="col-md-12 no-padding">
      <nav role="navigation" class="navbar navbar-default"> 
        <!-- Brand and toggle get grouped for better mobile display -->
        <div class="navbar-header">
          <button type="button" data-target="#navbarCollapse" data-toggle="collapse" class="navbar-toggle"> <span class="sr-only">
          <?php _e('Toggle navigation','top-mag'); ?>
          </span> <span class="icon-bar"></span> <span class="icon-bar"></span> <span class="icon-bar"></span> </button>
          <a href="<?php echo site_url(); ?>" class="home-icon"><img src="<?php echo get_template_directory_uri(); ?>/images/home.png" alt="home icon" /></a> </div>
        <!-- Collection of nav links and other content for toggling -->
        <div id="navbarCollapse" class="collapse navbar-collapse menu">
          <?php
				$top_mag_menu_args = array(
							'theme_location'  => 'primary',
							'container'       => 'div',
							'container_class' => '',
							'container_id'    => '',
							'menu_class'      => 'menu-header-menu-container',
							'menu_id'         => '',
							'echo'            => true,
							'fallback_cb'     => 'wp_page_menu',
							'before'          => '',
							'after'           => '',
							'link_before'     => '',
							'link_after'      => '',
							'items_wrap'      => '<ul class="nav navbar-nav top-mag-menu">%3$s</ul>',
							'depth'           => 0,
				);
				
				wp_nav_menu( $top_mag_menu_args );
			
		?>
        </div>
      </nav>
    </div>
    <!-- END MENU --> 
  </div>
</header>
<!-- End header -->
<div class="container container-magzemine no-padding">
<?php top_mag_breaking_news(); ?>
