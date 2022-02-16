// Start Sliders
let contentSlider = new Swiper('.content-slider', {
    speed: 400,
    navigation: {
        nextEl: '.image-slider-next',
        prevEl: '.image-slider-prev',
    },
    pagination: {
        el: '.swiper-pagination',
        type: 'bullets',
        clickable: true,
        dynamicBullets: true,
        dynamicMainBullets: 3
    },
    autoplay: {
        delay: 3000,
    },
    effect: 'fade',
    fadeEffect: {
        crossFade: true,
    }
});



let cardSlider1 = new Swiper('.card-container1', {
    slidesPerView: 1,
    spaceBetween: 40,
    navigation: {
        nextEl: '.card-slider1-next',
        prevEl: '.card-slider1-prev',
    },
    breakpoints: {
        640: {
            slidesPerView: 2,
            spaceBetween: 30,
        },
        1024: {
            slidesPerView: 3,
            spaceBetween: 30,
        },
        1400: {
            slidesPerView: 4,
            spaceBetween: 30,
        },
    }
});
let cardSlider2 = new Swiper('.card-container2', {
    slidesPerView: 1,
    spaceBetween: 40,
    navigation: {
        nextEl: '.card-slider2-next',
        prevEl: '.card-slider2-prev',
    },
    breakpoints: {
        640: {
            slidesPerView: 2,
            spaceBetween: 30,
        },
        1024: {
            slidesPerView: 3,
            spaceBetween: 30,
        },
        1400: {
            slidesPerView: 4,
            spaceBetween: 30,
        },
    }
});
// End Sliders
