<div class="sidebar col-sm-4">					

		<?php if(!dynamic_sidebar('sidebar')): ?>

			<div class="current-sidebar">
				<h3><?php bloginfo('name'); ?></h3>
				<p><?php bloginfo('description' ); ?></p>
			</div>

		<?php endif; ?>

</div> <!-- end sidebar -->