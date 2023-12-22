// using System.Runtime.Serialization;
//
// namespace kisa_gcs_service.Model;
//
// // [DataConstract]는 데이터 계약을 나타내는 속성이다. 이 속성을 붙이면 클래스가 WCF(Windows Communication Foundation)에서 사용 가능한 데이터 형식으로 표시된다. 클래스의 속성과 메서드가 WCF에서 자동으로 시리얼화 및 역직렬화 된다. 
// // WCF: Microsoft에서 개발한 분산 통신 프레임워크이다. WCF는 다양한 프로토콜과 형식을 지원하며, 이를 통해 다양한 환경에서 분산 애플리케이션을 개발할 수 있.다
// [DataContract]  
// public class SensorStt
// {
//     [DataMember] public string? Name;
//     [DataMember] public bool? Enabled;
//     [DataMember] public bool? Present;
//     [DataMember] public bool? Health;
// }
//
// [DataContract]
// public class Mavlinkinfo
// {
//     [DataMember] public string? FrameType;
//     [DataMember] public string? Ros;
//     [DataMember] public string? FC_HARDWAR;
//     [DataMember] public string? Autopilot;
//     [DataMember] public string? CommunicationOut;
// }
//
// [DataContract]
// public class DroneStt
// {
//     [DataMember] public float? WayPointNum;
//     [DataMember] public float? PowerV;
//     [DataMember] public char? GpsStt;
//     [DataMember] public float? TempC;
//     [DataMember] public char? LoaderLoad;
//     [DataMember] public char? LoaderLock;
//     [DataMember] public double? Lat;
//     [DataMember] public double? Lon;
//     [DataMember] public float? Alt;
//     [DataMember] public float? Speed;
//     [DataMember] public double? ROLL_ATTITUDE;
//     [DataMember] public double? PITCH_ATTITUDE;
//     [DataMember] public double? YAW_ATTITUDE;
//     [DataMember] public char? HoverStt;
//     [DataMember] public float? HODP;
//     [DataMember] public char? FlightMode;
//     [DataMember] public float? SatCount;
//     [DataMember] public float? MabSysStt;
//     [DataMember] public SensorStt SensorStt;
//     [DataMember] public Mavlinkinfo Mavlinkinfo;
// }
//
// [DataContract]
// public class DroneTrack
// {
//     [DataMember] public float? PathIndex;
//     [DataMember] public float[]? DroneTrails;
//     [DataMember] public float[]? DroneProgress;
//     [DataMember] public float[]? DroneProgressPresentation;
//     [DataMember] public float? TotalDistance;
//     [DataMember] public float? ElapsedDistance;
//     [DataMember] public float? RemainDistance;
// }
//
// [DataContract]
// public class DroneCamera
// {
//     [DataMember] public char? FWD_CAM_STATE;
//     [DataMember] public string? CameraIp;
//     [DataMember] public string? CameraUrl1;
//     [DataMember] public string? CameraUrl2;
//     [DataMember] public string? CameraProtocolType;
// }
//
// [DataContract]
// public class DroneMission
// {
//     [DataMember] public string? MavMission;
//     [DataMember] public DateTime? StartTime;
//     [DataMember] public DateTime? CompleteTime;
// }
//
// [DataContract]
// public class CommunicationLink
// {
//     [DataMember] public float? ConnectionProtocol;
//     [DataMember] public float? MessageProtocol;
//     [DataMember] public string? Address;
// }
//
// public struct DroneGCS
// {
//     public string? DroneId;
//     public string[]? DroneLogger;
//     public bool? IsOnline;
//     public bool? HasDeliverPlan;
//     public float[]? WayPointsDistance;
//     public DroneStt? DroneStt;
//     public DroneTrack? DroneTrack;
//     public DroneCamera? DroneCamera;
//     public DroneMission? DroneMission;
//     public CommunicationLink? CommunicationLink;
// }
//
