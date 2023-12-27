using MAVSDK;

using kisa_gcs_system.Models;

namespace kisa_gcs_system.Services;

public class MavlinkMapper
{
  // private SensorData sensorData = new();
  private Drone drone = new();

  public void gcsMapping(object data)
  {
    // if (data is MAVLink.mavlink_heartbeat_t heartbeat)
    // {
    //   Console.WriteLine(heartbeat);
    // }
    // if (data is MAVLink.mavlink_attitude_t attitude)
    // {
    //   Console.WriteLine(attitude);
    // }
    if (data is MAVLink.mavlink_global_position_int_t globalPositionInt)
    {
      // Console.WriteLine(globalPositionInt);
      drone.DroneStt.Alt = globalPositionInt.relative_alt * 1.0 / 1000;
    }
    if (data is MAVLink.mavlink_sys_status_t sysStatus)
    {
       // gcs
       // Console.WriteLine($"current_battery:{sysStatus.voltage_battery}");
       drone.DroneStt.PowerV = sysStatus.voltage_battery * 1.0 / 1000;
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
    //   Console.WriteLine(missionCurrent);
    // }
    // if (data is MAVLink.mavlink_vfr_hud_t vfrHud)
    // {
    //   Console.WriteLine(vfrHud);
    // }
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
      // gcs
      // Console.WriteLine($"lat:{gpsRawInt.lat}, lon:{gpsRawInt.lon}");
      drone.DroneStt.Lat = gpsRawInt.lat * 1.0 / 10000000;
      drone.DroneStt.Lon = gpsRawInt.lon * 1.0 / 10000000;
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
    // if (data is MAVLink.mavlink_sensor_offsets_t sensorOffsets)
    // {
    //   Console.WriteLine(sensorOffsets);
    // }
    if (data is MAVLink.mavlink_highres_imu_t highresImu)
    {
      drone.DroneStt.TempC = highresImu.temperature;
    }
  }

  public void predictionMapping(object data)
  {
    // if (data is MAVLink.mavlink_heartbeat_t heartbeat)
    // {
    //   Console.WriteLine(heartbeat);
    // }
    if (data is MAVLink.mavlink_attitude_t attitude)
    {
      // predicton
      // Console.WriteLine($"roll:{attitude.roll}, pitch:{attitude.pitch}, yaw:{attitude.yaw}");
      drone.SensorData.roll_ATTITUDE = attitude.roll;
      drone.SensorData.pitch_ATTITUDE = attitude.pitch;
      drone.SensorData.yaw_ATTITUDE = attitude.yaw;
    }
    if (data is MAVLink.mavlink_global_position_int_t globalPositionInt)
    {
      // predicton
      // Console.WriteLine($"vx:{globalPositionInt.vx}, vy:{globalPositionInt.vy}");
      drone.SensorData.vx_GLOBAL_POSITION_INT = globalPositionInt.vx;
      drone.SensorData.vy_GLOBAL_POSITION_INT = globalPositionInt.vy;
    }
    // if (data is MAVLink.mavlink_sys_status_t sysStatus)
    // {
    //   Console.WriteLine(sysStatus);
    // }
    if (data is MAVLink.mavlink_power_status_t powerStatus)
    {
      // predicton
      // Console.WriteLine($"Vservo:{powerStatus.Vservo}");
      drone.SensorData.Vservo_POSER_STATUS = powerStatus.Vservo;
    }
    // if (data is MAVLink.mavlink_meminfo_t meminfo)
    // {
    //   Console.WriteLine(meminfo);
    // }
    if (data is MAVLink.mavlink_nav_controller_output_t navControllerOutput)
    {
      // predicton
      // Console.WriteLine($"nav_pitch:{navControllerOutput.nav_pitch}, nav_bearing:{navControllerOutput.nav_bearing}");
      drone.SensorData.nav_pitch_NAV_CONTROLLER_OUTPUT = navControllerOutput.nav_pitch;
      drone.SensorData.nav_bearing_NAV_CONTROLLER_OUTPUT = navControllerOutput.nav_bearing;
    }
    // if (data is MAVLink.mavlink_mission_current_t missionCurrent)
    // {
    //   Console.WriteLine(missionCurrent);
    // }
    if (data is MAVLink.mavlink_vfr_hud_t vfrHud)
    {
      // predicton
      // Console.WriteLine($"airspeed:{vfrHud.airspeed}, groundspeed:{vfrHud.groundspeed}");
      drone.SensorData.airspeed_VFR_HUD = vfrHud.airspeed;
      drone.SensorData.groundspeed_VFR_HUD = vfrHud.groundspeed;
    }
    if (data is MAVLink.mavlink_servo_output_raw_t servoOutput)
    {
      // predicton
      // Console.WriteLine($"servo3_raw:{servoOutput.servo3_raw}, servo8_raw:{servoOutput.servo8_raw}");
      drone.SensorData.servo3_raw_SERVO_OUTPUT_RAW = servoOutput.servo3_raw;
      drone.SensorData.servo8_raw_SERVO_OUTPUT_RAW = servoOutput.servo8_raw;
    }
    if (data is MAVLink.mavlink_rc_channels_t rcChannels)
    {
      // predicton
      // Console.WriteLine($"chancount:{rcChannels.chancount}, chan12_raw:{rcChannels.chan12_raw}, chan13_raw:{rcChannels.chan13_raw}, chan14_raw:{rcChannels.chan14_raw}, chan15_raw:{rcChannels.chan15_raw}, chan16_raw:{rcChannels.chan16_raw}");
      drone.SensorData.chancount_RC_CHANNELS = rcChannels.chancount;
      drone.SensorData.chan12_raw_RC_CHANNELS = rcChannels.chan12_raw;
      drone.SensorData.chan13_raw_RC_CHANNELS = rcChannels.chan13_raw;
      drone.SensorData.chan14_raw_RC_CHANNELS = rcChannels.chan14_raw;
      drone.SensorData.chan15_raw_RC_CHANNELS = rcChannels.chan15_raw;
      drone.SensorData.chan16_raw_RC_CHANNELS = rcChannels.chan16_raw;
    }
    if (data is MAVLink.mavlink_raw_imu_t rawImu)
    {
      // predicton
      // Console.WriteLine($"xacc:{rawImu.xacc}, yacc:{rawImu.yacc}, zacc:{rawImu.zacc}");
      drone.SensorData.xacc_RAW_IMU = rawImu.xacc;
      drone.SensorData.yacc_RAW_IMU = rawImu.yacc;
      drone.SensorData.zacc_RAW_IMU = rawImu.zacc;
      // Console.WriteLine($"xgyro:{rawImu.xgyro}, ygyro:{rawImu.ygyro}, zgyro:{rawImu.zgyro}");
      drone.SensorData.xgyro_RAW_IMU = rawImu.xgyro;
      drone.SensorData.ygyro_RAW_IMU = rawImu.ygyro;
      drone.SensorData.zgyro_RAW_IMU = rawImu.zgyro;
      // Console.WriteLine($"xmag:{rawImu.xmag}, ymag:{rawImu.ymag}, zmag:{rawImu.zmag}");
      drone.SensorData.xmag_RAW_IMU = rawImu.xmag;
      drone.SensorData.ymag_RAW_IMU = rawImu.ymag;
      drone.SensorData.zmag_RAW_IMU = rawImu.zmag;
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
      drone.SensorData.press_abs_SCALED_PRESSURE = scaledPressure.press_abs;
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
      drone.SensorData.x_LOCAL_POSITION_NED = localPositionNed.x;
      drone.SensorData.vx_LOCAL_POSITION_NED = localPositionNed.vx;
      drone.SensorData.vy_LOVAL_POSITION_NED = localPositionNed.vy;
    }
    if (data is MAVLink.mavlink_vibration_t vibration)
    {
      // predicton
      // Console.WriteLine($"vibration_x:{vibration.vibration_x}, vibration_y:{vibration.vibration_y}, vibration_z:{vibration.vibration_z}");
      drone.SensorData.vibration_x_VIBRATION = vibration.vibration_x;
      drone.SensorData.vibration_y_VIBRATION = vibration.vibration_y;
      drone.SensorData.vibration_z_VIBRATION = vibration.vibration_z;
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
      drone.SensorData.accel_cal_x_SENSOR_OFFSETS = sensorOffsets.accel_cal_x;
      drone.SensorData.accel_cal_y_SENSOR_OFFSETS = sensorOffsets.accel_cal_y;
      drone.SensorData.accel_cal_z_SENSOR_OFFSETS = sensorOffsets.accel_cal_z;
      // Console.WriteLine($"mag_ofs_x:{sensorOffsets.mag_ofs_x}, mag_ofs_y:{sensorOffsets.mag_ofs_y}");
      drone.SensorData.mag_ofs_x_SENSOR_OFFSETS = sensorOffsets.mag_ofs_x;
      drone.SensorData.mag_ofs_y_SENSOR_OFFSETS = sensorOffsets.mag_ofs_y;
    }
  }
  
  public string objectToJson() 
  {
    string droneMessage = JsonConvert.SerializeObject(drone);
    // Console.WriteLine(droneMessage);
    return droneMessage;
  }
}