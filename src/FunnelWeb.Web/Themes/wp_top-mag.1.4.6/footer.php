<?php
/**
 * the footer template file.
 */
 $top_mag_options = get_option( 'topmag_theme_options' );
?>
<!-- footer -->
<div class="col-md-12 footer-post"> 
    <div class="row">
  <!-- Recent Posts -->
  <div class="col-md-4">
    <?php if ( is_active_sidebar( 'footer-area-one' ) ) : dynamic_sidebar( 'footer-area-one' ); endif; ?>
  </div>
  <div class="col-md-4">
    <?php if ( is_active_sidebar( 'footer-area-two' ) ) : dynamic_sidebar( 'footer-area-two' ); endif; ?>
  </div>
  <div class="col-md-4">
    <?php if ( is_active_sidebar( 'footer-area-three' ) ) : dynamic_sidebar( 'footer-area-three' ); endif; ?>
  </div>
    </div>
</div>
<footer>
  <div class="copyright col-lg-12">
    <div class="col-md-7 no-padding">
      <p><?php if(!empty($top_mag_options['footertext'])) { 
               	 echo sanitize_text_field($top_mag_options['footertext']).' '; 
			}
				
				
				printf( __( 'Powered by %1$s and %2$s.', 'top-mag' ), '<a href="http://wordpress.org" target="_blank">WordPress</a>',
				'<a href="http://fasterthemes.com/wordpress-themes/topmag" target="_blank">Top Mag</a>' ); 
				
		?></p>
    </div>
  </div>
</footer>
</div>
<?php wp_footer(); ?>
</body></html>
