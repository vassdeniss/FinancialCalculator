function scrollToTop() {
    window.scrollTo({
        top: 0,
        behavior: 'smooth',
    });
}

window.addEventListener('load', function () {
    const sections = document.querySelectorAll('section');

    sections.forEach((section) => {
        section.style.opacity = '0';
        section.style.transition = 'opacity 1s ease-in-out';
    });

    let index = 0;
    const revealSection = () => {
        if (index < sections.length) {
            sections[index].style.opacity = '1';
            index++;
        }
    };

    setInterval(revealSection, 500); // Показва всяка секция на интервал от 500ms
});

const hamburger = document.querySelector('.hamburger');
const navLinks = document.querySelector('.nav-links');

hamburger.addEventListener('click', () => {
    navLinks.classList.toggle('active');
});

// JavaScript for Auto Carousel
let currentIndex = 0;
const images = document.querySelectorAll('.carousel img');
const totalImages = images.length;

function showNextImage() {
    images[currentIndex].classList.remove('active');
    currentIndex = (currentIndex + 1) % totalImages;
    images[currentIndex].classList.add('active');
}

setInterval(showNextImage, 3000); // Change image every 3 seconds
const toggles = document.querySelectorAll('.accordion-toggle');

toggles.forEach((toggle) => {
    toggle.addEventListener('click', function () {
        this.classList.toggle('active');
        const content = this.nextElementSibling;
        if (content.style.display === 'block') {
            content.style.display = 'none';
        } else {
            content.style.display = 'block';
        }
    });
});
}
