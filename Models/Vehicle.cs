using System.ComponentModel.DataAnnotations;

namespace FleetManagementSystem.Models;

public class Vehicle
{
    // public int Id { get; set; }
    public required string VehicleId { get; set; }
    [Range(-90, 91)]
    public decimal Latitude { get; set; }
    [Range(-180, 181)]
    public decimal Longitude { get; set; }
    public DateTime Timestamp { get; set; }
}
