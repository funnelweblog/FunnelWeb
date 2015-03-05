<?php if(function_exists('register_field_group')){ ?>

<section class="section-slider">

    <div class="flexslider">   


      <ul class="slides">

      <?php 

        $slider=get_posts(array(
          'numberposts' => -1,
          'post_type' => 'post',
          'meta_key' => 'slider_options'

        ));

       ?>


      <?php foreach($slider as $slide){ ?>

      <?php if(function_exists('get_field')){
              $click=get_field('slider_options',$slide->ID);
        } ?>

      

      <?php if(isset($click)){

          if(!empty($click)){ ?>

              <li>

                <?php $image=get_field('slider_image',$slide->ID); ?>

                <img src="<?php echo $image['url']; ?>" />          

                  <div class="slider-content">
                    
                <h2><a href="<?php echo esc_url(get_permalink($slide->ID)); ?>"><?php echo get_the_title($slide->ID); ?></a></h2>

                <a href="<?php echo esc_url(get_permalink($slide->ID)); ?>" class="btn"><?php _e('Read More','flower'); ?></a>

                  </div>

             </li>

        <?php }

        } ?>  

        <?php } ?>   
      

      </ul>

    </div> 

</section>

<?php } ?>