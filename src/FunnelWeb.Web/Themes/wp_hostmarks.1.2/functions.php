<?php
/**
 * Sets up the theme and provides some helper functions. Some helper functions
 * are used in the theme as custom template tags. Others are attached to action and
 * filter hooks in WordPress to change core functionality.
 *
 *
 * For more information on hooks, actions, and filters, see http://codex.wordpress.org/Plugin_API.
 */


if ( ! function_exists( 'hostmarks_setup' ) ):
/**
 * Sets up theme defaults and registers support for various WordPress features.
 */
function hostmarks_setup() {
		
	/**
	 * Make theme available for translation
	 * Translations can be filed in the /languages/ directory
	 */
	load_theme_textdomain( 'hostmarks', get_template_directory() . '/languages' );

	/**
	 * Add default posts and comments RSS feed links to head
	 */
	add_post_type_support( 'page', 'excerpt' );
	add_theme_support( 'automatic-feed-links' );
	add_theme_support( "title-tag" );
	$defaults = array(
		'default-image'          => '',
		'random-default'         => false,
		'width'                  => 0,
		'height'                 => 0,
		'flex-height'            => false,
		'flex-width'             => false,
		'default-text-color'     => '',
		'header-text'            => true,
		'uploads'                => true,
		'wp-head-callback'       => '',
		'admin-head-callback'    => '',
		'admin-preview-callback' => '',
	);
	
	add_theme_support( 'custom-header', $defaults );
	remove_theme_support('custom-header');

	/**
	 * This theme uses wp_nav_menu() in one location.
	 */
	register_nav_menus( array(
		'primary' => __( 'Primary Menu', 'hostmarks' ),
	) );

	add_theme_support('post-thumbnails'); 
	// This theme styles the visual editor with editor-style.css to match the theme style.
	add_editor_style();
	
	
	// custom backgrounds
	$hostmarks_custom_background = array(
		// Background color default
		'default-color' => 'ffffff',
		// Background image default
		'default-image' => '',
		'wp-head-callback' => '_custom_background_cb'
	);
	add_theme_support('custom-background', $hostmarks_custom_background );
	
	
	// adding post format support
	add_theme_support( 'post-formats', 
		array( 
			'aside', /* Typically styled without a title. Similar to a Facebook note update */
			'gallery', /* A gallery of images. Post will likely contain a gallery shortcode and will have image attachments */
			'link', /* A link to another site. Themes may wish to use the first link tag in the post content as the external link for that post. An alternative approach could be if the post consists only of a URL, then that will be the URL and the title (post_title) will be the name attached to the anchor for it */
			'image', /* A single image. The first <img /> tag in the post could be considered the image. Alternatively, if the post consists only of a URL, that will be the image URL and the title of the post (post_title) will be the title attribute for the image */
			'quote', /* A quotation. Probably will contain a blockquote holding the quote content. Alternatively, the quote may be just the content, with the source/author being the title */
			'status', /*A short status update, similar to a Twitter status update */
			'video', /* A single video. The first <video /> tag or object/embed in the post content could be considered the video. Alternatively, if the post consists only of a URL, that will be the video URL. May also contain the video as an attachment to the post, if video support is enabled on the blog (like via a plugin) */
			'audio', /* An audio file. Could be used for Podcasting */
			'chat' /* A chat transcript */
		)
	);
}
endif;
add_action( 'after_setup_theme', 'hostmarks_setup' );

/**
 * Set the content width based on the theme's design and stylesheet.
 */
if ( ! function_exists( 'hostmarks_content_width' ) ) :
	function hostmarks_content_width() {
		global $content_width;
		if (!isset($content_width))
			$content_width = 550; /* pixels */
	}
endif;
add_action( 'after_setup_theme', 'hostmarks_content_width' );


/**
 * Title filter 
 */
if ( ! function_exists( 'hostmarks_filter_wp_title' ) ) :
	function hostmarks_filter_wp_title( $old_title, $sep, $sep_location ) {
		
		if ( is_feed() ) return $old_title;
	
		$site_name = get_bloginfo( 'name' );
		$site_description = get_bloginfo( 'description' );
		// add padding to the sep
		$ssep = ' ' . $sep . ' ';
		
		if ( $site_description && ( is_home() || is_front_page() ) ) {
			return $site_name . ' | ' . $site_description;
		} else {
			// find the type of index page this is
			if( is_category() ) $insert = $ssep . __( 'Category', 'hostmarks' );
			elseif( is_tag() ) $insert = $ssep . __( 'Tag', 'hostmarks' );
			elseif( is_author() ) $insert = $ssep . __( 'Author', 'hostmarks' );
			elseif( is_year() || is_month() || is_day() ) $insert = $ssep . __( 'Archives', 'hostmarks' );
			else $insert = NULL;
			 
			// get the page number we're on (index)
			if( get_query_var( 'paged' ) )
			$num = $ssep . __( 'Page ', 'hostmarks' ) . get_query_var( 'paged' );
			 
			// get the page number we're on (multipage post)
			elseif( get_query_var( 'page' ) )
			$num = $ssep . __( 'Page ', 'hostmarks' ) . get_query_var( 'page' );
			 
			// else
			else $num = NULL;
			 
			// concoct and return new title
			return $site_name . $insert . $old_title . $num;
			
		}
	
	}
endif;
// call our custom wp_title filter, with normal (10) priority, and 3 args
add_filter( 'wp_title', 'hostmarks_filter_wp_title', 10, 3 );


/*******************************************************************
* These are settings for the Theme Customizer in the admin panel. 
*******************************************************************/
if ( ! function_exists( 'hostmarks_theme_customizer' ) ) :
	function hostmarks_theme_customizer( $wp_customize ) {
		
		$wp_customize->remove_section( 'title_tagline');

		
		/* logo option */
		$wp_customize->add_section( 'hostmarks_logo_section' , array(
			'title'       => __( 'Site Logo', 'hostmarks' ),
			'priority'    => 31,
			'description' => __( 'Upload a logo to replace the default site name in the header', 'hostmarks' ),
		) );
		
		$wp_customize->add_setting( 'hostmarks_logo', array(
		'sanitize_callback' => 'esc_url_raw',) );
			
		
		$wp_customize->add_control( new WP_Customize_Image_Control( $wp_customize, 'hostmarks_logo', array(
			'label'    => __( 'Choose your logo (ideal width is 100-300px and ideal height is 40-100px)', 'hostmarks' ),
			'section'  => 'hostmarks_logo_section',
			'settings' => 'hostmarks_logo',
		) ) );
		
		/* header background color option */
		$wp_customize->add_setting( 'hostmarks_header_bg', array (
			'default' => '#04396c',
			'sanitize_callback' => 'sanitize_hex_color',
		) );
		
		$wp_customize->add_control( new WP_Customize_Color_Control( $wp_customize, 'hostmarks_header_bg', array(
			'label'    => __( 'Header Background', 'hostmarks' ),
			'section'  => 'colors',
			'settings' => 'hostmarks_header_bg',
			'priority' => 101,
		) ) );
		
		/* alt blog intro text bg */
		$wp_customize->add_setting( 'hostmarks_alt_blog_intro', array (
			'default' => '#408ad2',
			'sanitize_callback' => 'sanitize_hex_color',
		) );
		
		$wp_customize->add_control( new WP_Customize_Color_Control( $wp_customize, 'hostmarks_alt_blog_intro', array(
			'label'    => __( 'Alt Blog Welcome/Intro BG', 'hostmarks' ),
			'section'  => 'colors',
			'settings' => 'hostmarks_alt_blog_intro',
			'priority' => 102,
		) ) );
		
		/* post title color option */
		$wp_customize->add_setting( 'hostmarks_post_title_color', array (
			'default' => '#408ad2',
			'sanitize_callback' => 'sanitize_hex_color',
		) );
		
		$wp_customize->add_control( new WP_Customize_Color_Control( $wp_customize, 'hostmarks_post_title_color', array(
			'label'    => __( 'Post Title and Links Color', 'hostmarks' ),
			'section'  => 'colors',
			'settings' => 'hostmarks_post_title_color',
			'priority' => 103,
		) ) );
		
		/* body text color option */
		$wp_customize->add_setting( 'hostmarks_body_text_color', array (
			'default' => '#222222',
			'sanitize_callback' => 'sanitize_hex_color',
		) );
		
		$wp_customize->add_control( new WP_Customize_Color_Control( $wp_customize, 'hostmarks_body_text_color', array(
			'label'    => __( 'Body Text Color', 'hostmarks' ),
			'section'  => 'colors',
			'settings' => 'hostmarks_body_text_color',
			'priority' => 104,
		) ) );
		
		/* vertical divider color */
		$wp_customize->add_setting( 'hostmarks_divider_color', array (
			'default' => '#ffffff',
			'sanitize_callback' => 'sanitize_hex_color',
		) );
		
		$wp_customize->add_control( new WP_Customize_Color_Control( $wp_customize, 'hostmarks_divider_color', array(
			'label'    => __( 'Vertical Divider Color', 'hostmarks' ),
			'section'  => 'colors',
			'settings' => 'hostmarks_divider_color',
			'priority' => 105,
		) ) );
		
		/* comment/reply form title color */
		$wp_customize->add_setting( 'hostmarks_comment_title', array (
			'default' => '#ff9700',
			'sanitize_callback' => 'sanitize_hex_color',
		) );
		
		$wp_customize->add_control( new WP_Customize_Color_Control( $wp_customize, 'hostmarks_comment_title', array(
			'label'    => __( 'Comments/Respond Form Title and Button', 'hostmarks' ),
			'section'  => 'colors',
			'settings' => 'hostmarks_comment_title',
			'priority' => 106,
		) ) );
		
		/* widget title background color option */
		$wp_customize->add_setting( 'hostmarks_widget_title_bg', array (
			'default' => '#0c5aa6',
			'sanitize_callback' => 'sanitize_hex_color',
		) );
		
		$wp_customize->add_control( new WP_Customize_Color_Control( $wp_customize, 'hostmarks_widget_title_bg', array(
			'label'    => __( 'Widget Title and Pagination Background', 'hostmarks' ),
			'section'  => 'colors',
			'settings' => 'hostmarks_widget_title_bg',
			'priority' => 107,
		) ) );
		
		
		/* footer background color option */
		$wp_customize->add_setting( 'hostmarks_footer_bg', array (
			'default' => '#408AD2',
			'sanitize_callback' => 'sanitize_hex_color',
		) );
		
		$wp_customize->add_control( new WP_Customize_Color_Control( $wp_customize, 'hostmarks_footer_bg', array(
			'label'    => __( 'Footer Background', 'hostmarks' ),
			'section'  => 'colors',
			'settings' => 'hostmarks_footer_bg',
			'priority' => 108,
		) ) );
		
		/* display footer widget on all pages option */
		$wp_customize->add_section( 'hostmarks_footer_widget_section' , array(
			'title'       => __( 'Display Footer Widgets', 'hostmarks' ),
			'priority'    => 32,
			'description' => __( 'Option to show/hide the footer widgets on all pages.', 'hostmarks' ),
		) );
		
		$wp_customize->add_setting( 'hostmarks_footer_widget', array(
		'sanitize_callback' => 'esc_url_raw',) );
		
		$wp_customize->add_control('footer_widget', array(
			'settings' => 'hostmarks_footer_widget',
			'label' => __('Show the footer widget on all pages?', 'hostmarks'),
			'section' => 'hostmarks_footer_widget_section',
			'type' => 'checkbox',
		));
		
		/* option for the banner section */
		
		class hostmarks_Customize_Textarea_Control extends WP_Customize_Control {
			public $type = 'textarea';
		 
			public function render_content() {
				?>
				<label>
				<span class="customize-control-title"><?php echo esc_html( $this->label ); ?></span>
				<textarea rows="5" style="width:100%;" <?php $this->link(); ?>><?php echo esc_textarea( $this->value() ); ?></textarea>
				</label>
				<?php
			}
		}
		
		$wp_customize->add_section( 'hostmarks_banner_section' , array(
			'title'       => __( 'Sidebar Banner Code', 'hostmarks' ),
			'priority'    => 32,
			'description' => __( 'Enter the code for the sidebar banner.', 'hostmarks' ),
		) );
		
		/* Sidebar Banner (max 300px wide)  */
		$wp_customize->add_setting( 'hostmarks_banner_sidebar', array(
		'sanitize_callback' => 'esc_url_raw',) );
		
		$wp_customize->add_control( new hostmarks_Customize_Textarea_Control( $wp_customize, 'hostmarks_banner_sidebar', array(
			'label'    => __( 'Sidebar Banner (max 300px wide)', 'hostmarks' ),
			'section'  => 'hostmarks_banner_section',
			'settings' => 'hostmarks_banner_sidebar',
		) ) );
		
	}
endif;
add_action('customize_register', 'hostmarks_theme_customizer');

/**
* Apply Color Scheme
*/
if ( ! function_exists( 'hostmarks_apply_color' ) ) :
  function hostmarks_apply_color() {
	 if ( get_theme_mod('hostmarks_header_bg') || get_theme_mod('hostmarks_alt_blog_intro') || get_theme_mod('hostmarks_post_title_color') || get_theme_mod('hostmarks_body_text_color') || get_theme_mod('hostmarks_divider_color') || get_theme_mod('hostmarks_comment_title') || get_theme_mod('hostmarks_widget_title_bg') || get_theme_mod('hostmarks_footer_bg') )  {
	?>
	<style id="hostmarks-color-settings">
	<?php if ( get_theme_mod('hostmarks_header_bg') ) : ?>
		header[role=banner] {
			background-color: <?php echo get_theme_mod('hostmarks_header_bg'); ?>;
		}
	<?php endif; ?>
	
	<?php if ( get_theme_mod('hostmarks_alt_blog_intro') ) : ?>
		.copy-blue-box {
			background-color: <?php echo get_theme_mod('hostmarks_alt_blog_intro'); ?>;
		}
	<?php endif; ?>
	
	<?php if ( get_theme_mod('hostmarks_post_title_color') ) : ?>
		a, a:visited, body.page .entry-title, body.single .entry-title, .not-found .entry-title { 
			color: <?php echo get_theme_mod('hostmarks_post_title_color'); ?>;
		}
	<?php endif; ?>
	
	<?php if ( get_theme_mod('hostmarks_body_text_color') ) : ?>
		body, select, input, textarea, .entry-meta a, .category-archive-meta a, .more-link:hover, #sidebar .widget li a, .commentlist .vcard cite.fn a:hover, a:hover {   
			color: <?php echo get_theme_mod('hostmarks_body_text_color'); ?>;
		}
	<?php endif; ?>
	
	<?php if ( get_theme_mod('hostmarks_divider_color') ) : ?>
		#main { 
			border-right: 1px solid <?php echo get_theme_mod('hostmarks_divider_color'); ?>;
		}
	<?php endif; ?>
	
	<?php if ( get_theme_mod('hostmarks_comment_title') ) : ?>
		#comments-title, #reply-title {
			color: <?php echo get_theme_mod('hostmarks_comment_title'); ?>;
		}
		#respond #submit {
			background-color: <?php echo get_theme_mod('hostmarks_comment_title'); ?>;
		}
	
	<?php endif; ?>
	
	<?php if ( get_theme_mod('hostmarks_widget_title_bg') ) : ?>
		.pagination li a:hover, .pagination li.active a, #sidebar .widget-title {
			background-color: <?php echo get_theme_mod('hostmarks_widget_title_bg'); ?>;
		}
	<?php endif; ?>
	
	<?php if ( get_theme_mod('hostmarks_footer_bg') ) : ?>
		footer[role=contentinfo] {
			background-color: <?php echo get_theme_mod('hostmarks_footer_bg'); ?>;
		}
	<?php endif; ?>
	</style>
	<?php	  
	} 
  }
endif;
add_action( 'wp_head', 'hostmarks_apply_color' );


/**
 * Get our wp_nav_menu() fallback, wp_page_menu(), to show a home link.
 */
if ( ! function_exists( 'hostmarks_main_nav' ) ) :
function hostmarks_main_nav() {
	// display the wp3 menu if available
    wp_nav_menu( 
    	array( 
    		'theme_location' => 'primary', /* where in the theme it's assigned */
    		'container_class' => 'menu', /* div container class */
    		'fallback_cb' => 'hostmarks_main_nav_fallback' /* menu fallback */
    	)
    );
}
endif;

if ( ! function_exists( 'hostmarks_main_nav_fallback' ) ) :
	function hostmarks_main_nav_fallback() { wp_page_menu( 'show_home=Home&container_class=menu' ); }
endif;

if ( ! function_exists( 'hostmarks_enqueue_comment_reply' ) ) :
	function hostmarks_enqueue_comment_reply() {
			if ( is_singular() && comments_open() && get_option( 'thread_comments' ) ) {
					wp_enqueue_script( 'comment-reply' );
			}
	 }
endif;
add_action( 'comment_form_before', 'hostmarks_enqueue_comment_reply' );

if ( ! function_exists( 'hostmarks_page_menu_args' ) ) :
	function hostmarks_page_menu_args( $args ) {
		$args['show_home'] = true;
		return $args;
	}
endif;
add_filter( 'wp_page_menu_args', 'hostmarks_page_menu_args' );

/**
 * Register widgetized area and update sidebar with default widgets
 */
function hostmarks_widgets_init() {
	register_sidebar( array(
		'name' => __( 'Blog Sidebar', 'hostmarks' ),
		'id' => 'sidebar-right',
		'before_widget' => '<aside id="%1$s" class="widget %2$s">',
		'after_widget' => "</aside>",
		'before_title' => '<div class="widget-title">',
		'after_title' => '</div>',
	) );
	
	register_sidebar( array(
		'name' => __( 'Page Sidebar', 'hostmarks' ),
		'id' => 'sidebar-page',
		'before_widget' => '<aside id="%1$s" class="widget %2$s">',
		'after_widget' => "</aside>",
		'before_title' => '<div class="widget-title">',
		'after_title' => '</div>',
	) );
	
	register_sidebar( array(
		'name' => __( 'Footer Widgets', 'hostmarks' ),
		'id' => 'sidebar-footer',
		'before_widget' => '<aside id="%1$s" class="widget %2$s">',
		'after_widget' => "</aside>",
		'before_title' => '<div class="widget-title">',
		'after_title' => '</div>',
	) );
}
add_action( 'widgets_init', 'hostmarks_widgets_init' );

if ( ! function_exists( 'hostmarks_content_nav' ) ):
/**
 * Display navigation to next/previous pages when applicable
 */
function hostmarks_content_nav( $nav_id ) {
	global $wp_query;

	?>
	<nav id="<?php echo $nav_id; ?>">
		<h1 class="assistive-text section-heading"><?php _e( 'Post navigation', 'hostmarks' ); ?></h1>

	<?php if ( is_single() ) : // navigation links for single posts ?>

		<?php previous_post_link( '<div class="nav-previous">%link</div>', '<span class="meta-nav">' . _x( '&larr; Previous', 'Previous post link', 'hostmarks' ) . '</span>' ); ?>
		<?php next_post_link( '<div class="nav-next">%link</div>', '<span class="meta-nav">' . _x( 'Next &rarr;', 'Next post link', 'hostmarks' ) . '</span>' ); ?>

	<?php elseif ( $wp_query->max_num_pages > 1 && ( is_home() || is_archive() || is_search() ) ) : // navigation links for home, archive, and search pages ?>

		<?php if ( get_next_posts_link() ) : ?>
		<div class="nav-previous"><?php next_posts_link( __( '<span class="meta-nav">&larr;</span> Older posts', 'hostmarks' ) ); ?></div>
		<?php endif; ?>

		<?php if ( get_previous_posts_link() ) : ?>
		<div class="nav-next"><?php previous_posts_link( __( 'Newer posts <span class="meta-nav">&rarr;</span>', 'hostmarks' ) ); ?></div>
		<?php endif; ?>

	<?php endif; ?>

	</nav><!-- #<?php echo $nav_id; ?> -->
	<?php
}
endif;


if ( ! function_exists( 'hostmarks_comment' ) ) :
/**
 * Template for comments and pingbacks.
 */
function hostmarks_comment( $comment, $args, $depth ) {
	$GLOBALS['comment'] = $comment;
	switch ( $comment->comment_type ) :
		case 'pingback' :
		case 'trackback' :
	?>
	<li class="post pingback">
		<p><?php _e( 'Pingback:', 'hostmarks' ); ?> <?php comment_author_link(); ?><?php edit_comment_link( __( '(Edit)', 'hostmarks' ), ' ' ); ?></p>
	<?php
			break;
		default :
	?>
	<li <?php comment_class(); ?> id="li-comment-<?php comment_ID(); ?>">
		<article id="comment-<?php comment_ID(); ?>">
			<footer class="clearfix comment-head">
				<div class="comment-author vcard">
					<?php echo get_avatar( $comment, 67 ); ?>
					<?php printf( __( '%s', 'hostmarks' ), sprintf( '<cite class="fn">%s</cite>', get_comment_author_link() ) ); ?>
				</div><!-- .comment-author .vcard -->
				<?php if ( $comment->comment_approved == '0' ) : ?>
					<em><?php _e( 'Your comment is awaiting moderation.', 'hostmarks' ); ?></em>
					<br />
				<?php endif; ?>

				<div class="comment-meta commentmetadata">
					<a href="<?php echo esc_url( get_comment_link( $comment->comment_ID ) ); ?>"><time pubdate datetime="<?php comment_time( 'c' ); ?>">
					<?php
						/* translators: 1: date, 2: time */
						printf( __( '%1$s at %2$s', 'hostmarks' ), get_comment_date(), get_comment_time() ); ?>
					</time></a>
					<?php edit_comment_link( __( '(Edit)', 'hostmarks' ), ' ' );
					?>
				</div><!-- .comment-meta .commentmetadata -->
			</footer>

			<div class="comment-content"><?php comment_text(); ?></div>

			<div class="reply">
				<?php comment_reply_link( array_merge( $args, array( 'depth' => $depth, 'max_depth' => $args['max_depth'] ) ) ); ?>
			</div><!-- .reply -->
		</article><!-- #comment-## -->

	<?php
			break;
	endswitch;
}
endif;

if ( ! function_exists( 'hostmarks_posted_on' ) ) :
/**
 * Prints HTML with meta information for the current post-date/time and author.
 */
function hostmarks_posted_on() {
	printf( __( '<a href="%1$s" title="%2$s" rel="bookmark"><time class="entry-date" datetime="%3$s">%4$s</time></a><span class="byline"> &nbsp; <span class="sep"> by </span> <span class="author vcard"><a class="url fn n" href="%5$s" title="%6$s" rel="author">%7$s</a></span></span>', 'hostmarks' ),
		esc_url( get_permalink() ),
		esc_attr( get_the_time() ),
		esc_attr( get_the_date( 'c' ) ),
		esc_html( get_the_date() ),
		esc_url( get_author_posts_url( get_the_author_meta( 'ID' ) ) ),
		esc_attr( sprintf( __( 'View all posts by %s', 'hostmarks' ), get_the_author() ) ),
		esc_html( get_the_author() )
	);
}
endif;

/**
 * Adds custom classes to the array of body classes.
 */
if ( ! function_exists( 'hostmarks_body_classes' ) ) :
	function hostmarks_body_classes( $classes ) {
		// Adds a class of single-author to blogs with only 1 published author
		if ( ! is_multi_author() ) {
			$classes[] = 'single-author';
		}
	
		return $classes;
	}
endif;
add_filter( 'body_class', 'hostmarks_body_classes' );

/**
 * Returns true if a blog has more than 1 category
 */
if ( ! function_exists( 'hostmarks_categorized_blog' ) ) :
function hostmarks_categorized_blog() {
	if ( false === ( $all_the_cool_cats = get_transient( 'all_the_cool_cats' ) ) ) {
		// Create an array of all the categories that are attached to posts
		$all_the_cool_cats = get_categories( array(
			'hide_empty' => 1,
		) );

		// Count the number of categories that are attached to the posts
		$all_the_cool_cats = count( $all_the_cool_cats );

		set_transient( 'all_the_cool_cats', $all_the_cool_cats );
	}

	if ( '1' != $all_the_cool_cats ) {
		// This blog has more than 1 category so hostmarks_categorized_blog should return true
		return true;
	} else {
		// This blog has only 1 category so hostmarks_categorized_blog should return false
		return false;
	}
}
endif;
/**
 * Flush out the transients used in hostmarks_categorized_blog
 */
if ( ! function_exists( 'hostmarks_category_transient_flusher' ) ) :
	function hostmarks_category_transient_flusher() {
		// Like, beat it. Dig?
		delete_transient( 'all_the_cool_cats' );
	}
endif;
add_action( 'edit_category', 'hostmarks_category_transient_flusher' );
add_action( 'save_post', 'hostmarks_category_transient_flusher' );

/**
 * Remove WP default gallery styling
 */
add_filter( 'use_default_gallery_style', '__return_false' );


/**
 * The Pagination Function
 */
if ( ! function_exists( 'hostmarks_pagination' ) ) :
	function hostmarks_pagination() {
	
		if( is_singular() )
			return;
	
		global $wp_query;
	
		/** Stop execution if there's only 1 page */
		if( $wp_query->max_num_pages <= 1 )
			return;
	
		$paged = get_query_var( 'paged' ) ? absint( get_query_var( 'paged' ) ) : 1;
		$max   = intval( $wp_query->max_num_pages );
	
		/**	Add current page to the array */
		if ( $paged >= 1 )
			$links[] = $paged;
	
		/**	Add the pages around the current page to the array */
		if ( $paged >= 3 ) {
			$links[] = $paged - 1;
			$links[] = $paged - 2;
		}
	
		if ( ( $paged + 2 ) <= $max ) {
			$links[] = $paged + 2;
			$links[] = $paged + 1;
		}
	
		echo '<div class="pagination"><ul>' . "\n";
	
		/**	Previous Post Link */
		if ( get_previous_posts_link() )
			printf( '<li>%s</li>' . "\n", get_previous_posts_link() );
	
		/**	Link to first page, plus ellipses if necessary */
		if ( ! in_array( 1, $links ) ) {
			$class = 1 == $paged ? ' class="active"' : '';
	
			printf( '<li%s><a href="%s">%s</a></li>' . "\n", $class, esc_url( get_pagenum_link( 1 ) ), '1' );
	
			if ( ! in_array( 2, $links ) )
				echo '<li>&hellip;</li>';
		}
	
		/**	Link to current page, plus 2 pages in either direction if necessary */
		sort( $links );
		foreach ( (array) $links as $link ) {
			$class = $paged == $link ? ' class="active"' : '';
			printf( '<li%s><a href="%s">%s</a></li>' . "\n", $class, esc_url( get_pagenum_link( $link ) ), $link );
		}
	
		/**	Link to last page, plus ellipses if necessary */
		if ( ! in_array( $max, $links ) ) {
			if ( ! in_array( $max - 1, $links ) )
				echo '<li>&hellip;</li>' . "\n";
	
			$class = $paged == $max ? ' class="active"' : '';
			printf( '<li%s><a href="%s">%s</a></li>' . "\n", $class, esc_url( get_pagenum_link( $max ) ), $max );
		}
	
		/**	Next Post Link */
		if ( get_next_posts_link() )
			printf( '<li>%s</li>' . "\n", get_next_posts_link() );
	
		echo '</ul></div>' . "\n";
	
	}
endif;

/**
 * Add "Untitled" for posts without title, 
 */
function hostmarks_post_title($title) {
	if ($title == '') {
		return __('Untitled', 'hostmarks');
	} else {
		return $title;
	}
}
add_filter('the_title', 'hostmarks_post_title');

/**
 * Fix for W3C validation
 */
if ( ! function_exists( 'hostmarks_w3c_valid_rel' ) ) :
	function hostmarks_w3c_valid_rel( $text ) {
		$text = str_replace('rel="category tag"', 'rel="tag"', $text); return $text; 
	}
endif;
add_filter( 'the_category', 'hostmarks_w3c_valid_rel' );

/*
 * Modernizr functions
 */
if ( ! function_exists( 'hostmarks_modernizr_addclass' ) ) :
	function hostmarks_modernizr_addclass($output) {
		return $output . ' class="no-js"';
	}
endif;
add_filter('language_attributes', 'hostmarks_modernizr_addclass');

if ( ! function_exists( 'hostmarks_modernizr_script' ) ) :
	function hostmarks_modernizr_script() {
		wp_enqueue_script( 'modernizr', get_template_directory_uri() . '/library/js/modernizr-2.6.2.min.js', false, '2.6.2');
	}  
endif;  
add_action('wp_enqueue_scripts', 'hostmarks_modernizr_script');


/**
 * Custom excerpt function
 */
if ( ! function_exists( 'hostmarks_excerpt' ) ) :
	function hostmarks_excerpt($limit) {
		$excerpt = explode(' ', get_the_excerpt(), $limit);
		if (count($excerpt)>=$limit) {
		array_pop($excerpt);
		$excerpt = implode(" ",$excerpt).'...';
		} else {
		$excerpt = implode(" ",$excerpt);
		}	
		$excerpt = preg_replace('`\[[^\]]*\]`','',$excerpt);
		return $excerpt;
	}
endif;


/**
 * Enqueue scripts & styles
 */
function hostmarks_custom_scripts() {
	wp_enqueue_script( 'hostmarks_custom_js', get_template_directory_uri() . '/library/js/scripts.js', array( 'jquery', 'jquery-masonry' ), '1.0.0' );
	wp_enqueue_script( 'imagesloaded', get_template_directory_uri() . '/library/js/imagesloaded.pkgd.min.js');	
	wp_enqueue_style( 'hostmarks_style', get_stylesheet_uri() );
}

add_action('wp_enqueue_scripts', 'hostmarks_custom_scripts');

function new_excerpt_more( $more ) {
	return '...';
}
add_filter('excerpt_more', 'new_excerpt_more');
?>