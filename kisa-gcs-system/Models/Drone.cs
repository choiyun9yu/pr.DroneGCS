
using kisa_gcs_system.Interfaces;
using kisa_gcs_system.Models.Helper;

namespace kisa_gcs_system.Models;

public class DroneCommunication
{
    private DroneConnectionProtocol Protocol;
    private string Address;

    public DroneCommunication(DroneConnectionProtocol protocol, string address)
    {
        Protocol = protocol;
        Address = address;
    }
}

public enum DroneConnectionProtocol
{
    UDP,
    TCP,
    SERIAL,
}

public class Drone
{
    public string? DroneId;
    public List<MavlinkLog> DroneLogger = new ();
    public bool? IsOnline = true;
    public string? ControlStt = "auto";
    public DroneStt? DroneStt = new DroneStt();
    public SensorData? SensorData = new SensorData();
    public DroneMission? DroneMission = new DroneMission();
    public DroneCamera? DroneCamera = new DroneCamera();
    public CommunicationLink? CommunicationLink = new CommunicationLink();
}

public struct MavlinkLog
{
    public DateTime logtime;
    public string message;
}

public class DroneStt
{
    public double? WayPointNum = 0.0;
    public double? PowerV = 0.0;
    public double? BatteryStt = 0.0;
    public char? GpsStt = ' ';
    public double? TempC = 0.0;
    public char? LoaderLoad = ' ';
    public char? LoaderLock = ' ';
    public double Lat = 0.0;
    public double Lon = 0.0;
    public double? Alt = 0.0;
    public double? GlobalAlt = 0.0;
    public double? Head = 0.0;
    public double? Speed = 0.0;
    public char? HoverStt = ' ';
    public double? HDOP = 0.0;
    public double? SatellitesCount = 0.0;
    public CustomMode?  FlightMode = 0;
}

public class SensorStt
{
    public string? Name;
    public bool? Enabled;
    public bool? Present;
    public bool? Health;
}

public class Mavlinkinfo
{
    public string? FrameType;
    public string? Ros;
    public string? FC_HARDWAR;
    public string? Autopilot;
    public string? CommunicationOut;
}


public class DroneCamera
{
    public char? FWD_CAM_STATE;
    public string? CameraIp;
    public string? CameraUrl1;
    public string? CameraUrl2;
    public string? CameraProtocolType;
}

public class DroneMission
{
    public string FligthId = "None";
    public DroneLocation StartPoint;
    public List<DroneLocation> TransitPoint;
    public DroneLocation TargetPoint;
    public int MissionAlt = 10;             // 주행 고도
    // public int MissionSpeed = 10;
    public double? TotalDistance = 0.0;
    public double? CurrentDistance = 0;
    public int? PathIndex = 0;
    // public double? RemainDistance = 0.0;
    public FixedSizedQueue<DroneLocation> DroneTrails = new(600);    // size 가 600 이면 0.5초에 하나씩 이라서 1초 씩 300개 -> 5분 
    public DateTime? StartTime = null;      // Take Off 기준 
    public DateTime? CompleteTime = null;   // Disarm 기준 
}

public struct TransitItem
{
    public int id { get; set; }
    public DroneLocation position { get; set; }
}

public struct DroneLocation   
{
    public double lat;
    public double lng;
    public double global_frame_alt;
    public double terrain_alt;          // 글로벌 고도에서 상대 고도를 빼면 지형 고도를 알 수 있다. API로 받아오는게 더 정확하지만 API 사용료가 너무 많이 나온다.
}

public class CommunicationLink
{
    public double? ConnectionProtocol;
    public double? MessageProtocol;
    public string? Address;
}

public class SensorData
{
    public double roll_ATTITUDE = 0.0;
    public double pitch_ATTITUDE = 0.0;
    public double yaw_ATTITUDE = 0.0;
    public double xacc_RAW_IMU = 0.0;
    public double yacc_RAW_IMU = 0.0;
    public double zacc_RAW_IMU = 0.0;
    public double xgyro_RAW_IMU = 0.0;
    public double ygyro_RAW_IMU = 0.0;
    public double zgyro_RAW_IMU = 0.0;
    public double xmag_RAW_IMU = 0.0;
    public double ymag_RAW_IMU = 0.0;
    public double zmag_RAW_IMU = 0.0;
    public double vibration_x_VIBRATION = 0.0;
    public double vibration_y_VIBRATION = 0.0;
    public double vibration_z_VIBRATION = 0.0;
    public double accel_cal_x_SENSOR_OFFSETS = 0.0;
    public double accel_cal_y_SENSOR_OFFSETS = 0.0;
    public double accel_cal_z_SENSOR_OFFSETS = 0.0;
    public double mag_ofs_x_SENSOR_OFFSETS = 0.0;
    public double mag_ofs_y_SENSOR_OFFSETS = 0.0;
    public double vx_GLOBAL_POSITION_INT = 0.0;
    public double vy_GLOBAL_POSITION_INT = 0.0;
    public double x_LOCAL_POSITION_NED = 0.0;
    public double vx_LOCAL_POSITION_NED = 0.0;
    public double vy_LOCAL_POSITION_NED = 0.0;
    public double nav_pitch_NAV_CONTROLLER_OUTPUT = 0.0;
    public double nav_bearing_NAV_CONTROLLER_OUTPUT = 0.0;
    public double servo3_raw_SERVO_OUTPUT_RAW = 0.0;
    public double servo8_raw_SERVO_OUTPUT_RAW = 0.0;
    public double groundspeed_VFR_HUD = 0.0;
    public double airspeed_VFR_HUD = 0.0;
    public double press_abs_SCALED_PRESSURE = 0.0;
    public double Vservo_POWER_STATUS = 0.0;
    public double voltages1_BATTERY_STATUS = 0.0;
    public double chancount_RC_CHANNELS = 0.0;
    public double chan12_raw_RC_CHANNELS = 0.0;
    public double chan13_raw_RC_CHANNELS = 0.0;
    public double chan14_raw_RC_CHANNELS = 0.0;
    public double chan15_raw_RC_CHANNELS = 0.0;
    public double chan16_raw_RC_CHANNELS = 0.0;
}