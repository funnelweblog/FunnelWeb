<?php
/*
 * global variable for get option.
*/
 $top_mag_options = get_option( 'topmag_theme_options' ); 
 global $top_mag_options;
/*
 * thumbnail list
*/ 
function top_mag_thumbnail_image($content) {

    if( has_post_thumbnail() )
         return the_post_thumbnail( 'thumbnail' ); 
}
/**
 * Register Lato Google font for topmag.
 */
function top_mag_font_url() {
	$top_mag_font_url = '';
	/*
	 * Translators: If there are characters in your language that are not supported
	 * by Lato, trantopmag this to 'off'. Do not trantopmag into your own language.
	 */
	if ( 'off' !== _x( 'on', 'Lato font: on or off', 'top-mag' ) ) {
		$top_mag_font_url = add_query_arg( 'family', urlencode( 'Lato:300,400,700,900,300italic,400italic,700italic' ), "//fonts.googleapis.com/css" );
	}

	return $top_mag_font_url;
}
/*
 * topmag Title
*/
function top_mag_wp_title( $title, $sep ) {
	global $paged, $page;

	if ( is_feed() ) {
		return $title;
	}

	// Add the site name.
	$title .= get_bloginfo( 'name', 'display' );

	// Add the site description for the home/front page.
	$top_mag_site_description = get_bloginfo( 'description', 'display' );
	if ( $top_mag_site_description && ( is_home() || is_front_page() ) ) {
		$title = "$title $sep $top_mag_site_description";
	}

	// Add a page number if necessary.
	if ( $paged >= 2 || $page >= 2 ) {
		$title = "$title $sep " . sprintf( __( 'Page %s', 'top-mag' ), max( $paged, $page ) );
	}

	return $title;
}
add_filter( 'wp_title', 'top_mag_wp_title', 10, 2 );

/**
 * Add default menu style if menu is not set from the backend.
 */
function top_mag_add_menuid ($top_mag_page_markup) {
preg_match('/^<div class=\"([a-z0-9-_]+)\">/i', $top_mag_page_markup, $top_mag_matches);
if(!empty($top_mag_matches)) { $top_mag_divclass = $top_mag_matches[1]; } else { $top_mag_divclass = ''; }
$top_mag_toreplace = array('<div class="'.$top_mag_divclass.'">', '</div>');
$top_mag_replace = array('<div class="menu-header-menu-container">', '</div>');
$top_mag_new_markup = str_replace($top_mag_toreplace,$top_mag_replace, $top_mag_page_markup);
$top_mag_new_markup= preg_replace('/<ul/', '<ul class="nav navbar-nav top-mag-menu"', $top_mag_new_markup);
return $top_mag_new_markup; }
add_filter('wp_page_menu', 'top_mag_add_menuid');

/*
 * topmag Main Sidebar
*/
function top_mag_widgets_init() {

	register_sidebar( array(
		'name'          => __( 'Main Sidebar', 'top-mag' ),
		'id'            => 'main-sidebar',
		'description'   => __( 'Main sidebar that appears on the left.', 'top-mag' ),
		'before_widget' => '<aside id="%1$s" class="widget %2$s">',
		'after_widget'  => '</aside>',
		'before_title'  => '<h1 class="widget-title">',
		'after_title'   => '</h1>',
	) );
	
	register_sidebar( array(
		'name'          => __( 'Footer area one', 'top-mag' ),
		'id'            => 'footer-area-one',
		'description'   => __( 'Footer area one that appears in the footer.', 'top-mag' ),
		'before_widget' => '<aside id="%1$s" class="widget %2$s">',
		'after_widget'  => '</aside>',
		'before_title'  => '<h1 class="widget-title">',
		'after_title'   => '</h1>',
	) );
	
	register_sidebar( array(
		'name'          => __( 'Footer area two', 'top-mag' ),
		'id'            => 'footer-area-two',
		'description'   => __( 'Footer area two that appears in the footer.', 'top-mag' ),
		'before_widget' => '<aside id="%1$s" class="widget %2$s">',
		'after_widget'  => '</aside>',
		'before_title'  => '<h1 class="widget-title">',
		'after_title'   => '</h1>',
	) );
	
	register_sidebar( array(
		'name'          => __( 'Footer area three', 'top-mag' ),
		'id'            => 'footer-area-three',
		'description'   => __( 'Footer area threee that appears in the footer.', 'top-mag' ),
		'before_widget' => '<aside id="%1$s" class="widget %2$s">',
		'after_widget'  => '</aside>',
		'before_title'  => '<h1 class="widget-title">',
		'after_title'   => '</h1>',
	) );			
}
add_action( 'widgets_init', 'top_mag_widgets_init' );

/*
 * topmag Set up post entry meta.
 *
 * Meta information for current post: categories, tags, permalink, author, and date.
 */
function top_mag_entry_meta() {

	$top_mag_category_list = get_the_category_list( ', ', 'top-mag' );

	$top_mag_tag_list = get_the_tag_list( ', ', 'top-mag' );

	$top_mag_date = sprintf( '<time datetime="%3$s">%4$s</time>',
		esc_url( get_permalink() ),
		esc_attr( get_the_time() ),
		esc_attr( get_the_date( 'c' ) ),
		esc_html( get_the_date() )
	);

	$top_mag_author = sprintf( '<span><a href="%1$s" title="%2$s" >%3$s</a></span>',
		esc_url( get_author_posts_url( get_the_author_meta( 'ID' ) ) ),
		esc_attr( sprintf( __( 'View all posts by %s', 'top-mag' ), get_the_author() ) ),
		get_the_author()
	);

	if ( $top_mag_tag_list ) {
			$top_mag_utility_text = '<span><i class="fa fa-folder-open"></i></span>'.' '.$top_mag_category_list.'&nbsp '.'<span><i class="fa fa-user"></i></span>'.' '.$top_mag_author.' ';
			echo '<span><i class="fa fa-comments-o"></i> '.get_comments_number().'&nbsp &nbsp';
		} elseif ( $top_mag_category_list ) {
			$top_mag_utility_text = '<span><i class="fa fa-folder-open"></i></span>'.' '.$top_mag_category_list.'&nbsp '.'<span><i class="fa fa-user"></i></span>'.' '.$top_mag_author.' ';
			echo '<span><i class="fa fa-comments-o"></i> '.get_comments_number().'&nbsp &nbsp';
		} else {
			$top_mag_utility_text = '<span><i class="fa fa-folder-open"></i></span>'.' '.$top_mag_category_list.'&nbsp '.'<span><i class="fa fa-user"></i></span>'.' '.$top_mag_author.' ';
			echo '<span><i class="fa fa-comments-o"></i> '.get_comments_number().'&nbsp &nbsp';
		}
	

	printf(
		$top_mag_utility_text,
		$top_mag_category_list,
		$top_mag_tag_list,
		$top_mag_date,
		$top_mag_author
	);
}

if ( ! function_exists( 'top_mag_comment' ) ) :
/**
 * Template for comments and pingbacks.
 *
 * To override this walker in a child theme without modifying the comments template
 * simply create your own top_mag_comment(), and that function will be used instead.
 *
 * Used as a callback by wp_list_comments() for displaying the comments.
 *
 */
function top_mag_comment( $comment, $top_mag_args, $depth ) {
	$GLOBALS['comment'] = $comment;
	switch ( $comment->comment_type ) :
		case 'pingback' :
		case 'trackback' :
		// Display trackbacks differently than normal comments.
	?>
<li <?php comment_class(); ?> id="comment-<?php comment_ID(); ?>">
  <p>
    <?php _e( 'Pingback:', 'top-mag' ); ?>
    <?php comment_author_link(); ?>
    <?php edit_comment_link( __( 'Edit', 'top-mag' ), '<span class="edit-link">', '</span>' ); ?>
  </p>
</li>
<?php
	break;
		default :
		// Proceed with normal comments.
		if($comment->comment_approved==1)
		{
		global $post;
	?>
    
<div <?php comment_class('col-md-12 no-padding post-comments'); ?> id="li-comment-<?php comment_ID(); ?>">
  <?php echo get_avatar( get_the_author_meta('ID'),'52'); ?>
  <div class="comment-content">
  <?php printf( '<h1>%1$s</h1>', get_comment_author_link(), ( $comment->user_id === $post->post_author ) ? __( 'Post author ', 'top-mag' ) : ''); ?>
      <h6><?php echo get_comment_date().' at '.get_the_time(); ?></h6>
      <p><?php comment_text(); ?></p>
      <div class="reply-comment">
          <?php echo '<a href="#">'.comment_reply_link( array_merge( $top_mag_args, array( 'reply_text' => __( 'Reply', 'top-mag' ), 'after' => '', 'depth' => $depth, 'max_depth' => $top_mag_args['max_depth'] ) ) ).'</a>'; ?>
       </div>
   </div>   
</div>
  <!-- #comment-## -->
  <?php
		}
		break;
	endswitch; // end comment_type check
}
endif;

/**
 * Function for breaking news in home page.
 */
function top_mag_breaking_news() {
	
	global $top_mag_options;
	if(!empty($top_mag_options['breaking-news'])) { ?>
        <div class="col-md-12 breaking-news">
        	<div class="col-md-2 news-title" id="mf118"><?php _e('BREAKING NEWS','top-mag');?></div>
				<?php $top_mag_news =0;
                    if(!empty($top_mag_options['breaking-news-category'])) {
                    $top_mag_args = array(
                        'post_type' => 'post',
                        'posts_per_page' => -1,
                        'category_name' => esc_attr($top_mag_options['breaking-news-category']),
                        );
                    } else {
                    $top_mag_args = array(
                        'post_type' => 'post',
                        'posts_per_page' => -1,
                        );
                    }
                    $top_mag_post = new WP_Query( $top_mag_args );
                    while ( $top_mag_post->have_posts() ) { $top_mag_post->the_post();
                    $top_mag_news++;
                    if($top_mag_news!=1) { $top_mag_class ="breaking-news-hidden"; } else { $top_mag_class = ""; }
                ?>
            
                <ul id="js-news" class="js-hidden">
					<li class="news-item"><?php echo '<a href="'.get_permalink().'">'.get_the_title().'</a>'; ?></li>
				</ul>
            <?php } wp_reset_query(); ?> 
        </div>
	<?php } 
}

function top_mag_customize_excerpt_more( $more ) {
return '...';
}
add_filter('excerpt_more', 'top_mag_customize_excerpt_more');

function top_mag_excerpt_length($length) {
	return 50;
}
add_filter('excerpt_length', 'top_mag_excerpt_length');
