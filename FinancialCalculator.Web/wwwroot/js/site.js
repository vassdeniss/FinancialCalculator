function scrollToTop() {
    window.scrollTo({
        top: 0,
        behavior: 'smooth',
    });
}

function validate(element, maxLength) {
    element.value = element.value.replace(/[^0-9.]/g, '').replace(/(,.*?),/g, '$1');

    const parts = element.value.split('.');
    const beforeComma = parts[0];
    const afterComma = parts[1] || '';

    if (beforeComma.length > maxLength) {
        parts[0] = beforeComma.slice(0, maxLength);
    }

    element.value = parts[1] !== undefined ? parts[0] + '.' + afterComma : parts[0];
}
