<?php
/**
 * The template for displaying Comments.
 */
if ( post_password_required() )
	return;
?>

<div class="clearfix"></div>
<div id="comments" class="comments-area">
  <?php if ( have_comments() ) : 	?>
  <div class="col-md-12 no-padding clearfix"> <span class="recent-posts-title"><?php echo get_comments_number().__(' Comments','top-mag'); ?></span> </div>
  <ul class="">
    <?php wp_list_comments( array( 'callback' => 'top_mag_comment', 'short_ping' => true, 'style' => 'ul' ) ); ?>
  </ul>
  <?php paginate_comments_links(); ?>
  <?php endif; // have_comments() ?>
  <?php comment_form(); ?>
</div>
<!-- #comments .comments-area -->
