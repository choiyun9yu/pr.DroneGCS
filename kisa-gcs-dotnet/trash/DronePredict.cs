// using System.Runtime.Serialization;
//
// namespace kisa_gcs_service.Model;
//
// [DataContract]
// public class SensorData
// {
//     [DataMember] public double roll_ATTITUDE;
//     [DataMember] public double pitch_ATTITUDE;
//     [DataMember] public double yaw_ATTITUDE;
//     [DataMember] public double xacc_RAW_IMU;
//     [DataMember] public double yacc_RAW_IMU;
//     [DataMember] public double zacc_RAW_IMU;
//     [DataMember] public double xgyro_RAW_IMU;
//     [DataMember] public double ygyro_RAW_IMU;
//     [DataMember] public double zgyro_RAW_IMU;
//     [DataMember] public double xmag_RAW_IMU;
//     [DataMember] public double ymag_RAW_IMU;
//     [DataMember] public double zmag_RAW_IMU;
//     [DataMember] public double vibration_x_VIBRATION;
//     [DataMember] public double vibration_y_VIBRATION;
//     [DataMember] public double vibration_z_VIBRATION;
//     [DataMember] public double accel_cal_x_SENSOR_OFFSETS;
//     [DataMember] public double accel_cal_y_SENSOR_OFFSETS;
//     [DataMember] public double accel_cal_z_SENSOR_OFFSETS;
//     [DataMember] public double mag_ofs_x_SENSOR_OFFSETS;
//     [DataMember] public double mag_ofs_y_SENSOR_OFFSETS;
//     [DataMember] public double vx_GLOBAL_POSITION_INT;
//     [DataMember] public double vy_GLOBAL_POSITION_INT;
//     [DataMember] public double x_LOCAL_POSITION_NED;
//     [DataMember] public double vx_LOCAL_POSITION_NED;
//     [DataMember] public double vy_LOVAL_POSITION_NED;
//     [DataMember] public double nav_pitch_NAV_CONTROLLER_OUTPUT;
//     [DataMember] public double nav_bearing_NAV_CONTROLLER_OUTPUT;
//     [DataMember] public double servo3_raw_SERVO_OUTPUT_RAW;
//     [DataMember] public double servo8_raw_SERVO_OUTPUT_RAW;
//     [DataMember] public double groundspeed_VFR_HUD;
//     [DataMember] public double airspeed_VFR_HUD;
//     [DataMember] public double press_abs_SCALED_PRESSURE;
//     [DataMember] public double Vservo_POSER_STATUS;
//     [DataMember] public double voltages1_BATTERY_STATUS;
//     [DataMember] public double chancount_RC_CHANNELS;
//     [DataMember] public double chan12_raw_RC_CHANNELS;
//     [DataMember] public double chan13_raw_RC_CHANNELS;
//     [DataMember] public double chan14_raw_RC_CHANNELS;
//     [DataMember] public double chan15_raw_RC_CHANNELS;
//     [DataMember] public double chan16_raw_RC_CHANNELS;
// }
//
// [DataContract]
// public class DronePredict
// {
//     // PredictTime은 장애진단 서버에서 예측할 때 찍거나 몽고디비에 저장할 때 Timestamp 찍기
//     [DataMember] public string? DroneId;
//     [DataMember] public string? FlightId;
//     [DataMember] public float? Alt;
//     [DataMember] public SensorData? SensorData;
//     // PredictData도 장애진단 서버에서 예측해서 몽고디비에 저장
// }