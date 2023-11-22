// namespace kisa_gcs_service.Services;
//
// public class DroneState
// {
//     public struct DroneStateStruct
//     {
//         public string DroneId;
//         public double ElapsedDistance;
//         public double RemainDistance;
//         public double TotalDistance;
//         public int PathIndex;
//         public DRONE_STATE_RES? DroneRawState;
//
//         public IEnumerable<CGISLocation>? DroneTrails;
//         public List<WaypointDistance>? DroneProgress;
//         public List<WaypointProgress>? DroneProgressPercentages;
//
//         [JsonConverter(typeof(NullDateTimeConverter))]
//         public DateTime StartTime;
//
//         [JsonConverter(typeof(NullDateTimeConverter))]
//         public DateTime CompleteTime;
//         public bool IsOnline { get; set; }
//         public DroneCommunication? CommunicationLink;
//         public bool HasDeliveryPlan;
//         public MAVLink.mavlink_mission_item_int_t[]? MavMission;
//         public double DroneSpeed;
//         public DateTime LastHeartbeatMessage;
//         public List<MavlinkSeverityLog> DroneLogger = new();
//
//         public string? CameraIP;
//         public string? CameraURL1;
//         public string? CameraURL2;
//         public CameraType? CameraProtocolType;
//         public List<WaypointDistance> WaypointsDistance { get; set; } = new();
//
//         public DroneStateStruct(string droneId)
//         {
//             DroneId = droneId;
//             MavMission = null;
//             ElapsedDistance = 0;
//             RemainDistance = 0;
//             TotalDistance = 0;
//             PathIndex = 0;
//             DroneRawState = null;
//             DroneTrails = null;
//             DroneProgress = null;
//             DroneProgressPercentages = null;
//             StartTime = default;
//             CompleteTime = default;
//             CommunicationLink = null;
//             HasDeliveryPlan = false;
//             DroneSpeed = 0;
//             LastHeartbeatMessage = default;
//             CameraIP = null;
//             CameraURL1 = null;
//             CameraURL2 = null;
//             CameraProtocolType = null;
//             IsOnline = false;
//         }
//     }
// }