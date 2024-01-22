namespace kisa_gcs_system.Model;

public class AnomalyDetectionAPI
{
    [BsonId]
    [BsonRepresentation(BsonType.String)] // MongoDB의 ObjectId를 문자열로 표현
    public string? _id { get; set; }
    public DateTime PredictTime { get; set; }
    public string? DroneId { get; set; }
    public string? FlightId { get; set; }
    public SensorData? SensorData { get; set; }
    public PredictData? PredictData { get; set; }
}

public class SensorData
{
    public double roll_ATTITUDE { get; set; }
    public double pitch_ATTITUDE { get; set; }
    public double yaw_ATTITUDE { get; set; }
    public double xacc_RAW_IMU { get; set; }
    public double yacc_RAW_IMU { get; set; }
    public double zacc_RAW_IMU { get; set; }
    public double xgyro_RAW_IMU { get; set; }
    public double ygyro_RAW_IMU { get; set; }
    public double zgyro_RAW_IMU { get; set; }
    public double xmag_RAW_IMU { get; set; }
    public double ymag_RAW_IMU { get; set; }
    public double zmag_RAW_IMU { get; set; }
    public double vibration_x_VIBRATION { get; set; }
    public double vibration_y_VIBRATION { get; set; }
    public double vibration_z_VIBRATION { get; set; }
    public double accel_cal_x_SENSOR_OFFSETS { get; set; }
    public double accel_cal_y_SENSOR_OFFSETS { get; set; }
    public double accel_cal_z_SENSOR_OFFSETS { get; set; }
    public double mag_ofs_x_SENSOR_OFFSETS { get; set; }
    public double mag_ofs_y_SENSOR_OFFSETS { get; set; }
    public double vx_GLOBAL_POSITION_INT { get; set; }
    public double vy_GLOBAL_POSITION_INT { get; set; }
    public double x_LOCAL_POSITION_NED { get; set; }
    public double vx_LOCAL_POSITION_NED { get; set; }
    public double vy_LOCAL_POSITION_NED { get; set; }
    public double nav_pitch_NAV_CONTROLLER_OUTPUT { get; set; }
    public double nav_bearing_NAV_CONTROLLER_OUTPUT { get; set; }
    public double servo3_raw_SERVO_OUTPUT_RAW { get; set; }
    public double servo8_raw_SERVO_OUTPUT_RAW { get; set; }
    public double groundspeed_VFR_HUD { get; set; }
    public double airspeed_VFR_HUD { get; set; }
    public double press_abs_SCALED_PRESSURE { get; set; }
    public double Vservo_POWER_STATUS { get; set; }
    public double voltages1_BATTERY_STATUS { get; set; }
    public double chancount_RC_CHANNELS { get; set; }
    public double chan12_raw_RC_CHANNELS { get; set; }
    public double chan13_raw_RC_CHANNELS { get; set; }
    public double chan14_raw_RC_CHANNELS { get; set; }
    public double chan15_raw_RC_CHANNELS { get; set; }
    public double chan16_raw_RC_CHANNELS { get; set; }
}

public class PredictData
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

public class PredictionResponseFormat
{
    public DateTime PredictTime { get; set; }
    public string DroneId { get; set; }
    public string FlightId { get; set; }
    public List<double> SelectData { get; set; }
    public List<double> PredictData { get; set; }
    public SensorData SensorData { get; set; }
}