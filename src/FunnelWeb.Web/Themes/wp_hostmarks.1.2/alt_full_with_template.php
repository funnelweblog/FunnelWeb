<?php
/*
 * Template Name: Alt_Full-Width Template
 * Description: An alternative template for the default blog/homepage. Displays the title and content as intro copy/text.
 */
get_header(); ?>
	
    <?php while ( have_posts() ) : the_post(); ?>
      <div class="copy-blue-box">
       <h1 class="entry-title"><?php the_title(); ?></h1>
       <div id="full-width-content"><?php the_excerpt(); ?></div>
      </div>
    <?php endwhile; // end of the loop. ?>

    <div id="content" class="full-width-content clearfix">
 
       <div id="main" class="clearfix" role="main">
        	<?php while ( have_posts() ) : the_post(); ?>

				<article id="post-<?php the_ID(); ?>" <?php post_class(); ?>>
           
                    <div class="entry-content post-content">
                        <?php the_content(); ?>
                        <?php wp_link_pages( array( 'before' => '<div class="page-link">' . __( 'Pages:', 'hostmarks' ), 'after' => '</div>' ) ); ?>
                    </div><!-- .entry-content -->
                    
                    <?php edit_post_link( __( 'Edit', 'hostmarks' ), '<span class="edit-link">', '</span>' ); ?>
                </article><!-- #post-<?php the_ID(); ?> -->

				<?php comments_template( '', true ); ?>

				<?php endwhile; // end of the loop. ?>

        </div> <!-- end #main -->
        
     

    </div> <!-- end #content -->
    
    <?php if ( get_theme_mod('hostmarks_footer_widget') ) {
		get_sidebar('footer');
	} ?>
        
<?php get_footer(); ?>