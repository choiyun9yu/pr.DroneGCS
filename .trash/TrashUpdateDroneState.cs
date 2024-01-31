namespace DefaultNamespace;

public class TrashUpdateDroneState
{
      public async void UpdateDroneState(object data)
  {
    if (data is MAVLink.mavlink_heartbeat_t heartbeat)
    {
      _droneState.DroneStt.FlightMode = (CustomMode)heartbeat.custom_mode;
    }
    if (data is MAVLink.mavlink_attitude_t attitude)
    {
      _droneState.SensorData.roll_ATTITUDE = attitude.roll;
      _droneState.SensorData.pitch_ATTITUDE = attitude.pitch;
      _droneState.SensorData.yaw_ATTITUDE = attitude.yaw;
    }
    if (data is MAVLink.mavlink_global_position_int_t globalPositionInt)
    {
      _droneState.SensorData.vx_GLOBAL_POSITION_INT = globalPositionInt.vx;
      _droneState.SensorData.vy_GLOBAL_POSITION_INT = globalPositionInt.vy;
      
      double lat = globalPositionInt.lat * 1.0 / 10000000;
      double lon = globalPositionInt.lon * 1.0 / 10000000;
      double relative_alt = globalPositionInt.relative_alt * 1.0 / 1000;
      double global_alt = globalPositionInt.alt * 1.0 / 1000;
      
      _droneState.DroneStt.Lat = lat;
      _droneState.DroneStt.Lon = lon;
      _droneState.DroneStt.Alt = relative_alt;
      _droneState.DroneStt.GlobalAlt = global_alt;
      _droneState.DroneStt.Speed = (float)Math.Sqrt(globalPositionInt.vx * globalPositionInt.vx +
                                                      globalPositionInt.vy * globalPositionInt.vy +
                                                      globalPositionInt.vz * globalPositionInt.vz) / 100f;
      if (DateTime.Now.Subtract(_lastAddedTrails).Milliseconds > 500)
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
        // 미션이 시작했을 때만 update하는 것이 좋을 듯 
        UpdateDroneTrails(lat, lon, relative_alt, global_alt, true);
      }
    }
    if (data is MAVLink.mavlink_sys_status_t sysStatus)
    {
       _droneState.DroneStt.PowerV = sysStatus.voltage_battery * 1.0 / 1000;
    }
    if (data is MAVLink.mavlink_power_status_t powerStatus)
    {
      _droneState.SensorData.Vservo_POWER_STATUS = powerStatus.Vservo;
    }

    if (data is MAVLink.mavlink_nav_controller_output_t navControllerOutput)
    {
      _droneState.SensorData.nav_pitch_NAV_CONTROLLER_OUTPUT = navControllerOutput.nav_pitch;
      _droneState.SensorData.nav_bearing_NAV_CONTROLLER_OUTPUT = navControllerOutput.nav_bearing;
    }

    if (data is MAVLink.mavlink_vfr_hud_t vfrHud)
    {
      _droneState.DroneStt.Head = vfrHud.heading;
      _droneState.SensorData.airspeed_VFR_HUD = vfrHud.airspeed;
      _droneState.SensorData.groundspeed_VFR_HUD = vfrHud.groundspeed;
    }
    if (data is MAVLink.mavlink_servo_output_raw_t servoOutput)
    {
      _droneState.SensorData.servo3_raw_SERVO_OUTPUT_RAW = servoOutput.servo3_raw;
      _droneState.SensorData.servo8_raw_SERVO_OUTPUT_RAW = servoOutput.servo8_raw;
    }
    if (data is MAVLink.mavlink_rc_channels_t rcChannels)
    {
      _droneState.SensorData.chancount_RC_CHANNELS = rcChannels.chancount;
      _droneState.SensorData.chan12_raw_RC_CHANNELS = rcChannels.chan12_raw;
      _droneState.SensorData.chan13_raw_RC_CHANNELS = rcChannels.chan13_raw;
      _droneState.SensorData.chan14_raw_RC_CHANNELS = rcChannels.chan14_raw;
      _droneState.SensorData.chan15_raw_RC_CHANNELS = rcChannels.chan15_raw;
      _droneState.SensorData.chan16_raw_RC_CHANNELS = rcChannels.chan16_raw;
    }
    if (data is MAVLink.mavlink_raw_imu_t rawImu)
    {
      _droneState.SensorData.xacc_RAW_IMU = rawImu.xacc;
      _droneState.SensorData.yacc_RAW_IMU = rawImu.yacc;
      _droneState.SensorData.zacc_RAW_IMU = rawImu.zacc;
      _droneState.SensorData.xgyro_RAW_IMU = rawImu.xgyro;
      _droneState.SensorData.ygyro_RAW_IMU = rawImu.ygyro;
      _droneState.SensorData.zgyro_RAW_IMU = rawImu.zgyro;
      _droneState.SensorData.xmag_RAW_IMU = rawImu.xmag;
      _droneState.SensorData.ymag_RAW_IMU = rawImu.ymag;
      _droneState.SensorData.zmag_RAW_IMU = rawImu.zmag;
    }

    if (data is MAVLink.mavlink_scaled_pressure_t scaledPressure)
    {
      _droneState.SensorData.press_abs_SCALED_PRESSURE = scaledPressure.press_abs;
    }

    if (data is MAVLink.mavlink_gps_raw_int_t gpsRawInt)
    {
      _droneState.DroneStt.HDOP = gpsRawInt.eph < ushort.MaxValue ? gpsRawInt.eph / 100f : 0;
      _droneState.DroneStt.SatellitesCount = gpsRawInt.satellites_visible;
    }

    if (data is MAVLink.mavlink_local_position_ned_t localPositionNed)
    {
      _droneState.SensorData.x_LOCAL_POSITION_NED = localPositionNed.x;
      _droneState.SensorData.vx_LOCAL_POSITION_NED = localPositionNed.vx;
      _droneState.SensorData.vy_LOCAL_POSITION_NED = localPositionNed.vy;
    }
    if (data is MAVLink.mavlink_vibration_t vibration)
    {
      _droneState.SensorData.vibration_x_VIBRATION = vibration.vibration_x;
      _droneState.SensorData.vibration_y_VIBRATION = vibration.vibration_y;
      _droneState.SensorData.vibration_z_VIBRATION = vibration.vibration_z;    }
    if (data is MAVLink.mavlink_battery_status_t batteryStatus)
    {
      _droneState.DroneStt.BatteryStt = batteryStatus.battery_remaining;
    }
    // 미수신 로그
    if (data is MAVLink.mavlink_sensor_offsets_t sensorOffsets)
    {
      _droneState.SensorData.accel_cal_x_SENSOR_OFFSETS = sensorOffsets.accel_cal_x;
      _droneState.SensorData.accel_cal_y_SENSOR_OFFSETS = sensorOffsets.accel_cal_y;
      _droneState.SensorData.accel_cal_z_SENSOR_OFFSETS = sensorOffsets.accel_cal_z;
      _droneState.SensorData.mag_ofs_x_SENSOR_OFFSETS = sensorOffsets.mag_ofs_x;
      _droneState.SensorData.mag_ofs_y_SENSOR_OFFSETS = sensorOffsets.mag_ofs_y;
    }
    if (data is MAVLink.mavlink_highres_imu_t highresImu)
    {
      _droneState.DroneStt.TempC = highresImu.temperature;
    }
    if (data is MAVLink.mavlink_mission_item_reached_t missionreached)
    {
      Console.WriteLine($"도달 여부 : {missionreached}");
    }
    // 미사용 로그 
    // if (data is MAVLink.mavlink_meminfo_t meminfo){}
    // if (data is MAVLink.mavlink_mission_current_t missionCurrent){}
    // if (data is MAVLink.mavlink_scaled_imu2_t scaledImu2){}
    // if (data is MAVLink.mavlink_scaled_imu3_t scaledImu3){}
    // if (data is MAVLink.mavlink_scaled_pressure2_t scaledPressure2){}
    // if (data is MAVLink.mavlink_system_time_t systemTime){}
    // if (data is MAVLink.mavlink_ahrs_t ahrs){}
    // if (data is MAVLink.mavlink_simstate_t simstate){}
    // if (data is MAVLink.mavlink_ahrs2_t ahrs2){}
    // if (data is MAVLink.mavlink_wind_t wind){}
    // if (data is MAVLink.mavlink_terrain_report_t terrainReport){}
    // if (data is MAVLink.mavlink_ekf_status_report_t ekfStatusReport){}
    // if (data is MAVLink.mavlink_esc_telemetry_1_to_4_t escTelemetry1To4T){}
    // if (data is MAVLink.mavlink_esc_telemetry_5_to_8_t escTelemetry5To8T){}
    
  }
}