namespace kisa_gcs_system.Interfaces;

public partial class MissionItem
{
    public double LatitudeDeg { get; set; }
    public double LongitudeDeg { get; set; }
    public float RelativeAltitudeM { get; set; }
    public float Speed { get; set; }
    // public bool IsFlyThrough { get; set; }
    // public float GimbalPitchDeg { get; set; }
    // public float YawDeg { get; set; }
    public float AcceptanceRadiusM { get; set; }
    public float LoiterTimeS { get; set; }
}

public class MissionGenerator
{
    
}