namespace kisa_gcs_system.Models;

public class AnomalyDetection
{
    [BsonId]
    [BsonRepresentation(BsonType.String)] // MongoDB의 ObjectId를 문자열로 표현
    public string? _id { get; set; }
    public DateTime PredictTime { get; set; }
    public string? DroneId { get; set; }
    public string? FlightId { get; set; }
    public SensorData? SensorData { get; set; }
    public PredictData? PredictData { get; set; }
    public WarningData? WarningData { get; set; }
}

public partial class PredictData
{
    public double roll_ATTITUDE_PREDICT { get; set; }
    public double pitch_ATTITUDE_PREDICT { get; set; }
    public double yaw_ATTITUDE_PREDICT { get; set; }
    public double xacc_RAW_IMU_PREDICT { get; set; }
    public double yacc_RAW_IMU_PREDICT { get; set; }
    public double zacc_RAW_IMU_PREDICT { get; set; }
    public double xgyro_RAW_IMU_PREDICT { get; set; }
    public double ygyro_RAW_IMU_PREDICT { get; set; }
    public double zgyro_RAW_IMU_PREDICT { get; set; }
    public double xmag_RAW_IMU_PREDICT { get; set; }
    public double ymag_RAW_IMU_PREDICT { get; set; }
    public double zmag_RAW_IMU_PREDICT { get; set; }
    public double vibration_x_VIBRATION_PREDICT { get; set; }
    public double vibration_y_VIBRATION_PREDICT { get; set; }
    public double vibration_z_VIBRATION_PREDICT { get; set; }
}

public class WarningData
{
    public int warning_count { get; set; }
    public bool roll_ATTITUDE_WARNING { get; set; }
    public bool pitch_ATTITUDE_WARNING { get; set; }
    public bool yaw_ATTITUDE_WARNING { get; set; }
    public bool xacc_RAW_IMU_WARNING { get; set; }
    public bool yacc_RAW_IMU_WARNING { get; set; }
    public bool zacc_RAW_IMU_WARNING { get; set; }
    public bool xgyro_RAW_IMU_WARNING { get; set; }
    public bool ygyro_RAW_IMU_WARNING { get; set; }
    public bool zgyro_RAW_IMU_WARNING { get; set; }
    public bool xmag_RAW_IMU_WARNING { get; set; }
    public bool ymag_RAW_IMU_WARNING { get; set; }
    public bool zmag_RAW_IMU_WARNING { get; set; }
    public bool vibration_x_VIBRATION_WARNING { get; set; }
    public bool vibration_y_VIBRATION_WARNING { get; set; }
    public bool vibration_z_VIBRATION_WARNING { get; set; }
}

public class PredictionResponseFormat
{
    public DateTime PredictTime { get; set; }
    public string DroneId { get; set; }
    public string FlightId { get; set; }
    public List<double> SelectData { get; set; }
    public List<double> PredictData { get; set; }
    public SensorData SensorData { get; set; }
}