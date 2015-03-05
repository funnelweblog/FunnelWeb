<?php 
/**
 * The single template file.
**/
$top_mag_options = get_option( 'topmag_theme_options' );
get_header(); ?>
<div class="col-md-12">
  <div class="col-md-8 single-blog no-padding-left clearfix">
    <?php while ( have_posts() ) : the_post(); ?>
    <div class="col-md-12 no-padding">
      <h1 class="post-page-title">
        <?php the_title(); ?>
      </h1>
      <div class="blogpost-comment">
        <?php top_mag_entry_meta(); ?>
      </div>
    </div>
    <div   id="post-<?php the_ID(); ?>" <?php post_class("col-md-12 singleblog-img no-padding singleblog-contan"); ?>>
      <?php $top_mag_featured_image = wp_get_attachment_image_src(get_post_thumbnail_id(get_the_id()),'large');
			 if($top_mag_featured_image[0] != '') { ?>
      <img src="<?php echo esc_url($top_mag_featured_image[0]); ?>" width="<?php echo $top_mag_featured_image[1]; ?>" height="<?php echo $top_mag_featured_image[2]; ?>" alt="<?php echo get_the_title(); ?>" class="img-responsive alignleft" />
      <div class="caption-wrap-topimg">
        <div class="caption-date"><span><?php echo get_the_date('d M'); ?></span></div>
        <div class="caption-time"><i class="fa fa-clock-o"></i> <?php echo get_the_date('g:i'); ?></div>
      </div>
      <?php } ?>
      <?php the_content(); wp_link_pages(); ?>
      <p class="post-tags single-tags">
        <?php the_tags('<span>', '</span><span>', '</span>'); ?>
      </p>
    </div>
    <hr class="col-md-12 socialicon-like no-padding" />
    <div class="col-md-12 no-padding post-slider-video single-pagination clearfix"> <span class="pull-left">
      <?php previous_post_link(); ?>
      </span> <span class="pull-right">
      <?php next_post_link(); ?>
      </span> </div>
    <?php endwhile; ?>
    <?php comments_template(); ?>
  </div>
  <?php get_sidebar(); ?>
</div>
<?php get_footer(); ?>
