<?php 
/**
 * Template Name: Slider Home Page
**/
get_header(); 
$top_mag_options = get_option( 'topmag_theme_options' );
?>       
<!-- home slider -->
  <?php
  if(!empty($top_mag_options['post-slider-category'])) {
  $top_mag_slider_post_args = array(
								'orderby'          => 'post_date',
								'order'            => 'DESC',
								'post_type'        => 'post',
								'post_status'      => 'publish',
								'cat'    =>  $top_mag_options['post-slider-category'],
							);	
	$top_mag_slider_post = new WP_Query( $top_mag_slider_post_args );	
        if($top_mag_slider_post->max_num_pages != 0) {					
  ?>                          
 <div class="col-md-12">
   <div id="slider1_container">
        <!-- Loading Screen -->
        <div u="loading" class="slider-loader">
            <div class="slider-loader-content"></div>
            <div class="slider-loader-wrapper"></div>
        </div>

        <!-- Slides Container -->
        <div u="slides" class="slider-image-content">
            <?php
				while ( $top_mag_slider_post->have_posts() ) { $top_mag_slider_post->the_post();
				$top_mag_featured_image = wp_get_attachment_image_src(get_post_thumbnail_id(get_the_id()),'large');
				if($top_mag_featured_image[0] != ''){
			?>
            <div>
                <img u="image" alt="<?php echo get_the_title(); ?>" src="<?php echo esc_url($top_mag_featured_image[0]); ?>" width="<?php echo $top_mag_featured_image[1]; ?>" height="<?php echo $top_mag_featured_image[2]; ?>"   />
            </div>
            <?php } ?>
            <?php } ?>
        </div>
        
    </div> 
  </div>
  <?php } } ?>
  
  <!-- Recent Post -->
  <div class="col-md-12 no-padding ">
    <div class="col-md-12 clearfix"> <span class="recent-posts-title"><?php _e('Recent Posts','top-mag'); ?></span> <span  class="prev prev1"><img src="<?php echo get_template_directory_uri(); ?>/images/right-arrow.png" class="img-responsive" /> </span> <span  class="next next1"><img src="<?php echo get_template_directory_uri(); ?>/images/left-arrow.png" class="img-responsive" /></span> </div>
    <div class="col-md-12 clearfix" id="home-banner">
    <?php
	if(!empty($top_mag_options['recent-post-number'])) { $top_mag_recent_post_page = esc_attr($top_mag_options['recent-post-number']); } else { $top_mag_recent_post_page = get_option('posts_per_page');  } 
	    $top_mag_recent_post_args = array(
					'posts_per_page'   => $top_mag_recent_post_page,
                    'orderby'          => 'post_date',
                    'order'            => 'DESC',
                    'post_type'        => 'post',
                    'post_status'      => 'publish'
                );
    $top_mag_recent_post = new WP_Query( $top_mag_recent_post_args );
    while ( $top_mag_recent_post->have_posts() ) { $top_mag_recent_post->the_post();
	$top_mag_featured_image = wp_get_attachment_image_src(get_post_thumbnail_id(get_the_id()),'medium');
	?>
          <div class="col-md-12 recent-posts clearfix item">
            <div class="blog-post">
              <div class="ImageWrapper chrome-fix">
			  	<?php if($top_mag_featured_image[0] != '')  { ?>
                	<img src="<?php echo esc_url($top_mag_featured_image[0]); ?>" width="<?php echo $top_mag_featured_image[1]; ?>" height="<?php echo $top_mag_featured_image[2]; ?>" alt="<?php echo get_the_title(); ?>" class="img-responsive" />
                <?php } else { ?>    
                	<img src="<?php echo get_template_directory_uri(); ?>/images/no-image.png" width="320" height="221" alt="<?php echo get_the_title(); ?>" class="img-responsive" />
                <?php } ?>    
                <div class="ImageOverlayC"></div>
                <div class="Buttons StyleH"> <span class="WhiteRounded"><a href="<?php echo get_permalink(); ?>"><i class="fa fa-picture-o"></i></a> </span> </div>
                <?php if(get_the_tag_list()) { ?>
                <div class="caption-wrap">
                  <div class="caption"><?php the_tags('',', '); ?></div>
                </div>
				<?php } ?>
              </div>
            </div>
            <div>
              <p><a href="<?php echo get_permalink(); ?>" class="home-recent-post-slider"><?php echo get_the_title(); ?></a></p>
            </div>
            <div class="line-hr"></div>
            <div class="col-md-12 no-padding postblog">
              <div class="post-comment col-md-12 no-padding"> <span><i class="fa fa-clock-o"></i> <?php echo get_the_date(); ?></span> <span><?php if(get_comments_number() != 0) : ?><i class="fa fa-comments-o"></i> <?php comments_number( '0', '1', '%' ); ?></span><?php  endif; ?> </div>
            </div>
          </div>
    <?php } ?>      
      </div>
  </div>
  <!-- End Recent Post --> 
  
  <div class="col-md-12 technology-sports-news">
    <div class="col-md-8 news-list no-padding-left clearfix"> 
      <div class="col-md-12 no-padding-left">
        <h1 class="home_category_title"> <?php if(!empty($top_mag_options['home-post-category'])) { echo esc_attr($top_mag_options['home-post-category']); } else { _e('All Posts','top-mag'); } ?></h1>
        <?php
		 $paged = ( get_query_var( 'page' ) ) ? get_query_var( 'page' ) : 1;
		 if(!empty($top_mag_options['post-number'])) { $top_mag_post_page = esc_attr($top_mag_options['post-number']); } else { $top_mag_post_page = get_option('posts_per_page');  } 
		    $top_mag_args = array( 
					'posts_per_page'   => $top_mag_post_page,
                    'orderby'          => 'post_date',
                    'order'            => 'DESC',
                    'post_type'        => 'post',
                    'post_status'      => 'publish',
					'paged' 		   => $paged,
                    'category_name'    => esc_attr($top_mag_options['home-post-category'])
                );
		$top_mag_single_post = new WP_Query( $top_mag_args );
	   
		while ( $top_mag_single_post->have_posts() ) { $top_mag_single_post->the_post();
		$top_mag_featured_image = wp_get_attachment_image_src(get_post_thumbnail_id(get_the_id()),'medium');
		?> 
        <div class="home-category-post">
        <div class="news-list-col1 no-padding">
          <div class="ImageWrapper chrome-fix"> 
            <?php if($top_mag_featured_image[0] != '')  { ?>
                	<img src="<?php echo esc_url($top_mag_featured_image[0]); ?>" width="<?php echo $top_mag_featured_image[1]; ?>" height="<?php echo $top_mag_featured_image[2]; ?>" alt="<?php echo get_the_title(); ?>" class="img-responsive" />
			<?php } else { ?>    
                <img src="<?php echo get_template_directory_uri(); ?>/images/no-image.png" alt="No image" width="320" height="221" class="img-responsive" />
            <?php } ?>  
            <div class="ImageOverlayC"></div>
            <div class="Buttons StyleH"> <span class="WhiteRounded"><a href="<?php echo get_permalink(); ?>"><i class="fa fa-picture-o"></i></a> </span> </div>
          </div>
        </div>
        <div class="news-list-col2 no-padding">
          <h2><a href="<?php echo get_permalink(); ?>"><?php echo get_the_title(); ?></a></h2>
          <p> <?php the_excerpt(); ?> </p>
          <div class="news-post-comment"> <span><i class="fa fa-clock-o"></i> <?php echo get_the_date(); ?></span> <?php top_mag_entry_meta(); ?> </div>
        </div> 
        </div>
        <?php } ?>
        <div class="col-md-12 top_mag_pagination"> 
			<?php if(function_exists('faster_pagination')) { faster_pagination($top_mag_single_post->max_num_pages,1); } else { ?>     
            <div class="top_mag_previous_pagination"><?php previous_posts_link( '&laquo; Previous Page', $top_mag_single_post->max_num_pages ); ?></div>
            <div class="top_mag_next_pagination"><?php next_posts_link( 'Next Page &raquo;', $top_mag_single_post->max_num_pages ); ?></div>
			<?php } ?>
    	</div>
      </div>
    </div>
    
	<?php get_sidebar(); ?>

  </div>
   
<?php get_footer(); ?>
