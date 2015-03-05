
<article id="post-<?php the_ID(); ?>" <?php post_class(); ?>>
	
	<header class="entry-header">
		<h1 class="entry-title"><?php the_title(); ?></h1>
	</header><!-- .entry-header -->
	
    <?php if($post->post_content != "") : ?>
	<div class="entry-content post-content">
		<?php the_content( __( 'Continue reading <span class="meta-nav">&rarr;</span>', 'hostmarks' ) ); ?>
		<?php wp_link_pages( array( 'before' => '<div class="page-link">' . __( 'Pages:', 'hostmarks' ), 'after' => '</div>' ) ); ?>
	</div><!-- .entry-content -->
    <?php endif; ?>

</article><!-- #post-<?php the_ID(); ?> -->
