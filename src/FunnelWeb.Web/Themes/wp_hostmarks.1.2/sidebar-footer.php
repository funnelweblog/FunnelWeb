<?php
/**
 * The Footer Sidebar containing the main widget areas.
 */
?>
	<div class="sidebar-theme-wrap">
    
        <div id="sidebar" class="widget-area clearfix" role="complementary">

			<?php if ( ! dynamic_sidebar( 'sidebar-footer' ) ) : ?>

                
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
        
    </div>
