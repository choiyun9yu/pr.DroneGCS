namespace kisa_gcs_system.Models;

public class Dashboard
{
    public string DroneId;
    public DateTime? StartTime;
    public DateTime? CompleteTime;
    public DroneLocation? StartPoint;
    public List<DroneLocation>? TransitPoints;
    public DroneLocation? LandPoint;
    public double? FlightDistance;
}