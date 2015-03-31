<article id="post-<?php the_ID(); ?>" <?php post_class('standard category-content'); ?>>

	<div class="img-wrap">

			<?php the_post_thumbnail(); ?>

		<div class="overlay">

			<a href="<?php the_permalink(); ?>"><i class="fa fa-link"></i></a>

		</div> <!-- end overlay -->

	</div> <!-- end img-wrap -->							
								

	<h3 class="title"><a href="<?php the_permalink(); ?>"><?php the_title(); ?></a></h3> <!-- end title -->

	<div class="flower-meta">
									
		<span class="date"><i class="fa fa-calendar-o"></i><?php the_date(get_option('date_format')); ?></span>									
		<span class="category"><i class="fa fa-briefcase"></i><?php the_category(' | '); ?></span>
		<?php if(comments_open()){ ?>
			<span class="comment-count"><i class="fa fa-comment-o"></i>
				<?php comments_number(__('No Comments', 'flower'),__('1 Comment', 'flower'),__('% Comments', 'flower')); ?>
			</span>	
		<?php } ?>		

	</div> <!-- end flower-meta -->

	<div class="category-writing"><?php the_excerpt(); ?></div>


	<div class="read-more">
		<a href="<?php the_permalink(); ?>"><?php _e('Read More','flower'); ?></a>
	</div>

</article>	<!-- end category-content -->