from flask_pymongo import PyMongo
from pymongo import DESCENDING

from .config import Config
from . import app

# 첫 번째 MongoDB에 대한 PyMongo 인스턴스 생성
app.config['MONGO_URI_DB1'] = Config.MONGO_URI_DB1  # config.py에서 URI 설정 가져오기
mongodb = PyMongo(app, uri=app.config['MONGO_URI_DB1'])

# 두 번째 MongoDB에 대한 PyMongo 인스턴스 생성
app.config['MONGO_URI_DB2'] = Config.MONGO_URI_DB2  # config.py에서 URI 설정 가져오기
mongo_db2 = PyMongo(app, uri=app.config['MONGO_URI_DB2'])

class Drone:
    @staticmethod
    def get_drones():
        ai_drones_cursor = mongodb.db.ai_drone.find()
        ai_drones_list = list(ai_drones_cursor)
        return ai_drones_list

    @staticmethod
    def all_devices_id():
        cursor = mongodb.db.ai_drone.distinct('device_id')
        all_devices_list = list(cursor)
        return all_devices_list

    @staticmethod
    def all_flights_id():
        cursor = mongodb.db.ai_drone.distinct('flight_id')
        all_flights_list = list(cursor)
        return all_flights_list

    @staticmethod
    def get_realtime(DroneId):
        query = {
            "device_id": DroneId
        }
        cursor = mongodb.db.ai_drone.find_one(query, sort=[('server_response_timestamp', DESCENDING)])

        def transfer_data(obj):
            return {
                '_id': obj['_id'],
                'DroneId': obj['device_id'],
                'SensorData': {
                    'roll_ATTITUDE': float(obj['roll_ATTITUDE']),
                    'pitch_ATTITUDE': float(obj['pitch_ATTITUDE']),
                    'yaw_ATTITUDE': float(obj['yaw_ATTITUDE']),
                    'xacc_RAW_IMU': obj['xacc_RAW_IMU'],
                    'yacc_RAW_IMU': obj['yacc_RAW_IMU'],
                    'zacc_RAW_IMU': obj['zacc_RAW_IMU'],
                    'xgyro_RAW_IMU': obj['xgyro_RAW_IMU'],
                    'ygyro_RAW_IMU': obj['ygyro_RAW_IMU'],
                    'zgyro_RAW_IMU': obj['zgyro_RAW_IMU'],
                    'xmag_RAW_IMU': obj['xmag_RAW_IMU'],
                    'ymag_RAW_IMU': obj['ymag_RAW_IMU'],
                    'zmag_RAW_IMU': obj['zmag_RAW_IMU'],
                    'vibration_x_VIBRATION': float(obj['vibration_x_VIBRATION']),
                    'vibration_y_VIBRATION': float(obj['vibration_y_VIBRATION']),
                    'vibration_z_VIBRATION': float(obj['vibration_z_VIBRATION']),
                },
                'PredictData': {
                    'roll_ATTITUDE_PREDICT': float(obj['roll_ATTITUDE_predict']),
                    'pitch_ATTITUDE_PREDICT': float(obj['pitch_ATTITUDE_predict']),
                    'yaw_ATTITUDE_PREDICT': float(obj['yaw_ATTITUDE_predict']),
                    'xacc_RAW_IMU_PREDICT': float(obj['xacc_RAW_IMU_predict']),
                    'yacc_RAW_IMU_PREDICT': float(obj['yacc_RAW_IMU_predict']),
                    'zacc_RAW_IMU_PREDICT': float(obj['zacc_RAW_IMU_predict']),
                    'xgyro_RAW_IMU_PREDICT': float(obj['xgyro_RAW_IMU_predict']),
                    'ygyro_RAW_IMU_PREDICT': float(obj['ygyro_RAW_IMU_predict']),
                    'zgyro_RAW_IMU_PREDICT': float(obj['zgyro_RAW_IMU_predict']),
                    'xmag_RAW_IMU_PREDICT': float(obj['xmag_RAW_IMU_predict']),
                    'ymag_RAW_IMU_PREDICT': float(obj['ymag_RAW_IMU_predict']),
                    'zmag_RAW_IMU_PREDICT': float(obj['zmag_RAW_IMU_predict']),
                    'vibration_x_VIBRATION_PREDICT': float(obj['vibration_x_VIBRATION_predict']),
                    'vibration_y_VIBRATION_PREDICT': float(obj['vibration_y_VIBRATION_predict']),
                    'vibration_z_VIBRATION_PREDICT': float(obj['vibration_z_VIBRATION_predict']),
                },
            }
        if cursor:
            realtime_data = transfer_data(cursor)
            return realtime_data

    @staticmethod
    def get_alt(DroneId):
        if DroneId == "12345678CD":
            DroneId = "1"
        query = {
            'DroneId': DroneId
        }
        sort_order = [("LastHeartbeatMessage", -1)]
        cursor = mongo_db2.db.Drone.find(query).sort(sort_order)
        temporal_list = list(cursor)
        def transfer_data(obj):
            return {
                'Alt': obj['DroneRawState']['DR_ALT']
            }
        temporal_list = list(map(transfer_data, temporal_list))

        return temporal_list


    @staticmethod
    def get_logdata(DroneId, FlightId, periodFrom=None, periodTo=None):
        # MongoDB FieldName 변경시 아래 query만 수정해주면 된다.
        print('들어온 요청', DroneId, FlightId, periodFrom, periodTo)
        query = {
            "$and": [
                {"device_id": DroneId},
                {"flight_id": int(FlightId)},
                {"server_response_timestamp": {
                    "$gte": periodFrom,
                    "$lte": periodTo
                }}
            ]
        }
        sort_order = [("server_response_timestamp", -1)]
        cursor = mongodb.db.ai_drone.find(query).sort(sort_order)
        logdata_list = list(cursor)

        # MongoDB 구조 변경 전에 서버에서 데이터 수정
        def transfer_data(obj):
            return {
                '_id': obj['_id'],
                'DroneId': obj['device_id'],
                'FlightId': obj['flight_id'],
                'PredictTime': obj['server_response_timestamp'],
                'SensorData': {
                    'roll_ATTITUDE': float(obj['roll_ATTITUDE']),
                    'pitch_ATTITUDE': float(obj['pitch_ATTITUDE']),
                    'yaw_ATTITUDE': float(obj['yaw_ATTITUDE']),
                    'xacc_RAW_IMU': obj['xacc_RAW_IMU'],
                    'yacc_RAW_IMU': obj['yacc_RAW_IMU'],
                    'zacc_RAW_IMU': obj['zacc_RAW_IMU'],
                    'xgyro_RAW_IMU': obj['xgyro_RAW_IMU'],
                    'ygyro_RAW_IMU': obj['ygyro_RAW_IMU'],
                    'zgyro_RAW_IMU': obj['zgyro_RAW_IMU'],
                    'xmag_RAW_IMU': obj['xmag_RAW_IMU'],
                    'ymag_RAW_IMU': obj['ymag_RAW_IMU'],
                    'zmag_RAW_IMU': obj['zmag_RAW_IMU'],
                    'vibration_x_VIBRATION': float(obj['vibration_x_VIBRATION']),
                    'vibration_y_VIBRATION': float(obj['vibration_y_VIBRATION']),
                    'vibration_z_VIBRATION': float(obj['vibration_z_VIBRATION']),
                    'accel_cal_x_SENSOR_OFFSETS': float(obj['accel_cal_x_SENSOR_OFFSETS']),
                    'accel_cal_y_SENSOR_OFFSETS': float(obj['accel_cal_y_SENSOR_OFFSETS']),
                    'accel_cal_z_SENSOR_OFFSETS': float(obj['accel_cal_z_SENSOR_OFFSETS']),
                    'mag_ofs_x_SENSOR_OFFSETS': obj['mag_ofs_x_SENSOR_OFFSETS'],
                    'mag_ofs_y_SENSOR_OFFSETS': obj['mag_ofs_y_SENSOR_OFFSETS'],
                    'vx_GLOBAL_POSITION_INT': obj['vx_GLOBAL_POSITION_INT'],
                    'vy_GLOBAL_POSITION_INT': obj['vy_GLOBAL_POSITION_INT'],
                    'x_LOCAL_POSITION_NED': obj['x_LOCAL_POSITION_NED'],
                    'vx_LOCAL_POSITION_NED': obj['vx_LOCAL_POSITION_NED'],
                    'vy_LOCAL_POSITION_NED': obj['vy_LOCAL_POSITION_NED'],
                    'nav_pitch_NAV_CONTROLLER_OUTPUT': float(obj['nav_pitch_NAV_CONTROLLER_OUTPUT']),
                    'nav_bearing_NAV_CONTROLLER_OUTPUT': obj['nav_bearing_NAV_CONTROLLER_OUTPUT'],
                    'servo3_raw_SERVO_OUTPUT_RAW': obj['servo3_raw_SERVO_OUTPUT_RAW'],
                    'servo8_raw_SERVO_OUTPUT_RAW': obj['servo8_raw_SERVO_OUTPUT_RAW'],
                    'groundspeed_VFR_HUD': float(obj['groundspeed_VFR_HUD']),
                    'airspeed_VFR_HUD': obj['airspeed_VFR_HUD'],
                    'press_abs_SCALED_PRESSURE': float(obj['press_abs_SCALED_PRESSURE']),
                    'Vservo_POWER_STATUS': obj['Vservo_POWER_STATUS'],
                    'voltages1_BATTERY_STATUS': obj['voltages1_BATTERY_STATUS'],
                    'chancount_RC_CHANNELS': obj['chancount_RC_CHANNELS'],
                    'chan12_raw_RC_CHANNELS': obj['chan12_raw_RC_CHANNELS'],
                    'chan13_raw_RC_CHANNELS': obj['chan13_raw_RC_CHANNELS'],
                    'chan14_raw_RC_CHANNELS': obj['chan14_raw_RC_CHANNELS'],
                    'chan15_raw_RC_CHANNELS': obj['chan15_raw_RC_CHANNELS'],
                    'chan16_raw_RC_CHANNELS': obj['chan16_raw_RC_CHANNELS'],
                }
            }

        logdata_list = list(map(transfer_data, logdata_list))

        return logdata_list

    @staticmethod
    def get_predict(DroneId, FlightId, periodFrom=None, periodTo=None, SelectData=None):
        # MongoDB FieldName 변경시 아래 query만 수정해주면 된다.
        print('들어온 요청', DroneId, FlightId, periodFrom, periodTo)
        query = {
            "$and": [
                {"device_id": DroneId},
                {"flight_id": int(FlightId)},
                {"server_response_timestamp": {
                    "$gte": periodFrom,
                    "$lte": periodTo
                }}
            ]
        }
        sort_order = [("server_response_timestamp", -1)]
        cursor = mongodb.db.ai_drone.find(query).sort(sort_order)
        predict_list = list(cursor)

        # MongoDB 구조 변경 전에 서버에서 데이터 수정
        def transfer_data(obj):
            return {
                '_id': obj['_id'],
                'DroneId': obj['device_id'],
                'FlightId': obj['flight_id'],
                'PredictTime': obj['server_response_timestamp'],
                'SelectData': obj[f'{SelectData}'],
                # _predict 나중에 _PREDICT로 교체
                'PredictData': obj[f'{SelectData}_predict'],
                'SensorData': {
                    'roll_ATTITUDE': float(obj['roll_ATTITUDE']),
                    'pitch_ATTITUDE': float(obj['pitch_ATTITUDE']),
                    'yaw_ATTITUDE': float(obj['yaw_ATTITUDE']),
                    'xacc_RAW_IMU': obj['xacc_RAW_IMU'],
                    'yacc_RAW_IMU': obj['yacc_RAW_IMU'],
                    'zacc_RAW_IMU': obj['zacc_RAW_IMU'],
                    'xgyro_RAW_IMU': obj['xgyro_RAW_IMU'],
                    'ygyro_RAW_IMU': obj['ygyro_RAW_IMU'],
                    'zgyro_RAW_IMU': obj['zgyro_RAW_IMU'],
                    'xmag_RAW_IMU': obj['xmag_RAW_IMU'],
                    'ymag_RAW_IMU': obj['ymag_RAW_IMU'],
                    'zmag_RAW_IMU': obj['zmag_RAW_IMU'],
                    'vibration_x_VIBRATION': float(obj['vibration_x_VIBRATION']),
                    'vibration_y_VIBRATION': float(obj['vibration_y_VIBRATION']),
                    'vibration_z_VIBRATION': float(obj['vibration_z_VIBRATION']),
                    'accel_cal_x_SENSOR_OFFSETS': float(obj['accel_cal_x_SENSOR_OFFSETS']),
                    'accel_cal_y_SENSOR_OFFSETS': float(obj['accel_cal_y_SENSOR_OFFSETS']),
                    'accel_cal_z_SENSOR_OFFSETS': float(obj['accel_cal_z_SENSOR_OFFSETS']),
                    'mag_ofs_x_SENSOR_OFFSETS': obj['mag_ofs_x_SENSOR_OFFSETS'],
                    'mag_ofs_y_SENSOR_OFFSETS': obj['mag_ofs_y_SENSOR_OFFSETS'],
                    'vx_GLOBAL_POSITION_INT': obj['vx_GLOBAL_POSITION_INT'],
                    'vy_GLOBAL_POSITION_INT': obj['vy_GLOBAL_POSITION_INT'],
                    'x_LOCAL_POSITION_NED': obj['x_LOCAL_POSITION_NED'],
                    'vx_LOCAL_POSITION_NED': obj['vx_LOCAL_POSITION_NED'],
                    'vy_LOCAL_POSITION_NED': obj['vy_LOCAL_POSITION_NED'],
                    'nav_pitch_NAV_CONTROLLER_OUTPUT': float(obj['nav_pitch_NAV_CONTROLLER_OUTPUT']),
                    'nav_bearing_NAV_CONTROLLER_OUTPUT': obj['nav_bearing_NAV_CONTROLLER_OUTPUT'],
                    'servo3_raw_SERVO_OUTPUT_RAW': obj['servo3_raw_SERVO_OUTPUT_RAW'],
                    'servo8_raw_SERVO_OUTPUT_RAW': obj['servo8_raw_SERVO_OUTPUT_RAW'],
                    'groundspeed_VFR_HUD': float(obj['groundspeed_VFR_HUD']),
                    'airspeed_VFR_HUD': obj['airspeed_VFR_HUD'],
                    'press_abs_SCALED_PRESSURE': float(obj['press_abs_SCALED_PRESSURE']),
                    'Vservo_POWER_STATUS': obj['Vservo_POWER_STATUS'],
                    'voltages1_BATTERY_STATUS': obj['voltages1_BATTERY_STATUS'],
                    'chancount_RC_CHANNELS': obj['chancount_RC_CHANNELS'],
                    'chan12_raw_RC_CHANNELS': obj['chan12_raw_RC_CHANNELS'],
                    'chan13_raw_RC_CHANNELS': obj['chan13_raw_RC_CHANNELS'],
                    'chan14_raw_RC_CHANNELS': obj['chan14_raw_RC_CHANNELS'],
                    'chan15_raw_RC_CHANNELS': obj['chan15_raw_RC_CHANNELS'],
                    'chan16_raw_RC_CHANNELS': obj['chan16_raw_RC_CHANNELS'],
                },
            }

        predict_list = list(map(transfer_data, predict_list))

        return predict_list

