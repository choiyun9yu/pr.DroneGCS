//import java.time.LocalDateTime;
//import java.util.List;
//
//
//
//// DTO 사용, DTO는 데이터 전송을 위한 객체로 클래스와 비슷하게 생겼지만 주로 특정 응답 형태를 표현하기 위해 사용된다.
//// localhost:8080/api/realtime
//public class DroneListResponse {
//    private List<String> drones;
//
//    public DroneListResponse(List<String> drones) {
//        this.drones = drones;
//    }
//
//    public List<String> getDrones() {
//        return drones;
//    }
//
//    public void setDrones(List<String> drones) {
//        this.drones = drones;
//    }
//}
//
//
//public class Drone {
//    public String _id;
//    public String DroneId;
//    public String DroneId;
//    public String FlightId;
//    public LocalDateTime PredictTime;
//    public float Alt;
//    public SensorData SensorData;
//}
//
//
//public class SensorData
//{
//    public float roll_ATTITUDE;
//    public float pitch_ATTITUDE;
//    public float yaw_ATTITUDE;
//    public float xacc_RAW_IMU;
//    public float yacc_RAW_IMU;
//    public float zacc_RAW_IMU;
//    public float xgyro_RAW_IMU;
//    public float ygyro_RAW_IMU;
//    public float zgyro_RAW_IMU;
//    public float xmag_RAW_IMU;
//    public float ymag_RAW_IMU;
//    public float zmag_RAW_IMU;
//    public float vibration_x_VIBRATION;
//    public float vibration_y_VIBRATION;
//    public float vibration_z_VIBRATION;
//    public float accel_cal_x_SENSOR_OFFSETS;
//    public float accel_cal_y_SENSOR_OFFSETS;
//    public float accel_cal_z_SENSOR_OFFSETS;
//    public float mag_ofs_x_SENSOR_OFFSETS;
//    public float mag_ofs_y_SENSOR_OFFSETS;
//    public float vx_GLOBAL_POSITION_INT;
//    public float vy_GLOBAL_POSITION_INT;
//    public float x_LOCAL_POSITION_NED;
//    public float vx_LOCAL_POSITION_NED;
//    public float vy_LOCAL_POSITION_NED;
//    public float nav_pitch_NAV_CONTROLLER_OUTPUT;
//    public float nav_bearing_NAV_CONTROLLER_OUTPUT;
//    public float servo3_raw_SERVO_OUTPUT_RAW;
//    public float servo8_raw_SERVO_OUTPUT_RAW;
//    public float groundspeed_VFR_HUD;
//    public float airspeed_VFR_HUD;
//    public float press_abs_SCALED_PRESSURE;
//    public float Vservo_POSER_STATUS;
//    public float voltages1_BATTERY_STATUS;
//    public float chancount_RC_CHANNELS;
//    public float chan12_raw_RC_CHANNELS;
//    public float chan13_raw_RC_CHANNELS;
//    public float chan14_raw_RC_CHANNELS;
//    public float chan15_raw_RC_CHANNELS;
//    public float chan16_raw_RC_CHANNELS;
//}
