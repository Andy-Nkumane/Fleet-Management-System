using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FleetManagementSystem.Controllers;
using FleetManagementSystem.Data;
using FleetManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace FleetManagementSystem.Tests
{
    public class VehiclesControllerTests
    {
        private readonly VehiclesController _controller;
        private readonly FleetManagementSystemContext _context;

        public VehiclesControllerTests()
        {
            var options = new DbContextOptionsBuilder<FleetManagementSystemContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new FleetManagementSystemContext(options);
            _controller = new VehiclesController(_context);
        }

        // [Fact]
        // public async Task Post_VehicleLocation_SavesDataToDatabase()
        // {
        //     // Arrange
        //     var vehicleLocation = new Vehicle
        //     {
        //         VehicleId = "V123",
        //         Latitude = 34.0522,
        //         Longitude = -118.2437,
        //         Timestamp = DateTime.UtcNow
        //     };

        //     // Act
        //     var result = await _controller.PostVehicleLocation(vehicleLocation); // Assuming you have a method for this

        //     // Assert
        //     Assert.IsType<CreatedAtActionResult>(result);
        //     Assert.Equal(1, await _context.Vehicle.CountAsync());
        // }

        // [Fact]
        // public async Task Get_VehicleLocations_ReturnsCorrectLocations()
        // {
        //     // Arrange
        //     var vehicle1 = new Vehicle { VehicleId = "V123", Latitude = 34.0522, Longitude = -118.2437, Timestamp = DateTime.UtcNow };
        //     var vehicle2 = new Vehicle { VehicleId = "V124", Latitude = 40.7128, Longitude = -74.0060, Timestamp = DateTime.UtcNow.AddMinutes(-10) };
            
        //     _context.Vehicle.AddRange(vehicle1, vehicle2);
        //     await _context.SaveChangesAsync();

        //     // Act
        //     var result = await _controller.GetVehicleLocations() as OkObjectResult;

        //     // Assert
        //     var locations = Assert.IsType<List<Vehicle>>(result.Value);
        //     Assert.Equal(2, locations.Count);
        //     Assert.Contains(locations, v => v.VehicleId == "V123");
        //     Assert.Contains(locations, v => v.VehicleId == "V124");
        // }

        [Theory]
        [InlineData(null, 34.0522, -118.2437)]
        [InlineData("V123", 91.0, -118.2437)] // Invalid latitude
        [InlineData("V123", 34.0522, -190.0)] // Invalid longitude
        public async Task Post_VehicleLocation_InvalidInput_ReturnsBadRequest(string vehicleId, decimal latitude, decimal longitude)
        {
            // Arrange
            var vehicleLocation = new Vehicle 
            { 
                VehicleId = vehicleId,
                Latitude = latitude,
                Longitude = longitude,
                Timestamp = DateTime.UtcNow 
            };

            // Act
            var result = await _controller.PostVehicleLocation(vehicleLocation);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}