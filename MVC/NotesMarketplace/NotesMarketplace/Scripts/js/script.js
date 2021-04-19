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
$(function () {
    
    //FAQ Toggle Key
    $(".faq-toggle-key").click(function (e) {

        e.preventDefault();
        var notthis = $(".active-tab").not(this);
        notthis
            .find(".fa-minus")
            .addClass("fa-plus")
            .removeClass("fa-minus");
        notthis
            .toggleClass("active-tab")
            .next(".faqanswer")
            .slideToggle(300);
        $(this)
            .toggleClass("active-tab")
            .next()
            .slideToggle("fast");
        $(this)
            .find("i")
            .toggleClass("fa-plus fa-minus");
    });

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
            $(".logo-changer img").attr("src", "/Content/Front/images/homepage/logo.png");

        } else {

            // Hide white nav
            $("header").addClass("navbar-transparent");

            // Show dark logo 
            $(".logo-changer img").attr("src", "/Content/Front/images/homepage/top-logo.png");

        }
        if ($(window).width() < 991) {
            $('header').removeClass('navbar-transparent');
            // Show dark logo 
            $(".navbar-brand img").attr("src", "/Content/Front/images/homepage/logo.png");
        } else {
            //
        }

    }

});


/*==================================================
      Add and Remove active class in navbar
==================================================*/

$(document).ready(function () {
    var url2 = window.location.href.split('/');
    var url4 = url2[4].split('?');
    var url5 = url4[0];
    var url = url2.slice(0, 4).join('/') + '/' + url5;
    $('.navbar .navbar-nav a[href="' + url + '"]').addClass('active');
    $('.navbar .navbar-nav a').filter(function () {
        return this.href == url;
    }).addClass('active');
});

$(document).ready(function () {
    var url2 = window.location.href.split('/');
    var url4 = url2[4].split('?');
    var url5 = url4[0];
    var url = url2.slice(0, 4).join('/') + '/' + url5;
    $('.navbar .navbar-nav .dropdown-content-profile a[href="' + url + '"]').parent().parent().addClass('active');
    $('.navbar .navbar-nav .dropdown-content-profile a').filter(function () {
        return this.href == url;
    }).parent().parent().addClass('active');
});

$(document).ready(function () {
    var url2 = window.location.href.split('/');
    var url4 = url2[4].split('?');
    var url5 = url4[0];
    var url = url2.slice(0, 4).join('/') + '/' + url5;
    $('.navbar .navbar-nav .dropdown-content-navbar a[href="' + url + '"]').parent().parent().addClass('active');
    $('.navbar .navbar-nav .dropdown-content-navbar a').filter(function () {
        return this.href == url;
    }).parent().parent().addClass('active');
});

/*==================================================
         Sell Price input unable and disable
==================================================*/

$(document).ready(function () {
    $('input[name=IsPaid]').click(function () {
        if ($('input[name=IsPaid]:checked').val() === "false") {
            $("#sell-price-box").hide();
        } else {
            $("#sell-price-box").show();
        }
    });
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

