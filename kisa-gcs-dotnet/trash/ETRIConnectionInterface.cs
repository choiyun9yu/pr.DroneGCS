// namespace kisa_gcs_service.Service;
//
// [DataContract]
//   public class ResCode
//   {
//     [DataMember]
//     public string RES_CODE { get; set; }
//
//     public ResCode()
//     {
//       RES_CODE = "S";
//     }
//
//     public ResCode(string value)
//     {
//       RES_CODE = value;
//     }
//   }
//
//   [DataContract]
//   public class DRONE_NET_ON_REQ
//   {
//     [DataMember]
//     public string? DR_ID;
//     [DataMember]
//     public string? IP_ADDR;
//     [DataMember]
//     public int PORT_NUM;
//   }
//
//   [DataContract]
//   public class DRONE_NET_ON_RES
//   {
//     [DataMember]
//     public char RES_CODE;
//     [DataMember]
//     public string? RES_MSG;
//   }
//
//   [DataContract]
//   public class DRONE_STATE_REQ
//   {
//     //
//   }
//
//   [DataContract]
//   public struct DRONE_STATE_RES
//   {
//     [DataMember]
//     public string DR_ID;
//     [DataMember]
//     public byte DR_STATE;
//     [DataMember]
//     public byte DR_STATE_SUB;
//     [DataMember]
//     public float POWER_V;
//     [DataMember]
//     //public char GPS_STATE;
//     //public char FWD_CAM_STATE;
//     public string GPS_STATE;
//     [DataMember]
//     public string FWD_CAM_STATE;
//     [DataMember]
//     public float TEMPERATURE_C;
//     [DataMember]
//     //public char LOADER_LOAD;
//     //public char LOADER_LOCK;
//     public string LOADER_LOAD;
//     [DataMember]
//     public string LOADER_LOCK;
//     [DataMember]
//     public uint WP_NO;
//     [DataMember]
//     public double DR_LAT;
//     [DataMember]
//     public double DR_LON;
//     [DataMember]
//     public float DR_ALT;
//     [DataMember]
//     public float DR_SPEED;
//     [DataMember]
//     //public char HOVERING_STATE;
//     public string HOVERING_STATE;
//     [DataMember]
//     public float DR_ROLL;
//     [DataMember]
//     public float DR_PITCH;
//     [DataMember]
//     public float DR_YAW;
//
//     #region mavlink specific
//     [DataMember]
//     public float HDOP;
//     [DataMember]
//     public byte FLIGHT_MODE = 255;
//     [DataMember]
//     public byte SAT_COUNT; // satellite
//     [DataMember]
//     public byte MAV_SYS_STATUS; // SYS_STATUS (#1)
//     #endregion
//
//     #region uas specific
//     [DataMember]
//     public MAVLINK_SENSOR_STATUS[] SENSOR_STATUSES;
//     [DataMember]
//     public MAVLINK_DRONE_INFO? MAVLINK_INFO;
//     #endregion
//
//     public DRONE_STATE_RES()
//     {
//       DR_ID = "";
//       DR_STATE = 255;
//       DR_STATE_SUB = 0;
//       POWER_V = 0.0f;
//       GPS_STATE = "F";
//       FWD_CAM_STATE = "F";
//       TEMPERATURE_C = 0.0f;
//       LOADER_LOAD = "F";
//       LOADER_LOCK = "F";
//       WP_NO = 0;
//       DR_LAT = 0.0;
//       DR_LON = 0.0;
//       DR_ALT = 0.0f;
//       DR_SPEED = 0.0f;
//       HOVERING_STATE = "F";
//       DR_ROLL = 0.0f;
//       DR_PITCH = 0.0f;
//       DR_YAW = 0.0f;
//       HDOP = 0.0f;
//       SENSOR_STATUSES = new MAVLINK_SENSOR_STATUS[]{};
//       SAT_COUNT = 0;
//       MAV_SYS_STATUS = 0;
//       MAVLINK_INFO = null;
//     }
//   }
//
//   [DataContract]
//   public class DELIVERY_ROUTE_SEND_REQ
//   {
//     [DataMember]
//     public string? DR_ID;
//     [DataMember]
//     public uint DISTANCE;
//     [DataMember]
//     public uint WP_COUNT;
//     [DataMember]
//     public DELIVERY_ROUTE_WAYPOINTS[]? WAYPOINTS;
//   }
// #nullable disable
//   [DataContract]
//   public class DELIVERY_ROUTE_WAYPOINTS
//   {
//     [DataMember]
//     public uint WP_S2D_NO;
//     [DataMember]
//     public double WP_S2D_LAT;
//     [DataMember]
//     public double WP_S2D_LON;
//     [DataMember]
//     public float WP_S2D_ALT;
//     [DataMember]
//     public float WP_S2D_GEO_ALT;
//     [DataMember]
//     //public byte WP_PROPERTY;
//     public string WP_PROPERTY;
//     [JsonIgnoreAttribute]
//     public int WP_nodeId;
//
//     public static DELIVERY_ROUTE_WAYPOINTS Copy(DELIVERY_ROUTE_WAYPOINTS source)
//     {
//       var waypoint = new DELIVERY_ROUTE_WAYPOINTS();
//       waypoint.WP_PROPERTY = source.WP_PROPERTY;
//       waypoint.WP_S2D_ALT = source.WP_S2D_ALT;
//       waypoint.WP_S2D_GEO_ALT = source.WP_S2D_GEO_ALT;
//       waypoint.WP_S2D_LAT = source.WP_S2D_LAT;
//       waypoint.WP_S2D_LON = source.WP_S2D_LON;
//       waypoint.WP_S2D_NO = source.WP_S2D_NO;
//       waypoint.WP_nodeId = source.WP_nodeId;
//       return waypoint;
//     }
//   }
//
//   [DataContract]
//   public class DELIVERY_ROUTE_SEND_RES
//   {
//     [DataMember]
//     public string RES_CODE;
//     [DataMember]
//     public string RES_MSG;
//   }
//
//   [DataContract]
//   public class SAFE_ZONE_INFO_SEND_REQ
//   {
//     [DataMember]
//     public string DR_ID;
//     [DataMember]
//     public uint SZ_COUNT;
//     [DataMember]
//     public SAFE_ZONE_INFO_POINTS[] SAFEZONES;
//   }
//
//   [DataContract]
//   public class SAFE_ZONE_INFO_POINTS
//   {
//     [DataMember]
//     public uint SZ_S2D_NO;
//     [DataMember]
//     public double SZ_S2D_LAT;
//     [DataMember]
//     public double SZ_S2D_LON;
//     [DataMember]
//     public float SZ_S2D_ALT;
//   }
//
//   [DataContract]
//   public class SAFE_ZONE_INFO_SEND_RES
//   {
//     [DataMember]
//     public string RES_CODE;
//   }
//
//   [DataContract]
//   public class DELIVERY_START_REQ
//   {
//     //
//   }
//
//   [DataContract]
//   public class DELIVERY_START_RES
//   {
//     [DataMember]
//     public string RES_CODE;
//     [DataMember]
//     public string RES_MSG;
//   }
//
//   [DataContract]
//   public class EMERGENCY_COMMAND_REQ
//   {
//     [DataMember]
//     public string DR_ID;
//     [DataMember]
//     public string EC_S2D_CMD;
//
//     // 0: 대기
//     // 1: 재개
//     // 2: 안전지점 착륙
//     // 3: 출발지점 착륙
//     // 4: 회귀지점 착륙
//     // 5: 배송지점 착륙
//     // 6: 비상 착륙
//   }
//
//   [DataContract]
//   public class EMERGENCY_COMMAND_RES
//   {
//     [DataMember]
//     public string RES_CODE; // 성공: S, 실패: F
//     [DataMember]
//     public string RES_MSG; // 실패시 상세 메시지
//   }
//
//   [DataContract]
//   public class EMERGENCY_ACTION_REQ
//   {
//     [DataMember]
//     public string DR_ID;
//     [DataMember]
//     public double EA_S2D_LAT; // 안전지점 위도
//     [DataMember]
//     public double EA_S2D_LON; // 안전지점 경도
//     [DataMember]
//     public float EA_S2D_ALT; // 안전지점 고도
//     [DataMember]
//     public float EA_S2D_HGT; // 드론이 안전지점까지 이동할 때 유지하는 고도
//   }
//
//   [DataContract]
//   public class EMERGENCY_ACTION_RES
//   {
//     [DataMember]
//     public string RES_CODE; // 성공: S, 실패: F
//     [DataMember]
//     public string RES_MSG; // 실패시 상세 메시지
//   }
//
//   public enum COMMAND_CODE : ushort
//   {
//     CMDID_DRONE_NET_ON_REQ = 11,
//     CMDID_DRONE_NET_ON_RES = 12,
//
//     CMDID_DRONE_STATE_REQ = 21,
//     CMDID_DRONE_STATE_RES = 22,
//
//     CMDID_DELIVERY_ROUTE_SEND_REQ = 31,
//     CMDID_DELIVERY_ROUTE_SEND_RES = 32,
//
//     CMDID_SAFE_ZONE_INFO_SEND_REQ = 41,
//     CMDID_SAFE_ZONE_INFO_SEND_RES = 42,
//
//     CMDID_DELIVERY_START_REQ = 51,
//     CMDID_DELIVERY_START_RES = 52,
//
//     CMDID_EMERGENCY_COMMAND_REQ = 61,
//     CMDID_EMERGENCY_COMMAND_RES = 62,
//
//     CMDID_EMERGENCY_ACTION_REQ = 71,
//     CMDID_EMERGENCY_ACTION_RES = 72,
//
//     #region for mavlink override
//     CMDID_MAVLINK_OVERRIDE_REQ = 90,
//     #endregion
//   }
//
//   [DataContract]
//   public class MAVLINK_SENSOR_STATUS
//   {
//     [DataMember]
//     public string Name;
//     [DataMember]
//     public bool Enabled;
//     [DataMember]
//     public bool Present;
//     [DataMember]
//     public bool Health;
//   }
//
//   [DataContract]
//   public class MAVLINK_DRONE_INFO
//   {
//     [DataMember]
//     public string FrameType;
//     [DataMember]
//     public string ROS; 
//     [DataMember]
//     public string FC_HARDWARE;
//     [DataMember]
//     public string Autopilot;
//     [DataMember]
//     public string CommunicationOut;
//   }