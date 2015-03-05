<?php
/*
 * the main sidebar file
 */
?>
<div class="col-md-4 main-sidebar no-padding-right">
  <?php 
	if(is_active_sidebar('main-sidebar')) {
		dynamic_sidebar('main-sidebar');	
	}
?>
</div>
