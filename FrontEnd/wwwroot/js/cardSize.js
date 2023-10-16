$(document).ready(function () {
    // Find all card containers
    var cardContainers = $('#cardView');

    cardContainers.each(function () {
        // Find all cards within the current container
        var cards = $(this).find('.card');

        // Calculate the maximum height within this row
        var maxHeight = 0;

        cards.each(function () {
            var cardHeight = $(this).height();
            if (cardHeight > maxHeight) {
                maxHeight = cardHeight;
            }
        });

        // Set the height of all cards within the row to the maximum height
        cards.height(maxHeight);
    });
});
