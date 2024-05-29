// Obtain the user's current location using the Geolocation API
function getUserLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(sendLocationToServer);
    } else {
        console.error("Geolocation is not supported by this browser.");
    }
}

// Callback function to send location data to the server
function sendLocationToServer(position) {
    var latitude = position.coords.latitude;
    var longitude = position.coords.longitude;

    // Send location data to the server using AJAX
    $.ajax({
        url: 'https://ilquadrifogliodb.database.windows.net/api/location/update', // Update this with your server endpoint
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ latitude: latitude, longitude: longitude }),
        success: function (response) {
            console.log('Location updated successfully.');
        },
        error: function (xhr, status, error) {
            console.error('Error updating location:', error);
        }
    });
}
