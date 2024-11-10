using FleetManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace FleetManagementSystem.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new FleetManagementSystemContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<FleetManagementSystemContext>>()))
        {
            // Look for any Vehicles.
            if (context.Vehicle.Any())
            {
                return;   // DB has been seeded
            }
            context.Vehicle.AddRange(
                new Vehicle
                {
                    // Id = 1,
                    VehicleId = "V01",
                    Timestamp = DateTime.Now,
                    Latitude = -26.205M,
                    Longitude = 28.049722M
                },
                new Vehicle
                {
                    // Id = 2,
                    VehicleId = "V02",
                    Timestamp = DateTime.Now,
                    Latitude = -26.2217693M,
                    Longitude = 27.9607024M
                },
                new Vehicle
                {
                    // Id = 3,
                    VehicleId = "V03",
                    Timestamp = DateTime.Now,
                    Latitude = -26.1687966M,
                    Longitude = 28.0762266M
                },
                new Vehicle
                {
                    // Id = 4,
                    VehicleId = "V04",
                    Timestamp = DateTime.Now,
                    Latitude = -26.2608773M,
                    Longitude = 28.1072469M
                }
            );
            context.SaveChanges();
        }
    }
}