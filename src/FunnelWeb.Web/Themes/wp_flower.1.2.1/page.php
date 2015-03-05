<?php get_header(); ?>

	<!-- FRONT POSTS AREA -->
	<section class="front-post-area">
		
		<div class="container">
			
			<div class="row">

				<!-- ********* POSTS AREA******** -->				
				<div class="category-posts single-post col-sm-8">
					
					
					<!-- ******* Posts Loop ******** -->
					

					<div class="posts-loop single-loop">
					
					<?php while(have_posts()):the_post(); ?>

						
						<article class="category-content">

								
								<h3 class="title"><?php the_title(); ?></h3> <!-- end title -->


								<div class="flower-meta">

									<?php if(comments_open()){ ?>
										<span class="comment-count"><i class="fa fa-comment-o"></i>
											<?php comments_number(__('No Comments', 'flower'),__('1 Comment', 'flower'),__('% Comments', 'flower')); ?>
										</span>	
									<?php } ?>
									<span class="edit-post"><?php edit_post_link(__('Edit','flower')); ?></span>								

								</div> <!-- end flower-meta -->						
								

								<div class="category-writing">
									<?php the_content(); ?>
									<?php wp_link_pages(array(
										'before' => '<div class="page-links">'.__('Pages : ','flower').'<span>',
										'after' => '</span></div>'
									)); ?>

								</div>

								<div class="single-tags">
									<?php the_tags(__('Tags : ','flower'),'  ',''); ?>
								</div>

							</article>	<!-- end category-content -->

							



							<hr class="single-hr"></hr>

							

							<?php comments_template(); ?>

					<?php endwhile; ?>

					</div>
					
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



<?php get_template_part('footer','main'); ?>

<?php get_footer(); ?>