# Prediction Service

## 1. Project Architecture

    kisa-prediction-service/
    │
    ├── app/
    │   ├── __init__.py
    │   ├── config.py
    │   ├── routes.py
    │   └── controller.py
    ├── run.py
    └── requirements.txt


## 2. Dependency management

    % pip install -r requirements.txt

## 3. Database
### [MongoDB Specification](https://docs.google.com/spreadsheets/d/1H0tCsqDfMZ2z4MZ82Cf29FznkBN-HQiu5DckXepfIy8/edit?usp=sharing)

## 4. URLs

### 4-1. 실시간 장애진단 : hostip:5050/api/realtime
#### GET
> request : url

> response

     {
        drones[...]
     }

#### POST
> request : form(DroneId)
######
> response

    {
        realtimePage: {
            {
                _id,
                DroneId,
                Alt: [ // 드론 고도 리스트
                        {Alt:},
                        {Alt:},
                        ...
                ],    
                SensorData: {
                    pitch_ATTITUDE,
                    roll_ATTITUDE,
                    vibration_x_VIBRATION,
                    vibration_y_VIBRATION,
                    vibration_z_VIBRATION,
                    xacc_RAW_IMU,
                    xgyro_RAW_IMU,
                    xmag_RAW_IMU,
                    yacc_RAW_IMU,
                    yaw_ATTITUDE,
                    ygyro_RAW_IMU,
                    ymag_RAW_IMU,
                    zacc_RAW_IMU,
                    zgyro_RAW_IMU,
                    zmag_RAW_IMU,
                },
                PredictData: {
                    pitch_ATTITUDE_PREDICT,
                    roll_ATTITUDE_PREDICT,
                    vibration_x_VIBRATION_PREDICT,
                    vibration_y_VIBRATION_PREDICT,
                    vibration_z_VIBRATION_PREDICT,
                    xacc_RAW_IMU_PREDICT,
                    xgyro_RAW_IMU_PREDICT,
                    xmag_RAW_IMU_PREDICT,
                    yacc_RAW_IMU_PREDICT,
                    yaw_ATTITUDE_PREDICT,
                    ygyro_RAW_IMU_PREDICT,
                    ymag_RAW_IMU_PREDICT,
                    zacc_RAW_IMU_PREDICT,
                    zgyro_RAW_IMU_PREDICT,
                    zmag_RAW_IMU_PREDICT,
                },
              }  
            }
        }
    }

### 4-2. 로그 데이터 조회 : hostip:5050/api/logdata
#### GET
> request : url

> response

     {
        drones: [...], 
        flights: [...]
     }

#### POST
> request : form(DroneId, FlightId, periodFrom, periodTo)
######
> response

    {        
        logPage: [
            { 
                _id,
                DroneId,
                FlightId,
                PredictTime,
                SensorData{
                    Vservo_POWER_STATUS,
                    accel_cal_x_SENSOR_OFFSETS,
                    accel_cal_y_SENSOR_OFFSETS,
                    accel_cal_z_SENSOR_OFFSETS,
                    airspeed_VFR_HUD,
                    chan12_raw_RC_CHANNELS,
                    chan13_raw_RC_CHANNELS,
                    chan14_raw_RC_CHANNELS,
                    chan15_raw_RC_CHANNELS,
                    chan16_raw_RC_CHANNELS,
                    chancount_RC_CHANNELS,
                    groundspeed_VFR_HUD,
                    mag_ofs_x_SENSOR_OFFSETS,
                    mag_ofs_y_SENSOR_OFFSETS,
                    nav_bearing_NAV_CONTROLLER_OUTPUT,
                    nav_pitch_NAV_CONTROLLER_OUTPUT,
                    pitch_ATTITUDE,
                    press_abs_SCALED_PRESSURE,
                    roll_ATTITUDE,
                    servo3_raw_SERVO_OUTPUT_RAW,
                    servo8_raw_SERVO_OUTPUT_RAW,
                    vibration_x_VIBRATION,
                    vibration_y_VIBRATION,
                    vibration_z_VIBRATION,
                    voltages1_BATTERY_STATUS,
                    vx_GLOBAL_POSITION_INT,
                    vx_LOCAL_POSITION_NED,
                    vy_GLOBAL_POSITION_INT,
                    vy_LOCAL_POSITION_NED,
                    x_LOCAL_POSITION_NED,
                    xacc_RAW_IMU,
                    xgyro_RAW_IMU,
                    xmag_RAW_IMU,
                    yacc_RAW_IMU,
                    yaw_ATTITUDE,
                    ygyro_RAW_IMU,
                    ymag_RAW_IMU,
                    zacc_RAW_IMU,
                    zgyro_RAW_IMU,
                    zmag_RAW_IMU,
                }
            }
        ]
    }

### 4-3. 장애 진단 예측 결과 : hostip:5050/api/predict
#### GET
> request : url

> response

    {
        drones: [...], 
        flights: [...]
    }

#### POST
> request : form(DroneId, FlightId, periodFrom, periodTo, SelectData)

> response

       {
            predictPage: [
                { 
                  _id,
                  DroneId,
                  FlightId,
                  PredictTime,
                  SelectData,
                  PredictData,
                  SensorData: {
                        Vservo_POWER_STATUS,
                        accel_cal_x_SENSOR_OFFSETS,
                        accel_cal_y_SENSOR_OFFSETS,
                        accel_cal_z_SENSOR_OFFSETS,
                        airspeed_VFR_HUD,
                        chan12_raw_RC_CHANNELS,
                        chan13_raw_RC_CHANNELS,
                        chan14_raw_RC_CHANNELS,
                        chan15_raw_RC_CHANNELS,
                        chan16_raw_RC_CHANNELS,
                        chancount_RC_CHANNELS,
                        groundspeed_VFR_HUD,
                        mag_ofs_x_SENSOR_OFFSETS,
                        mag_ofs_y_SENSOR_OFFSETS,
                        nav_bearing_NAV_CONTROLLER_OUTPUT,
                        nav_pitch_NAV_CONTROLLER_OUTPUT,
                        pitch_ATTITUDE,
                        press_abs_SCALED_PRESSURE,
                        roll_ATTITUDE,
                        servo3_raw_SERVO_OUTPUT_RAW,
                        servo8_raw_SERVO_OUTPUT_RAW,
                        vibration_x_VIBRATION,
                        vibration_y_VIBRATION,
                        vibration_z_VIBRATION,
                        voltages1_BATTERY_STATUS,
                        vx_GLOBAL_POSITION_INT,
                        vx_LOCAL_POSITION_NED,
                        vy_GLOBAL_POSITION_INT,
                        vy_LOCAL_POSITION_NED,
                        x_LOCAL_POSITION_NED,
                        xacc_RAW_IMU,
                        xgyro_RAW_IMU,
                        xmag_RAW_IMU,
                        yacc_RAW_IMU,
                        yaw_ATTITUDE,
                        ygyro_RAW_IMU,
                        ymag_RAW_IMU,
                        zacc_RAW_IMU,
                        zgyro_RAW_IMU,
                        zmag_RAW_IMU,
                  }
                }
            ]
       }