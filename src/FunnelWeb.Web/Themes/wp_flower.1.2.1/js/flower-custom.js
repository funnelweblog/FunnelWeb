jQuery(function($){


/**********************************************************************************
/* Top Menu *
**********************************************************************************/
$('.top-menu .nav li').hover(function(){

	$(this).children('ul').stop().slideToggle(200);

	$(this).children('a').toggleClass('active');

});








/**********************************************************************************
/* Mobile Top Menu *
**********************************************************************************/
$('.mobile-fa').click(function(e){

	$('.mobile-menu').slideToggle(200);

});



$('.mobile-top-menu .nav li').hover(function(e){	

	$(this).children('ul').stop().slideToggle(200);
	
});




/**********************************************************************************
/* Middle Menu *
**********************************************************************************/
if($(window).width()>767){

	$('.middle-menu .nav li').hover(function(){

		$(this).children('ul').stop().slideToggle(150);

		$(this).children('a').toggleClass('active-2');


	});

}else{


	$('.middle-menu .fa').click(function(){

		$('.middle-menu .nav').stop().slideToggle(200);

	});

	$('.middle-menu .nav li').hover(function(e){

		e.preventDefault();

		$(this).children('ul').stop().slideToggle(150);

		

	});

}




/**********************************************************************************
/* Content *
**********************************************************************************/
$('.img-wrap').hover(function(){

	$(this).children('.overlay').stop().fadeToggle(200);

});








/**********************************************************************************
/* Go Top Button *
**********************************************************************************/
$(window).scroll(function(){

	if($(window).scrollTop() > 1000){
		$('.go-top').fadeIn(0);
	}else{
		$('.go-top').fadeOut(0);
	}


});




$('.go-top').click(function(){

	$('html, body').animate({scrollTop:0},600);

});






/**********************************************************************************
/* Slider *
**********************************************************************************/
$('.flexslider').flexslider({
        animation: "fade"
        
      });






/************************************************************************
// Search Form
*************************************************************************/
$('.fa-search').click(function(){

	$('#searchform').submit();

});



/************************************************************************
// Featured Category
*************************************************************************/

$('.featured-category-1:gt(0), .featured-category-2:gt(0), .featured-category-3:gt(0), .featured-category-4:gt(0)').find('img').addClass('category-content-small-img');

$('.featured-category-1:gt(0), .featured-category-2:gt(0), .featured-category-3:gt(0), .featured-category-4:gt(0)').find('.category-writing').addClass('category-display-content');

$('.featured-category-1:gt(0), .featured-category-2:gt(0), .featured-category-3:gt(0), .featured-category-4:gt(0)').addClass('category-content-small');

$('.featured-category-1:gt(0), .featured-category-2:gt(0), .featured-category-3:gt(0), .featured-category-4:gt(0)').find('.overlay').addClass('category-display-content');

$('.featured-category-1:gt(0), .featured-category-2:gt(0), .featured-category-3:gt(0), .featured-category-4:gt(0)').find('.img-wrap').addClass('category-display-content-overlay');

$('.featured-category-1:gt(0), .featured-category-2:gt(0), .featured-category-3:gt(0), .featured-category-4:gt(0)').find('.title').addClass('category-content-small-h4');



/**********************************************************************************
/* Responsive Video *
**********************************************************************************/
$(".container").fitVids();


});