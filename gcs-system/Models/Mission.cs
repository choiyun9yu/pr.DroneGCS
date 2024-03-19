using MongoDB.Bson.Serialization.Attributes;

namespace gcs_system.Models;
public class LocalPointAPI
{
    [BsonId]
    public string? _id { get; set; }      // LocalName을 식별자로 사용 
    public double? LocalLat { get; set; }
    public double? LocalLon { get; set; }
}

public class MissionLoadAPI
{
    [BsonId] 
    public string? _id { get; set; }
    public string? StartPoint { get; set; }
    public string? TargetPoint { get; set; }
    public int? FlightAlt { get; set; }
    public List<string> TransitPoints { get; set; }
    public LatLng StartLatLng { get; set; }
    public LatLng TargetLatLng { get; set; }
    public List<Transit> TransitLatLng { get; set; }
    public string? FlightDistance { get; set; }
    public string? TakeTime { get; set; }
}

public class Transit
{
    public int? id { get; set; }
    public LatLng? position { get; set; }
}

public class LatLng
{
    public double? lat { get; set; }
    public double? lng { get; set; }
}