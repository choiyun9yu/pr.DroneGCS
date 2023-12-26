// using System.Runtime.Serialization;
//
// namespace kisa_gcs_service.Model;
//
// /*
//  * [DataContract]와 [DataMember]는 .NET에서 데이터 직렬화를 위해 사용된다.
//  * [DataContract] 어트리뷰트: 이 어트리뷰트는 클래스가 데이터 컨트랙트의 일부임을 나타낸다. 데이터 컨트랙트는 직렬화된 데이터 형식읠 정의하는데 사용된다.
//  *                         클래스에 이 어트리뷰트를 지정하면 해당 클래스의 인스턴스를 직렬화 및 역직렬화할 때 사용할 데이터 구조를 정의한다.
//  * [DataMember] 어트리뷰트: 이 어트리뷰트는 데이터 컨트랙트 내에서 직렬화할 멤버(변수 또는 속성)를 나타낸다.
//  *                       멤버에 이 어트리뷰트를 지정하면 멤버는 직렬화 프로세스에서 고려며되, 이를 통해 해당 멤버의 데이터가 저장 및 전송 된다.
//  * 즉, 이 어트리뷰트들은 클래스의 특정 멤버가 데이터를 표현하고 이를 직렬화할 때 사용되는 규칙을 제공한다.
//  * 이것은 주로 웹 서비스나 데이터베이스와 같은 곳에서 객체의 상태를 전송하거나 저장할 때 유용하다.
//  */
//
//
// // 네트워크 연결 요청 클래스
// [DataContract] public class DRONE_NET_ON_REQ
// {
//     [DataMember] public string? DR_ID;
//     [DataMember] public string? IP_ADDR;
//     [DataMember] public int PORT_NUM;
// }
//
// // 네트워크 연결 응답 클래스
// [DataContract] public class DRONE_NET_ON_RES
// {
//     [DataMember] public char RES_CODE;
//     [DataMember] public string? RES_MSG;
// }
//
// // 드론 상태 응답 구조체
// [DataContract] public struct DRONE_STATE_RES
// {
//     [DataMember] public string DR_ID;
//     [DataMember] public byte DR_STATE;
//     [DataMember] public byte DR_STATE_SUB;
//     [DataMember] public float POWER_V;
//     [DataMember] public string GPS_STATE;
//     [DataMember] public string FWD_CAM_STATE;
//     [DataMember] public float TEMPERATURE_C;
//     [DataMember] public string LOADER_LOAD;
//     [DataMember] public string LOADER_LOCK;
//     [DataMember] public uint WP_NO;
//     [DataMember] public double DR_LAT;
//     [DataMember] public double DR_LON;
//     [DataMember] public float DR_ALT;
//     [DataMember] public float DR_SPEED;
//     [DataMember] public string HOVERING_STATE;
//     [DataMember] public float DR_ROLL;
//     [DataMember] public float DR_PITCH;
//     [DataMember] public float DR_YAW;
//     
//     [DataMember] public float HDOP;
//     [DataMember] public byte FLIGHT_MODE = 255;
//     [DataMember] public byte SAT_COUNT; // satellite
//     [DataMember] public byte MAV_SYS_STATUS; // SYS_STATUS (#1)
//     
//     [DataMember] public MAVLINK_SENSOR_STATUS[] SENSOR_STATUSES;
//     [DataMember] public MAVLINK_DRONE_INFO? MAVLINK_INFO;
//     
//     // 기본 값으로 초기화하는 생성자
//     public DRONE_STATE_RES()
//     {
//         DR_ID = "";
//         DR_STATE = 255;
//         DR_STATE_SUB = 0;
//         POWER_V = 0.0f;
//         GPS_STATE = "F";
//         FWD_CAM_STATE = "F";
//         TEMPERATURE_C = 0.0f;
//         LOADER_LOAD = "F";
//         LOADER_LOCK = "F";
//         WP_NO = 0;
//         DR_LAT = 0.0;
//         DR_LON = 0.0;
//         DR_ALT = 0.0f;
//         DR_SPEED = 0.0f;
//         HOVERING_STATE = "F";
//         DR_ROLL = 0.0f;
//         DR_PITCH = 0.0f;
//         DR_YAW = 0.0f;
//         HDOP = 0.0f;
//         SENSOR_STATUSES = new MAVLINK_SENSOR_STATUS[]{};
//         SAT_COUNT = 0;
//         MAV_SYS_STATUS = 0;
//         MAVLINK_INFO = null;
//     }
// }
//
//
// // MAVLink 센서 상태 클래스
// [DataContract] public class MAVLINK_SENSOR_STATUS
// {
//     [DataMember] public string Name;
//     [DataMember] public bool Enabled;
//     [DataMember] public bool Present;
//     [DataMember] public bool Health;
// }
//
// // MAVLink 드론 정보 클래스
// [DataContract] public class MAVLINK_DRONE_INFO
// {
//     [DataMember] public string FrameType;
//     [DataMember] public string ROS; 
//     [DataMember] public string FC_HARDWARE;
//     [DataMember] public string Autopilot;
//     [DataMember] public string CommunicationOut;
// }
