
<article id="post-<?php the_ID(); ?>" <?php post_class(); ?>>
	<header class="entry-header">
		<h1 class="entry-title"><a href="<?php the_permalink(); ?>" title="<?php printf( esc_attr__( 'Permalink to %s', 'hostmarks' ), the_title_attribute( 'echo=0' ) ); ?>" rel="bookmark"><?php the_title(); ?></a></h1>
	</header><!-- .entry-header -->

	<div class="entry-content post-content">
		<?php the_content( __( 'Continue reading <span class="meta-nav">&rarr;</span>', 'hostmarks' ) ); ?>
		<?php wp_link_pages( array( 'before' => '<div class="page-link">' . __( 'Pages:', 'hostmarks' ), 'after' => '</div>' ) ); ?>
	</div><!-- .entry-content -->

	<?php if( !is_home() ) : ?>
    <footer class="entry-meta">
		<?php hostmarks_posted_on(); ?>
		<?php if ( comments_open() || ( '0' != get_comments_number() && ! comments_open() ) ) : ?>
		<span class="sep"> | </span>
		<span class="comments-link"><?php comments_popup_link( __( 'Leave a comment', 'hostmarks' ), __( '1 Comment', 'hostmarks' ), __( '% Comments', 'hostmarks' ) ); ?></span>
		<?php endif; ?>
		<?php edit_post_link( __( 'Edit', 'hostmarks' ), '<span class="sep"> | </span><span class="edit-link">', '</span>' ); ?>
	</footer><!-- #entry-meta -->
    <?php endif; ?>
    
</article><!-- #post-<?php the_ID(); ?> -->
