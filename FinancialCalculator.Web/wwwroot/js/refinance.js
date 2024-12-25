function toggleAdditionalFields() {
    const additionalFields = document.getElementById('additionalFields');
    if (additionalFields.style.display === 'none' || additionalFields.style.display === '') {
        additionalFields.style.display = 'block';
    } else {
        additionalFields.style.display = 'none';
    }
}
