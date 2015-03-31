<?php get_header(); ?>


	<!-- FRONT POSTS AREA -->
	<section class="front-post-area">
		
		<div class="container">
			
			<div class="row">

				<!-- ********* POSTS AREA******** -->				
				<div class="category-posts col-sm-8 front-page">

					<div class="breadcrumbs">
						<?php if(is_month()){ ?>
							<h4><?php _e('Monthly Archives','flower'); ?> : <?php echo get_the_date(); ?></h4>
						<?php }elseif(is_day()){ ?>
							<h4><?php _e('Daily Archives','flower'); ?> : <?php echo get_the_date(); ?></h4>
						<?php }elseif(is_year()){ ?>
							<h4><?php _e('Yearly Archives','flower'); ?> : <?php echo get_the_date(); ?></h4>
						<?php }else{?>
							<h4><?php _e('Archives','flower'); ?> : <?php echo get_the_date(); ?></h4>					
						<?php } ?>
					</div>

					
					
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