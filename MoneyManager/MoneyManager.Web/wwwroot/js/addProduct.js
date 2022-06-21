let dataTable;

$(document).ready(function () {
    dataTable = $("#productTable").DataTable({
        "ajax": {
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