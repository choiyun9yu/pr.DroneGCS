using gcs_system.Interfaces;
using gcs_system.MAVSDK;
using Newtonsoft.Json;

namespace gcs_system.Services.Helper;

public class MavlinkMapper
{
  public event Action<MAVLink.MAVLinkMessage>? OnNewMavlinkMessage;
  public event Action<string, DroneStt>? OnUpdateDroneState; 
 
  private DroneState _droneState;
  
  private readonly GoogleMapHelper _googleMapHelper = GoogleMapHelper.GetInstace();
  private VincentyCalculator _vincentyCalculator = new();


  public void HandleDroneMessage(DroneState droneInstance, MAVLink.MAVLinkMessage droneMessage)
  {
    OnNewMavlinkMessage?.Invoke(droneMessage);
    
    _droneState = droneInstance;
    switch ((MAVLink.MAVLINK_MSG_ID)droneMessage.msgid)
    {
      case MAVLink.MAVLINK_MSG_ID.HEARTBEAT:
      {
        var heartbeat = (MAVLink.mavlink_heartbeat_t)droneMessage.data;
        if (heartbeat.type != (byte)MAVLink.MAV_TYPE.GCS)
        {
          _droneState.DroneStt.FlightMode = (FlightMode)heartbeat.custom_mode;
        }
          
        break;
      }
      case MAVLink.MAVLINK_MSG_ID.ATTITUDE:
      {
        var attitude = (MAVLink.mavlink_attitude_t)droneMessage.data;
        _droneState.SensorData.roll_ATTITUDE = attitude.roll;
        _droneState.DroneStt.Roll = attitude.roll;
        _droneState.SensorData.pitch_ATTITUDE = attitude.pitch;
        _droneState.DroneStt.Pitch = attitude.pitch;
        _droneState.SensorData.yaw_ATTITUDE = attitude.yaw;
        
        break;
      }
      case MAVLink.MAVLINK_MSG_ID.GLOBAL_POSITION_INT:
      {
        var globalPositionInt = (MAVLink.mavlink_global_position_int_t)droneMessage.data;

        double lat = globalPositionInt.lat * 1.0 / 10000000;
        double lon = globalPositionInt.lon * 1.0 / 10000000;
        double relativeAlt = globalPositionInt.relative_alt * 1.0 / 1000;
        double globalAlt = globalPositionInt.alt * 1.0 / 1000;

        _droneState.DroneStt.Lat = lat;
        _droneState.DroneStt.Lon = lon;
        _droneState.DroneStt.Alt = relativeAlt;
        _droneState.DroneStt.GlobalAlt = globalAlt;

        _droneState.SensorData.vx_GLOBAL_POSITION_INT = globalPositionInt.vx;
        _droneState.SensorData.vy_GLOBAL_POSITION_INT = globalPositionInt.vy;

        _droneState.DroneStt.Speed = (float)Math.Sqrt(globalPositionInt.vx * globalPositionInt.vx +
                                                      globalPositionInt.vy * globalPositionInt.vy +
                                                      globalPositionInt.vz * globalPositionInt.vz) / 100f;

        // 드론이 비행중이지 않을때 드론 고도 변화, 좌표 변화 받지 않기
        if (DateTime.Now.Subtract((DateTime)_droneState.DroneMission.LastAddedTrails).Milliseconds > 500 &&
            _droneState.IsLanded == false)
        {
          if (_droneState.DroneMission.DroneTrails.q.Count != 0)
          {
            double lastLat = _droneState.DroneMission.DroneTrails.q.Last().lat;
            double lastLng = _droneState.DroneMission.DroneTrails.q.Last().lng;

            if (lastLat != lat && lastLng != lon)
            {
              _droneState.DroneMission.CurrentDistance += _vincentyCalculator.DistanceCalculater(
                lastLat, lastLng, lat, lon);
            }
          }

          UpdateDroneTrails(lat, lon, relativeAlt, globalAlt, true);
          
        }
        break;
      }
      case MAVLink.MAVLINK_MSG_ID.SYS_STATUS:
      {
        var sysStatus = (MAVLink.mavlink_sys_status_t)droneMessage.data;
        _droneState.DroneStt.PowerV = sysStatus.voltage_battery * 1 / 1000;
        
        break;
      }
      case MAVLink.MAVLINK_MSG_ID.POWER_STATUS:
      {
        var powerStatus = (MAVLink.mavlink_power_status_t)droneMessage.data;
        _droneState.SensorData.Vservo_POWER_STATUS = powerStatus.Vservo;
        
        break;
      }
      case MAVLink.MAVLINK_MSG_ID.NAV_CONTROLLER_OUTPUT:
      {
        var navControllerOutput = (MAVLink.mavlink_nav_controller_output_t)droneMessage.data;
        _droneState.SensorData.nav_pitch_NAV_CONTROLLER_OUTPUT = navControllerOutput.nav_pitch;
        _droneState.SensorData.nav_bearing_NAV_CONTROLLER_OUTPUT = navControllerOutput.nav_bearing;
        
        break;
      }
      case MAVLink.MAVLINK_MSG_ID.VFR_HUD:
      {
        var vfrHud = (MAVLink.mavlink_vfr_hud_t)droneMessage.data;
        _droneState.DroneStt.Head = vfrHud.heading;
        _droneState.SensorData.airspeed_VFR_HUD = vfrHud.airspeed;
        _droneState.SensorData.groundspeed_VFR_HUD = vfrHud.groundspeed;

        break;
      }
      case MAVLink.MAVLINK_MSG_ID.SERVO_OUTPUT_RAW:
      {
        var servoOutput = (MAVLink.mavlink_servo_output_raw_t)droneMessage.data;
        _droneState.SensorData.servo3_raw_SERVO_OUTPUT_RAW = servoOutput.servo3_raw;
        _droneState.SensorData.servo8_raw_SERVO_OUTPUT_RAW = servoOutput.servo8_raw;

        break;
      }
      case MAVLink.MAVLINK_MSG_ID.RC_CHANNELS:
      {
        var rcChannels = (MAVLink.mavlink_rc_channels_t)droneMessage.data;
        _droneState.SensorData.chancount_RC_CHANNELS = rcChannels.chancount;
        _droneState.SensorData.chan12_raw_RC_CHANNELS = rcChannels.chan12_raw;
        _droneState.SensorData.chan13_raw_RC_CHANNELS = rcChannels.chan13_raw;
        _droneState.SensorData.chan14_raw_RC_CHANNELS = rcChannels.chan14_raw;
        _droneState.SensorData.chan15_raw_RC_CHANNELS = rcChannels.chan15_raw;
        _droneState.SensorData.chan16_raw_RC_CHANNELS = rcChannels.chan16_raw;

        break;
      }
      case MAVLink.MAVLINK_MSG_ID.RAW_IMU:
      {
        var rawImu = (MAVLink.mavlink_raw_imu_t)droneMessage.data;
        _droneState.SensorData.xacc_RAW_IMU = rawImu.xacc;
        _droneState.SensorData.yacc_RAW_IMU = rawImu.yacc;
        _droneState.SensorData.zacc_RAW_IMU = rawImu.zacc;
        _droneState.SensorData.xgyro_RAW_IMU = rawImu.xgyro;
        _droneState.SensorData.ygyro_RAW_IMU = rawImu.ygyro;
        _droneState.SensorData.zgyro_RAW_IMU = rawImu.zgyro;
        _droneState.SensorData.xmag_RAW_IMU = rawImu.xmag;
        _droneState.SensorData.ymag_RAW_IMU = rawImu.ymag;
        _droneState.SensorData.zmag_RAW_IMU = rawImu.zmag;
      
        break;
      }
      case MAVLink.MAVLINK_MSG_ID.SCALED_PRESSURE:
      {
        var scaledPressure = (MAVLink.mavlink_scaled_pressure_t)droneMessage.data;
        _droneState.SensorData.press_abs_SCALED_PRESSURE = scaledPressure.press_abs;
        
        break;
      }
      case MAVLink.MAVLINK_MSG_ID.GPS_RAW_INT:
      {
        var gpsRawInt = (MAVLink.mavlink_gps_raw_int_t)droneMessage.data;
        _droneState.DroneStt.HDOP = gpsRawInt.eph < ushort.MaxValue ? gpsRawInt.eph / 100f : 0;
        _droneState.DroneStt.SatellitesCount = gpsRawInt.satellites_visible;
        
        break;
      }
      case MAVLink.MAVLINK_MSG_ID.LOCAL_POSITION_NED:
      {
        var localPositionNed = (MAVLink.mavlink_local_position_ned_t)droneMessage.data;
        _droneState.SensorData.x_LOCAL_POSITION_NED = localPositionNed.x;
        _droneState.SensorData.vx_LOCAL_POSITION_NED = localPositionNed.vx;
        _droneState.SensorData.vy_LOCAL_POSITION_NED = localPositionNed.vy;

        break;
      }
      case MAVLink.MAVLINK_MSG_ID.VIBRATION:
      {
        var vibration = (MAVLink.mavlink_vibration_t)droneMessage.data;
        _droneState.SensorData.vibration_x_VIBRATION = vibration.vibration_x;
        _droneState.SensorData.vibration_y_VIBRATION = vibration.vibration_y;
        _droneState.SensorData.vibration_z_VIBRATION = vibration.vibration_z;    

        break;
      }
      case MAVLink.MAVLINK_MSG_ID.BATTERY_STATUS:
      {
        var batteryStatus = (MAVLink.mavlink_battery_status_t)droneMessage.data;
        _droneState.DroneStt.BatteryStt = batteryStatus.battery_remaining;
        
        break;
      }
      case MAVLink.MAVLINK_MSG_ID.MISSION_CURRENT:
      {
        Console.WriteLine("MISSION_CURRENT");
        break;
      }
    }
  }

  // private void UpdateDroneState(DroneStt rawDroneStateData)
  // {
  //   var currentDroneStateData = MergeDroneRawState(_droneStt, rawDroneStateData);
  //   
  //   _droneStt = currentDroneStateData;
  //   
  //   OnUpdateDroneState.Invoke(_droneId, ToDummyDroneStt());
  // }
  //
  // private DroneStt MergeDroneRawState(DroneStt? nBefore, DroneStt after)
  // {
  //   if (nBefore == null) return after;
  //   var before = nBefore.Value;
  //
  //   return new DroneStt
  //   {
  //     PowerV = after.PowerV != 0 ? after.PowerV : before.PowerV,
  //     BatteryStt = after.BatteryStt != 0 ? after.BatteryStt : before.BatteryStt,
  //     GpsStt = after.GpsStt != "" ? after.GpsStt : before.GpsStt,
  //     TempC = after.TempC != 0 ? after.TempC : before.TempC,
  //     Lat = after.Lat != 0 ? after.Lat : before.Lat,
  //     Lon = after.Lon != 0 ? after.Lon : before.Lon,
  //     Alt = after.Alt != 0 ? after.Alt : before.Alt,
  //     GlobalAlt = after.GlobalAlt != 0 ? after.GlobalAlt : before.GlobalAlt,
  //     Roll = after.Roll != 0 ? after.Roll : before.Roll,
  //     Pitch = after.Pitch != 0 ? after.Pitch : before.Pitch,
  //     Head = after.Head != 0 ? after.Head : before.Head,
  //     Speed = after.Speed != 0 ? after.Speed : before.Speed,
  //     HoverStt = after.HoverStt != "" ? after.HoverStt : before.HoverStt,
  //     HDOP = after.HDOP != 0 ? after.HDOP : before.HDOP,
  //     SatellitesCount = after.SatellitesCount != 0 ? after.SatellitesCount : before.SatellitesCount,
  //     FlightMode = after.FlightMode != FlightMode.LAND ? after.FlightMode : before.FlightMode,
  //   };
  // }
  //
  //
  // public DroneStateStruct ToDummyDroneStt()
  // {
  //   return new DroneStateStruct()
  //   {
  //     
  //   };
  // }
  
  public void UpdateDroneTrails(double lat, double lon, double relativeAlt, double globalAlt, bool updatedLocation = false)
  {
    _droneState.DroneMission.DroneTrails.Enqueue(new DroneLocation
    {
      lat = lat,
      lng = lon,
      global_frame_alt = globalAlt,
      terrain_alt = globalAlt - relativeAlt,
    });
        
    _droneState.DroneMission.LastAddedTrails = DateTime.Now;
  }
  
  // public string ObjectToJson() 
  // {
  //   string droneMessage = JsonConvert.SerializeObject(_droneState);
  //   return droneMessage;
  // }
}