<div class="author-info">
	<div class="author-avatar">
		<?php
		
		$author_bio_avatar_size = apply_filters( 'flower_author_bio_avatar_size', 74 );
		echo get_avatar( get_the_author_meta( 'user_email' ), $author_bio_avatar_size );
		?>
	</div><!-- .author-avatar -->
	<div class="author-description">
		<h3 class="author-title"><?php the_author(); ?><small> ( <?php the_author_meta('user_email'); ?> ) </small></h3>

		<p class="author-bio">

			<?php the_author_meta( 'description' ); ?><br>

			<a class="author-link" href="<?php echo esc_url( get_author_posts_url( get_the_author_meta( 'ID' ) ) ); ?>" rel="author">
				<?php printf( __( 'View all posts by %s <span class="meta-nav">&rarr;</span>', 'flower' ), get_the_author() ); ?>
			</a>
		</p>
	</div><!-- .author-description -->
</div><!-- .author-info -->