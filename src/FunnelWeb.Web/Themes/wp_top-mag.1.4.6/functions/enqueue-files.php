<?php 
/*
 * topmag Enqueue css and js files
*/
function top_mag_enqueue()
{
	wp_enqueue_style('bootstrap',get_template_directory_uri().'/css/bootstrap.css',array());
	wp_enqueue_style('tickercss',get_template_directory_uri().'/css/ticker-style.css',array());
	wp_enqueue_style('owl-carousel-css',get_template_directory_uri().'/css/owl.carousel.css',array());
	wp_enqueue_style('googleFonts', '//fonts.googleapis.com/css?family=Lato');	
	wp_enqueue_style('style',get_stylesheet_uri());
	wp_enqueue_style('font-awesome',get_template_directory_uri().'/css/font-awesome.css',array());
	wp_enqueue_script('bootstrapjs',get_template_directory_uri().'/js/bootstrap.js',array('jquery'));	
	wp_enqueue_script('owl-carousel-js',get_template_directory_uri().'/js/owl.carousel.min.js',array('jquery'));
	wp_enqueue_script('tickerjs',get_template_directory_uri().'/js/jquery.ticker.js',array('jquery'));
	if ( is_page_template( 'page-templates/home-slider.php' ) ) {
		wp_enqueue_script('jssor-core',get_template_directory_uri().'/js/jssor.core.js',array('jquery'));
		wp_enqueue_script('jssor-utils',get_template_directory_uri().'/js/jssor.utils.js',array('jquery'));	
		wp_enqueue_script('jssor-slider',get_template_directory_uri().'/js/jssor.slider.js',array('jquery'));
	}		
	wp_enqueue_script('default',get_template_directory_uri().'/js/default.js',array('jquery'));
	if ( is_singular() ) wp_enqueue_script( "comment-reply" ); 
	if(preg_match('/(?i)msie [1-8]/',$_SERVER['HTTP_USER_AGENT']))
		 wp_enqueue_script('respondminjs', get_template_directory_uri().'/js/respond.min.js',array('jquery'), '', true );
}
add_action('wp_enqueue_scripts', 'top_mag_enqueue');
