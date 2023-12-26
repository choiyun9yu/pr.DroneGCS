using MAVSDK;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace kisa_gcs_service.Model;

public class DroneState
{
    // private static readonly string LogFileDateTimeNamingFormat =
    //     DroneConfiguration.Environment.GetValue<string>("LOG_FILE_DATE_TIME_NAMING_FORMAT", "yyyy-mm-dd-HH-mm-ss");
    //
    // 드론의 고유 아이디
    public readonly string DroneId;
    
    // 드론 이동과 관련된 변수들 
    private double _elapsedDistance;
    private double _remainDistance;
    private double _totalDistance;
    private int _pathIndex;
    
    // 드론의 원시 상태 정보
    public DRONE_STATE_RES? DroneRawState { get; set; }
    
    // 드론의 활동 시작 및 완료 시간
    private DateTime _startTime;
    private DateTime _completeTime;
    
    // 드론의 온라인 여부
    public bool IsOnline { get; set; }
    
    // 이벤트들
    public event Action<string, DroneStateStruct>? OnDroneStatsUpdate;
    public event DroneDisconnected? OnDroneDisconnected;
    public event Action<MAVLink.MAVLinkMessage>? OnNewMavlinkMessage;
    
    // 드론 속도, 모니터 유닛, 메시지 카운터 현재 위치 및 타임 스태프 등의 변수  
    public double DroneSpeed;
    private uint _messageCounter = 0;
    private DateTime _lastReceivedStatusMsg;
    public DateTime LastHeartbeatMessage;
    private readonly MAVLink.MavlinkParse _mavlinkParser = new();
    
    
    // 
    public struct DroneStateStruct
    {
        public string DroneId;
        public double ElapsedDistance;
        public double RemainDistance;
        public double TotalDistance;
        public int PathIndex;
        public DRONE_STATE_RES? DroneRawState;
    }

    //
    public delegate void DroneDisconnected(string droneId, DroneStateStruct droneState);
    
    public class NullDateTimeConverter : DateTimeConverterBase
    {
        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue,
            JsonSerializer serializer)
        {
            return reader.Value == null ? DateTime.MinValue : (DateTime)reader.Value;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value == null || ((DateTime)value) == DateTime.MinValue)
            {
                writer.WriteNull();
                return;
            }
            writer.WriteValue(value);
        }
    }
    
    //

    // public DroneStateStruct ToDummyStruct(bool isFull = false)
    // {
    //     return DroneStateStruct(DroneId)
    //     {
    //         ElapsedDistance = _elapsedDistance,
    //         RemainDistance = _remainDistance,
    //         TotalDistance = _totalDistance,
    //         PathIndex = _pathIndex,
    //         DroneRawState = DroneRawState,
    //             
    //     }
    // }
}