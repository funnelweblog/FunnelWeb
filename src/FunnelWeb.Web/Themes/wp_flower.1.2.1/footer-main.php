<section class="main-footer">		
		
		<div class="container">
			
			<div class="row">				

				<div class="col-sm-4">

					<?php if(!dynamic_sidebar('footer-1')): ?>

						<h3><?php bloginfo('name' ); ?></h3>
						<p><?php bloginfo('description' ); ?></p>

					<?php endif; ?>

				</div>


				<div class="col-sm-4">

					<?php if(dynamic_sidebar('footer-2')); ?>

				</div>


				<div class="col-sm-4">

					<?php if(dynamic_sidebar('footer-3')); ?>
					
				</div>
				

			</div> <!-- end row -->
			

		</div> <!-- end container -->

	
</section> <!-- end bottom-footer -->
<!-- END MAIN FOOTER -->




<!-- Go Top Button -->
<section class="go-top">
		
	<i class="fa fa-sort-desc"></i>

</section>