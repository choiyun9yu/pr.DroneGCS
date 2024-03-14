from google.protobuf import timestamp_pb2 as _timestamp_pb2
from google.protobuf.internal import containers as _containers
from google.protobuf import descriptor as _descriptor
from google.protobuf import message as _message
from typing import ClassVar as _ClassVar, Iterable as _Iterable, Mapping as _Mapping, Optional as _Optional, Union as _Union

DESCRIPTOR: _descriptor.FileDescriptor

class UpdateDroneStatusPayload(_message.Message):
    __slots__ = ("droneStatuses",)
    DRONESTATUSES_FIELD_NUMBER: _ClassVar[int]
    droneStatuses: _containers.RepeatedCompositeFieldContainer[GrpcDroneStatus]
    def __init__(self, droneStatuses: _Optional[_Iterable[_Union[GrpcDroneStatus, _Mapping]]] = ...) -> None: ...

class GrpcDroneStatus(_message.Message):
    __slots__ = ("DroneId", "FlightId", "IsOnline", "IsLanded", "ControllStt", "DroneStt", "SensorData")
    DRONEID_FIELD_NUMBER: _ClassVar[int]
    FLIGHTID_FIELD_NUMBER: _ClassVar[int]
    ISONLINE_FIELD_NUMBER: _ClassVar[int]
    ISLANDED_FIELD_NUMBER: _ClassVar[int]
    CONTROLLSTT_FIELD_NUMBER: _ClassVar[int]
    DRONESTT_FIELD_NUMBER: _ClassVar[int]
    SENSORDATA_FIELD_NUMBER: _ClassVar[int]
    DroneId: str
    FlightId: str
    IsOnline: bool
    IsLanded: bool
    ControllStt: str
    DroneStt: GrpcDroneStt
    SensorData: GrpcSensorData
    def __init__(self, DroneId: _Optional[str] = ..., FlightId: _Optional[str] = ..., IsOnline: bool = ..., IsLanded: bool = ..., ControllStt: _Optional[str] = ..., DroneStt: _Optional[_Union[GrpcDroneStt, _Mapping]] = ..., SensorData: _Optional[_Union[GrpcSensorData, _Mapping]] = ...) -> None: ...

class GrpcDroneStt(_message.Message):
    __slots__ = ("PowerV", "BatteryStt", "GpsStt", "TempC", "Lat", "Lon", "Alt", "GlobalAlt", "Roll", "Pitch", "Head", "Speed", "HoverStt", "HDOP", "SatellitesCount", "FlightMode")
    POWERV_FIELD_NUMBER: _ClassVar[int]
    BATTERYSTT_FIELD_NUMBER: _ClassVar[int]
    GPSSTT_FIELD_NUMBER: _ClassVar[int]
    TEMPC_FIELD_NUMBER: _ClassVar[int]
    LAT_FIELD_NUMBER: _ClassVar[int]
    LON_FIELD_NUMBER: _ClassVar[int]
    ALT_FIELD_NUMBER: _ClassVar[int]
    GLOBALALT_FIELD_NUMBER: _ClassVar[int]
    ROLL_FIELD_NUMBER: _ClassVar[int]
    PITCH_FIELD_NUMBER: _ClassVar[int]
    HEAD_FIELD_NUMBER: _ClassVar[int]
    SPEED_FIELD_NUMBER: _ClassVar[int]
    HOVERSTT_FIELD_NUMBER: _ClassVar[int]
    HDOP_FIELD_NUMBER: _ClassVar[int]
    SATELLITESCOUNT_FIELD_NUMBER: _ClassVar[int]
    FLIGHTMODE_FIELD_NUMBER: _ClassVar[int]
    PowerV: float
    BatteryStt: int
    GpsStt: str
    TempC: float
    Lat: float
    Lon: float
    Alt: float
    GlobalAlt: float
    Roll: float
    Pitch: float
    Head: int
    Speed: float
    HoverStt: str
    HDOP: float
    SatellitesCount: int
    FlightMode: int
    def __init__(self, PowerV: _Optional[float] = ..., BatteryStt: _Optional[int] = ..., GpsStt: _Optional[str] = ..., TempC: _Optional[float] = ..., Lat: _Optional[float] = ..., Lon: _Optional[float] = ..., Alt: _Optional[float] = ..., GlobalAlt: _Optional[float] = ..., Roll: _Optional[float] = ..., Pitch: _Optional[float] = ..., Head: _Optional[int] = ..., Speed: _Optional[float] = ..., HoverStt: _Optional[str] = ..., HDOP: _Optional[float] = ..., SatellitesCount: _Optional[int] = ..., FlightMode: _Optional[int] = ...) -> None: ...

class GrpcSensorData(_message.Message):
    __slots__ = ("roll_ATTITUDE", "pitch_ATTITUDE", "yaw_ATTITUDE", "xacc_RAW_IMU", "yacc_RAW_IMU", "zacc_RAW_IMU", "xgyro_RAW_IMU", "ygyro_RAW_IMU", "zgyro_RAW_IMU", "xmag_RAW_IMU", "ymag_RAW_IMU", "zmag_RAW_IMU", "vibration_x_VIBRATION", "vibration_y_VIBRATION", "vibration_z_VIBRATION", "accel_cal_x_SENSOR_OFFSETS", "accel_cal_y_SENSOR_OFFSETS", "accel_cal_z_SENSOR_OFFSETS", "mag_ofs_x_SENSOR_OFFSETS", "mag_ofs_y_SENSOR_OFFSETS", "vx_GLOBAL_POSITION_INT", "vy_GLOBAL_POSITION_INT", "x_LOCAL_POSITION_NED", "vx_LOCAL_POSITION_NED", "vy_LOCAL_POSITION_NED", "nav_pitch_NAV_CONTROLLER_OUTPUT", "nav_bearing_NAV_CONTROLLER_OUTPUT", "servo3_raw_SERVO_OUTPUT_RAW", "servo8_raw_SERVO_OUTPUT_RAW", "groundspeed_VFR_HUD", "airspeed_VFR_HUD", "press_abs_SCALED_PRESSURE", "Vservo_POWER_STATUS", "voltages1_BATTERY_STATUS", "chancount_RC_CHANNELS", "chan12_raw_RC_CHANNELS", "chan13_raw_RC_CHANNELS", "chan14_raw_RC_CHANNELS", "chan15_raw_RC_CHANNELS", "chan16_raw_RC_CHANNELS")
    ROLL_ATTITUDE_FIELD_NUMBER: _ClassVar[int]
    PITCH_ATTITUDE_FIELD_NUMBER: _ClassVar[int]
    YAW_ATTITUDE_FIELD_NUMBER: _ClassVar[int]
    XACC_RAW_IMU_FIELD_NUMBER: _ClassVar[int]
    YACC_RAW_IMU_FIELD_NUMBER: _ClassVar[int]
    ZACC_RAW_IMU_FIELD_NUMBER: _ClassVar[int]
    XGYRO_RAW_IMU_FIELD_NUMBER: _ClassVar[int]
    YGYRO_RAW_IMU_FIELD_NUMBER: _ClassVar[int]
    ZGYRO_RAW_IMU_FIELD_NUMBER: _ClassVar[int]
    XMAG_RAW_IMU_FIELD_NUMBER: _ClassVar[int]
    YMAG_RAW_IMU_FIELD_NUMBER: _ClassVar[int]
    ZMAG_RAW_IMU_FIELD_NUMBER: _ClassVar[int]
    VIBRATION_X_VIBRATION_FIELD_NUMBER: _ClassVar[int]
    VIBRATION_Y_VIBRATION_FIELD_NUMBER: _ClassVar[int]
    VIBRATION_Z_VIBRATION_FIELD_NUMBER: _ClassVar[int]
    ACCEL_CAL_X_SENSOR_OFFSETS_FIELD_NUMBER: _ClassVar[int]
    ACCEL_CAL_Y_SENSOR_OFFSETS_FIELD_NUMBER: _ClassVar[int]
    ACCEL_CAL_Z_SENSOR_OFFSETS_FIELD_NUMBER: _ClassVar[int]
    MAG_OFS_X_SENSOR_OFFSETS_FIELD_NUMBER: _ClassVar[int]
    MAG_OFS_Y_SENSOR_OFFSETS_FIELD_NUMBER: _ClassVar[int]
    VX_GLOBAL_POSITION_INT_FIELD_NUMBER: _ClassVar[int]
    VY_GLOBAL_POSITION_INT_FIELD_NUMBER: _ClassVar[int]
    X_LOCAL_POSITION_NED_FIELD_NUMBER: _ClassVar[int]
    VX_LOCAL_POSITION_NED_FIELD_NUMBER: _ClassVar[int]
    VY_LOCAL_POSITION_NED_FIELD_NUMBER: _ClassVar[int]
    NAV_PITCH_NAV_CONTROLLER_OUTPUT_FIELD_NUMBER: _ClassVar[int]
    NAV_BEARING_NAV_CONTROLLER_OUTPUT_FIELD_NUMBER: _ClassVar[int]
    SERVO3_RAW_SERVO_OUTPUT_RAW_FIELD_NUMBER: _ClassVar[int]
    SERVO8_RAW_SERVO_OUTPUT_RAW_FIELD_NUMBER: _ClassVar[int]
    GROUNDSPEED_VFR_HUD_FIELD_NUMBER: _ClassVar[int]
    AIRSPEED_VFR_HUD_FIELD_NUMBER: _ClassVar[int]
    PRESS_ABS_SCALED_PRESSURE_FIELD_NUMBER: _ClassVar[int]
    VSERVO_POWER_STATUS_FIELD_NUMBER: _ClassVar[int]
    VOLTAGES1_BATTERY_STATUS_FIELD_NUMBER: _ClassVar[int]
    CHANCOUNT_RC_CHANNELS_FIELD_NUMBER: _ClassVar[int]
    CHAN12_RAW_RC_CHANNELS_FIELD_NUMBER: _ClassVar[int]
    CHAN13_RAW_RC_CHANNELS_FIELD_NUMBER: _ClassVar[int]
    CHAN14_RAW_RC_CHANNELS_FIELD_NUMBER: _ClassVar[int]
    CHAN15_RAW_RC_CHANNELS_FIELD_NUMBER: _ClassVar[int]
    CHAN16_RAW_RC_CHANNELS_FIELD_NUMBER: _ClassVar[int]
    roll_ATTITUDE: float
    pitch_ATTITUDE: float
    yaw_ATTITUDE: float
    xacc_RAW_IMU: int
    yacc_RAW_IMU: int
    zacc_RAW_IMU: int
    xgyro_RAW_IMU: int
    ygyro_RAW_IMU: int
    zgyro_RAW_IMU: int
    xmag_RAW_IMU: int
    ymag_RAW_IMU: int
    zmag_RAW_IMU: int
    vibration_x_VIBRATION: float
    vibration_y_VIBRATION: float
    vibration_z_VIBRATION: float
    accel_cal_x_SENSOR_OFFSETS: float
    accel_cal_y_SENSOR_OFFSETS: float
    accel_cal_z_SENSOR_OFFSETS: float
    mag_ofs_x_SENSOR_OFFSETS: int
    mag_ofs_y_SENSOR_OFFSETS: int
    vx_GLOBAL_POSITION_INT: int
    vy_GLOBAL_POSITION_INT: int
    x_LOCAL_POSITION_NED: float
    vx_LOCAL_POSITION_NED: float
    vy_LOCAL_POSITION_NED: float
    nav_pitch_NAV_CONTROLLER_OUTPUT: float
    nav_bearing_NAV_CONTROLLER_OUTPUT: int
    servo3_raw_SERVO_OUTPUT_RAW: int
    servo8_raw_SERVO_OUTPUT_RAW: int
    groundspeed_VFR_HUD: float
    airspeed_VFR_HUD: float
    press_abs_SCALED_PRESSURE: float
    Vservo_POWER_STATUS: int
    voltages1_BATTERY_STATUS: float
    chancount_RC_CHANNELS: int
    chan12_raw_RC_CHANNELS: int
    chan13_raw_RC_CHANNELS: int
    chan14_raw_RC_CHANNELS: int
    chan15_raw_RC_CHANNELS: int
    chan16_raw_RC_CHANNELS: int
    def __init__(self, roll_ATTITUDE: _Optional[float] = ..., pitch_ATTITUDE: _Optional[float] = ..., yaw_ATTITUDE: _Optional[float] = ..., xacc_RAW_IMU: _Optional[int] = ..., yacc_RAW_IMU: _Optional[int] = ..., zacc_RAW_IMU: _Optional[int] = ..., xgyro_RAW_IMU: _Optional[int] = ..., ygyro_RAW_IMU: _Optional[int] = ..., zgyro_RAW_IMU: _Optional[int] = ..., xmag_RAW_IMU: _Optional[int] = ..., ymag_RAW_IMU: _Optional[int] = ..., zmag_RAW_IMU: _Optional[int] = ..., vibration_x_VIBRATION: _Optional[float] = ..., vibration_y_VIBRATION: _Optional[float] = ..., vibration_z_VIBRATION: _Optional[float] = ..., accel_cal_x_SENSOR_OFFSETS: _Optional[float] = ..., accel_cal_y_SENSOR_OFFSETS: _Optional[float] = ..., accel_cal_z_SENSOR_OFFSETS: _Optional[float] = ..., mag_ofs_x_SENSOR_OFFSETS: _Optional[int] = ..., mag_ofs_y_SENSOR_OFFSETS: _Optional[int] = ..., vx_GLOBAL_POSITION_INT: _Optional[int] = ..., vy_GLOBAL_POSITION_INT: _Optional[int] = ..., x_LOCAL_POSITION_NED: _Optional[float] = ..., vx_LOCAL_POSITION_NED: _Optional[float] = ..., vy_LOCAL_POSITION_NED: _Optional[float] = ..., nav_pitch_NAV_CONTROLLER_OUTPUT: _Optional[float] = ..., nav_bearing_NAV_CONTROLLER_OUTPUT: _Optional[int] = ..., servo3_raw_SERVO_OUTPUT_RAW: _Optional[int] = ..., servo8_raw_SERVO_OUTPUT_RAW: _Optional[int] = ..., groundspeed_VFR_HUD: _Optional[float] = ..., airspeed_VFR_HUD: _Optional[float] = ..., press_abs_SCALED_PRESSURE: _Optional[float] = ..., Vservo_POWER_STATUS: _Optional[int] = ..., voltages1_BATTERY_STATUS: _Optional[float] = ..., chancount_RC_CHANNELS: _Optional[int] = ..., chan12_raw_RC_CHANNELS: _Optional[int] = ..., chan13_raw_RC_CHANNELS: _Optional[int] = ..., chan14_raw_RC_CHANNELS: _Optional[int] = ..., chan15_raw_RC_CHANNELS: _Optional[int] = ..., chan16_raw_RC_CHANNELS: _Optional[int] = ...) -> None: ...

class StatusResponse(_message.Message):
    __slots__ = ("DroneId", "PredictData", "WarningData")
    DRONEID_FIELD_NUMBER: _ClassVar[int]
    PREDICTDATA_FIELD_NUMBER: _ClassVar[int]
    WARNINGDATA_FIELD_NUMBER: _ClassVar[int]
    DroneId: str
    PredictData: GrpcPredictData
    WarningData: GrpcWarningData
    def __init__(self, DroneId: _Optional[str] = ..., PredictData: _Optional[_Union[GrpcPredictData, _Mapping]] = ..., WarningData: _Optional[_Union[GrpcWarningData, _Mapping]] = ...) -> None: ...

class GrpcPredictData(_message.Message):
    __slots__ = ("roll_ATTITUDE_PREDICT", "yaw_ATTITUDE_PREDICT", "pitch_ATTITUDE_PREDICT", "xacc_RAW_IMU_PREDICT", "yacc_RAW_IMU_PREDICT", "zacc_RAW_IMU_PREDICT", "xgyro_RAW_IMU_PREDICT", "ygyro_RAW_IMU_PREDICT", "zgyro_RAW_IMU_PREDICT", "xmag_RAW_IMU_PREDICT", "ymag_RAW_IMU_PREDICT", "zmag_RAW_IMU_PREDICT", "vibration_x_VIBRATION_PREDICT", "vibration_y_VIBRATION_PREDICT", "vibration_z_VIBRATION_PREDICT")
    ROLL_ATTITUDE_PREDICT_FIELD_NUMBER: _ClassVar[int]
    YAW_ATTITUDE_PREDICT_FIELD_NUMBER: _ClassVar[int]
    PITCH_ATTITUDE_PREDICT_FIELD_NUMBER: _ClassVar[int]
    XACC_RAW_IMU_PREDICT_FIELD_NUMBER: _ClassVar[int]
    YACC_RAW_IMU_PREDICT_FIELD_NUMBER: _ClassVar[int]
    ZACC_RAW_IMU_PREDICT_FIELD_NUMBER: _ClassVar[int]
    XGYRO_RAW_IMU_PREDICT_FIELD_NUMBER: _ClassVar[int]
    YGYRO_RAW_IMU_PREDICT_FIELD_NUMBER: _ClassVar[int]
    ZGYRO_RAW_IMU_PREDICT_FIELD_NUMBER: _ClassVar[int]
    XMAG_RAW_IMU_PREDICT_FIELD_NUMBER: _ClassVar[int]
    YMAG_RAW_IMU_PREDICT_FIELD_NUMBER: _ClassVar[int]
    ZMAG_RAW_IMU_PREDICT_FIELD_NUMBER: _ClassVar[int]
    VIBRATION_X_VIBRATION_PREDICT_FIELD_NUMBER: _ClassVar[int]
    VIBRATION_Y_VIBRATION_PREDICT_FIELD_NUMBER: _ClassVar[int]
    VIBRATION_Z_VIBRATION_PREDICT_FIELD_NUMBER: _ClassVar[int]
    roll_ATTITUDE_PREDICT: float
    yaw_ATTITUDE_PREDICT: float
    pitch_ATTITUDE_PREDICT: float
    xacc_RAW_IMU_PREDICT: float
    yacc_RAW_IMU_PREDICT: float
    zacc_RAW_IMU_PREDICT: float
    xgyro_RAW_IMU_PREDICT: float
    ygyro_RAW_IMU_PREDICT: float
    zgyro_RAW_IMU_PREDICT: float
    xmag_RAW_IMU_PREDICT: float
    ymag_RAW_IMU_PREDICT: float
    zmag_RAW_IMU_PREDICT: float
    vibration_x_VIBRATION_PREDICT: float
    vibration_y_VIBRATION_PREDICT: float
    vibration_z_VIBRATION_PREDICT: float
    def __init__(self, roll_ATTITUDE_PREDICT: _Optional[float] = ..., yaw_ATTITUDE_PREDICT: _Optional[float] = ..., pitch_ATTITUDE_PREDICT: _Optional[float] = ..., xacc_RAW_IMU_PREDICT: _Optional[float] = ..., yacc_RAW_IMU_PREDICT: _Optional[float] = ..., zacc_RAW_IMU_PREDICT: _Optional[float] = ..., xgyro_RAW_IMU_PREDICT: _Optional[float] = ..., ygyro_RAW_IMU_PREDICT: _Optional[float] = ..., zgyro_RAW_IMU_PREDICT: _Optional[float] = ..., xmag_RAW_IMU_PREDICT: _Optional[float] = ..., ymag_RAW_IMU_PREDICT: _Optional[float] = ..., zmag_RAW_IMU_PREDICT: _Optional[float] = ..., vibration_x_VIBRATION_PREDICT: _Optional[float] = ..., vibration_y_VIBRATION_PREDICT: _Optional[float] = ..., vibration_z_VIBRATION_PREDICT: _Optional[float] = ...) -> None: ...

class GrpcWarningData(_message.Message):
    __slots__ = ("warning_count", "roll_ATTITUDE_WARNING", "pitch_ATTITUDE_WARNING", "yaw_ATTITUDE_WARNING", "xacc_RAW_IMU_WARNING", "yacc_RAW_IMU_WARNING", "zacc_RAW_IMU_WARNING", "xgyro_RAW_IMU_WARNING", "ygyro_RAW_IMU_WARNING", "zgyro_RAW_IMU_WARNING", "xmag_RAW_IMU_WARNING", "ymag_RAW_IMU_WARNING", "zmag_RAW_IMU_WARNING", "vibration_x_VIBRATION_WARNING", "vibration_y_VIBRATION_WARNING", "vibration_z_VIBRATION_WARNING")
    WARNING_COUNT_FIELD_NUMBER: _ClassVar[int]
    ROLL_ATTITUDE_WARNING_FIELD_NUMBER: _ClassVar[int]
    PITCH_ATTITUDE_WARNING_FIELD_NUMBER: _ClassVar[int]
    YAW_ATTITUDE_WARNING_FIELD_NUMBER: _ClassVar[int]
    XACC_RAW_IMU_WARNING_FIELD_NUMBER: _ClassVar[int]
    YACC_RAW_IMU_WARNING_FIELD_NUMBER: _ClassVar[int]
    ZACC_RAW_IMU_WARNING_FIELD_NUMBER: _ClassVar[int]
    XGYRO_RAW_IMU_WARNING_FIELD_NUMBER: _ClassVar[int]
    YGYRO_RAW_IMU_WARNING_FIELD_NUMBER: _ClassVar[int]
    ZGYRO_RAW_IMU_WARNING_FIELD_NUMBER: _ClassVar[int]
    XMAG_RAW_IMU_WARNING_FIELD_NUMBER: _ClassVar[int]
    YMAG_RAW_IMU_WARNING_FIELD_NUMBER: _ClassVar[int]
    ZMAG_RAW_IMU_WARNING_FIELD_NUMBER: _ClassVar[int]
    VIBRATION_X_VIBRATION_WARNING_FIELD_NUMBER: _ClassVar[int]
    VIBRATION_Y_VIBRATION_WARNING_FIELD_NUMBER: _ClassVar[int]
    VIBRATION_Z_VIBRATION_WARNING_FIELD_NUMBER: _ClassVar[int]
    warning_count: int
    roll_ATTITUDE_WARNING: bool
    pitch_ATTITUDE_WARNING: bool
    yaw_ATTITUDE_WARNING: bool
    xacc_RAW_IMU_WARNING: bool
    yacc_RAW_IMU_WARNING: bool
    zacc_RAW_IMU_WARNING: bool
    xgyro_RAW_IMU_WARNING: bool
    ygyro_RAW_IMU_WARNING: bool
    zgyro_RAW_IMU_WARNING: bool
    xmag_RAW_IMU_WARNING: bool
    ymag_RAW_IMU_WARNING: bool
    zmag_RAW_IMU_WARNING: bool
    vibration_x_VIBRATION_WARNING: bool
    vibration_y_VIBRATION_WARNING: bool
    vibration_z_VIBRATION_WARNING: bool
    def __init__(self, warning_count: _Optional[int] = ..., roll_ATTITUDE_WARNING: bool = ..., pitch_ATTITUDE_WARNING: bool = ..., yaw_ATTITUDE_WARNING: bool = ..., xacc_RAW_IMU_WARNING: bool = ..., yacc_RAW_IMU_WARNING: bool = ..., zacc_RAW_IMU_WARNING: bool = ..., xgyro_RAW_IMU_WARNING: bool = ..., ygyro_RAW_IMU_WARNING: bool = ..., zgyro_RAW_IMU_WARNING: bool = ..., xmag_RAW_IMU_WARNING: bool = ..., ymag_RAW_IMU_WARNING: bool = ..., zmag_RAW_IMU_WARNING: bool = ..., vibration_x_VIBRATION_WARNING: bool = ..., vibration_y_VIBRATION_WARNING: bool = ..., vibration_z_VIBRATION_WARNING: bool = ...) -> None: ...
