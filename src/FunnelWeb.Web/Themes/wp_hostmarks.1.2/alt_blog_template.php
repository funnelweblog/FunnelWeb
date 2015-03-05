<?php
/*
 * Template Name: Alt_Blog Template
 * Description: An alternative template for the default blog/homepage. Displays the title and content as intro copy/text.
 */
get_header(); ?>
	
    <?php while ( have_posts() ) : the_post(); ?>
      <div class="copy-blue-box">
        <?php get_template_part( 'content', 'intro' ); ?>
      </div>
    <?php endwhile; // end of the loop. ?>

    <div id="content" class="alt-blog-content clearfix">
 
       <div id="main" class="col620 clearfix" role="main">

        	<?php
				if ( get_query_var('paged') ) {
                        $paged = get_query_var('paged');
                } elseif ( get_query_var('page') ) {
                        $paged = get_query_var('page');
                } else {
                        $paged = 1;
                }
				
				$temp = $wp_query;
 				$wp_query = null;
				$wp_query = new WP_Query();
				$wp_query->query( array(
					'post_type' => 'post',
					'paged' => $paged
				));
			?>

			<?php if ( $wp_query->have_posts() ) : ?>
            	<?php /* Adds Odd/Even Classes */
				$i=0;
				$class=array('odd','even'); ?>
				<?php /* Start the Loop */ ?>
				<?php while ( $wp_query->have_posts() ) : $wp_query->the_post(); ?>
				  <div class="<?php echo $class[$i++%2]; ?>">
					<?php
						/* Include the Post-Format-specific template for the content.
						 * If you want to overload this in a child theme then include a file
						 * called content-___.php (where ___ is the Post Format name) and that will be used instead.
						 */
						get_template_part( 'content', 'alt_blog' );
					?>
				  </div>
				<?php endwhile; ?>
				
				<?php if (function_exists("hostmarks_pagination")) {
							hostmarks_pagination(); 
				} elseif (function_exists("hostmarks_content_nav")) { 
							hostmarks_content_nav( 'nav-below' );
				}?>
                
                <?php wp_reset_query(); ?>

				

			<?php else : ?>

				<article id="post-0" class="post no-results not-found">
					<header class="entry-header">
						<h1 class="entry-title"><?php _e( 'No Themes Found!', 'hostmarks' ); ?></h1>
					</header><!-- .entry-header -->

					<div class="entry-content post-content">
						<p><?php _e( 'It seems we can&rsquo;t find what you&rsquo;re looking for. Perhaps searching can help.', 'hostmarks' ); ?></p>
						<?php get_search_form(); ?>
					</div><!-- .entry-content -->
				</article><!-- #post-0 -->

			<?php endif; ?>


        </div> <!-- end #main -->
        
        <?php get_sidebar(); ?>

    </div> <!-- end #content -->
    
    <?php if ( get_theme_mod('hostmarks_footer_widget') ) {
		get_sidebar('footer');
	} ?>
        
<?php get_footer(); ?>