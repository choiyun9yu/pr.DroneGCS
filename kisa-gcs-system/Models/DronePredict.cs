namespace kisa_gcs_system.Models;

[DataContract] 
public struct SensorData
{
    [DataMember] 
    public double roll_ATTITUDE;
    
    [DataMember] 
    public double pitch_ATTITUDE;
    
    [DataMember] 
    public double yaw_ATTITUDE;
    
    [DataMember] 
    public double xacc_RAW_IMU;
    
    [DataMember] 
    public double yacc_RAW_IMU;
    
    [DataMember] 
    public double zacc_RAW_IMU;
    
    [DataMember] 
    public double xgyro_RAW_IMU;
    
    [DataMember] 
    public double ygyro_RAW_IMU;
    
    [DataMember] 
    public double zgyro_RAW_IMU;
    
    [DataMember] 
    public double xmag_RAW_IMU;
    
    [DataMember] 
    public double ymag_RAW_IMU;
    
    [DataMember] 
    public double zmag_RAW_IMU;
    
    [DataMember] 
    public double vibration_x_VIBRATION;
    
    [DataMember] 
    public double vibration_y_VIBRATION;
    
    [DataMember] 
    public double vibration_z_VIBRATION;
    
    [DataMember] 
    public double accel_cal_x_SENSOR_OFFSETS;
    
    [DataMember] 
    public double accel_cal_y_SENSOR_OFFSETS;
    
    [DataMember] 
    public double accel_cal_z_SENSOR_OFFSETS;
    
    [DataMember] 
    public double mag_ofs_x_SENSOR_OFFSETS;
    
    [DataMember] 
    public double mag_ofs_y_SENSOR_OFFSETS;
    
    [DataMember] 
    public double vx_GLOBAL_POSITION_INT;
    
    [DataMember] 
    public double vy_GLOBAL_POSITION_INT;
    
    [DataMember] 
    public double x_LOCAL_POSITION_NED;
    
    [DataMember] 
    public double vx_LOCAL_POSITION_NED;
    
    [DataMember] 
    public double vy_LOVAL_POSITION_NED;
    
    [DataMember] 
    public double nav_pitch_NAV_CONTROLLER_OUTPUT;
    
    [DataMember] 
    public double nav_bearing_NAV_CONTROLLER_OUTPUT;
    
    [DataMember] 
    public double servo3_raw_SERVO_OUTPUT_RAW;
    
    [DataMember] 
    public double servo8_raw_SERVO_OUTPUT_RAW;
    
    [DataMember] 
    public double groundspeed_VFR_HUD;
    
    [DataMember] 
    public double airspeed_VFR_HUD;
    
    [DataMember] 
    public double press_abs_SCALED_PRESSURE;

    [DataMember] 
    public double Vservo_POSER_STATUS;
    
    [DataMember] 
    public double voltages1_BATTERY_STATUS;
    
    [DataMember] 
    public double chancount_RC_CHANNELS;
    
    [DataMember] 
    public double chan12_raw_RC_CHANNELS;
    
    [DataMember] 
    public double chan13_raw_RC_CHANNELS;
    
    [DataMember] 
    public double chan14_raw_RC_CHANNELS;
    
    [DataMember] 
    public double chan15_raw_RC_CHANNELS;
    
    [DataMember] 
    public double chan16_raw_RC_CHANNELS;

    public SensorData()
    {
        roll_ATTITUDE = 0.0;
        
        pitch_ATTITUDE = 0.0;
        
        yaw_ATTITUDE = 0.0;
        
        xacc_RAW_IMU = 0.0;
        
        yacc_RAW_IMU = 0.0;
        
        zacc_RAW_IMU = 0.0;
        
        xgyro_RAW_IMU = 0.0;
        
        ygyro_RAW_IMU = 0.0;
        
        zgyro_RAW_IMU = 0.0;
        
        xmag_RAW_IMU = 0.0;
        
        ymag_RAW_IMU = 0.0;
        
        zmag_RAW_IMU = 0.0;
        
        vibration_x_VIBRATION = 0.0;
        
        vibration_y_VIBRATION = 0.0;
        
        vibration_z_VIBRATION = 0.0;
        
        accel_cal_x_SENSOR_OFFSETS = 0.0;
        
        accel_cal_y_SENSOR_OFFSETS = 0.0;
        
        accel_cal_z_SENSOR_OFFSETS = 0.0;
        
        mag_ofs_x_SENSOR_OFFSETS = 0.0;
        
        mag_ofs_y_SENSOR_OFFSETS = 0.0;
        
        vx_GLOBAL_POSITION_INT = 0.0;
        
        vy_GLOBAL_POSITION_INT = 0.0;
        
        x_LOCAL_POSITION_NED = 0.0;
        
        vx_LOCAL_POSITION_NED = 0.0;
        
        vy_LOVAL_POSITION_NED = 0.0;
        
        nav_pitch_NAV_CONTROLLER_OUTPUT = 0.0;
        
        nav_bearing_NAV_CONTROLLER_OUTPUT = 0.0;
        
        servo3_raw_SERVO_OUTPUT_RAW = 0.0;
        
        servo8_raw_SERVO_OUTPUT_RAW = 0.0;
        
        groundspeed_VFR_HUD = 0.0;
        
        airspeed_VFR_HUD = 0.0;
        
        press_abs_SCALED_PRESSURE = 0.0;
        
        Vservo_POSER_STATUS = 0.0;
        
        voltages1_BATTERY_STATUS = 0.0;
        
        chancount_RC_CHANNELS = 0.0;
        
        chan12_raw_RC_CHANNELS = 0.0;
        
        chan13_raw_RC_CHANNELS = 0.0;
        
        chan14_raw_RC_CHANNELS = 0.0;
        
        chan15_raw_RC_CHANNELS = 0.0;
        
        chan16_raw_RC_CHANNELS = 0.0;
    }
}