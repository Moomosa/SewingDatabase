// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener("DOMContentLoaded", function () {
    var tableRows = document.querySelectorAll(".table-row-clickable");
    tableRows.forEach(function (row) {
        row.addEventListener("click", function () {
            var href = row.getAttribute("data-href");
            if (href) {
                window.location.href = href;
            }
        });
    });
});