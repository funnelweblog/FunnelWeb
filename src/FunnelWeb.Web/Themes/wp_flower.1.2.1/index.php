<?php get_header(); ?>


	<!-- FRONT POSTS AREA -->
	<section class="front-post-area">
		
		<div class="container">
			
			<div class="row">

				<!-- ********* POSTS AREA******** -->				
				<div class="category-posts col-sm-8 front-page">				
					
					
					<?php global $paged; ?>

						<?php if(is_home() && $paged<2){						

							get_template_part('slider');						

							get_template_part('category','featured');

						} ?>				
					
					<!-- ******* Posts Loop ******** -->
					<?php if(have_posts()): ?>

					<div class="posts-loop">
					
					<?php while(have_posts()):the_post(); ?>

						
						<div class="col-sm-6 box-post">

							<?php get_template_part('content',get_post_format()); ?>

						</div> <!-- end col-sm-6 -->

					<?php endwhile; ?>

					</div>

					<?php flower_pagenavi(); ?>

					<?php endif; ?>
					<!-- ******* End Posts Loop ******** -->


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