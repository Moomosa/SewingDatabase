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

document.addEventListener("DOMContentLoaded", function () {
    var cards = document.querySelectorAll(".card-clickable");
    cards.forEach(function (card) {
        card.addEventListener("click", function () {
            var href = card.getAttribute("data-href");
            if (href) {
                window.location.href = href;
            }
        });
    });
});

$(document).ready(function () {
    // Function to show the card view and hide the table view
    function showCardView() {
        $('#tableView').hide();
        $('#cardView').show();
        $('#tableViewButton').removeClass('btn-selected');
        $('#cardViewButton').addClass('btn-selected');
        localStorage.setItem('selectedView', 'cardView');
    }

    // Function to show the table view and hide the card view
    function showTableView() {
        $('#cardView').hide();
        $('#tableView').show();
        $('#cardViewButton').removeClass('btn-selected');
        $('#tableViewButton').addClass('btn-selected');
        localStorage.setItem('selectedView', 'tableView');
    }

    // Check the selected view from local storage
    var selectedView = localStorage.getItem('selectedView');
    if (selectedView === 'cardView') {
        showCardView();
    } else {
        showTableView();
    }

    // Handle the card view button click
    $('#cardViewButton').click(function () {
        showCardView();
    });

    // Handle the table view button click
    $('#tableViewButton').click(function () {
        showTableView();
    });
});
