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
