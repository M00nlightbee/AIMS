// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function showAccessDenied() {
    var modal = new bootstrap.Modal(document.getElementById('accessDeniedModal'));
    modal.show();
}

// Filter products based on search and filters
function filterTable() {
    const searchInput = document.getElementById('searchInput').value.toLowerCase();
    const rows = document.querySelectorAll('#inventoryTableBody tr');

    rows.forEach(row => {
        const itemName = row.querySelector('td:nth-child(1)').textContent.toLowerCase();
        row.style.display = itemName.includes(searchInput) ? '' : 'none';
    });
}

// Filter products by category
document.querySelectorAll('.filter-btn').forEach(btn => {
    btn.addEventListener('click', function () {
        const category = btn.getAttribute('data-category');
        document.querySelectorAll('#productGrid .col').forEach(card => {
            if (category === 'All' || card.getAttribute('data-category') === category) {
                card.style.display = '';
            } else {
                card.style.display = 'none';
            }
        });
    });
});

// Filter products by category in dropdown
document.querySelectorAll('.filter-dropdown').forEach(item => {
    item.addEventListener('click', function (e) {
        e.preventDefault();
        const category = item.getAttribute('data-category');
        document.querySelectorAll('#productGrid .col').forEach(card => {
            if (category === 'All' || card.getAttribute('data-category') === category) {
                card.style.display = '';
            } else {
                card.style.display = 'none';
            }
        });

        document.getElementById('categoryDropdown').textContent = category;
    });
});

// Dynamic images for the product
const apiKey = 'Your own Access key';
const productCards = document.querySelectorAll('.card');

// Loop through each product card
productCards.forEach(card => {
    // Get the product name from the h5 element inside the current card
    const query = card.querySelector('.card-title').textContent;

    // Find the image element within the current card
    const imageElement = card.querySelector('img.card-img-top');

    // Make a fetch request to the Unsplash API for each product query
    fetch(`https://api.unsplash.com/search/photos?page=1&query=${query}&client_id=${apiKey}`)
        .then(response => {
            if (!response.ok) {
                console.error(`Unsplash API failed for query: "${query}" with status: ${response.status}`);
                throw new Error('Unsplash API request failed.');
            }
            return response.json();
        })
        .then(data => {
            if (data.results && data.results.length > 0) {

                // Update the image source with the URL from the Unsplash response.
                imageElement.src = data.results[0].urls.regular;
            }
            else {
                console.warn(`No images found for query: "${query}". Using placeholder image.`);
            }
        })
        .catch(error => {
            console.error("Error fetching Unsplash image:", error);
        });
});

// Function to filter the user table
function filterUserTable() {
    const searchInput = document.getElementById('searchInput').value.toLowerCase();
    const branchFilter = document.getElementById('branchFilter').value.toLowerCase();
    const roleFilter = document.getElementById('roleFilter').value.toLowerCase();
    const tableRows = document.querySelectorAll('#userTableBody tr');

    tableRows.forEach(row => {
        const firstName = row.children[0].textContent.toLowerCase();
        const lastName = row.children[1].textContent.toLowerCase();
        const branch = row.children[2].textContent.toLowerCase();
        const role = row.children[3].textContent.toLowerCase();

        // Filter conditions
        const matchesSearch = !searchInput || firstName.includes(searchInput) || lastName.includes(searchInput);
        const matchesBranch = !branchFilter || branch === branchFilter;
        const matchesRole = !roleFilter || role === roleFilter;

        // Show/hide rows based on filters
        if (matchesSearch && matchesBranch && matchesRole) {
            row.style.display = '';
        } else {
            row.style.display = 'none';
        }
    });
}

// Function to show the user added modal
function showUserAddedModal() {
    const userAddedModal = new bootstrap.Modal(document.getElementById('userAddedModal'));
    userAddedModal.show();
}

// Function to show the product added modal
function showProductAddedModal() {
    const productAddedModal = new bootstrap.Modal(document.getElementById('productAddedModal'));
    productAddedModal.show();
}

// Function to show the LogOut modal
function showLoggedOutModal() {
    const loggedOutModal = new bootstrap.Modal(document.getElementById('loggedOutModal'));
    loggedOutModal.show();
}

// Function to filter the inventory table
function filterInventoryTable() {
    const searchInput = document.getElementById('searchInput').value.toLowerCase();
    const branchFilter = document.getElementById('branchFilter').value.toLowerCase();
    const categoryFilter = document.getElementById('categoryFilter').value.toLowerCase();
    const tableRows = document.querySelectorAll('#inventoryTableBody tr');

    tableRows.forEach(row => {
        const itemName = row.children[0].textContent.toLowerCase();
        const category = row.children[1].textContent.toLowerCase();
        const branch = row.children[2].textContent.toLowerCase();

        // Filter conditions
        const matchesSearch = !searchInput || itemName.includes(searchInput);
        const matchesBranch = !branchFilter || branch === branchFilter;
        const matchesCategory = !categoryFilter || category === categoryFilter;

        // Show/hide rows based on filters
        if (matchesSearch && matchesBranch && matchesCategory) {
            row.style.display = '';
        } else {
            row.style.display = 'none';
        }
    });
}

// Generate PDF report
document.addEventListener('DOMContentLoaded', () => {
    const generateReport = document.getElementById('generateReport');
    const table = document.getElementById('inventoryTable');

    generateReport.addEventListener('click', () => {
        const { jsPDF } = window.jspdf;
        const doc = new jsPDF();

        let y = 20;
        doc.text('Inventory Report', 14, 10);
        doc.setFontSize(10);

        // Add headers
        const headers = Array.from(table.querySelectorAll('thead th')).map(th => th.textContent.trim());
        doc.text(headers.join(' | '), 14, y);
        y += 10;

        // Add data rows
        const rows = table.querySelectorAll('tbody tr');
        rows.forEach(row => {
            if (row.style.display !== 'none') {
                const cells = Array.from(row.querySelectorAll('td:not(:last-child)')).map(td => td.textContent.trim());
                doc.text(cells.join(' | '), 14, y);
                y += 10;
            }
        });

        doc.save('inventory_report.pdf');
    });
});


