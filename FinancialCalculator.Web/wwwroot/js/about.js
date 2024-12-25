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
