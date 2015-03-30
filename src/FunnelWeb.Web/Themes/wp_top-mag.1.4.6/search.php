<?php 
/**
 * Search Page template file
**/
get_header(); ?>
<div class="col-md-12 blogpost-list">
  <div class="col-md-8 blogpost no-padding-left clearfix">
    <div class="col-md-12 no-padding">
      <h1 class="post-page-title">
      <?php _e( 'Search Results for', 'top-mag' ); echo ' : '.get_search_query(); ?>
      </h1>
    </div>
    <?php while ( have_posts() ) : the_post(); 
	$top_mag_featured_image = wp_get_attachment_image_src(get_post_thumbnail_id(get_the_id()),'medium');
?>
    <div class="col-md-12 no-padding blogpost-border">
      <div class="blogpost-col1 no-padding">
        <?php if($top_mag_featured_image[0] != '') { ?>
        <img src="<?php echo esc_url($top_mag_featured_image[0]); ?>" width="<?php echo $top_mag_featured_image[1]; ?>" height="<?php echo $top_mag_featured_image[2]; ?>" alt="<?php echo get_the_title(); ?>" class="img-responsive single-post-image" />
        <?php } else { ?>
        <img src="<?php echo get_template_directory_uri(); ?>/images/no-image.png" width="320" height="221" alt="<?php echo get_the_title(); ?>" class="img-responsive single-post-image" />
        <?php } ?>
        <div class="caption-wrap-topimg">
          <div class="caption-date"><span><?php echo get_the_date('d M'); ?></span></div>
          <div class="caption-time"><i class="fa fa-clock-o"></i><?php echo get_the_date('g:i'); ?></div>
        </div>
      </div>
      <div class="blogpost-col2 no-padding">
        <h2>
          <?php the_title(); ?>
        </h2>
        <div class="blogpost-comment">
          <?php top_mag_entry_meta(); ?>
          <p class="post-tags">
            <?php the_tags(); ?>
          </p>
        </div>
        <?php the_excerpt(); ?>
        <div class="blogpost-readmore">
          <p><a href="<?php the_permalink(); ?>">
            <?php _e('Read More','top-mag'); ?>
            </a></p>
        </div>
      </div>
    </div>
    <?php endwhile; ?>
    <div class="col-md-12 top-mag-pagination">
      <?php if(function_exists('faster_pagination')) { faster_pagination('',1); } else { ?>
      <div class="top-mag-previous-pagination"><?php previous_posts_link(); ?></div>
      <div class="top-mag-next-pagination"><?php next_posts_link(); ?></div>
      <?php } ?>
    </div>
  </div>
  <?php get_sidebar(); ?>
</div>
<?php get_footer(); ?>
