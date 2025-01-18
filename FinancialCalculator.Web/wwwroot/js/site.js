function scrollToTop() {
    window.scrollTo({
        top: 0,
        behavior: 'smooth',
    });
}

function validate(element, maxLength) {
    element.value = element.value.replace(/\D/g, '');
    
    if (element.value.length > maxLength) {
        element.value = element.value.slice(0, maxLength);
    }
}
