<?php 
/**
 * The 404 template file.
**/
get_header(); ?>
<div class="col-md-12 blogpost-list">
  <div class="jumbotron">
    <h1>
      <?php _e('Epic 404 - Article Not Found','top-mag'); ?>
    </h1>
    <p>
      <?php _e("This is embarassing. We can't find what you were looking for.",'top-mag'); ?>
    </p>
    <section class="post_content">
      <p>
        <?php _e('Whatever you were looking for was not found, but maybe try looking again or search using the form below.','top-mag'); ?>
      </p>
      <div class="row">
        <div class="col-sm-12">
          <?php get_search_form(); ?>
        </div>
      </div>
    </section>
  </div>
</div>
<?php get_footer(); ?>
