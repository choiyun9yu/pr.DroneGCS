using kisa_gcs_system.Interfaces;
using kisa_gcs_system.Models;
using kisa_gcs_system.Models.Helper;
using MAVSDK;

namespace kisa_gcs_system.Services.Helper;

public class MavlinkMapper
{
  // constructor가 새로운 연결일 때 동작하면 그때 마다 멤버 초기화 가능? / 지역변수는 직접 초기화 해야한다 
  private Drone _drone = new();
  private readonly GoogleMapHelper _googleMapHelper = GoogleMapHelper.GetInstace();
  private DateTime _lastAddedTrails;
  private VincentyCalculator _vincentyCalculator = new();

  
  public async void GcsMapping(object data)
  {
    // if (data is MAVLink.mavlink_mission_item_reached_t missionreached)
    // {
    //   Console.WriteLine($"도달 여부 : {missionreached}");
    // }
    if (data is MAVLink.mavlink_heartbeat_t heartbeat)
    {
      // Console.WriteLine("base_mode: "+$"{(uint)heartbeat.base_mode}");
      // Console.WriteLine("custom_mode: "+$"{(CustomMode)heartbeat.custom_mode}");
      _drone.DroneStt.FlightMode = (CustomMode)heartbeat.custom_mode;
      
      // if (heartbeat.system_status == 3) // standby
      // {
      //   _drone.DroneMission.IsLanded = true;
      // }
      // if (heartbeat.system_status == 4) // active
      // {
      //   _drone.DroneMission.IsLanded = false;
      // }
    }
    if (data is MAVLink.mavlink_attitude_t attitude)
    {
      // Console.WriteLine(attitude);
    }
    if (data is MAVLink.mavlink_global_position_int_t globalPositionInt)
    {
      // Console.WriteLine(globalPositionInt);
      double lat = globalPositionInt.lat * 1.0 / 10000000;
      double lon = globalPositionInt.lon * 1.0 / 10000000;
      double relative_alt = globalPositionInt.relative_alt * 1.0 / 1000;
      double global_alt = globalPositionInt.alt * 1.0 / 1000;
      
      _drone.DroneStt.Lat = lat;
      _drone.DroneStt.Lon = lon;
      _drone.DroneStt.Alt = relative_alt;
      _drone.DroneStt.GlobalAlt = global_alt;
      _drone.DroneStt.Speed = (float)Math.Sqrt(globalPositionInt.vx * globalPositionInt.vx +
                                                      globalPositionInt.vy * globalPositionInt.vy +
                                                      globalPositionInt.vz * globalPositionInt.vz) / 100f;
      if (DateTime.Now.Subtract(_lastAddedTrails).Milliseconds > 500)
      {
        if (_drone.DroneMission.DroneTrails.q.Count != 0)
        {
          double lastLat = _drone.DroneMission.DroneTrails.q.Last().lat;
          double lastLng = _drone.DroneMission.DroneTrails.q.Last().lng;
          if (lastLat != lat && lastLng != lon)
          {
            _drone.DroneMission.CurrentDistance += _vincentyCalculator.DistanceCalculater(
              lastLat, lastLng, lat, lon);   
          }
        }
        UpdateDroneTrails(lat, lon, relative_alt, global_alt, true);
      }
    }
    if (data is MAVLink.mavlink_sys_status_t sysStatus)
    {
       // gcs
       // Console.WriteLine($"current_battery:{sysStatus.voltage_battery}");
       _drone.DroneStt.PowerV = sysStatus.voltage_battery * 1.0 / 1000;
    }
    // if (data is MAVLink.mavlink_power_status_t powerStatus)
    // {
    //   // gcs
    //   Console.WriteLine($"Vcc:{powerStatus.Vcc}");
    //   drone.DroneStt.PowerV = powerStatus.Vcc;
    // }
    // if (data is MAVLink.mavlink_meminfo_t meminfo)
    // {
    //   Console.WriteLine(meminfo);
    // }
    // if (data is MAVLink.mavlink_nav_controller_output_t navControllerOutput)
    // {
    //   Console.WriteLine(navControllerOutput);
    // }
    // if (data is MAVLink.mavlink_mission_current_t missionCurrent)
    // {
    //   Console.WriteLine($"seq: {missionCurrent.seq}, total: {missionCurrent.total}, mission_state: {missionCurrent.mission_state}, mission_mode: {missionCurrent.mission_mode}" );
    // }
    if (data is MAVLink.mavlink_vfr_hud_t vfrHud)
    {
      // Console.WriteLine(vfrHud.heading);
      _drone.DroneStt.Head = vfrHud.heading;
    }
    // if (data is MAVLink.mavlink_servo_output_raw_t servoOutput)
    // {
    //   Console.WriteLine(sensorData);
    // }
    // if (data is MAVLink.mavlink_rc_channels_t rcChannels)
    // {
    //   Console.WriteLine(rcChannels);
    // }
    // if (data is MAVLink.mavlink_raw_imu_t rawImu)
    // {
    //   Console.WriteLine(rawImu);
    // }
    // if (data is MAVLink.mavlink_scaled_imu2_t scaledImu2)
    // {
    //   Console.WriteLine(scaledImu2);
    // }
    // if (data is MAVLink.mavlink_scaled_imu3_t scaledImu3)
    // {
    //   Console.WriteLine(scaledImu3);
    // }
    // if (data is MAVLink.mavlink_scaled_pressure_t scaledPressure)
    // {
    //   Console.WriteLine(scaledPressure);
    // }
    // if (data is MAVLink.mavlink_scaled_pressure2_t scaledPressure2)
    // {
    //   Console.WriteLine(scaledPressure2);
    // }
    if (data is MAVLink.mavlink_gps_raw_int_t gpsRawInt)
    {
      // Console.WriteLine($"lat:{gpsRawInt.lat}, lon:{gpsRawInt.lon}");
      _drone.DroneStt.HDOP = gpsRawInt.eph < ushort.MaxValue ? gpsRawInt.eph / 100f : 0;
      _drone.DroneStt.SatellitesCount = gpsRawInt.satellites_visible;
    }
    // if (data is MAVLink.mavlink_system_time_t systemTime)
    // {
    //   Console.WriteLine(systemTime);
    // }
    // if (data is MAVLink.mavlink_ahrs_t ahrs)
    // {
    //   Console.WriteLine(ahrs);
    // }
    // if (data is MAVLink.mavlink_simstate_t simstate)
    // {
    //   Console.WriteLine(simstate);
    // }
    // if (data is MAVLink.mavlink_ahrs2_t ahrs2)
    // {
    //   Console.WriteLine(ahrs2);
    // }
    // if (data is MAVLink.mavlink_wind_t wind)
    // {
    //   Console.WriteLine(wind);
    // }
    // if (data is MAVLink.mavlink_terrain_report_t terrainReport)
    // {
    //   Console.WriteLine($"lat:{terrainReport.lat}, lon:{terrainReport.lon}");
    // }
    // if (data is MAVLink.mavlink_ekf_status_report_t ekfStatusReport)
    // {
    //   // gcs
    //   Console.WriteLine($"alt:{ekfStatusReport.terrain_alt_variance}");
    // }
    // if (data is MAVLink.mavlink_local_position_ned_t localPositionNed)
    // {
    //   Console.WriteLine(localPositionNed);
    // }
    // if (data is MAVLink.mavlink_vibration_t vibration)
    // {
    //   Console.WriteLine(vibration);
    // }
    if (data is MAVLink.mavlink_battery_status_t batteryStatus)
    {
      // Console.WriteLine(batteryStatus);
      _drone.DroneStt.BatteryStt = batteryStatus.battery_remaining;
    }
    // if (data is MAVLink.mavlink_esc_telemetry_1_to_4_t escTelemetry1To4T)
    // {
    //   Console.WriteLine(escTelemetry1To4T);
    // }
    // if (data is MAVLink.mavlink_esc_telemetry_5_to_8_t escTelemetry5To8T)
    // {
    //   Console.WriteLine(escTelemetry5To8T);
    // }
    // if (data is MAVLink.mavlink_sensor_offsets_t sensorOffsets)
    // {
    //   Console.WriteLine(sensorOffsets);
    // }
    if (data is MAVLink.mavlink_highres_imu_t highresImu)
    {
      _drone.DroneStt.TempC = highresImu.temperature;
    }
  }

  public void PredictionMapping(object data)
  {
    // if (data is MAVLink.mavlink_heartbeat_t heartbeat)
    // {
    //   Console.WriteLine(heartbeat);
    // }
    if (data is MAVLink.mavlink_attitude_t attitude)
    {
      // predicton
      // Console.WriteLine($"roll:{attitude.roll}, pitch:{attitude.pitch}, yaw:{attitude.yaw}");
      _drone.SensorData.roll_ATTITUDE = attitude.roll;
      _drone.SensorData.pitch_ATTITUDE = attitude.pitch;
      _drone.SensorData.yaw_ATTITUDE = attitude.yaw;
    }
    if (data is MAVLink.mavlink_global_position_int_t globalPositionInt)
    {
      // predicton
      // Console.WriteLine($"vx:{globalPositionInt.vx}, vy:{globalPositionInt.vy}");
      _drone.SensorData.vx_GLOBAL_POSITION_INT = globalPositionInt.vx;
      _drone.SensorData.vy_GLOBAL_POSITION_INT = globalPositionInt.vy;
    }
    // if (data is MAVLink.mavlink_sys_status_t sysStatus)
    // {
    //   Console.WriteLine(sysStatus);
    // }
    if (data is MAVLink.mavlink_power_status_t powerStatus)
    {
      // predicton
      // Console.WriteLine($"Vservo:{powerStatus.Vservo}");
      _drone.SensorData.Vservo_POWER_STATUS = powerStatus.Vservo;
    }
    // if (data is MAVLink.mavlink_meminfo_t meminfo)
    // {
    //   Console.WriteLine(meminfo);
    // }
    if (data is MAVLink.mavlink_nav_controller_output_t navControllerOutput)
    {
      // predicton
      // Console.WriteLine($"nav_pitch:{navControllerOutput.nav_pitch}, nav_bearing:{navControllerOutput.nav_bearing}");
      _drone.SensorData.nav_pitch_NAV_CONTROLLER_OUTPUT = navControllerOutput.nav_pitch;
      _drone.SensorData.nav_bearing_NAV_CONTROLLER_OUTPUT = navControllerOutput.nav_bearing;
    }
    // if (data is MAVLink.mavlink_mission_current_t missionCurrent)
    // {
    //   Console.WriteLine(missionCurrent);
    // }
    if (data is MAVLink.mavlink_vfr_hud_t vfrHud)
    {
      // predicton
      // Console.WriteLine($"airspeed:{vfrHud.airspeed}, groundspeed:{vfrHud.groundspeed}");
      _drone.SensorData.airspeed_VFR_HUD = vfrHud.airspeed;
      _drone.SensorData.groundspeed_VFR_HUD = vfrHud.groundspeed;
    }
    if (data is MAVLink.mavlink_servo_output_raw_t servoOutput)
    {
      // predicton
      // Console.WriteLine($"servo3_raw:{servoOutput.servo3_raw}, servo8_raw:{servoOutput.servo8_raw}");
      _drone.SensorData.servo3_raw_SERVO_OUTPUT_RAW = servoOutput.servo3_raw;
      _drone.SensorData.servo8_raw_SERVO_OUTPUT_RAW = servoOutput.servo8_raw;
    }
    if (data is MAVLink.mavlink_rc_channels_t rcChannels)
    {
      // predicton
      // Console.WriteLine($"chancount:{rcChannels.chancount}, chan12_raw:{rcChannels.chan12_raw}, chan13_raw:{rcChannels.chan13_raw}, chan14_raw:{rcChannels.chan14_raw}, chan15_raw:{rcChannels.chan15_raw}, chan16_raw:{rcChannels.chan16_raw}");
      _drone.SensorData.chancount_RC_CHANNELS = rcChannels.chancount;
      _drone.SensorData.chan12_raw_RC_CHANNELS = rcChannels.chan12_raw;
      _drone.SensorData.chan13_raw_RC_CHANNELS = rcChannels.chan13_raw;
      _drone.SensorData.chan14_raw_RC_CHANNELS = rcChannels.chan14_raw;
      _drone.SensorData.chan15_raw_RC_CHANNELS = rcChannels.chan15_raw;
      _drone.SensorData.chan16_raw_RC_CHANNELS = rcChannels.chan16_raw;
    }
    if (data is MAVLink.mavlink_raw_imu_t rawImu)
    {
      // predicton
      // Console.WriteLine($"xacc:{rawImu.xacc}, yacc:{rawImu.yacc}, zacc:{rawImu.zacc}");
      _drone.SensorData.xacc_RAW_IMU = rawImu.xacc;
      _drone.SensorData.yacc_RAW_IMU = rawImu.yacc;
      _drone.SensorData.zacc_RAW_IMU = rawImu.zacc;
      // Console.WriteLine($"xgyro:{rawImu.xgyro}, ygyro:{rawImu.ygyro}, zgyro:{rawImu.zgyro}");
      _drone.SensorData.xgyro_RAW_IMU = rawImu.xgyro;
      _drone.SensorData.ygyro_RAW_IMU = rawImu.ygyro;
      _drone.SensorData.zgyro_RAW_IMU = rawImu.zgyro;
      // Console.WriteLine($"xmag:{rawImu.xmag}, ymag:{rawImu.ymag}, zmag:{rawImu.zmag}");
      _drone.SensorData.xmag_RAW_IMU = rawImu.xmag;
      _drone.SensorData.ymag_RAW_IMU = rawImu.ymag;
      _drone.SensorData.zmag_RAW_IMU = rawImu.zmag;
    }
    // if (data is MAVLink.mavlink_scaled_imu2_t scaledImu2)
    // {
    //   Console.WriteLine(scaledImu2);
    // }
    // if (data is MAVLink.mavlink_scaled_imu3_t scaledImu3)
    // {
    //   Console.WriteLine(scaledImu3);
    // }
    if (data is MAVLink.mavlink_scaled_pressure_t scaledPressure)
    {
      // predicton
      // Console.WriteLine($"press_abs:{scaledPressure.press_abs}");
      _drone.SensorData.press_abs_SCALED_PRESSURE = scaledPressure.press_abs;
    }
    // if (data is MAVLink.mavlink_scaled_pressure2_t scaledPressure2)
    // {
    //   Console.WriteLine(scaledPressure2);
    // }
    // if (data is MAVLink.mavlink_gps_raw_int_t gpsRawInt)
    // {
    //   Console.WriteLine(gpsRawInt);
    // }
    // if (data is MAVLink.mavlink_system_time_t systemTime)
    // {
    //   Console.WriteLine(systemTime);
    // }
    // if (data is MAVLink.mavlink_ahrs_t ahrs)
    // {
    //   Console.WriteLine(ahrs);
    // }
    // if (data is MAVLink.mavlink_simstate_t simstate)
    // {
    //   Console.WriteLine(simstate);
    // }
    // if (data is MAVLink.mavlink_ahrs2_t ahrs2)
    // {
    //   Console.WriteLine(ahrs2);
    // }
    // if (data is MAVLink.mavlink_wind_t wind)
    // {
    //   Console.WriteLine(wind);
    // }
    // if (data is MAVLink.mavlink_terrain_report_t terrainReport)
    // {
    //   Console.WriteLine(terrainReport);
    // }
    // if (data is MAVLink.mavlink_ekf_status_report_t ekfStatusReport)
    // {
    //   Console.WriteLine(ekfStatusReport);
    // }
    if (data is MAVLink.mavlink_local_position_ned_t localPositionNed)
    {
      // predicton
      // Console.WriteLine($"x:{localPositionNed.x}, vx:{localPositionNed.vx}, vy:{localPositionNed.vy}");
      _drone.SensorData.x_LOCAL_POSITION_NED = localPositionNed.x;
      _drone.SensorData.vx_LOCAL_POSITION_NED = localPositionNed.vx;
      _drone.SensorData.vy_LOCAL_POSITION_NED = localPositionNed.vy;
    }
    if (data is MAVLink.mavlink_vibration_t vibration)
    {
      // predicton
      // Console.WriteLine($"vibration_x:{vibration.vibration_x}, vibration_y:{vibration.vibration_y}, vibration_z:{vibration.vibration_z}");
      _drone.SensorData.vibration_x_VIBRATION = vibration.vibration_x;
      _drone.SensorData.vibration_y_VIBRATION = vibration.vibration_y;
      _drone.SensorData.vibration_z_VIBRATION = vibration.vibration_z;
    }
    // if (data is MAVLink.mavlink_battery_status_t batteryStatus)
    // {
    //   Console.WriteLine(batteryStatus);
    // }
    // if (data is MAVLink.mavlink_esc_telemetry_1_to_4_t escTelemetry1To4T)
    // {
    //   Console.WriteLine(escTelemetry1To4T);
    // }
    // if (data is MAVLink.mavlink_esc_telemetry_5_to_8_t escTelemetry5To8T)
    // {
    //   Console.WriteLine(escTelemetry5To8T);
    // }
    if (data is MAVLink.mavlink_sensor_offsets_t sensorOffsets)
    {
      // predicton
      // Console.WriteLine($"accel_cal_x:{sensorOffsets.accel_cal_x}, accel_cal_y:{sensorOffsets.accel_cal_y}, accel_cal_z:{sensorOffsets.accel_cal_z}");
      _drone.SensorData.accel_cal_x_SENSOR_OFFSETS = sensorOffsets.accel_cal_x;
      _drone.SensorData.accel_cal_y_SENSOR_OFFSETS = sensorOffsets.accel_cal_y;
      _drone.SensorData.accel_cal_z_SENSOR_OFFSETS = sensorOffsets.accel_cal_z;
      // Console.WriteLine($"mag_ofs_x:{sensorOffsets.mag_ofs_x}, mag_ofs_y:{sensorOffsets.mag_ofs_y}");
      _drone.SensorData.mag_ofs_x_SENSOR_OFFSETS = sensorOffsets.mag_ofs_x;
      _drone.SensorData.mag_ofs_y_SENSOR_OFFSETS = sensorOffsets.mag_ofs_y;
    }
  }
  

  public void SetDroneId(string droneId)
  {
    // 여기서 드론 아이디에 다른 로직이 필요할 듯 
    _drone.DroneId = droneId;
  }

  public void UpdateDroneLogger(string text)
  {
    _drone.DroneLogger.Add(
      new MavlinkLog()
      {
        logtime = DateTime.Now,
        message = text
      }
    );
  }

  public void HandleMissionStart()
  {
    setStartTime();
    _drone.DroneMission.CompleteTime = null;
    _drone.DroneMission.DroneTrails = new FixedSizedQueue<DroneLocation>(600); // 0.5 초에 1개 씩이니까 약 3분 정도 경로 저장 
    // setStartPoint();
    // _droneMessage.DroneMission.TotalDistance = _haversineCalculator.Haversine(_droneMessage.DroneMission.StartPoint.lat, _droneMessage.DroneMission.StartPoint.lng,
    //                                                                         _droneMessage.DroneMission.TargetPoint.lat, _droneMessage.DroneMission.TargetPoint.lng);
  }
  
  public void HandleMissionComplete()
  {
    setCompleteTime();
    // _droneMessage.DroneMission.StartPoint = new();
    // _droneMessage.DroneMission.TargetPoint = new();
  }
  
  public void UpdateDroneTrails(double lat, double lon, double relative_alt, double global_alt, bool updatedLocation = false)
  {
    _drone.DroneMission.DroneTrails.Enqueue(new DroneLocation
    {
      lat = lat,
      lng = lon,
      global_frame_alt = global_alt,
      terrain_alt = global_alt - relative_alt,
    });

    // _drone.DroneMission.RemainDistance = _vincentyCalculator.DistanceCalculater(lat, lon, _drone.DroneMission.TargetPoint.lat,
    //   _drone.DroneMission.TargetPoint.lng);
    
    _lastAddedTrails = DateTime.Now;
  }

  public string ObjectToJson() 
  {
    string droneMessage = JsonConvert.SerializeObject(_drone);
    return droneMessage;
  }

  public CustomMode? getFlightMode()
  {
    return _drone.DroneStt.FlightMode;
  }

  public double getRelativeAlt()
  {
    return (double)_drone.DroneStt.Alt;
  }

  public double getStartPointLat()
  {
    return _drone.DroneMission.StartPoint.lat;
  }
  
  public double getStartPointLng()
  {
    return _drone.DroneMission.StartPoint.lng;
  }

  public List<DroneLocation> getTransitPoint()
  {
    return _drone.DroneMission.TransitPoint;
  }

  public double getCurrentLat()
  {
    return _drone.DroneStt.Lat;
  }
  
  public double getCurrentLon()
  {
    return _drone.DroneStt.Lon;
  }

  public double getTargetPointLat()
  {
    return _drone.DroneMission.TargetPoint.lat;
  }
  
  public double getTargetPointLng()
  {
    return _drone.DroneMission.TargetPoint.lng;
  }

  public int getMissionAlt()
  {
    return _drone.DroneMission.MissionAlt;
  }

  public string getControlStt()
  {
    return _drone.ControlStt;
  }

  public async Task setMissionAlt(int missionAlt)
  {
    _drone.DroneMission.MissionAlt = missionAlt;
    // Console.WriteLine($"set mission alt: {missionAlt}");
  }

  public async Task setStartPoint()
  {
    _drone.DroneMission.StartPoint = new DroneLocation{
      lat= _drone.DroneStt.Lat,
      lng= _drone.DroneStt.Lon
    };
  }

  public async Task setStartPoint(double lat, double lng)
  {
    _drone.DroneMission.StartPoint = new DroneLocation{
      lat= lat,
      lng= lng
    };
  }

  public async Task setTransitPoint(List<DroneLocation> transitPoinst)
  {
    _drone.DroneMission.TransitPoint = transitPoinst;
  }

  public async Task setTargetPoint(double lat, double lng)
  {
    // setStartPoint();
    _drone.DroneMission.TargetPoint = new DroneLocation
    {
      lat = lat,
      lng = lng
    };  
    
    _drone.DroneMission.TotalDistance = _vincentyCalculator.DistanceCalculater(
      _drone.DroneMission.StartPoint.lat, _drone.DroneMission.StartPoint.lng, lat, lng);
    
    // List<float> path_terrain_alt = await _googleMapHelper.FetchElevations(
    //   _droneMessage.DroneMission.StartPoint.lat, _droneMessage.DroneMission.StartPoint.lng, 
    //   lat, lng, 10);
    
    // sample 300, api 요청이 많으면 Google Maps API 요금이 올라감 (매달 200 달러 까지만 무료), 2018년 6월 11일 부터 API 키를 발급 받으려면 결제 계정이 필수적으로 필요
    // List<GoogleWaypoint> path_terrain_alt = await _googleMapHelper.FetchElevations(
    // _droneMessage.DroneMission.StartPoint.lat, _droneMessage.DroneMission.StartPoint.lng, 
    // lat, lng, 10);
    // Console.Write("path: ");
    // path_terrain_alt.ForEach(path_num => Console.Write($"{path_num.Elevation}, "));
    
    // double path_max_alt = path_terrain_alt.Max();
    // double path_terrain_min_alt = path_terrain_alt.Min();
    // path_terrain_alt.ForEach(path_num => Console.Write($"[{path_num.Location.Lat}, {path_num.Location.Lng}, {path_num.Elevation}], "));
  }

  public async Task setStartTime()
  {
    _drone.DroneMission.StartTime = DateTime.Now;
  }

  public async Task setCompleteTime()
  {
    _drone.DroneMission.CompleteTime = DateTime.Now;
  }

  public async Task setTotalDistance(double totalDistance)
  {
    _drone.DroneMission.TotalDistance = totalDistance;
  }

  public async Task setCurrentDistance(double currentDistance)
  {
    _drone.DroneMission.CurrentDistance = currentDistance;
  }

  public async Task setControlStt(string controlStt)
  {
    _drone.ControlStt = controlStt;
  }

  public async Task setPathIndex(int i)
  {
    _drone.DroneMission.PathIndex = i;
  }
}