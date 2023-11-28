// #nullable enable
//
// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using DroneController.Helper;
// using System.Linq;
// using DroneController.Interfaces;
// using Newtonsoft.Json;
// using Newtonsoft.Json.Converters;
//
// namespace kisa_gcs_service.Controllers
// {
//   public class DroneState
//   {
//     private static readonly string LogFileDateTimeNamingFormat = DroneConfiguration.Environment.GetValue<string>("LOG_FILE_DATE_TIME_NAMING_FORMAT", "yyyy-mm-dd-HH-mm-ss");
//     public readonly string DroneId;
//     private double _elapsedDistance;
//     private double _remainDistance;
//     private double _totalDistance;
//     private int _pathIndex;
//     public DRONE_STATE_RES? DroneRawState { get; set; }
//     private readonly FixedSizedQueue<CGISLocation> _droneTrails = new (600);
//
//     private List<WaypointDistance> DroneProgress { get; set; } = new();
//     private List<WaypointProgress> DroneProgressPercentages { get; set; } = new();
//     private DateTime _startTime;
//     private DateTime _completeTime;
//     private int _nextDroneWaypointNumber = -1;
//     private int _droneWaypointNumberReached = -1;
//     public bool IsOnline { get; set; }
//     private Model.Drone? _droneModel;
//     public readonly DroneDeliveryInfo DroneDeliveryInfo;
//     public DroneCommunication CommunicationLink;
//
//     private readonly DroneDBContext _database;
//     public event Action<string, DroneStateStruct>? OnDroneMonitorStatsUpdate;
//     public event Action<string, DroneDeliveryInfo>? OnFetchedDeliveryInfo;
//     public event Action<string, MAVLink.mavlink_mission_item_int_t[]>? OnDroneMavMissionChanged;
//     public event DroneDisconnected? OnDroneDisconnected;
//     public event Action<MAVLink.MAVLinkMessage>? OnNewMavlinkMessage;
//     private DroneParameterLoader? _parametersLoader;
//     private readonly MavMissionMicroservice _missionMicroservice;
//     public MAVLink.mavlink_mission_item_int_t[]? MavMission => this._missionMicroservice.MavMission;
//     public event Action<MavlinkParameter[]>? OnMavlinkParametersFetched;
//     public event Action<ProgressEvent>? OnNewProgressEvent;
//     public event Action<MavlinkParameter>? OnMavlinkParameterUpdated;
//     public double DroneSpeed;
//     public IDroneMonitorUnit? MonitorUnit;
//     private uint _messageCounter = 0;
//     private CGISLocation _droneCurrentLocation;
//     private DateTime _lastReceivedStatusMsg;
//     private DateTime _lastAddedTrails;
//     public DateTime LastHeartbeatMessage;
//     private Logger? _planLogger;
//     public readonly List<MavlinkSeverityLog> DroneLogger = new();
//
//     private string? _cameraIp;
//     private string? _cameraUrl1;
//     private string? _cameraUrl2;
//     private CameraType? _cameraProtocolType;
//     private DroneCameraHandler? _cameraConnectionHandler;
//     public AbstractCameraInterface? CameraInterface;
//
//     private readonly MAVLink.MavlinkParse _mavlinkParser = new();
//
//     public List<WaypointDistance> WaypointsDistance { get; set; } = new();
//
//     public MavlinkParameter[]? MavParameters => this._parametersLoader?.Parameters;
//
//     private readonly DroneVideoService _videoService = new();
//
//     public static DroneStateStruct TrimDownStruct(DroneStateStruct fullStruct)
//     {
//       fullStruct.DroneTrails = null;
//       fullStruct.DroneLogger = new List<MavlinkSeverityLog>();
//       return fullStruct;
//     }
//
//     public DroneStateStruct ToDummyStruct(bool isFull = false)
//     {
//       return new DroneStateStruct(this.DroneId)
//       {
//         MavMission = this._missionMicroservice?.MavMission,
//         ElapsedDistance = this._elapsedDistance,
//         RemainDistance = this._remainDistance,
//         TotalDistance = this._totalDistance,
//         PathIndex = this._pathIndex,
//         DroneRawState = this.DroneRawState,
//         DroneTrails = isFull ? this._droneTrails.q: null,
//         DroneLogger = isFull ? this.DroneLogger : new List<MavlinkSeverityLog>(),
//         DroneProgress = this.DroneProgress,
//         DroneProgressPercentages = this.DroneProgressPercentages,
//         StartTime = this._startTime,
//         CompleteTime = this._completeTime,
//         CommunicationLink = this.CommunicationLink,
//         HasDeliveryPlan = this.DroneDeliveryInfo.DeliveryPlan != null,
//         LastHeartbeatMessage = this.LastHeartbeatMessage,
//         CameraIP = this._cameraIp,
//         CameraURL1 = this._cameraUrl1,
//         CameraURL2 = this._cameraUrl2,
//         CameraProtocolType = this._cameraProtocolType,
//         IsOnline = this.IsOnline
//       };
//     }
//
//     public static double MavMissionTotalDistance(MAVLink.mavlink_mission_item_int_t[] missionItems)
//     {
//       var firstItem = missionItems.Skip(1)
//         .First(item => item.command is (byte)MAVLink.MAV_CMD.WAYPOINT);
//       var waypointItems = missionItems
//         .Where(item => item.command is (byte)MAVLink.MAV_CMD.WAYPOINT or (byte)MAVLink.MAV_CMD.RETURN_TO_LAUNCH)
//         .ToList();
//       return waypointItems.Skip(1)
//         .Select((item, index) => CDroneMap.GetDistanceTwoGISLocations(
//           MissionItemIntToCgisLocation(item, firstItem),
//           MissionItemIntToCgisLocation(waypointItems[index], firstItem),
//           true
//         )).Sum();
//     }
//
//     private static CGISLocation MissionItemIntToCgisLocation(MAVLink.mavlink_mission_item_int_t item, MAVLink.mavlink_mission_item_int_t firstItem)
//     {
//       return new CGISLocation
//       {
//         // @TODO: Fix altitude based on `frame` type
//         m_dHeight = item.z / 1000,
//         m_dLatitude = item.command is (byte)MAVLink.MAV_CMD.RETURN_TO_LAUNCH
//           ? ((double)firstItem.x) / 10000000
//           : ((double)item.x) / 10000000,
//         m_dLongitude = item.command is (byte)MAVLink.MAV_CMD.RETURN_TO_LAUNCH
//           ? ((double)firstItem.y) / 10000000
//           : ((double)item.y) / 10000000
//       };
//     }
//
//     public DroneState(string droneId, DroneDBContext database, IDroneMonitorUnit? monitorUnit, DroneCommunication? communication = null)
//     {
//       this._missionMicroservice = new MavMissionMicroservice(this, droneId);
//       this.DroneId = droneId;
//       this._database = database;
//       this.DroneDeliveryInfo = new DroneDeliveryInfo(database, droneId);
//       this.MonitorUnit = monitorUnit;
//       this.CommunicationLink = communication ?? new DroneCommunication(
//         DroneConnectionProtocol.UDP,
//         DroneMessageProtocol.MAVLINK,
//         ""
//       );
//     }
//
//     private void _handleHeartbeat(MAVLink.MAVLinkMessage droneMessage)
//     {
//       var droneInfoData = (MAVLink.mavlink_heartbeat_t)(droneMessage.data);
//       var droneState = new DRONE_STATE_RES()
//       {
//         DR_ID = droneMessage.sysid.ToString(),
//         FLIGHT_MODE = ((byte)droneInfoData.custom_mode),
//         MAVLINK_INFO = new MAVLINK_DRONE_INFO()
//         {
//           FrameType = ((MAVLink.MAV_TYPE)droneInfoData.type).ToString(),
//           Autopilot = ((MAVLink.MAV_AUTOPILOT)droneInfoData.autopilot).ToString(),
//         },
//       };
//       this.UpdateDroneStatus(droneState);
//     }
//
//     public async Task HandleMavlinkMessage(MAVLink.MAVLinkMessage droneMessage)
//     {
//       this.OnNewMavlinkMessage?.Invoke(droneMessage);
//       switch ((MAVLink.MAVLINK_MSG_ID)droneMessage.msgid)
//       {
//         case MAVLink.MAVLINK_MSG_ID.HEARTBEAT:
//         {
//           this._handleHeartbeat(droneMessage);
//           break;
//         }
//         case MAVLink.MAVLINK_MSG_ID.GLOBAL_POSITION_INT:
//         {
//           var data = (MAVLink.mavlink_global_position_int_t)droneMessage.data;
//           var droneState = new DRONE_STATE_RES()
//           {
//             DR_ID = droneMessage.sysid.ToString(),
//             DR_LAT = (float)data.lat * 1.0 / 10000000,
//             DR_LON = (float)data.lon * 1.0 / 10000000,
//             DR_ALT = data.relative_alt,
//             DR_SPEED = (float)Math.Sqrt(data.vy * data.vy + data.vx * data.vx + data.vz * data.vz) / 100f,
//           };
//
//           this.UpdateDroneStatus(droneState, true);
//           break;
//         }
//         case MAVLink.MAVLINK_MSG_ID.GPS_RAW_INT:
//         {
//           var data = (MAVLink.mavlink_gps_raw_int_t)droneMessage.data;
//           var droneState = new DRONE_STATE_RES()
//           {
//             HDOP = data.eph < ushort.MaxValue ? ((float)data.eph) / 100f : 0,
//             SAT_COUNT = data.satellites_visible
//           };
//           this.UpdateDroneStatus(droneState);
//           break;
//         }
//         case MAVLink.MAVLINK_MSG_ID.SYS_STATUS:
//         {
//           var data = (MAVLink.mavlink_sys_status_t)droneMessage.data;
//           var sensorHealth = data.onboard_control_sensors_health;
//           var sensors = new MAVLink.MAV_SYS_STATUS_SENSOR[] {
//             MAVLink.MAV_SYS_STATUS_SENSOR._3D_GYRO,
//             MAVLink.MAV_SYS_STATUS_SENSOR.MAV_SYS_STATUS_AHRS,
//             MAVLink.MAV_SYS_STATUS_SENSOR._3D_ACCEL,
//             MAVLink.MAV_SYS_STATUS_SENSOR.GPS,
//             MAVLink.MAV_SYS_STATUS_SENSOR.ATTITUDE_STABILIZATION,
//             MAVLink.MAV_SYS_STATUS_SENSOR.BATTERY,
//             MAVLink.MAV_SYS_STATUS_SENSOR.MAV_SYS_STATUS_PREARM_CHECK,
//             MAVLink.MAV_SYS_STATUS_SENSOR.MAV_SYS_STATUS_TERRAIN,
//           }.Select(sensor => new MAVLINK_SENSOR_STATUS()
//           {
//             Name = sensor.ToString(),
//             Health = (sensorHealth & (uint)sensor) == (uint)sensor,
//             Enabled = (data.onboard_control_sensors_enabled & (uint)sensor) == (uint)sensor,
//             Present = (data.onboard_control_sensors_present & (uint)sensor) == (uint)sensor,
//           });
//           var droneState = new DRONE_STATE_RES()
//           {
//             POWER_V = (float)(data.voltage_battery * 1.0 / 1000),
//           };
//           droneState.SENSOR_STATUSES = sensors.ToArray();
//           this.UpdateDroneStatus(droneState);
//           break;
//         }
//         case MAVLink.MAVLINK_MSG_ID.ATTITUDE:
//         {
//           var data = (MAVLink.mavlink_attitude_t)droneMessage.data;
//           var droneState = new DRONE_STATE_RES()
//           {
//             DR_ID = droneMessage.sysid.ToString(),
//             DR_ROLL = data.roll,
//             DR_PITCH = data.pitch,
//             DR_YAW = data.yaw,
//           };
//
//           this.UpdateDroneStatus(droneState);
//           break;
//         }
//         case MAVLink.MAVLINK_MSG_ID.BATTERY_STATUS:
//         {
//           var data = (MAVLink.mavlink_battery_status_t)droneMessage.data;
//           var droneState = new DRONE_STATE_RES()
//           {
//             DR_ID = droneMessage.sysid.ToString(),
//           };
//
//           this.UpdateDroneStatus(droneState);
//
//           break;
//         }
//         case MAVLink.MAVLINK_MSG_ID.STATUSTEXT:
//         {
//           var data = (MAVLink.mavlink_statustext_t)droneMessage.data;
//           var droneState = new DRONE_STATE_RES()
//           {
//             MAV_SYS_STATUS = data.severity,
//           };
//           this.UpdateDroneStatus(droneState);
//           var text = string.Join("", data.text.Select(c => (char)c));
//           if (text.StartsWith("Disarming motors"))
//           {
//             this.HandleSetDisarmTime();
//           }
//           this.UpdateDroneLog(text, data.severity);
//           break;
//         }
//         case MAVLink.MAVLINK_MSG_ID.COMMAND_ACK:
//         {
//           var data = (MAVLink.mavlink_command_ack_t)droneMessage.data;
//           if (data.command == (ushort)MAVLink.MAV_CMD.TAKEOFF && data.result == (byte)MAVLink.MAV_RESULT.ACCEPTED)
//           {
//             this.HandleSetTakeoffTime();
//           }
//           break;
//         }
//         case MAVLink.MAVLINK_MSG_ID.AUTOPILOT_VERSION:
//         {
//           var data = (MAVLink.mavlink_autopilot_version_t)droneMessage.data;
//
//           this.UpdateDroneLog(string.Join("", data.os_custom_version.Select(c => (char)c)), 6);
//           break;
//         }
//         case MAVLink.MAVLINK_MSG_ID.MISSION_CURRENT:
//         {
//           var data = (MAVLink.mavlink_mission_current_t)droneMessage.data;
//           this.HandleSetNextDroneWaypointNumber(data.seq);
//           break;
//         }
//         case MAVLink.MAVLINK_MSG_ID.MISSION_ITEM_REACHED:
//         {
//           var data = (MAVLink.mavlink_mission_item_reached_t)droneMessage.data;
//           this.HandleSetDroneWaypointNumberReached(data.seq);
//           break;
//         }
//         case MAVLink.MAVLINK_MSG_ID.HIGHRES_IMU:
//         {
//           var data = (MAVLink.mavlink_highres_imu_t)droneMessage.data;
//           var droneState = new DRONE_STATE_RES()
//           {
//             DR_ID = droneMessage.sysid.ToString(),
//             TEMPERATURE_C = data.temperature
//           };
//
//           this.UpdateDroneStatus(droneState);
//           break;
//         }
//       }
//     }
//
//     private void UpdateDroneLog(string log, int severity)
//     {
//       this.DroneLogger.Add(new MavlinkSeverityLog()
//       {
//         time = DateTime.Now,
//         severity = severity,
//         message = log
//       });
//     }
//     public async Task CancelDeliveryPlan()
//     {
//       if (this._planLogger != null)
//       {
//         await this._planLogger.CloseAsync();
//         this._planLogger = null;
//       }
//       this.DroneDeliveryInfo.CleanUpDeliveryPlan();
//       this.CleanUpOldMission();
//       OnFetchedDeliveryInfo?.Invoke(this.DroneId, this.DroneDeliveryInfo);
//       OnDroneMonitorStatsUpdate?.Invoke(this.DroneId, this.ToDummyStruct(true));
//     }
//     public async Task FetchDeliveryPlan()
//     {
//       if (this.DroneDeliveryInfo.DeliveryPlan == null ||
//         this.DroneDeliveryInfo.DeliveryPlan.Status != Model.DeliveryPlanStatus.Delivering)
//       {
//         if ((await this.DroneDeliveryInfo.Fetch()))
//         {
//           this._planLogger = new Logger(
//             $"{DateTime.Now.ToString(LogFileDateTimeNamingFormat)}-{this.DroneDeliveryInfo.DeliveryPlan?.DeliveryID}"
//           );
//           this._droneModel = this.DroneDeliveryInfo.DeliveryPlan?.Drone;
//           this.CleanUpOldMission();
//           OnFetchedDeliveryInfo?.Invoke(this.DroneId, this.DroneDeliveryInfo);
//           OnDroneMonitorStatsUpdate?.Invoke(this.DroneId, this.ToDummyStruct(true));
//         }
//       }
//     }
//     public async Task ResponseToDroneConnectRequest()
//     {
//       if (this.DroneDeliveryInfo.DeliveryPlan != null)
//       {
//         await this.SendAcceptConnectAsync();
//         await this.SendStatusRequest();
//         await this.SendDeliveryInfoAsync();
//       }
//       else
//       {
//         await this.SendAcceptConnectAsync();
//         await this.SendStatusRequest();
//       }
//     }
//     public async Task SetStatusOfflineAsync()
//     {
//       if (this._droneModel != null)
//       {
//         await this.UpdateDroneState('0');
//       }
//       this.IsOnline = false;
//       var dummyStruct = this.ToDummyStruct(true);
//       this.OnDroneDisconnected?.Invoke(this.DroneId, dummyStruct);
//       this.OnDroneMonitorStatsUpdate?.Invoke(this.DroneId, dummyStruct);
//     }
//     private void CleanUpOldMission()
//     {
//       this._completeTime = new DateTime(0);
//       this._startTime = new DateTime(0);
//       this.DroneProgress = new List<WaypointDistance>();
//       this.DroneProgressPercentages = new List<WaypointProgress>();
//       this.DroneRawState = null;
//       this._elapsedDistance = 0;
//       this._totalDistance = 0;
//       this._nextDroneWaypointNumber = 0;
//       this._remainDistance = 0;
//       this._pathIndex = 0;
//     }
//
//     private void HandleSetNextDroneWaypointNumber(ushort seq)
//     {
//       this._nextDroneWaypointNumber = seq;
//       var droneState = new DRONE_STATE_RES()
//       {
//         DR_ID = this.DroneId,
//         WP_NO = seq
//       };
//       this.UpdateDroneStatus(droneState);
//     }
//
//     public void HandleSetTakeoffTime()
//     {
//       this.StartRecordVideo();
//       this._startTime = DateTime.Now;
//     }
//
//     public void StartRecordVideo()
//     {
//       if (this._cameraUrl1 is not null)
//       {
//         this._videoService.CaptureStream(this._cameraUrl1, this.DroneId, "1");
//       }
//       if (this._cameraUrl2 is not null)
//       {
//         this._videoService.CaptureStream(this._cameraUrl2, this.DroneId, "2");
//       }
//     }
//     public void HandleSetDisarmTime()
//     {
//       this.StopRecordVideo();
//       this._completeTime = DateTime.Now;
//     }
//
//     public void StopRecordVideo()
//     {
//       if (this._cameraUrl1 is not null)
//       {
//         this._videoService.FinishStream(this.DroneId, "1");
//       }
//       if (this._cameraUrl2 is not null)
//       {
//         this._videoService.FinishStream(this.DroneId, "2");
//       }
//     }
//     public void HandleSetDroneWaypointNumberReached(ushort seq)
//     {
//       if (this.MavMission?.Length > 0 && seq == this.MavMission.Length - 1)
//       {
//         this._completeTime = DateTime.Now;
//         if (this.DroneRawState != null)
//         {
//           var state = this.DroneRawState.Value;
//           state.WP_NO = seq;
//           this.DroneRawState = state;
//         }
//       }
//       this._droneWaypointNumberReached = seq;
//     }
//     public async Task SetStatusOnlineAsync()
//     {
//       if (this._droneModel != null)
//       {
//         await this.UpdateDroneState('1');
//       }
//       this.IsOnline = true;
//     }
//     private async Task UpdateDroneState(char status)
//     {
//       if (this._droneModel is null)
//       {
//         return;
//       }
//       this._droneModel.Status = status;
//       await this._database.SaveChangesAsync();
//     }
//
//     private double GetCurrentDistance(CGISLocation cur)
//     {
//       if (this._nextDroneWaypointNumber == this.MavMission?.Length - 1)
//       {
//         return this.DroneDeliveryInfo.TotalDistance;
//       }
//       var prev = this.DroneDeliveryInfo.WaypointsDistance[this._nextDroneWaypointNumber - 1];
//       return prev.Distance + Math.Abs(CDroneMap.GetDistanceTwoGISLocations(prev.Location, cur, true));
//     }
//     private double GetRemainDistance(CGISLocation cur)
//     {
//       if (this._nextDroneWaypointNumber == this.DroneDeliveryInfo.Waypoints?.Count() - 1)
//       {
//         return 0;
//       }
//       var next = this.DroneDeliveryInfo.WaypointsDistance[this._nextDroneWaypointNumber];
//       return (this.DroneDeliveryInfo.TotalDistance - next.Distance) +
//         Math.Abs(CDroneMap.GetDistanceTwoGISLocations(cur, next.Location, true));
//     }
//     private void UpdateDroneProgress(DRONE_STATE_RES? droneRawStatus)
//     {
//       CGISLocation droneLocation;
//       droneLocation.m_dHeight = droneRawStatus?.DR_ALT ?? 0;
//       droneLocation.m_dLatitude = droneRawStatus?.DR_LAT ?? 0;
//       droneLocation.m_dLongitude = droneRawStatus?.DR_LON ?? 0;
//       
//       // throttle to 0.5s
//       if (DateTime.Now.Subtract(this._lastReceivedStatusMsg).Milliseconds <= 500) return;
//       if (this.MavMission?.Length > 0)
//       {
//         this._totalDistance = MavMissionTotalDistance(this.MavMission);
//         this._elapsedDistance += CDroneMap.GetDistanceTwoGISLocations(this._droneCurrentLocation, droneLocation);
//         this._elapsedDistance = Math.Min(this._elapsedDistance, this._totalDistance);
//         this._remainDistance = this._totalDistance - this._elapsedDistance;
//       }
//       this._droneCurrentLocation = droneLocation;
//       this._lastReceivedStatusMsg = DateTime.Now;
//     }
//
//     private DroneParameterLoader GetOrCreateParametersLoader()
//     {
//       if (this._parametersLoader is not null) return this._parametersLoader;
//       this._parametersLoader = new DroneParameterLoader(this);
//       this._parametersLoader.Progress += (e) =>
//       {
//         this.OnNewProgressEvent?.Invoke(e);
//       };
//       this._parametersLoader.Finished += () =>
//       {
//         Console.WriteLine("Parameters load finished");
//         this.OnMavlinkParametersFetched?.Invoke(this._parametersLoader.Parameters!);
//       };
//       this._parametersLoader.UpdatedParamValue += (param) =>
//       {
//         Console.WriteLine("Parameter updated");
//         this.OnMavlinkParameterUpdated?.Invoke(
//           DroneParameterLoader.ConvertMavParamValueToClassMavlinkParameter(param));
//       };
//       return this._parametersLoader;
//     }
//     public async Task LoadMavLinkParameters()
//     {
//       await this.GetOrCreateParametersLoader().StartDownloading();
//     }
//
//     public async Task UpdateMavlinkParamValue(string paramId, float paramValue)
//     {
//       await this.GetOrCreateParametersLoader().UpdateDroneParameterValue(paramId, paramValue);
//     }
//
//     private static DRONE_STATE_RES MergeDroneRawState(DRONE_STATE_RES? nFirst, DRONE_STATE_RES second)
//     {
//       if (nFirst == null) return second;
//       var first = nFirst.Value;
//
//       return new DRONE_STATE_RES()
//       {
//         DR_LAT = second.DR_LAT != 0 ? second.DR_LAT : first.DR_LAT,
//         DR_LON = second.DR_LON != 0 ? second.DR_LON : first.DR_LON,
//         DR_ALT = second.DR_ALT != 0 ? second.DR_ALT : first.DR_ALT,
//         DR_YAW = second.DR_YAW != 0 ? second.DR_YAW : first.DR_YAW,
//         DR_PITCH = second.DR_PITCH != 0 ? second.DR_PITCH : first.DR_PITCH,
//         DR_ROLL = second.DR_ROLL != 0 ? second.DR_ROLL : first.DR_ROLL,
//         DR_SPEED = second.DR_SPEED != 0 ? second.DR_SPEED : first.DR_SPEED,
//         POWER_V = second.POWER_V != 0 ? second.POWER_V : first.POWER_V,
//         HDOP = second.HDOP != 0 ? second.HDOP : first.HDOP,
//         FLIGHT_MODE = second.FLIGHT_MODE != 255 ? second.FLIGHT_MODE : first.FLIGHT_MODE,
//         GPS_STATE = second.GPS_STATE,
//         SAT_COUNT = second.SAT_COUNT != 0 ? second.SAT_COUNT : first.SAT_COUNT,
//         FWD_CAM_STATE = second.FWD_CAM_STATE,
//         DR_STATE = second.DR_STATE != 0 ? second.DR_STATE : first.DR_STATE,
//         DR_STATE_SUB = second.DR_STATE_SUB != 0 ? second.DR_STATE_SUB : first.DR_STATE_SUB,
//         TEMPERATURE_C = second.TEMPERATURE_C != 0 ? second.TEMPERATURE_C : first.TEMPERATURE_C,
//         MAV_SYS_STATUS = second.MAV_SYS_STATUS != 0 ? second.MAV_SYS_STATUS : first.MAV_SYS_STATUS,
//         SENSOR_STATUSES = (second.SENSOR_STATUSES.Length > 0) ? second.SENSOR_STATUSES : first.SENSOR_STATUSES,
//         WP_NO = second.WP_NO != 0 ? second.WP_NO : first.WP_NO,
//       };
//     }
//     public void UpdateDroneStatus(DRONE_STATE_RES rawDroneStatusData, bool updatedLocation = false)
//     {
//       var droneStatusData = MergeDroneRawState(this.DroneRawState, rawDroneStatusData);
//       if (updatedLocation)
//       {
//         this.UpdateDroneProgress(droneStatusData);
//         if (DateTime.Now.Subtract(this._lastAddedTrails).Milliseconds > 500)
//         {
//           this._droneTrails.Enqueue(new CGISLocation()
//           {
//             m_dHeight = droneStatusData.DR_ALT,
//             m_dLatitude = droneStatusData.DR_LAT,
//             m_dLongitude = droneStatusData.DR_LON
//           });
//           this._lastAddedTrails = DateTime.Now;
//         }
//
//       }
//       this.DroneRawState = droneStatusData;
//       // if (this.MAVMission is { Length: > 0 })
//       // {
//       //   if (this.NextDroneWaypointNumber >= 0)
//       //   {
//       //     this.updateDistanceProgress(this.DroneRawState, this.NextDroneWaypointNumber > 0);
//       //   }
//       //   if (this.planLogger != null)
//       //   {
//       //    await this.planLogger.LogAsync(String.Format("Received message from drone {0}: {1}", this.DroneId, droneStatusData.ToString()));
//       //   }
//       // }
//
//       OnDroneMonitorStatsUpdate?.Invoke(this.DroneId, this.ToDummyStruct(true));
//     }
//     private void UpdateDistanceProgress(DRONE_STATE_RES droneStats, bool passedStartingPoint)
//     {
//       var droneLocation = new CGISLocation
//       {
//         m_dHeight = droneStats.DR_ALT,
//         m_dLatitude = droneStats.DR_LAT,
//         m_dLongitude = droneStats.DR_LON
//       };
//
//       var droneProgress = new WaypointProgress
//       {
//         Location = droneLocation,
//         PathIndex = this._pathIndex
//       };
//       if (passedStartingPoint)
//       {
//         var droneStatusProgress = new WaypointDistance
//         {
//           Location = droneLocation,
//           Distance = GetCurrentDistance(droneLocation)
//         };
//         this.DroneProgress.Add(droneStatusProgress);
//
//         var curPathStartLocation = this.DroneDeliveryInfo.CWaypointList![this._pathIndex].First().m_tGISLoc;
//         var curPathEndLocation = this.DroneDeliveryInfo.CWaypointList[this._pathIndex].Last().m_tGISLoc;
//         var distanceFromStartLoc = CDroneMap.GetDistanceTwoGISLocations(curPathStartLocation, droneLocation, true);
//         var distanceToEndLoc = CDroneMap.GetDistanceTwoGISLocations(droneLocation, curPathEndLocation, true);
//         var numberOfGoogleWaypoints = this.DroneDeliveryInfo.GoogleWaypoints![this._pathIndex]?.Count ?? 0;
//         droneProgress.Progress = (distanceFromStartLoc * (numberOfGoogleWaypoints - 1)) / (distanceFromStartLoc + distanceToEndLoc);
//       }
//       else
//       {
//         droneProgress.Progress = 0;
//       }
//       this.DroneProgressPercentages.Add(droneProgress);
//     }
//     public Task SendAcceptConnectAsync()
//     {
//       var responseBody = new DRONE_NET_ON_RES
//       {
//         RES_CODE = 'S',
//         RES_MSG = string.Empty
//       };
//       return this.SendMessageAsync(COMMAND_CODE.CMDID_DRONE_NET_ON_RES, responseBody);
//     }
//     public Task SendStatusRequest()
//     {
//       return this.SendMessageAsync(COMMAND_CODE.CMDID_DRONE_STATE_REQ);
//     }
//     public async Task SendDeliveryInfoAsync()
//     {
//       if (this.DroneDeliveryInfo.Waypoints is null)
//       {
//         return;
//       }
//       var deliveryRequest = new DELIVERY_ROUTE_SEND_REQ
//       {
//         DR_ID = this.DroneId,
//         WAYPOINTS = shiftWPNoBy1(this.DroneDeliveryInfo.Waypoints),
//         WP_COUNT = (uint)DroneDeliveryInfo.Waypoints.Length
//       };
//       await this.SendMessageAsync(COMMAND_CODE.CMDID_DELIVERY_ROUTE_SEND_REQ, deliveryRequest);
//     }
//     /**
//       Drone has waypoint number's counted from 1
//      */
//     private static DELIVERY_ROUTE_WAYPOINTS[] shiftWPNoBy1(IEnumerable<DELIVERY_ROUTE_WAYPOINTS> waypoints)
//     {
//       return waypoints.Select(waypoint =>
//       {
//         var wp = DELIVERY_ROUTE_WAYPOINTS.Copy(waypoint);
//         wp.WP_S2D_NO += 1;
//         return wp;
//       }).ToArray();
//     }
//     public async Task SendStartSignalAsync()
//     {
//       if (this._droneModel is null)
//       {
//         return;
//       }
//       await this.SendMessageAsync(COMMAND_CODE.CMDID_DELIVERY_START_REQ);
//       this._droneModel.Status = '0';
//       this._droneModel.DeliveryStatus = Model.DroneDeliveryStatus.DELIVERING;
//       await this._database.SaveChangesAsync();
//       await this.DroneDeliveryInfo.UpdateDeliveryPlanModelAsync(Model.DeliveryPlanStatus.Delivering);
//       this._startTime = DateTime.Now;
//     }
//
//     public async Task SendMavlinkMsg(MAVLink.MAVLinkMessage msg)
//     {
//       if (this.MonitorUnit != null)
//       {
//         await this.MonitorUnit.SendMessageAsync(msg);
//       }
//     }
//
//     public async Task SendStartMavMission()
//     {
//       if (this.MavMission != null)
//       {
//         await this.SendMavlinkMsg(new MAVLink.MAVLinkMessage(this._mavlinkParser.GenerateMAVLinkPacket20(
//           MAVLink.MAVLINK_MSG_ID.COMMAND_INT, new MAVLink.mavlink_command_int_t()
//           {
//             command = (ushort)MAVLink.MAV_CMD.MISSION_START,
//             target_system = byte.Parse(this.DroneId),
//           })));
//       }
//     }
//     private async Task SendMessageAsync(COMMAND_CODE commandId, object? content = null)
//     {
//       if (this.MonitorUnit is null)
//       {
//         return;
//       }
//       var droneMessage = new DroneMessage(commandId, this._messageCounter);
//       if (content != null)
//       {
//         droneMessage.RawContent = JsonConvert.SerializeObject(content);
//       }
//       this._messageCounter += 1;
//       await this.MonitorUnit.SendMessageAsync(droneMessage);
//       if (this._planLogger != null)
//       {
//         await this._planLogger.LogAsync(
//           $"Sent message with ID {commandId}, content length: {droneMessage.RawContent.Length}, to Drone {this.DroneId}");
//       }
//     }
//
//     public async Task SendMessageToDroneCameraAsync(byte[] data)
//     {
//       if (this._cameraConnectionHandler is null)
//       {
//         return;
//       }
//       try
//       {
//         await this._cameraConnectionHandler.SendMessage(data);
//       }
//       catch (System.Exception e)
//       {
//         Console.WriteLine("Fuck error" + e.Message);
//       }
//     }
//
//     public string[] GetSavedVideos()
//     {
//       return this._videoService.GetDroneStreamVideoList(this.DroneId);
//     }
//
//     #region Camera
//
//     public async Task UpdateDroneCameraAddress(
//       string? address, CameraType? type, string? cameraAddress1, string? cameraAddress2)
//     {
//       if ((address != null && type != null) && this._cameraIp != address || this._cameraProtocolType != type)
//       {
//         this.CameraInterface = type == CameraType.Cam1 ? new CameraPacket() : new CameraPacket10X();
//       }
//       
//       this._cameraIp = address;
//       this._cameraUrl1 = cameraAddress1;
//       this._cameraUrl2 = cameraAddress2;
//       this._cameraProtocolType = type;
//       
//       if (this._cameraConnectionHandler != null)
//       {
//         await this._cameraConnectionHandler.DisconnectAsync();
//       }
//       this._cameraConnectionHandler = new DroneCameraHandler(address, type == CameraType.Cam1 ? 32 : 24);
//       await this._cameraConnectionHandler.StartConnectionAsync();
//     }
//
//     #endregion
//
//     #region Mavlink Mission
//
//     public async Task StartMAVMission(MAVLink.mavlink_mission_item_int_t[] missionItems, MAVLink.MAV_MISSION_TYPE missionType)
//     {
//       await this._missionMicroservice.StartMAVMission(missionItems, missionType);
//     }
//
//     public void SetMavMission(MAVLink.mavlink_mission_item_int_t[] missionItems, MAVLink.MAV_MISSION_TYPE missionType)
//     {
//       this._missionMicroservice.SetMavMission(missionItems, missionType);
//     }
//
//     public async Task StartDownloadMAVMission(MAVLink.MAV_MISSION_TYPE missionType)
//     {
//       await this._missionMicroservice.StartDownloadMAVMission(missionType);
//     }
//
//     public async Task SendClearMavMissionCommand(MAVLink.MAV_MISSION_TYPE missionType)
//     {
//       await this._missionMicroservice.SendClearMavMissionCommand(missionType);
//     }
//
//     #endregion
//
//     #region handle progress
//
//     public void HandleProgressEvent(ProgressEvent e)
//     {
//       this.OnNewProgressEvent?.Invoke(e);
//     }
//
//     public void HandleMavMissionChanged(MAVLink.mavlink_mission_item_int_t[] mavMission)
//     {
//       this.OnDroneMavMissionChanged?.Invoke(this.DroneId, mavMission);
//     }
//
//     #endregion
//   }
//
//   public class NullDateTimeConverter : DateTimeConverterBase
//   {
//     public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
//     {
//       return reader.Value == null ? DateTime.MinValue : (DateTime)reader.Value;
//     }
//
//     public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
//     {
//       if (value == null || ((DateTime)value) == DateTime.MinValue)
//       {
//         writer.WriteNull();
//         return;
//       }
//       writer.WriteValue(value);
//     }
//   }
//   public delegate void DroneDisconnected(string droneId, DroneStateStruct droneState);
//   public enum DroneConnectionProtocol
//   {
//     UDP, TCP, SERIAL, UDPCI
//   }
//   public enum DroneMessageProtocol
//   {
//     ETRI, MAVLINK
//   }
//   public struct MavlinkSeverityLog
//   {
//     public int severity;
//     public string message;
//     public DateTime time;
//   }
//   public class DroneCommunication
//   {
//     public DroneConnectionProtocol ConnectionProtocol;
//     public DroneMessageProtocol MessageProtocol;
//     public string Address;
//
//     public DroneCommunication(DroneConnectionProtocol connectionProtocol, DroneMessageProtocol messageProtocol, string address)
//     {
//       this.ConnectionProtocol = connectionProtocol;
//       this.MessageProtocol = messageProtocol;
//       this.Address = address;
//     }
//
//     public override string ToString()
//     {
//       return this.MessageProtocol + ":" + this.ConnectionProtocol + ":" + this.Address;
//     }
//
//     public override bool Equals(object? obj)
//     {
//       return obj != null && obj.ToString() == this.ToString();
//     }
//     public override int GetHashCode()
//     {
//       return this.ToString().GetHashCode();
//     }
//
//     public static DroneCommunication Parse(String address)
//     {
//       var tokens = address.Split(":");
//       if (tokens.Length != 2 && tokens.Length != 3)
//       {
//         throw new Exception("Invalid Address");
//       }
//       if (tokens.Length == 2)
//       {
//         return new DroneCommunication(
//           DroneConnectionProtocol.UDP,
//           DroneMessageProtocol.MAVLINK,
//           address
//         );
//       }
//       return new DroneCommunication(
//           tokens[0].ToUpper() == "TCP" ? DroneConnectionProtocol.TCP
//             : tokens[0].ToUpper() == "UDP" ? DroneConnectionProtocol.UDP
//             : DroneConnectionProtocol.SERIAL,
//           DroneMessageProtocol.MAVLINK,
//           tokens[1] + ":" + tokens[2]
//           );
//     }
//   }
//
//   public struct ProgressEvent
//   {
//     public int Total;
//     public int Current;
//     public string Type;
//   }
//
//   public struct DroneStateStruct
//   {
//     public string DroneId;
//     public double ElapsedDistance;
//     public double RemainDistance;
//     public double TotalDistance;
//     public int PathIndex;
//     public DRONE_STATE_RES? DroneRawState;
//
//     public IEnumerable<CGISLocation>? DroneTrails;
//     public List<WaypointDistance>? DroneProgress;
//     public List<WaypointProgress>? DroneProgressPercentages;
//
//     [JsonConverter(typeof(NullDateTimeConverter))]
//     public DateTime StartTime;
//
//     [JsonConverter(typeof(NullDateTimeConverter))]
//     public DateTime CompleteTime;
//     public bool IsOnline { get; set; }
//     public DroneCommunication? CommunicationLink;
//     public bool HasDeliveryPlan;
//     public MAVLink.mavlink_mission_item_int_t[]? MavMission;
//     public double DroneSpeed;
//     public DateTime LastHeartbeatMessage;
//     public List<MavlinkSeverityLog> DroneLogger = new();
//
//     public string? CameraIP;
//     public string? CameraURL1;
//     public string? CameraURL2;
//     public CameraType? CameraProtocolType;
//     public List<WaypointDistance> WaypointsDistance { get; set; } = new();
//
//     public DroneStateStruct(string droneId)
//     {
//       DroneId = droneId;
//       MavMission = null;
//       ElapsedDistance = 0;
//       RemainDistance = 0;
//       TotalDistance = 0;
//       PathIndex = 0;
//       DroneRawState = null;
//       DroneTrails = null;
//       DroneProgress = null;
//       DroneProgressPercentages = null;
//       StartTime = default;
//       CompleteTime = default;
//       CommunicationLink = null;
//       HasDeliveryPlan = false;
//       DroneSpeed = 0;
//       LastHeartbeatMessage = default;
//       CameraIP = null;
//       CameraURL1 = null;
//       CameraURL2 = null;
//       CameraProtocolType = null;
//       IsOnline = false;
//     }
//   }
// }
