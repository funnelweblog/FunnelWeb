<!DOCTYPE html>
<!--[if IE 8]> <html <?php language_attributes(); ?> class="ie8"> <![endif]-->
<!--[if !IE]><!--> <html <?php language_attributes(); ?>> <!--<![endif]-->

<head>
	<meta charset="<?php bloginfo( 'charset' ); ?>">
	<title><?php wp_title( '|', true, 'right' ); ?></title>	
	
	
	<!-- Mobile Specific Meta -->
	<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">

	<link rel="profile" href="http://gmpg.org/xfn/11">
	<link rel="pingback" href="<?php bloginfo( 'pingback_url' ); ?>">

	<!--[if lt IE 9]>
	<script src="<?php echo get_template_directory_uri(); ?>/js/html5.js"></script>
	<![endif]-->

<?php wp_head(); ?>
</head>
<body <?php body_class(); ?>>

<?php global $flower; ?>


<!-- HEADER -->
	<header class="top-menu">

		<div class="container">
			
			<div class="row">				

				<!-- Top Menu -->
				<?php wp_nav_menu(array(
					'theme_location' => 'top-menu',
					'container' => 'nav',
					'container_class' => 'col-sm-9',					
					'menu_class' => 'nav navbar-nav pull-left',	
					'fallback_cb' => 'flower_menu_page',			
					'depth' => 3
				)); ?>
				<!-- End Top Menu -->				


				<div class="header-meta col-sm-3">
					<ul>
						<li><i class="fa fa-calendar-o"></i></li>
						<li><?php echo date('l'); ?></li>
						<li><?php echo date( get_option( 'date_format' ) ); ?></li>
					</ul>
				</div>


			</div> <!-- end row -->

		</div><!-- end container -->		
		
	</header>
	<!-- END HEADER -->


	<!-- MOBILE HEADER -->
	<header class="mobile-top-menu">

		<div class="container">
			
			<div class="row">

				<div class="col-sm-3">
				
					<?php if(isset($flower['logo']['url'])){ ?>

							<?php if($flower['logo']['url']){ ?>

								<a href="<?php echo esc_url(home_url('/')); ?>" title='<?php bloginfo('name'); ?>'><img src="<?php echo esc_attr($flower['logo']['url']); ?>" class="logo img-responsive" alt="<?php bloginfo('name'); ?>"></a>							

							<?php } } ?>


						<?php if(isset($flower['logo']['url'])){ ?>

							<?php if(!$flower['logo']['url']){ ?>

								<h3><a href="<?php echo esc_url(home_url('/')); ?>" title="<?php bloginfo('description'); ?>"><?php bloginfo('name' ); ?></a></h3>

						<?php } } ?>

					<i class="fa fa-align-justify mobile-fa"></i>
				</div> <!-- end col-sm-3 -->

				<!-- Top Menu -->
				<?php wp_nav_menu(array(
					'theme_location' => 'top-menu-mobile',
					'container' => 'nav',
					'container_class' => 'col-sm-9',					
					'menu_class' => 'nav navbar-nav mobile-menu',	
					'fallback_cb' => 'flower_menu_page',			
					'depth' => 3
				)); ?>
				<!-- End Top Menu -->				

			</div> <!-- end row -->

		</div><!-- end container -->		
		
	</header>
	<!-- END HEADER -->


	

	<section class="section-logo-search">

		<div class="container">
			
			<div class="row">				
					
					<div class="col-sm-3">						

						<?php if(isset($flower['logo']['url'])){ ?>

							<?php if($flower['logo']['url']){ ?>

								<a href="<?php echo esc_url(home_url('/')); ?>" title='<?php bloginfo('name'); ?>'><img src="<?php echo esc_attr($flower['logo']['url']); ?>" class="logo img-responsive" alt="<?php bloginfo('name'); ?>"></a>							

							<?php } } ?>


						<?php if(isset($flower['logo']['url'])){ ?>

							<?php if(!$flower['logo']['url']){ ?>

								<h3><a href="<?php echo esc_url(home_url('/')); ?>" title="<?php bloginfo('description'); ?>"><?php bloginfo('name' ); ?></a></h3>

						<?php } } ?>			
					


					</div> <!-- end col-sm-3 -->

					<?php get_search_form(); ?>				

			</div> <!-- end row -->

		</div> <!-- end container -->

	</section> <!-- end section-logo -->




	<section class="middle-menu">

		<div class="container">
			
			<div class="row">

				<i class="fa fa-align-justify"></i>
				
				<!-- Top Menu -->
				<?php wp_nav_menu(array(
					'theme_location' => 'middle-menu',
					'container' => 'nav',
					'container_class' => 'col-sm-12',					
					'menu_class' => 'nav navbar-nav pull-left',	
					'fallback_cb' => 'flower_menu_page_two',			
					'depth' => 3
				)); ?>
				<!-- End Top Menu -->


				

			</div> <!-- end row -->

		</div> <!-- end container -->



	</section> <!-- end middle-menu -->