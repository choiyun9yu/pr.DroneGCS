namespace kisa_gcs_system.Models;

public class Dashboard
{        
    public DateTime _id;
    public string DroneId;
    public string? FlightTime;
    public DroneLocation? StartPoint;
    public List<DroneLocation>? TransitPoints;
    public DroneLocation? LandPoint;
    public double? FlightDistance;
}

public struct FlightRate
{
    public string DroneId;
    public int FlightCount;
}

public class DailyFlightTime
{
    public int FlightDay { get; set; }
    public int FlightTime { get; set; }
}

public struct DailyAnomalyCount
{
    public int FlightDay { get; set; }
    public int AnomalyCount { get; set; }
}