<?php
/**
 * The sidebar containing the front page widget areas.
 * If there are no active widgets, the sidebar will be hidden completely.
 *
 * @package Quark
 * @since Quark 1.0
 */
?>
	<?php
	// Count how many front page sidebars are active so we can work out how many containers we need
	$footerSidebars = 0;
	for ( $x=1; $x<=4; $x++ ) {
		if ( is_active_sidebar( 'sidebar-homepage' . $x ) ) {
			$footerSidebars++;
		}
	}

	// If there's one or more one active sidebars, create a row and add them
	if ( $footerSidebars > 0 ) { ?>
		<div id="secondary" class="home-sidebar row">
			<?php
			// Work out the container class name based on the number of active front page sidebars
			$containerClass = "grid_" . 12 / $footerSidebars . "_of_12";

			// Display the active front page sidebars
			for ( $x=1; $x<=4; $x++ ) {
				if ( is_active_sidebar( 'sidebar-homepage'.  $x ) ) { ?>
					<div class="col <?php echo $containerClass?>">
						<div class="widget-area" role="complementary">
							<?php dynamic_sidebar( 'sidebar-homepage'.  $x ); ?>
						</div> <!-- #widget-area -->
					</div> <!-- /.col.<?php echo $containerClass?> -->
				<?php }
			} ?>
		</div> <!-- /#secondary.row -->

	<?php } ?>
