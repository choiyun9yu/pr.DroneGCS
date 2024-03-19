using gcs_system.Models.Helper;
using KisaGcsSystem.Services;

namespace gcs_system.Interfaces;

public class DroneState(string droneId)
{
    public string? DroneId = droneId;
    public string FlightId = "None";
    public bool IsOnline = true;
    public bool IsLanded = true;
    public string? ControlStt = "auto";
    public DroneStt? DroneStt = new ();
    public DroneMission? DroneMission = new ();
    public SensorData? SensorData = new ();
    public GrpcPredictData PredictData = new();
    public GrpcWarningData WarningData = new();

    /* event
     * 객체 내부에서 발생하는 특정한 이벤트를 외부에 알릴 수 있도록 선언하는 C# 키워드이다.
     * 델리게이트(Delegate)를 기반으로 구현된다.
     * 다른 클래스나 메서드에서 이 이벤트에 가입(subscribe)하여 이벤트가 발생했을 때 알림을 받을 수 있다.
     */

    // 이벤트 인스턴스에 .Invoke 를 붙이면 이벤트에 연결된 모든 메서드를 호출한다. 즉, 이벤트를 구독하는 모든 객체들이 이벤트 발생에 대한 알림을 받고 각자의 처리 코드를 실행한다.

    /* Action<>
     * 인자와 반환값이 없는 메서드를 나타내는 C#의 델리게이트 형식이다.
     * 이벤트를 처리할 때 자주 사용 된다.
     * <> 안에 인자의 타입을 지정한다.
     */

}

public struct MavlinkLog
{
    public DateTime logtime;
    public string message;
}

public class DroneStt
{
    public List<MavlinkLog> DroneLogger = new ();
    public float PowerV = 0;
    public sbyte? BatteryStt = 0;
    public string? GpsStt = "";
    public double? TempC = 0.0;
    public double Lat = 0.0;
    public double Lon = 0.0;
    public double? Alt = 0.0;
    public double? GlobalAlt = 0.0;
    public double? Roll = 0.0;
    public double? Pitch = 0.0;
    public short? Head = 0;
    public float? Speed = 0;
    public string? HoverStt = "";
    public double? HDOP = 0.0;
    public byte? SatellitesCount = 0;
    public FlightMode FlightMode = 0;
}

public class DroneMission
{
    public DroneLocation StartPoint;
    public List<DroneLocation> TransitPoint = new();
    public DroneLocation TargetPoint;
    public int MissionAlt = 10;             // 주행 고도
    // public int MissionSpeed = 10;
    public double? TotalDistance = 0;
    public double? CurrentDistance = 0;
    public int? PathIndex = 0;
    public FixedSizedQueue<DroneLocation> DroneTrails = new(600);    // size 가 600 이면 0.5초에 하나씩 이라서 1초 씩 300개 -> 5분 
    public DateTime LastAddedTrails;
    public DateTime? StartTime = null;      // Take Off 기준 
    public DateTime? CompleteTime = null;   // Disarm 기준 
}

public struct DroneLocation   
{
    public double lat;
    public double lng;
    public double global_frame_alt;
    public double terrain_alt;          // 글로벌 고도에서 상대 고도를 빼면 지형 고도를 알 수 있다. API로 받아오는게 더 정확하지만 API 사용료가 너무 많이 나온다.
}

public class SensorData
{
    public float roll_ATTITUDE = 0;
    public float pitch_ATTITUDE = 0;
    public float yaw_ATTITUDE = 0;
    public short xacc_RAW_IMU = 0;
    public short yacc_RAW_IMU = 0;
    public short zacc_RAW_IMU = 0;
    public short xgyro_RAW_IMU = 0;
    public short ygyro_RAW_IMU = 0;
    public short zgyro_RAW_IMU = 0;
    public short xmag_RAW_IMU = 0;
    public short ymag_RAW_IMU = 0;
    public short zmag_RAW_IMU = 0;
    public float vibration_x_VIBRATION = 0;
    public float vibration_y_VIBRATION = 0;
    public float vibration_z_VIBRATION = 0;
    public float accel_cal_x_SENSOR_OFFSETS = 0;
    public float accel_cal_y_SENSOR_OFFSETS = 0;
    public float accel_cal_z_SENSOR_OFFSETS = 0;
    public short mag_ofs_x_SENSOR_OFFSETS = 0;
    public short mag_ofs_y_SENSOR_OFFSETS = 0;
    public short vx_GLOBAL_POSITION_INT = 0;
    public short vy_GLOBAL_POSITION_INT = 0;
    public float x_LOCAL_POSITION_NED = 0;
    public float vx_LOCAL_POSITION_NED = 0;
    public float vy_LOCAL_POSITION_NED = 0;
    public float nav_pitch_NAV_CONTROLLER_OUTPUT = 0;
    public short nav_bearing_NAV_CONTROLLER_OUTPUT = 0;
    public ushort servo3_raw_SERVO_OUTPUT_RAW = 0;
    public ushort servo8_raw_SERVO_OUTPUT_RAW = 0;
    public float groundspeed_VFR_HUD = 0;
    public float airspeed_VFR_HUD = 0;
    public float press_abs_SCALED_PRESSURE = 0;
    public ushort Vservo_POWER_STATUS = 0;
    public double voltages1_BATTERY_STATUS = 0.0;
    public byte chancount_RC_CHANNELS = 0;
    public ushort chan12_raw_RC_CHANNELS = 0;
    public ushort chan13_raw_RC_CHANNELS = 0;
    public ushort chan14_raw_RC_CHANNELS = 0;
    public ushort chan15_raw_RC_CHANNELS = 0;
    public ushort chan16_raw_RC_CHANNELS = 0;
}
