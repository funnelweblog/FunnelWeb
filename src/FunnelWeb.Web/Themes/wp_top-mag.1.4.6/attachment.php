<?php 
/**
 * The attechment template file.
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
    <div class="col-md-12 singleblog-img no-padding singleblog-contan">
      <?php $top_mag_featured_image = wp_get_attachment_image_src( get_post_thumbnail_id( $post->ID ), 'large' ); 
			 if($top_mag_featured_image[0] != '') { ?>
      <img src="<?php echo esc_url($top_mag_featured_image[0]); ?>" width="<?php echo $top_mag_featured_image[1]; ?>" height="<?php echo $top_mag_featured_image[2]; ?>" alt="<?php echo get_the_title(); ?>" class="img-responsive" />
      <div class="caption-wrap-topimg">
        <div class="caption-date"><span><?php echo get_the_date('d M'); ?></span></div>
        <div class="caption-time"><i class="fa fa-clock-o"></i> <?php echo get_the_date('g:i'); ?></div>
      </div>
      <?php } ?>
      <?php the_content(); ?>
      <p class="post-tags single-tags">
        <?php the_tags('<span>', '</span><span>', '</span>'); ?>
      </p>
    </div>
    <hr class="col-md-12 socialicon-like no-padding" />
    <?php endwhile; ?>
    <?php comments_template(); ?>
  </div>
  <?php get_sidebar(); ?>
</div>
<?php get_footer(); ?>
