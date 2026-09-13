/**
 * REFACTORED APP.JS FOR BLAZOR SERVER
 * تمام توابع برای فراخوانی مجدد (Re-initialization) بهینه‌سازی شده‌اند.
 */

window.initPageScripts = () => {
    initTabs();
    initFilterModal();
    initDarkMode();
    initOffcanvas();
    initCitySelector();
    initModals();
    initMegaMenu();
    initRatings();
    initTagInputs();
    initAccordions();
    initStickyMenu();
    initPriceRange();
    initCopyButtons();
    // فراخوانی اسلایدرها با تاخیر بسیار کوتاه برای اطمینان از آماده بودن DOM
    setTimeout(initSwiperSliders, 100); 
    console.log("All scripts re-initialized successfully.");
};

// --- ۱. تب‌ها ---
window.initTabs = () => {
    const tabs = document.querySelectorAll('.tab-button');
    const contents = document.querySelectorAll('.tab-content');
    if (!tabs.length) return;

    tabs.forEach(tab => {
        tab.onclick = function () {
            tabs.forEach(b => b.classList.remove("active", "bg-primary/10"));
            this.classList.add("active", "bg-primary/10");
            contents.forEach(c => c.classList.add("hidden"));
            const target = document.getElementById(this.dataset.tab);
            if (target) target.classList.remove("hidden");
        };
    });
};

// --- ۲. مودال فیلتر ---
window.initFilterModal = () => {
    const trigger = document.querySelector('.modal-trigger[data-modal-target="filterModal"]');
    const modal = document.getElementById('filterModal');
    if (!trigger || !modal) return;
    trigger.onclick = () => modal.classList.remove('hidden');
    modal.querySelectorAll('[data-modal-close]').forEach(b => b.onclick = () => modal.classList.add('hidden'));
};

// --- ۳. دارک مود ---
window.initDarkMode = () => {
    const toggle = document.getElementById('dark-mode-toggle');
    if (localStorage.getItem("theme") === "dark") document.documentElement.classList.add("dark");
    if (toggle) {
        toggle.onclick = () => {
            document.documentElement.classList.toggle('dark');
            localStorage.setItem("theme", document.documentElement.classList.contains("dark") ? "dark" : "light");
        };
    }
};

// --- ۴. آکاردئون (اصلاح شده برای جلوگیری از تکرار) ---
window.initAccordions = () => {
    document.querySelectorAll('.accordion-btn').forEach(button => {
        // حذف لیسنر قبلی
        button.removeEventListener('click', accordionClickHandler);
        button.addEventListener('click', accordionClickHandler);
    });
};

function accordionClickHandler() {
    const content = this.nextElementSibling;
    const icon = this.querySelector('.svg-accordion');
    this.classList.toggle('active');
    content.classList.toggle('max-h-0');
    content.classList.toggle('max-h-[500px]');
    if (icon) icon.classList.toggle('rotate-180');
}

// --- ۵. انتخاب‌گر شهر ---
window.initCitySelector = () => {
    const container = document.getElementById('citySelector');
    if (!container) return;
};

// --- ۶. مودال‌ها ---
window.initModals = () => {
    document.querySelectorAll('.modal-trigger').forEach(trigger => {
        trigger.onclick = (e) => {
            e.preventDefault();
            const modal = document.querySelector(`[data-modal-id="${trigger.dataset.modalTarget}"]`);
            if (modal) modal.classList.remove('hidden');
        };
    });
    document.querySelectorAll('[data-modal-close]').forEach(btn => {
        btn.onclick = () => btn.closest('.modal').classList.add('hidden');
    });
};

// --- ۷. مگا منو ---
window.initMegaMenu = () => {
    const megaMenuFire = document.getElementById('mega-menu-fire');
    const megaMenuFireTarget = document.getElementById('mega-menu-fire-target');
    if (megaMenuFire && megaMenuFireTarget) {
        megaMenuFire.onmouseover = () => megaMenuFireTarget.classList.remove('hidden');
        document.onmousemove = (e) => {
            if (!megaMenuFire.contains(e.target) && !megaMenuFireTarget.contains(e.target)) {
                megaMenuFireTarget.classList.add('hidden');
            }
        };
    }
};

window.toggleOffcanvas = (id) => {
    const el = document.getElementById(id);
    const overlay = document.querySelector('.overlay');
    if (!el) return;

    // بستن سایر offcanvasها
    document.querySelectorAll('.offcanvas').forEach(o => {
        o.classList.remove('visible', 'translate-x-0', 'opacity-100');
        o.classList.add('invisible', 'translate-x-full', 'opacity-0');
        o.setAttribute('aria-hidden', 'true');
    });

    // باز کردن offcanvas موردنظر
    el.classList.remove('invisible', 'translate-x-full', 'opacity-0');
    el.classList.add('visible', 'translate-x-0', 'opacity-100');
    el.setAttribute('aria-hidden', 'false');

    if (overlay) {
        overlay.classList.remove('hidden');
    }
}

window.closeOffcanvas = () => {
    document.querySelectorAll('.offcanvas').forEach(o => {
        o.classList.remove('visible', 'translate-x-0', 'opacity-100');
        o.classList.add('invisible', 'translate-x-full', 'opacity-0');
        o.setAttribute('aria-hidden', 'true');
    });

    const overlay = document.querySelector('.overlay');
    if (overlay) {
        overlay.classList.add('hidden');
    }
};
window.toggleDropdown = (id) => {
    const menu = document.getElementById(id);
    if (!menu) return;

    const button = document.querySelector(`[aria-controls="${id}"]`);
    const icon = button?.querySelector('.dropdown-icon');
    const isHidden = menu.classList.contains('hidden');

    // اول همه dropdownهای باز را ببند
    document.querySelectorAll('.dropdown-menu').forEach(m => {
        m.classList.add('hidden');
    });

    document.querySelectorAll('[aria-controls]').forEach(b => {
        b.setAttribute('aria-expanded', 'false');
        b.querySelector('.dropdown-icon')?.classList.remove('rotate-180');
    });

    if (isHidden) {
        menu.classList.remove('hidden');
        button?.setAttribute('aria-expanded', 'true');
        icon?.classList.add('rotate-180');

        // ✅ بستن هنگام کلیک بیرون
        setTimeout(() => {
            document.addEventListener('click', function outsideClick(e) {
                if (!menu.contains(e.target) && !button.contains(e.target)) {
                    menu.classList.add('hidden');
                    button?.setAttribute('aria-expanded', 'false');
                    icon?.classList.remove('rotate-180');
                    document.removeEventListener('click', outsideClick);
                }
            });
        }, 0);
    }
};



window.initRatings = () => {
    document.querySelectorAll('input[name="rating"]').forEach(star => {
        star.onchange = () => {
            const index = Array.from(document.querySelectorAll('input[name="rating"]')).indexOf(star);
            document.querySelectorAll('label svg').forEach((svg, i) => {
                svg.classList.toggle('text-orange-300', i <= index);
            });
        };
    });
};

window.initTagInputs = () => { };
window.initStickyMenu = () => { };
window.initPriceRange = () => { };
window.initCopyButtons = () => { };
window.initOffcanvas = () => {
    const overlay = document.querySelector('.overlay');
    if (overlay) {
        overlay.onclick = closeOffcanvas;
    }
};


window.initSwiperSliders = function () {
    // 1. بررسی وجود کتابخانه Swiper
    if (typeof Swiper === 'undefined') {
        console.error("Swiper library is not loaded!");
        return;
    }

    // 2. تابع کمکی برای ایجاد اسلایدر
    const createSwiper = (selector, config) => {
        const elements = document.querySelectorAll(selector);
        elements.forEach(el => {
            // چک کردن اینکه آیا قبلاً مقداردهی شده یا نه
            if (!el.swiper) {
                new Swiper(el, config);
            }
        });
    };
    createSwiper(".Product-carousel", {
        loop: true,
        slidesPerView: 1, // یا تعداد دلخواه
        spaceBetween: 20,
        navigation: {
            nextEl: ".swiper-button-next",
            prevEl: ".swiper-button-prev"
        },
        breakpoints: {
            640: { slidesPerView: 2 },
            1024: { slidesPerView: 3 }
        }
    });
    createSwiper(".product-list-carousel", {
        slidesPerView: 1,
        spaceBetween: 10,
        // اگر دکمه‌های ناوبری در این بخش خاص ندارید، این دو خط را حذف کنید:
        navigation: {
            nextEl: ".swiper-button-next",
            prevEl: ".swiper-button-prev"
        },
        breakpoints: {
            640: { slidesPerView: 2 },
            1024: { slidesPerView: 4 } // تنظیم تعداد نمایش در صفحات بزرگتر
        }
    });
    // 3. مقداردهی اسلایدرهای مشخص
    createSwiper(".default-carousel", { loop: true, pagination: { el: ".swiper-pagination", clickable: true }, navigation: { nextEl: ".swiper-button-next", prevEl: ".swiper-button-prev" } });
    createSwiper(".amazing-carousel", { slidesPerView: "auto", spaceBetween: 10, freeMode: true });
    createSwiper(".category-carousel", { slidesPerView: 5, spaceBetween: 30, pagination: { el: ".swiper-pagination", clickable: true }, navigation: { nextEl: ".swiper-button-next", prevEl: ".swiper-button-prev" } });
    createSwiper(".product-carousel", {
        slidesPerView: 5, spaceBetween: 10,
        navigation: { nextEl: ".swiper-button-next", prevEl: ".swiper-button-prev" },
        breakpoints: { 100: { slidesPerView: 1 }, 576: { slidesPerView: 2 }, 768: { slidesPerView: 3 }, 1024: { slidesPerView: 4 }, 1400: { slidesPerView: 5 } }
    });
    createSwiper(".default-carousel", { loop: true, pagination: { el: ".swiper-pagination", clickable: true }, navigation: { nextEl: ".swiper-button-next", prevEl: ".swiper-button-prev" } });
    createSwiper(".amazing-carousel", { slidesPerView: "auto", spaceBetween: 10, freeMode: true });
    // گالری محصول
    const galleryOne = document.querySelector("#productGalleryOne");
    const galleryTwo = document.querySelector("#productGalleryTwo");
    if (galleryOne && galleryTwo && !galleryTwo.swiper) {
        const thumbSwiper = new Swiper(galleryOne, { spaceBetween: 10, slidesPerView: 3, freeMode: true, watchSlidesProgress: true });
        new Swiper(galleryTwo, { spaceBetween: 10, navigation: { nextEl: ".swiper-button-next", prevEl: ".swiper-button-prev" }, thumbs: { swiper: thumbSwiper } });
    }
};

// ... سایر توابع (initTabs, initAccordions و ...) را همان‌طور که داشتید نگه دارید ...

