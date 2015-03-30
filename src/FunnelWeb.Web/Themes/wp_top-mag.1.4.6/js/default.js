// JavaScript Document
jQuery(document).ready(function(e) {
	jQuery('.dropdown').hover(function(e) {
        jQuery('.dropdown-toggle').trigger('click');
    });
	jQuery( ".footer-tertiary-menu ul" ).addClass( "pull-right" );
});
jQuery(function () {
     // start the ticker 
	jQuery('#js-news').ticker();
	
	// hide the release history when the page loads
	jQuery('#release-wrapper').css('margin-top', '-' + (jQuery('#release-wrapper').height() + 20) + 'px');

	// show/hide the release history on click
	jQuery('a[href="#release-history"]').toggle(function () {	
		jQuery('#release-wrapper').animate({
			marginTop: '0px'
		}, 600, 'linear');
	}, function () {
		jQuery('#release-wrapper').animate({
			marginTop: '-' + (jQuery('#release-wrapper').height() + 20) + 'px'
		}, 600, 'linear');
	});	
	
	
		jQuery("#home-banner").owlCarousel({
			autoPlay: false, //Set AutoPlay to 3 seconds
			items : 4,
			itemsDesktop : [1199,3],
			itemsDesktopSmall : [979,3]
			});
		 // Custom Navigation Events
		 jQuery(".next1").click(function(){
		 jQuery("#home-banner").trigger('owl.next');
		 })
		 jQuery(".prev1").click(function(){
		 jQuery("#home-banner").trigger('owl.prev');
		 })
		
});
