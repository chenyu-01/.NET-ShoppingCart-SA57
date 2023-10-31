
    const nameInput = document.getElementById('searchInput');
    const form = document.getElementById('searchForm');

nameInput.addEventListener('input', () => {
    if (nameInput.value.length == 0) {
        form.submit();
    }
});