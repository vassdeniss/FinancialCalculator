document.querySelector("form").addEventListener("submit", function(event) {
    event.preventDefault(); // Спира стандартното поведение на формата

    const name = document.getElementById("name").value;
    const surname = document.getElementById("surname").value;
    const email = document.getElementById("email").value;
    const subject = document.getElementById("subject").value;

    // Проверка дали всички полета са попълнени
    if (!name || !surname || !email || !subject) {
        alert("Моля, попълнете всички полета!");
        return;
    }

    // Проверка дали имейлът е валиден
    if (!validateEmail(email)) {
        alert("Моля, въведете валиден имейл адрес!");
        return;
    }

    // Извеждане на съобщение за потвърждение
    alert(`Благодарим ви за съобщението, ${name} ${surname}!\nЩе се свържем с вас на имейл: ${email}.`);

    // Изчистване на формата
    document.querySelector("form").reset();
});

// Функция за проверка на имейл адрес
function validateEmail(email) {
    const re = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
    return re.test(String(email).toLowerCase());
}
