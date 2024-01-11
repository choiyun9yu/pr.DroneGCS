# SignalR
(JAVA에서는 Atmosphere이 SignalR과 비슷한 라이브러리 인듯)

    [
        {
            "DroneId":"",
            "DroneLogger":[],
            "IsOnline":false,
            "HasDeliverPlan":false,
            "WayPointsDistance":[],
            "DroneStt":{
                            "WayPointNum":0.0,
                            "PowerV":5000.0,
                            "GpsStt":" ",
                            "TempC":0.0,
                            "LoaderLoad":" ",
                            "LoaderLock":" ",
                            "Lat":-353632621.0,
                            "Lon":1491652374.0,
                            "Alt":0.0,"Speed":0.0,
                            "ROLL_ATTITUDE":0.0,
                            "PITCH_ATTITUDE":0.0,
                            "YAW_ATTITUDE":0.0,
                            "HoverStt":" ",
                            "HODP":0.0,
                            "FlightMode":" ",
                            "SatCount":0.0,
                            "MabSysStt":0.0,
                            "SensorStt":{
                                            "Name":null,
                                            "Enabled":null, 
                                            "Present":null,
                                            "Health":null
                                        },
                            "Mavlinkinfo":{
                                            "FrameType":null,
                                            "Ros":null,
                                            "FC_HARDWAR":null,
                                            "Autopilot":null,
                                            "CommunicationOut":null
                                            }
                        },
            "DroneTrack":{
                            "PathIndex":null,
                            "DroneTrails":null,
                            "DroneProgress":null,
                            "DroneProgressPresentation":null,
                            "TotalDistance":null,
                            "ElapsedDistance":null,
                            "RemainDistance":null
                        },
            "DroneCamera":{
                            "FWD_CAM_STATE":null,   
                            "CameraIp":null,
                            "CameraUrl1":null,
                            "CameraUrl2":null,
                            "CameraProtocolType":null
                            },
            "DroneMission":{
                            "MavMission":null,
                            "StartTime":null,
                            "CompleteTime":null
                            },
            "CommunicationLink":{
                                "ConnectionProtocol":null,
                                "MessageProtocol":null,
                                "Address":null
                                },
            "SensorData":{
                            "roll_ATTITUDE":0.001236144220456481,
                            "pitch_ATTITUDE":0.0010411576367914677,
                            "yaw_ATTITUDE":-0.10372279584407806,
                            "xacc_RAW_IMU":0.0,"yacc_RAW_IMU":0.0,
                            "zacc_RAW_IMU":-1001.0,
                            "xgyro_RAW_IMU":2.0,
                            "ygyro_RAW_IMU":2.0,
                            "zgyro_RAW_IMU":2.0,
                            "xmag_RAW_IMU":224.0,
                            "ymag_RAW_IMU":80.0,
                            "zmag_RAW_IMU":-528.0,
                            "vibration_x_VIBRATION":0.002617495134472847,
                            "vibration_y_VIBRATION":0.0029051746241748333,  
                            "vibration_z_VIBRATION":0.0026729062665253878,
                            "accel_cal_x_SENSOR_OFFSETS":0.0,
                            "accel_cal_y_SENSOR_OFFSETS":0.0,
                            "accel_cal_z_SENSOR_OFFSETS":0.0,
                            "mag_ofs_x_SENSOR_OFFSETS":0.0,
                            "mag_ofs_y_SENSOR_OFFSETS":0.0,
                            "vx_GLOBAL_POSITION_INT":-1.0,
                            "vy_GLOBAL_POSITION_INT":1.0,
                            "x_LOCAL_POSITION_NED":-0.01187055092304945,
                            "vx_LOCAL_POSITION_NED":-0.015008852817118168,
                            "vy_LOVAL_POSITION_NED":0.01831723004579544,
                            "nav_pitch_NAV_CONTROLLER_OUTPUT":0.00021179835312068462,
                            "nav_bearing_NAV_CONTROLLER_OUTPUT":-5.0,
                            "servo3_raw_SERVO_OUTPUT_RAW":1000.0,
                            "servo8_raw_SERVO_OUTPUT_RAW":0.0,
                            "groundspeed_VFR_HUD":0.023680932819843292,
                            "airspeed_VFR_HUD":0.02368166483938694,
                            "press_abs_SCALED_PRESSURE":945.0279541015625,
                            "Vservo_POSER_STATUS":0.0, 
                            "voltages1_BATTERY_STATUS":0.0,
                            "chancount_RC_CHANNELS":16.0,
                            "chan12_raw_RC_CHANNELS":0.0,
                            "chan13_raw_RC_CHANNELS":0.0,
                            "chan14_raw_RC_CHANNELS":0.0,
                            "chan15_raw_RC_CHANNELS":0.0,
                            "chan16_raw_RC_CHANNELS":0.0}
        }
    ]


###

DroneHub의 인스턴스는 SignalR 허브가 시작될 때 자동으로 생성되며, ASP.NET Core의 의존성 주입(Dependency Injection) 시스템에 의해 관리됩니다. 허브를 생성하는 코드를 직접 작성하지 않아도 ASP.NET Core가 적절한 시점에 허브를 초기화하고 관리합니다.