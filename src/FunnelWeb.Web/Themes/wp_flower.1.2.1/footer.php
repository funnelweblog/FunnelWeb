	<?php global $flower; ?>

	<footer class="bottom-footer">
		
		<div class="container">
			
			<div class="row">


				<div class="col-sm-3">
						<?php if(isset($flower['logo']['url'])){ ?>

						<?php if($flower['logo']['url']){ ?>
	
							<a href="<?php echo esc_url(home_url('/')); ?>" title='<?php bloginfo('name'); ?>'><img src="<?php if(isset($flower['logo']['url'])){

							echo esc_attr($flower['logo']['url']); 

							}else{
								
								echo get_template_directory_uri().'/img/logo.png';

							} ?>" class="logo img-responsive" alt="<?php bloginfo('name'); ?>"></a>

						<?php } } ?>
				</div> <!-- end col-sm-3 -->
				

				<?php wp_nav_menu(array(
					'theme_location' => 'footer-menu',
					'container' => 'nav',
					'container_class' => 'footer-menu col-sm-9',					
					'menu_class' => 'nav navbar-nav pull-right',	
					'fallback_cb' => 'flower_menu_footer_page',			
					'depth' => 1
				)); ?>

				<div class="hr"></div>



				<div class="copyright col-sm-6">
					<?php if($flower){ ?>

						<p><?php echo sanitize_text_field($flower['opt-editor']); ?></p>

					<?php }else{ ?>
						
						<p><?php _e('Flower WordPress Theme','flower'); ?></p>

					<?php } ?>

				</div> <!-- end copyright -->


				<div class="bookmark col-sm-6">

					<?php if(isset($flower['opt-switch'])){ ?>

					<?php if($flower['opt-switch']==true){ ?>

						<p><?php _e('Theme by','flower'); ?> <a href="<?php echo esc_url('http://burak-aydin.com'); ?>" target="_blank">Burak Aydin</a> <?php _e('Powered by','flower') ?> <a href="<?php echo esc_url('http://wordpress.org'); ?>" target="_blank">WordPress</a></p>

					<?php } } ?>
				</div> <!-- end copyright -->

			</div> <!-- end row -->

		</div> <!-- end container -->

	</footer>
	<!-- END FOOTER -->

	
<?php wp_footer(); ?>
</body>
</html>