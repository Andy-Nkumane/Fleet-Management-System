// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const vehicleListDiv = document.getElementById('vehicle-list');
const vehicleTableBody = document.querySelector('#vehicle-table tbody');

var map = L.map('map').setView([-26.2340723, 28.0897142], 10); // Set initial coordinates and zoom level

// Load and display tile layers from OpenStreetMap
L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    maxZoom: 19,
    attribution: '© OpenStreetMap'
}).addTo(map);

var vehicleMarkers = {};

// Function to fetch data from the API
const fetchData = () => {
    // Change cursor to loading state
    vehicleListDiv.style.cursor = 'wait';
    fetch('http://localhost:5251/api/vehicles/locations') 
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => 
        {        
            // Clear existing table rows
            vehicleTableBody.innerHTML = '';

            // Clear existing markers
            for (var id in vehicleMarkers) 
            {
                map.removeLayer(vehicleMarkers[id]);
            }
            vehicleMarkers = {};

            // Add new markers
            data.forEach(function (vehicle) 
            {
                 // Create a new row for each vehicle
                const row = document.createElement('tr');
                
                // Create cells for Vehicle ID, Latitude, Longitude, and Timestamp
                row.innerHTML = `
                    <td>${vehicle.vehicleId}</td>
                    <td>${vehicle.latitude}</td>
                    <td>${vehicle.longitude}</td>
                    <td>${new Date(vehicle.timestamp).toLocaleString()}</td>
                `;
                
                // Add click event to focus on the vehicle location on the map
                row.onclick = () => {
                    map.setView([vehicle.latitude, vehicle.longitude], 15);
                };
                
                // Append the row to the table body
                vehicleTableBody.appendChild(row);

                var marker = L.marker([vehicle.latitude, vehicle.longitude]).addTo(map)
                    .bindPopup('Vehicle ID: ' + vehicle.vehicleId + '<br>Last Updated: ' + vehicle.timestamp);
                vehicleMarkers[vehicle.vehicleId] = marker;
            });
            updateAllLocations();
        })
        .catch(error => console.error('Fetch error:', error))
        .finally(() => {
            // Reset cursor back to default after loading is complete
            vehicleListDiv.style.cursor = 'default';
        });
};

// Function to generate a random offset between -0.009 and 0.009
const generateRandomOffset = () => {
    return (Math.random() * 0.018) - 0.009; 
};

// Function to update all vehicle locations
const updateAllLocations = async () => {
    try {
        // Fetch the current vehicle data 
        const response = await fetch('http://localhost:5251/api/vehicles/locations');
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        
        const vehicles = await response.json(); // returns an array of vehicle objects

        // Iterate over each vehicle and update its location
        for (const vehicle of vehicles) {
            const updatedVehicle = {
                VehicleId: vehicle.vehicleId,
                Latitude: vehicle.latitude + generateRandomOffset(), // Update latitude with random offset
                Longitude: vehicle.longitude + generateRandomOffset(), // Update longitude with random offset
                Timestamp: new Date().toISOString() // Current datetime
            };

            // Send updated location to the server for each vehicle
            const updateResponse = await fetch('http://localhost:5251/api/vehicles/location', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'X-Requested-With': 'XMLHttpRequest'
                },
                body: JSON.stringify(updatedVehicle) // Send the updated vehicle data
            });

            if (!updateResponse.ok) {
                throw new Error(`Failed to update location for vehicle ID ${vehicle.vehicleId}`);
            }

            // const data = await updateResponse.json();
            // console.log(`Location updated successfully for vehicle ID ${vehicle.vehicleId}:`, data);
        }

    } catch (error) {
        console.error('Error updating vehicle locations:', error);
        vehicleTableBody.innerHTML = '<tr><td colspan="4">Error loading vehicle locations.</td></tr>';
    }
};

// Fetch data immediately and then every 30 seconds
fetchData();
setInterval(fetchData, 30000); // Call fetchData every 30 seconds