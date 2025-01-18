function toggleFields(groupId) {
    const group = document.getElementById(groupId);
    if (group.classList.contains('hidden')) {
        group.classList.remove('hidden');
    } else {
        group.classList.add('hidden');
    }
}

const input = document.getElementById('paymentType');
const dropdownMenu = document.getElementById('dropdown-menu');

input.addEventListener('click', () => {
    dropdownMenu.classList.toggle('show');
});

dropdownMenu.addEventListener('click', (event) => {
    if (event.target.dataset.value) {
        input.value = event.target.dataset.text;
        
        const hiddenField = document.getElementById('hiddenPaymentType');
        hiddenField.value = event.target.dataset.value;
        
        dropdownMenu.classList.remove('show');
    }
});

document.addEventListener('click', (event) => {
    if (!event.target.closest('.dropdown-field')) {
        dropdownMenu.classList.remove('show');
    }
});
