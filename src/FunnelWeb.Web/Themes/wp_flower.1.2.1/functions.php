<?php


function flower_setup() {

	global $content_width;	
	if ( ! isset( $content_width ) ){
		$content_width = 745;
	}
	
	load_theme_textdomain( 'flower', get_template_directory() . '/lang' );
	
	add_theme_support( 'automatic-feed-links' );

	add_theme_support( 'html5', array( 'search-form', 'comment-form', 'comment-list' ) );
	

	add_theme_support('custom-background');

	add_theme_support( 'post-thumbnails' );


	register_nav_menus(array(
		'top-menu' => __( 'Top Menu', 'flower' ),
		'top-menu-mobile' => __( 'Top Menu Mobile', 'flower' ),
		'middle-menu' => __( 'Middle Menu', 'flower' ),		
		'footer-menu' => __('Footer Menu', 'flower')
		));
	
}
add_action( 'after_setup_theme', 'flower_setup' );



function flower_widgets(){

		

	register_sidebar(array(
		'id'          => 'sidebar',
	    'name'        => __( 'Right Sidebar', 'flower' ),
	    'description' => __( 'This widget is located right side.', 'flower' ),
	    'before_widget' => '<div class="widget">',
		'after_widget'  => '</div>',
		'before_title'  => '<h3>',
		'after_title'   => '</h3>',
		));


	register_sidebar(array(
		'id'          => 'footer-1',
	    'name'        => __( 'Left Footer', 'flower' ),
	    'description' => __( 'This widget is located left side.', 'flower' ),
	    'before_widget' => '<div class="footer-widget">',
		'after_widget'  => '</div>',
		'before_title'  => '<h3>',
		'after_title'   => '</h3>',
		));


	register_sidebar(array(
		'id'          => 'footer-2',
	    'name'        => __( 'Middle Footer', 'flower' ),
	    'description' => __( 'This widget is located middle.', 'flower' ),
	    'before_widget' => '<div class="footer-widget">',
		'after_widget'  => '</div>',
		'before_title'  => '<h3>',
		'after_title'   => '</h3>',
		));


	register_sidebar(array(
		'id'          => 'footer-3',
	    'name'        => __( 'Right Footer', 'flower' ),
	    'description' => __( 'This widget is located right side.', 'flower' ),
	    'before_widget' => '<div class="footer-widget">',
		'after_widget'  => '</div>',
		'before_title'  => '<h3>',
		'after_title'   => '</h3>',
		));


}

add_action('widgets_init','flower_widgets');


function flower_menu_page(){ ?>

	<nav class="col-sm-9" role="navigation">				
		<ul class="nav navbar-nav pull-left">
			<?php wp_list_pages(array(
				'title_li' => '',
				'depth' => 3
			)); ?>												
		</ul>				
	</nav><!-- end col-sm-9 -->
	
<?php }

function flower_menu_page_two(){ ?>

	<nav class="col-sm-12" role="navigation">				
		<ul class="nav navbar-nav pull-left">
			<?php wp_list_pages(array(
				'title_li' => '',
				'depth' => 3
			)); ?>												
		</ul>				
	</nav><!-- end col-sm-9 -->
	
<?php }


function flower_title($title){

	$name=get_bloginfo('title');

	$desc=get_bloginfo('description');

	$title.=strip_tags($name).' | '.strip_tags($desc);

	return $title;

}

add_filter('wp_title','flower_title');



function flower_menu_footer_page(){ ?>

	<nav class="footer-menu col-sm-9" role="navigation">				
		<ul class="nav navbar-nav pull-right">
			<?php wp_list_pages(array(
				'title_li' => '',
				'depth' => 1
			)); ?>												
		</ul>				
	</nav><!-- end col-sm-9 -->
	
<?php }


function flower_scripts_styles() {
	
	if ( is_singular() && comments_open() && get_option( 'thread_comments' ) )
		wp_enqueue_script( 'comment-reply' );	
	

	wp_enqueue_script('smoothscroll',get_template_directory_uri().'/js/SmoothScroll.js',array('jquery'),'',true);

	wp_enqueue_script('flexslider',get_template_directory_uri().'/js/jquery.flexslider-min.js',array('jquery'),'',true);

	wp_enqueue_script('fitvid',get_template_directory_uri().'/js/jquery.fitvids.js',array('jquery'),'',true);	

	wp_enqueue_script('flower-custom',get_template_directory_uri().'/js/flower-custom.js',array('jquery'),'',true);


	wp_enqueue_style( 'flower-style', get_stylesheet_uri(), array(), '' );
	
}
add_action( 'wp_enqueue_scripts', 'flower_scripts_styles' );


function flower_load_fonts() {

	wp_register_style('googleFontsDroid','//fonts.googleapis.com/css?family=Droid+Sans');
    wp_enqueue_style( 'googleFontsDroid'); 

    wp_register_style('googleFontsNoto','//fonts.googleapis.com/css?family=Noto+Sans:400,700');
    wp_enqueue_style( 'googleFontsNoto');

}
add_action('wp_print_styles', 'flower_load_fonts');


function flower_excerpt_length( $length ) {
	return 45;
}
add_filter( 'excerpt_length', 'flower_excerpt_length');



function flower_excerpt_more( $more ) {
	return ' .....';
}
add_filter('excerpt_more', 'flower_excerpt_more');




if ( ! function_exists( 'flower_pagenavi' ) ) :
    function flower_pagenavi(){
    global $wp_query;

    $big = 999999999; ?>

	<div class="flower-pagination">
		<?php 
					echo paginate_links( array(
		      'base' => str_replace( $big, '%#%', esc_url( get_pagenum_link( $big ) ) ),
		      'format' => '?paged=%#%',
		      'current' => max( 1, get_query_var('paged') ),
		      'total' => $wp_query->max_num_pages
		    ) );
		 ?>
	</div>
    

<?php   }
endif;



function flower_custom_comment_form($defaults) {
	
	
	$defaults['comment_notes_before'] = '';	
	$defaults['id_form'] = 'comment-form';
	$defaults['comment_field'] = '<p><textarea name="comment" id="comment" class="form-control" rows="6"></textarea></p>';

	return $defaults;
}

add_filter('comment_form_defaults', 'flower_custom_comment_form');

function flower_custom_comment_fields() {
	$commenter = wp_get_current_commenter();
	$req = get_option('require_name_email');
	$aria_req = ($req ? " aria-required='true'" : '');
	
	$fields = array(
		'author' => '<p>' . 
						'<input id="author" name="author" type="text" class="form-control" placeholder="Name ( required )" value="' . esc_attr($commenter['comment_author']) . '" ' . $aria_req . ' />' .
						
		            '</p>',
		'email' => '<p>' . 
						'<input id="email" name="email" type="text" class="form-control" placeholder="Email ( required )" value="' . esc_attr($commenter['comment_author_email']) . '" ' . $aria_req . ' />'  .
		            '</p>',
		'url' => '<p>' . 
						'<input id="url" name="url" type="text" class="form-control" placeholder="Website" value="' . esc_attr($commenter['comment_author_url']) . '" />'  .
		            '</p>'
	);

	return $fields;
}

add_filter('comment_form_default_fields', 'flower_custom_comment_fields');



define( 'ACF_LITE' , true );



if(function_exists("register_field_group"))
{
	register_field_group(array (
		'id' => 'acf_top-slider-options',
		'title' => __('Top Slider Options','flower'),
		'fields' => array (
			array (
				'key' => 'field_5359085210be8',
				'label' => __('Slider Options','flower'),
				'name' => 'slider_options',
				'type' => 'true_false',
				'instructions' => __('Featued Slider Post ?','flower'),
				'message' => __('Featured Slider for this post ?','flower'),
				'default_value' => 0,
			),
			array (
				'key' => 'field_5354f8b88dcb9',
				'label' => __('Slider Image','flower'),
				'name' => 'slider_image',
				'type' => 'image',
				'instructions' => __('Ideal slider size should be	750 x 400. ( for retina screens : 1500 x 800 ).','flower'),
				'save_format' => 'object',
				'preview_size' => 'thumbnail',
				'library' => 'all',
			),
		),
		'location' => array (
			array (
				array (
					'param' => 'post_type',
					'operator' => '==',
					'value' => 'post',
					'order_no' => 0,
					'group_no' => 0,
				),
			),
		),
		'options' => array (
			'position' => 'normal',
			'layout' => 'default',
			'hide_on_screen' => array (
				0 => 'custom_fields',
			),
		),
		'menu_order' => 0,
	));
}






function flower_head(){ 

	global $flower;

	if(isset($flower['favicon']['url'])){

		if($flower['favicon']['url']){

			echo '<link rel="shortcut icon" href="'.$flower['favicon']['url'].'">';	

		}

	}

	if(isset($flower)){

		if($flower['opt-ace-editor-css']){

			echo '<style>'.$flower['opt-ace-editor-css'].'</style>';

		}
		
	}

 }

add_action('wp_head','flower_head');





/************************************************************************
// TGM
*************************************************************************/
require get_template_directory() . '/admin/admin-init.php';