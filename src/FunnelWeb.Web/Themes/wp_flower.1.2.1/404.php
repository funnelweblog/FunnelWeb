<?php get_header(); ?>


	<!-- FRONT POSTS AREA -->
	<section class="front-post-area">
		
		<div class="container">
			
			<div class="row">

				<!-- ********* POSTS AREA******** -->				
				<div class="category-posts col-sm-8 front-page">
					
					<div class="broken-page">
						
						<i class="fa fa-unlink"></i>

						<h2><?php _e('Page not found.','flower'); ?> <a href="<?php esc_url(home_url('/')); ?>"><?php _e('Go Homepage','flower'); ?></a></h2>						

					</div> <!-- end broken-page -->

				</div> 
				<!-- ********* END POSTS AREA ******* -->


				<!-- ********* SIDEBAR ******* -->

					<?php get_sidebar(); ?>				

				<!-- ********* SIDEBAR ******* -->

			</div> <!-- end row -->

		</div> <!-- end container -->

	</section>
	<!-- END FRONT POSTS AREA -->




<!-- MAIN FOOTER -->
	
	<?php get_template_part('footer','main'); ?>

<!-- END MAIN FOOTER -->
	

<?php get_footer(); ?>