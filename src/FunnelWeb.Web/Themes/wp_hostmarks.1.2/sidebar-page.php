<?php
/**
 * The Sidebar containing the main widget areas.
 */
?>
		<div id="sidebar" class="widget-area col300" role="complementary">

			<?php if ( get_theme_mod( 'hostmarks_banner_sidebar' ) ) : ?>
            <div id="banner-sidebar" >
                <?php echo wp_kses_post( get_theme_mod( 'hostmarks_banner_sidebar' ) ); ?>
            </div>
            <?php endif; ?>
			
			<?php if ( ! dynamic_sidebar( 'sidebar-page' ) ) : ?>

				<aside id="search" class="widget widget_search">
					<?php get_search_form(); ?>
				</aside>
                
                <aside id="categories" class="widget">
					<div class="widget-title"><?php _e( 'Categories', 'hostmarks' ); ?></div>
					<ul>
						<?php wp_list_categories( array( 
							'title_li' => '',
							'hierarchical' => 0
						) ); ?>
					</ul>
				</aside>
                
                <aside id="recent-posts" class="widget">
					<div class="widget-title"><?php _e( 'Latest Posts', 'hostmarks' ); ?></div>
					<ul>
						<?php
							$args = array( 'numberposts' => '10', 'post_status' => 'publish' );
							$recent_posts = wp_get_recent_posts( $args );
							
							foreach( $recent_posts as $recent ){
								if ($recent["post_title"] == '') {
									 $recent["post_title"] = __('Untitled', 'hostmarks');
								}
								echo '<li><a href="' . get_permalink($recent["ID"]) . '" title="Look '.esc_attr($recent["post_title"]).'" >' . $recent["post_title"] .'</a> </li> ';
							}
						?>
                    </ul>
				</aside>

				<aside id="archives" class="widget">
					<div class="widget-title"><?php _e( 'Archives', 'hostmarks' ); ?></div>
					<ul>
						<?php wp_get_archives( array( 'type' => 'monthly' ) ); ?>
					</ul>
				</aside>

			<?php endif; // end sidebar widget area ?>
		</div><!-- #sidebar .widget-area -->
