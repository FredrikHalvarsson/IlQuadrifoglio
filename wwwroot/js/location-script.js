// Function to load the partial view dynamically
function loadPartialView() {
    $.get("https://localhost:7223/LocationPartial?userName=" + userName, function (data) {
        $("#partial-container").html(data);

    });
}

// Call the function to load the partial view
$(document).ready(function () {
    loadPartialView();
});