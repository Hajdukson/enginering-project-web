window.addEventListener('DOMContentLoaded', () => {
    let dataTable = new DataTable("#productTable", {
        "language": {
            "lengthMenu": "Pokaż _MENU_ rekordów na strone",
            "zeroRecords": "Nothing found - sorry",
            "info": "Showing page _PAGE_ of _PAGES_",
            "infoEmpty": "No records available",
            "infoFiltered": "(filtered from _MAX_ total records)"
        },
        "ajax": {
            type: "GET",
            "url": "/Products/GetAllProducts"
        },
        "columns": [
            { "data": "name" },
            { "data": "category.name" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <a href="/BoughtProducts/AddProduct?id=${data}" class="btn btn-primary">
                            <i class="bi bi-pencil-square"></i> &nbsp; Add Product
                        </a>`
                }
            }
        ]
    })
})