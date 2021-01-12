/* =========================================
                Preloader
============================================ */
$(window).on('load', function () { // makes sure that whole site is loaded
    $('#status').fadeOut();
    $('#preloader').delay(350).fadeOut('slow');
});


/* =========================================
      Password Toggle Eye Effect
============================================ */
$(function () {

    $(".toggle-password").click(function () {

        $(this).toggleClass("fa-eye fa-eye-slash");
        
        var input = $($(this).attr("toggle"));
        
        if (input.attr("type") == "password") {
            input.attr("type", "text");
        } 
        else 
        {
            input.attr("type", "password");
        }
    });

});

/* =========================================
              Mobile Menu
============================================ */

$(function () {

    $("#mobile-menu-button i").click(function () {

        $(this).toggleClass("fa-bars fa-times");
    });

});



/*==================================================
                    FAQ
==================================================*/

//faq toggle stuff
$(".question-toggle").click(function (e) {
    e.preventDefault();
    var notthis = $(".active").not(this);
    notthis
        .find(".fa-minus")
        .addClass("fa-plus")
        .removeClass("fa-minus");
    notthis
        .toggleClass("active")
        .next(".faqanswer")
        .slideToggle(300);
    $(this)
        .toggleClass("active")
        .next()
        .slideToggle("fast");
    $(this)
        .find("i")
        .toggleClass("fa-plus fa-minus");
});


/*==================================================
                    Navbar
==================================================*/
$(function () {

    //Show/Hide nav on page load
    ShowHideNav();

    $(window).scroll(function () {

        //Show/Hide nav on window's Scroll
        ShowHideNav();

    });

    function ShowHideNav() {

        if ($(window).scrollTop() > 50) {

            // Show white nav
            $("header").removeClass("navbar-transparent");

            // Show dark logo 
            $(".navbar-brand-2 img").attr("src", "images/Login/top-logo_2.png");

        } else {

            // Hide white nav
            $("header").addClass("navbar-transparent");

            // Show dark logo 
            $(".navbar-brand-2 img").attr("src", "images/Login/top-logo.png");

        }
        if ($(window).width() < 991) {
            $('header').removeClass('navbar-transparent');
            // Show dark logo 
            $(".navbar-brand img").attr("src", "images/Login/top-logo_2.png");
        } else {
            //
        }

    }

});


/*==================================================
                    add and remove active class in navbar
==================================================*/
$( '.navbar .navbar-nav a' ).on( 'click', function () {
	$( '.navbar .navbar-nav' ).find( 'a.active' ).removeClass( 'active' );
	$( this ).addClass( 'active' );
});


/*==================================================
                    File input
==================================================*/

$(".custom-file-input").on("change", function() {
  var fileName = $(this)
    .val()
    .split("\\")
    .pop();
  $(this)
    .siblings(".custom-file-label")
    .addClass("selected")
    .html(fileName);
});