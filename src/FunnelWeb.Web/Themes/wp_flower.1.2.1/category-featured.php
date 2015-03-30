<?php global $flower; ?>

<?php if(isset($flower['categories-1'])){ ?>

<?php if($flower['categories-1']){ ?>

<div class="category-posts-inline col-sm-6">
							
						<div class="category-head">

							<h3><?php echo get_cat_name($flower['categories-1']); ?></h3>

						</div> <!-- end category-head -->


						<?php if(have_posts()): ?>

							<?php $category=new WP_Query(array(
								'posts_per_page' => 4,
								'cat' => $flower['categories-1']
							)); ?>

						<?php while($category->have_posts()):$category->the_post(); ?>

						<article class="category-content featured-category-1">
							
							<div class="img-wrap">

								<?php the_post_thumbnail(); ?>

								<div class="overlay">
									<a href="<?php the_permalink(); ?>"><i class="fa fa-link"></i></a>
								</div> <!-- end overlay -->
							</div> <!-- end img-wrap -->

							<h3 class="title"><a href="<?php the_permalink(); ?>"><?php the_title(); ?></a></h3> <!-- end title -->

							<div class="flower-meta">

								<span class="date"><i class="fa fa-calendar-o"></i><?php the_date(get_option('date_format')); ?></span>&nbsp;&nbsp;&nbsp;
								
								<?php if(comments_open()){ ?>
										<span class="comment-count"><i class="fa fa-comment-o"></i>
											<?php comments_number(__('No Comments', 'flower'),__('1 Comment', 'flower'),__('% Comments', 'flower')); ?>
										</span>	
								<?php } ?>							

							</div> <!-- end flower-meta -->

							<div class="category-writing"><?php the_excerpt(); ?></div>

						</article>	<!-- end category-content -->

						<?php endwhile; ?>

						<?php endif; ?>

</div> <!-- category-posts -->

<?php } } ?>


<?php if(isset($flower['categories-2'])){ ?>

<?php if($flower['categories-2']){ ?>

<div class="category-posts-inline col-sm-6">	
							
						<div class="category-head">

							<h3><?php echo get_cat_name($flower['categories-2']); ?></h3>

						</div> <!-- end category-head -->


						<?php if(have_posts()): ?>

							<?php $category=new WP_Query(array(
								'posts_per_page' => 4,
								'cat' => $flower['categories-2']
							)); ?>

						<?php while($category->have_posts()):$category->the_post(); ?>

						<article class="category-content featured-category-2">
							
							<div class="img-wrap">

								<?php the_post_thumbnail(); ?>

								<div class="overlay">
									<a href="<?php the_permalink(); ?>"><i class="fa fa-link"></i></a>
								</div> <!-- end overlay -->
							</div> <!-- end img-wrap -->

							<h3 class="title"><a href="<?php the_permalink(); ?>"><?php the_title(); ?></a></h3> <!-- end title -->

							<div class="flower-meta">

								<span class="date"><i class="fa fa-calendar-o"></i><?php the_date(get_option('date_format')); ?></span>&nbsp;&nbsp;&nbsp;
								
								<?php if(comments_open()){ ?>
										<span class="comment-count"><i class="fa fa-comment-o"></i>
											<?php comments_number(__('No Comments', 'flower'),__('1 Comment', 'flower'),__('% Comments', 'flower')); ?>
										</span>	
								<?php } ?>							

							</div> <!-- end flower-meta -->

							<div class="category-writing"><?php the_excerpt(); ?></div>

						</article>	<!-- end category-content -->

						<?php endwhile; ?>

						<?php endif; ?>

</div> <!-- category-posts -->
<?php } } ?>


<?php if(isset($flower['categories-3'])){ ?>

<?php if($flower['categories-3']){ ?>

<div class="category-posts-inline col-sm-6">	
							
						<div class="category-head">

							<h3><?php echo get_cat_name($flower['categories-3']); ?></h3>

						</div> <!-- end category-head -->


						<?php if(have_posts()): ?>

							<?php $category=new WP_Query(array(
								'posts_per_page' => 4,
								'cat' => $flower['categories-3']
							)); ?>

						<?php while($category->have_posts()):$category->the_post(); ?>

						<article class="category-content featured-category-3">
							
							<div class="img-wrap">

								<?php the_post_thumbnail(); ?>

								<div class="overlay">
									<a href="<?php the_permalink(); ?>"><i class="fa fa-link"></i></a>
								</div> <!-- end overlay -->
							</div> <!-- end img-wrap -->

							<h3 class="title"><a href="<?php the_permalink(); ?>"><?php the_title(); ?></a></h3> <!-- end title -->

							<div class="flower-meta">

								<span class="date"><i class="fa fa-calendar-o"></i><?php the_date(get_option('date_format')); ?></span>&nbsp;&nbsp;&nbsp;
								
								<?php if(comments_open()){ ?>
										<span class="comment-count"><i class="fa fa-comment-o"></i>
											<?php comments_number(__('No Comments', 'flower'),__('1 Comment', 'flower'),__('% Comments', 'flower')); ?>
										</span>	
								<?php } ?>							

							</div> <!-- end flower-meta -->

							<div class="category-writing"><?php the_excerpt(); ?></div>

						</article>	<!-- end category-content -->

						<?php endwhile; ?>

						<?php endif; ?>

</div> <!-- category-posts -->
<?php } } ?>


<?php if(isset($flower['categories-4'])){ ?>

<?php if($flower['categories-4']){ ?>

<div class="category-posts-inline col-sm-6">	
							
						<div class="category-head">

							<h3><?php echo get_cat_name($flower['categories-4']); ?></h3>

						</div> <!-- end category-head -->


						<?php if(have_posts()): ?>

							<?php $category=new WP_Query(array(
								'posts_per_page' => 4,
								'cat' => $flower['categories-4']
							)); ?>

						<?php while($category->have_posts()):$category->the_post(); ?>

						<article class="category-content featured-category-4">
							
							<div class="img-wrap">

								<?php the_post_thumbnail(); ?>

								<div class="overlay">
									<a href="<?php the_permalink(); ?>"><i class="fa fa-link"></i></a>
								</div> <!-- end overlay -->
							</div> <!-- end img-wrap -->

							<h3 class="title"><a href="<?php the_permalink(); ?>"><?php the_title(); ?></a></h3> <!-- end title -->

							<div class="flower-meta">

								<span class="date"><i class="fa fa-calendar-o"></i><?php the_date(get_option('date_format')); ?></span>
								
								<?php if(comments_open()){ ?>
										<span class="comment-count"><i class="fa fa-comment-o"></i>
											<?php comments_number(__('No Comments', 'flower'),__('1 Comment', 'flower'),__('% Comments', 'flower')); ?>
										</span>	
								<?php } ?>							

							</div> <!-- end flower-meta -->

							<div class="category-writing"><?php the_excerpt(); ?></div>

						</article>	<!-- end category-content -->

						<?php endwhile; ?>

						<?php endif; ?>

</div> <!-- category-posts -->
<?php } } ?>




