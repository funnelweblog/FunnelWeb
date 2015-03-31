
<article id="post-<?php the_ID(); ?>" <?php post_class(); ?>>
	
	<header class="entry-header">
		<h2 class="entry-title"><a href="<?php the_permalink(); ?>" title="<?php printf( esc_attr__( 'Permalink to %s', 'hostmarks' ), the_title_attribute( 'echo=0' ) ); ?>" rel="bookmark"><?php the_title(); ?></a></h2>
	</header><!-- .entry-header -->


    <div class="entry-content post-content">
    
	<?php if ( has_post_format('video') || has_post_format('audio') || has_post_format('quote') || has_post_format('aside') || has_post_format('status') || has_post_format('link') ) : ?>
    
    	<?php if ( has_post_thumbnail()) : ?>
            <div class="imgthumb"><a href="<?php the_permalink() ?>" rel="bookmark" title="<?php the_title_attribute(); ?>"><?php the_post_thumbnail( 'medium' ); ?></a></div>
        <?php else : ?>
        <?php
            $postimgs =& get_children( array( 'post_parent' => $post->ID, 'post_type' => 'attachment', 'post_mime_type' => 'image', 'orderby' => 'menu_order', 'order' => 'ASC' ) );
            if ( !empty($postimgs) ) {
                $firstimg = array_shift( $postimgs );
                $th_image = wp_get_attachment_image( $firstimg->ID, 'full', false );
             ?>
                <div class="imgthumb"><a href="<?php the_permalink() ?>" rel="bookmark" title="<?php the_title_attribute(); ?>"><?php echo $th_image; ?></a></div>
                
        <?php } ?>
        <?php endif; ?>
		<?php the_content( __( 'Continue reading <span class="meta-nav">&rarr;</span>', 'hostmarks' ) ); ?>
		<?php wp_link_pages( array( 'before' => '<div class="page-link">' . __( 'Pages:', 'hostmarks' ), 'after' => '</div>' ) ); ?>
	
	<?php elseif ( has_post_format('image') ) : ?>
		<?php the_content( __( 'Continue reading <span class="meta-nav">&rarr;</span>', 'hostmarks' ) ); ?>
		<?php wp_link_pages( array( 'before' => '<div class="page-link">' . __( 'Pages:', 'hostmarks' ), 'after' => '</div>' ) ); ?>
	
	<?php elseif ( has_post_format('gallery') ) : ?>
    	<?php if( has_shortcode( $post->post_content, 'gallery' ) ) : ?>
    
		<?php 
        $gallery = get_post_gallery( $post, false );
        $ids = explode( ",", $gallery['ids'] );
        $total_images = 0;
        foreach( $ids as $id ) {
            $title = get_post_field('post_title', $id);
            $meta = get_post_field('post_excerpt', $id);
            $link = wp_get_attachment_url( $id );
            $image  = wp_get_attachment_image( $id, 'medium');
            $total_images++;
            
            if ($total_images == 1) {
                $first_img = $image;
            }
        }    
        ?>
        
        <?php if ( has_post_thumbnail()) : ?>
            <div class="imgthumb"><a href="<?php the_permalink() ?>" rel="bookmark" title="<?php the_title_attribute(); ?>"><?php the_post_thumbnail( 'medium' ); ?></a></div><!-- .imgthumb -->
        <?php else : ?>
            <div class="imgthumb"><a href="<?php the_permalink(); ?>"><?php echo $first_img; ?></a></div><!-- .imgthumb -->
        <?php endif; ?>
        
        <?php if ( post_password_required() ) : ?>
			<?php the_content( __( 'Continue reading <span class="meta-nav">&rarr;</span>', 'hostmarks' ) ); ?>

			<?php else : ?>

				<p><em><?php printf( _n( 'This gallery contains <a %1$s>%2$s photo</a>.', 'This gallery contains <a %1$s>%2$s photos</a>.', $total_images, 'hostmarks' ),
						'href="' . get_permalink() . '" title="' . sprintf( esc_attr__( 'Permalink to %s', 'hostmarks' ), the_title_attribute( 'echo=0' ) ) . '" rel="bookmark"',
						number_format_i18n( $total_images )
					); ?></em></p>

			<?php if (has_excerpt()) the_excerpt(); ?>
		<?php endif; ?>
		<?php wp_link_pages( array( 'before' => '<div class="page-link">' . __( 'Pages:', 'hostmarks' ), 'after' => '</div>' ) ); ?>
        
        <?php endif; ?>
    <?php else : ?>
    	<?php if ( has_post_thumbnail()) : ?>
            <div class="imgthumb"><a href="<?php the_permalink() ?>" rel="bookmark" title="<?php the_title_attribute(); ?>"><?php the_post_thumbnail( 'medium' ); ?></a></div>
            
        <?php else : ?>
        <?php
            $postimgs =& get_children( array( 'post_parent' => $post->ID, 'post_type' => 'attachment', 'post_mime_type' => 'image', 'orderby' => 'menu_order', 'order' => 'ASC' ) );
            if ( !empty($postimgs) ) {
                $firstimg = array_shift( $postimgs );
                $th_image = wp_get_attachment_image( $firstimg->ID, 'full', false );
             ?>
                <div class="imgthumb"><a href="<?php the_permalink() ?>" rel="bookmark" title="<?php the_title_attribute(); ?>"><?php echo $th_image; ?></a></div>
                
        <?php } ?>
        <?php endif; ?>
        
		<?php if (has_excerpt()) { 
			the_excerpt();
		} else {
    		echo hostmarks_excerpt(20);
		} ?>
        <a class="more-link" href="<?php the_permalink(); ?>"><?php _e('READ MORE &rarr;', 'hostmarks'); ?></a>
	<?php endif; ?>
	</div><!-- .entry-content -->
	
    <?php if ( has_post_format('quote') || has_post_format('aside') || has_post_format('status') ) : ?>
    <footer class="entry-meta">
		<?php hostmarks_posted_on(); ?>
	</footer><!-- #entry-meta -->
    <?php endif; ?>

</article><!-- #post-<?php the_ID(); ?> -->
