using System.Runtime.Serialization;

namespace kisa_gcs_service.Model;

/*
 * [DataContract]와 [DataMember]는 .NET에서 데이터 직렬화를 위해 사용된다.
 * [DataContract] 어트리뷰트: 이 어트리뷰트는 클래스가 데이터 컨트랙트의 일부임을 나타낸다. 데이터 컨트랙트는 직렬화된 데이터 형식을 정의하는데 사용된다.
 *                         클래스에 이 어트리뷰트를 지정하면 해당 클래스의 인스턴스를 직렬화 및 역직렬화할 때 사용할 데이터 구조를 정의한다.
 * [DataMember] 어트리뷰트: 이 어트리뷰트는 데이터 컨트랙트 내에서 직렬화할 멤버(변수 또는 속성)를 나타낸다.
 *                       멤버에 이 어트리뷰트를 지정하면 멤버는 직렬화 프로세스에서 고려며되, 이를 통해 해당 멤버의 데이터가 저장 및 전송 된다.
 * 즉, 이 어트리뷰트들은 클래스의 특정 멤버가 데이터를 표현하고 이를 직렬화할 때 사용되는 규칙을 제공한다.
 * 이것은 주로 웹 서비스나 데이터베이스와 같은 곳에서 객체의 상태를 전송하거나 저장할 때 유용하다.
 */

// [DataConstract]는 데이터 계약을 나타내는 속성이다. 이 속성을 붙이면 클래스가 WCF(Windows Communication Foundation)에서 사용 가능한 데이터 형식으로 표시된다. 클래스의 속성과 메서드가 WCF에서 자동으로 시리얼화 및 역직렬화 된다. 
// WCF: Microsoft에서 개발한 분산 통신 프레임워크이다. WCF는 다양한 프로토콜과 형식을 지원하며, 이를 통해 다양한 환경에서 분산 애플리케이션을 개발할 수 있다.

[DataContract] public struct SensorData
{
    [DataMember] public double roll_ATTITUDE;
    [DataMember] public double pitch_ATTITUDE;
    [DataMember] public double yaw_ATTITUDE;
    [DataMember] public double xacc_RAW_IMU;
    [DataMember] public double yacc_RAW_IMU;
    [DataMember] public double zacc_RAW_IMU;
    [DataMember] public double xgyro_RAW_IMU;
    [DataMember] public double ygyro_RAW_IMU;
    [DataMember] public double zgyro_RAW_IMU;
    [DataMember] public double xmag_RAW_IMU;
    [DataMember] public double ymag_RAW_IMU;
    [DataMember] public double zmag_RAW_IMU;
    [DataMember] public double vibration_x_VIBRATION;
    [DataMember] public double vibration_y_VIBRATION;
    [DataMember] public double vibration_z_VIBRATION;
    [DataMember] public double accel_cal_x_SENSOR_OFFSETS;
    [DataMember] public double accel_cal_y_SENSOR_OFFSETS;
    [DataMember] public double accel_cal_z_SENSOR_OFFSETS;
    [DataMember] public double mag_ofs_x_SENSOR_OFFSETS;
    [DataMember] public double mag_ofs_y_SENSOR_OFFSETS;
    [DataMember] public double vx_GLOBAL_POSITION_INT;
    [DataMember] public double vy_GLOBAL_POSITION_INT;
    [DataMember] public double x_LOCAL_POSITION_NED;
    [DataMember] public double vx_LOCAL_POSITION_NED;
    [DataMember] public double vy_LOVAL_POSITION_NED;
    [DataMember] public double nav_pitch_NAV_CONTROLLER_OUTPUT;
    [DataMember] public double nav_bearing_NAV_CONTROLLER_OUTPUT;
    [DataMember] public double servo3_raw_SERVO_OUTPUT_RAW;
    [DataMember] public double servo8_raw_SERVO_OUTPUT_RAW;
    [DataMember] public double groundspeed_VFR_HUD;
    [DataMember] public double airspeed_VFR_HUD;
    [DataMember] public double press_abs_SCALED_PRESSURE;
    [DataMember] public double Vservo_POSER_STATUS;
    [DataMember] public double voltages1_BATTERY_STATUS;
    [DataMember] public double chancount_RC_CHANNELS;
    [DataMember] public double chan12_raw_RC_CHANNELS;
    [DataMember] public double chan13_raw_RC_CHANNELS;
    [DataMember] public double chan14_raw_RC_CHANNELS;
    [DataMember] public double chan15_raw_RC_CHANNELS;
    [DataMember] public double chan16_raw_RC_CHANNELS;

    public SensorData()
    {
        roll_ATTITUDE = 0.0;
        pitch_ATTITUDE = 0.0;
        yaw_ATTITUDE = 0.0;
        xacc_RAW_IMU = 0.0;
        yacc_RAW_IMU = 0.0;
        zacc_RAW_IMU = 0.0;
        xgyro_RAW_IMU = 0.0;
        ygyro_RAW_IMU = 0.0;
        zgyro_RAW_IMU = 0.0;
        xmag_RAW_IMU = 0.0;
        ymag_RAW_IMU = 0.0;
        zmag_RAW_IMU = 0.0;
        vibration_x_VIBRATION = 0.0;
        vibration_y_VIBRATION = 0.0;
        vibration_z_VIBRATION = 0.0;
        accel_cal_x_SENSOR_OFFSETS = 0.0;
        accel_cal_y_SENSOR_OFFSETS = 0.0;
        accel_cal_z_SENSOR_OFFSETS = 0.0;
        mag_ofs_x_SENSOR_OFFSETS = 0.0;
        mag_ofs_y_SENSOR_OFFSETS = 0.0;
        vx_GLOBAL_POSITION_INT = 0.0;
        vy_GLOBAL_POSITION_INT = 0.0;
        x_LOCAL_POSITION_NED = 0.0;
        vx_LOCAL_POSITION_NED = 0.0;
        vy_LOVAL_POSITION_NED = 0.0;
        nav_pitch_NAV_CONTROLLER_OUTPUT = 0.0;
        nav_bearing_NAV_CONTROLLER_OUTPUT = 0.0;
        servo3_raw_SERVO_OUTPUT_RAW = 0.0;
        servo8_raw_SERVO_OUTPUT_RAW = 0.0;
        groundspeed_VFR_HUD = 0.0;
        airspeed_VFR_HUD = 0.0;
        press_abs_SCALED_PRESSURE = 0.0;
        Vservo_POSER_STATUS = 0.0;
        voltages1_BATTERY_STATUS = 0.0;
        chancount_RC_CHANNELS = 0.0;
        chan12_raw_RC_CHANNELS = 0.0;
        chan13_raw_RC_CHANNELS = 0.0;
        chan14_raw_RC_CHANNELS = 0.0;
        chan15_raw_RC_CHANNELS = 0.0;
        chan16_raw_RC_CHANNELS = 0.0;
    }
}

[DataContract] public struct DroneGps
{
    [DataMember] public string? DroneId;
    [DataMember] public string[]? DroneLogger;
    [DataMember] public bool? IsOnline;
    [DataMember] public bool? HasDeliverPlan;
    [DataMember] public double[]? WayPointsDistance;
    [DataMember] public DroneStt? DroneStt;
    [DataMember] public DroneTrack? DroneTrack;
    [DataMember] public DroneCamera? DroneCamera;
    [DataMember] public DroneMission? DroneMission;
    [DataMember] public CommunicationLink? CommunicationLink;

    public DroneGps()
    {
        DroneId = "";
        DroneLogger = [];
        IsOnline = false;
        HasDeliverPlan = false;
        WayPointsDistance = [];
        DroneStt = new DroneStt();
        DroneTrack = new DroneTrack();
        DroneCamera = new DroneCamera();
        DroneMission = new DroneMission();
        CommunicationLink = new CommunicationLink();
    }
}

[DataContract]
public class DroneStt
{
    [DataMember] public double? WayPointNum;
    [DataMember] public double? PowerV;
    [DataMember] public char? GpsStt;
    [DataMember] public double? TempC;
    [DataMember] public char? LoaderLoad;
    [DataMember] public char? LoaderLock;
    [DataMember] public double? Lat;
    [DataMember] public double? Lon;
    [DataMember] public double? Alt;
    [DataMember] public double? Speed;
    [DataMember] public double? ROLL_ATTITUDE;
    [DataMember] public double? PITCH_ATTITUDE;
    [DataMember] public double? YAW_ATTITUDE;
    [DataMember] public char? HoverStt;
    [DataMember] public double? HODP;
    [DataMember] public char? FlightMode;
    [DataMember] public double? SatCount;
    [DataMember] public double? MabSysStt;
    [DataMember] public SensorStt SensorStt;
    [DataMember] public Mavlinkinfo Mavlinkinfo;

    public DroneStt()
    {
        WayPointNum = 0.0;
        PowerV = 0.0;
        GpsStt = ' ';
        TempC = 0.0;
        LoaderLoad = ' ';
        LoaderLock = ' ';
        Lat = 0.0;
        Lon = 0.0;
        Alt = 0.0;
        Speed = 0.0;
        ROLL_ATTITUDE = 0.0;
        PITCH_ATTITUDE = 0.0;
        YAW_ATTITUDE = 0.0;
        HoverStt = ' ';
        HODP = 0.0;
        FlightMode = ' ';
        SatCount = 0.0;
        MabSysStt = 0.0;
        SensorStt = new SensorStt();
        Mavlinkinfo = new Mavlinkinfo();
    }
}

[DataContract] public class SensorStt
{
    [DataMember] public string? Name;
    [DataMember] public bool? Enabled;
    [DataMember] public bool? Present;
    [DataMember] public bool? Health;
}

[DataContract] public class Mavlinkinfo
{
    [DataMember] public string? FrameType;
    [DataMember] public string? Ros;
    [DataMember] public string? FC_HARDWAR;
    [DataMember] public string? Autopilot;
    [DataMember] public string? CommunicationOut;
}


[DataContract] public class DroneCamera
{
    [DataMember] public char? FWD_CAM_STATE;
    [DataMember] public string? CameraIp;
    [DataMember] public string? CameraUrl1;
    [DataMember] public string? CameraUrl2;
    [DataMember] public string? CameraProtocolType;
}

[DataContract] public class DroneMission
{
    [DataMember] public string? MavMission;
    [DataMember] public DateTime? StartTime;
    [DataMember] public DateTime? CompleteTime;
}

[DataContract] public class DroneTrack
{
    [DataMember] public double? PathIndex;
    [DataMember] public double[]? DroneTrails;
    [DataMember] public double[]? DroneProgress;
    [DataMember] public double[]? DroneProgressPresentation;
    [DataMember] public double? TotalDistance;
    [DataMember] public double? ElapsedDistance;
    [DataMember] public double? RemainDistance;
}

[DataContract] public class CommunicationLink
{
    [DataMember] public double? ConnectionProtocol;
    [DataMember] public double? MessageProtocol;
    [DataMember] public string? Address;
}