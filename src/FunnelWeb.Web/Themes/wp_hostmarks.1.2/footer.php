
	<footer id="colophon" role="contentinfo">
		<div id="site-generator">

			<?php echo __('&copy; ', 'hostmarks') . esc_attr( get_bloginfo( 'name', 'display' ) );  ?>
            <?php if ( (is_front_page() && ! is_paged()) || (is_page_template('alt_blog_template.php') && is_front_page() && ! is_paged()) ) : ?>
            <?php _e('- Powered by ', 'hostmarks'); ?><a href="<?php echo esc_url( __( 'http://wordpress.org/', 'hostmarks' ) ); ?>" title="<?php esc_attr_e( 'Semantic Personal Publishing Platform', 'hostmarks' ); ?>"><?php _e('WordPress' ,'hostmarks'); ?></a>
			<?php _e(' and ', 'hostmarks'); ?><a href="<?php echo esc_url( __( 'http://hostmarks.com/', 'hostmarks' ) ); ?>"><?php _e('Hostmarks', 'hostmarks'); ?></a>
            <?php endif; ?>
            
		</div>
	</footer><!-- #colophon -->
</div><!-- #container -->

<?php wp_footer(); ?>


</body>
</html>