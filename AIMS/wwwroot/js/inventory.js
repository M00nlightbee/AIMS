// Filter products based on search and filters
function filterTable() {
    const searchInput = document.getElementById('searchInput').value.toLowerCase();
    const rows = document.querySelectorAll('#inventoryTableBody tr');

    rows.forEach(row => {
        const itemName = row.querySelector('td:nth-child(1)').textContent.toLowerCase();
        row.style.display = itemName.includes(searchInput) ? '' : 'none'; // Show or hide rows
    });
}