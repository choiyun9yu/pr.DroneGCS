import pandas as pd
import warnings
import joblib
import json

def parser(data):
    try:
        droneMessage = json.loads(data)
        DroneId = droneMessage["DroneId"]
        FlightId = droneMessage["FlightId"]
        SensorData = droneMessage["SensorData"]
        return DroneId, FlightId, SensorData
    except Exception as e:
        print("An error occurred while parsing JSON:", e)

def gRPCparser(data):
    try:
        DroneId = data["DroneId"],
        FlightID = data["FlightId"],
        SensorData = data["SensorData"],
        return DroneId, FlightID, SensorData
    except Exception as e:
        print("An error occurred while parsing")


def dict_to_df(dict_data):
    try:
        df = pd.DataFrame(dict_data)
        return df
    except Exception as e:
        print(e)

warnings.filterwarnings(action='ignore')

def predict(sensor_data):

    predict_data = {
        'rollATTITUDE_PREDICT': predict_roll_ATTITUDE(sensor_data),
        'yawATTITUDE_PREDICT': predict_pitch_ATTITUDE(sensor_data),
        'pitchATTITUDE_PREDICT': predict_yaw_ATTITUDE(sensor_data),
        'xaccRAWIMU_PREDICT': predict_xacc_RAW_IMU(sensor_data),
        'yaccRAWIMU_PREDICT': predict_yacc_RAW_IMU(sensor_data),
        'zaccRAWIMU_PREDICT': predict_zacc_RAW_IMU(sensor_data),
        'xgyroRAWIMU_PREDICT': predict_xgyro_RAW_IMU(sensor_data),
        'ygyroRAWIMU_PREDICT': predict_ygyro_RAW_IMU(sensor_data),
        'zgyroRAWIMU_PREDICT': predict_zgyro_RAW_IMU(sensor_data),
        'xmagRAWIMU_PREDICT': predict_xmag_RAW_IMU(sensor_data),
        'ymagRAWIMU_PREDICT': predict_ymag_RAW_IMU(sensor_data),
        'zmagRAWIMU_PREDICT': predict_zmag_RAW_IMU(sensor_data),
        'vibrationXVIBRATION_PREDICT': predict_vibration_x_VIBRATION(sensor_data),
        'vibrationYVIBRATION_PREDICT': predict_vibration_y_VIBRATION(sensor_data),
        'vibrationZVIBRATION_PREDICT': predict_vibration_z_VIBRATION(sensor_data),
    }
    rollATTITUDE_WARNING = True if (abs(sensor_data['rollATTITUDE'][0] - predict_data['rollATTITUDE_PREDICT']) - 0.519040579) > 0 else False
    pitchATTITUDE_WARNING = True if (abs(sensor_data['pitchATTITUDE'][0] - predict_data['pitchATTITUDE_PREDICT']) - 0.330952408) > 0 else False
    yawATTITUDE_WARNING = True if (abs(sensor_data['yawATTITUDE'][0] - predict_data['yawATTITUDE_PREDICT']) - 4.426681577) > 0 else False
    xaccRAWIMU_WARNING = True if (abs(sensor_data['xaccRAWIMU'][0] - predict_data['xaccRAWIMU_PREDICT']) - 505.059753) > 0 else False
    yaccRAWIMU_WARNING = True if (abs(sensor_data['yaccRAWIMU'][0] - predict_data['yaccRAWIMU_PREDICT']) - 253.607117) > 0 else False
    zaccRAWIMU_WARNING = True if (abs(sensor_data['zaccRAWIMU'][0] - predict_data['zaccRAWIMU_PREDICT']) - 592.7912866) > 0 else False
    xgyroRAWIMU_WARNING = True if (abs(sensor_data['xgyroRAWIMU'][0] - predict_data['xgyroRAWIMU_PREDICT']) - 2419.060251) > 0 else False
    ygyroRAWIMU_WARNING = True if (abs(sensor_data['ygyroRAWIMU'][0] - predict_data['ygyroRAWIMU_PREDICT']) - 1655.598837) > 0 else False
    zgyroRAWIMU_WARNING = True if (abs(sensor_data['zgyroRAWIMU'][0] - predict_data['zgyroRAWIMU_PREDICT']) - 1430.775122) > 0 else False
    xmagRAWIMU_WARNING = True if (abs(sensor_data['xmagRAWIMU'][0] - predict_data['xmagRAWIMU_PREDICT']) - 432.8393278) > 0 else False
    ymagRAWIMU_WARNING = True if (abs(sensor_data['ymagRAWIMU'][0] - predict_data['ymagRAWIMU_PREDICT']) - 365.9837014) > 0 else False
    zmagRAWIMU_WARNING = True if (abs(sensor_data['zmagRAWIMU'][0] - predict_data['zmagRAWIMU_PREDICT']) - 566.2303151) > 0 else False
    vibrationXVIBRATION_WARNING = True if (abs(sensor_data['vibrationXVIBRATION'][0] - predict_data['vibrationXVIBRATION_PREDICT']) - 4.997481432) > 0 else False
    vibrationYVIBRATION_WARNING = True if (abs(sensor_data['vibrationYVIBRATION'][0] - predict_data['vibrationYVIBRATION_PREDICT']) - 3.934628874) > 0 else False
    vibrationZVIBRATION_WARNING =True if (abs(sensor_data['vibrationZVIBRATION'][0] - predict_data['vibrationZVIBRATION_PREDICT']) - 9.405666076) > 0 else False

    warning_count = sum([
        rollATTITUDE_WARNING, pitchATTITUDE_WARNING, yawATTITUDE_WARNING,
        xaccRAWIMU_WARNING, yaccRAWIMU_WARNING, zaccRAWIMU_WARNING,
        xgyroRAWIMU_WARNING, ygyroRAWIMU_WARNING, zgyroRAWIMU_WARNING,
        xmagRAWIMU_WARNING, ymagRAWIMU_WARNING, zmagRAWIMU_WARNING,
        vibrationXVIBRATION_WARNING, vibrationYVIBRATION_WARNING, vibrationZVIBRATION_WARNING
    ])

    warning_data = {
        'warning_count': warning_count,
        'rollATTITUDE_WARNING': rollATTITUDE_WARNING,
        'pitchATTITUDE_WARNING': pitchATTITUDE_WARNING,
        'yawATTITUDE_WARNING': yawATTITUDE_WARNING,
        'xaccRAWIMU_WARNING': xaccRAWIMU_WARNING,
        'yaccRAWIMU_WARNING': yaccRAWIMU_WARNING,
        'zaccRAWIMU_WARNING': zaccRAWIMU_WARNING,
        'xgyroRAWIMU_WARNING': xgyroRAWIMU_WARNING,
        'ygyroRAWIMU_WARNING': ygyroRAWIMU_WARNING,
        'zgyroRAWIMU_WARNING': zgyroRAWIMU_WARNING,
        'xmagRAWIMU_WARNING': xmagRAWIMU_WARNING,
        'ymagRAWIMU_WARNING': ymagRAWIMU_WARNING,
        'zmagRAWIMU_WARNING': zmagRAWIMU_WARNING,
        'vibrationXVIBRATION_WARNING': vibrationXVIBRATION_WARNING,
        'vibrationYVIBRATION_WARNING': vibrationYVIBRATION_WARNING,
        'vibrationZVIBRATION_WARNING': vibrationZVIBRATION_WARNING
    }

    return predict_data, warning_data

def predict_roll_ATTITUDE(sensor_data):
    try:
        # pandas 데이터 프레임은 객체참조 이기 때문에 원본 데이터 프레임에도 영향을 미침
        X = sensor_data.copy()
        del X['rollATTITUDE']
        model = joblib.load('./ml/Gradient_boosting_regression_roll_ATTITUDE.pkl')
        roll_ATTITUDE_PREDCIT = model.predict(X)
        return roll_ATTITUDE_PREDCIT[0]
    except Exception as e:
        print(e)

def predict_pitch_ATTITUDE(sensor_data):
    X = sensor_data.copy()
    del X['pitchATTITUDE']
    model = joblib.load('./ml/Gradient_boosting_regression_pitch_ATTITUDE.pkl')
    pitch_ATTITUDE_PREDCIT = model.predict(X)
    return pitch_ATTITUDE_PREDCIT[0]

def predict_yaw_ATTITUDE(sensor_data):
    X = sensor_data.copy()
    del X['yawATTITUDE']
    model = joblib.load('./ml/Gradient_boosting_regression_yaw_ATTITUDE.pkl')
    yaw_ATTITUDE_PREDCIT = model.predict(X)
    return yaw_ATTITUDE_PREDCIT[0]

def predict_xacc_RAW_IMU(sensor_data):
    X = sensor_data.copy()
    del X['xaccRAWIMU']
    model = joblib.load('./ml/Gradient_boosting_regression_xacc_RAW_IMU.pkl')
    xacc_RAW_IMU_PREDCIT = model.predict(X)
    return xacc_RAW_IMU_PREDCIT[0]

def predict_yacc_RAW_IMU(sensor_data):
    X = sensor_data.copy()
    del X['yaccRAWIMU']
    model = joblib.load('./ml/MLP_regression_yacc_RAW_IMU.pkl')
    yacc_RAW_IMU_PREDCIT = model.predict(X)
    return yacc_RAW_IMU_PREDCIT[0]

def predict_zacc_RAW_IMU(sensor_data):
    X = sensor_data.copy()
    del X['zaccRAWIMU']
    model = joblib.load('./ml/Gradient_boosting_regression_zacc_RAW_IMU.pkl')
    zacc_RAW_IMU_PREDCIT = model.predict(X)
    return zacc_RAW_IMU_PREDCIT[0]

def predict_xgyro_RAW_IMU(sensor_data):
    X = sensor_data.copy()
    del X['xgyroRAWIMU']
    model = joblib.load('./ml/Lasso_regression_xgyro_RAW_IMU.pkl')
    xgyro_RAW_IMU_PREDCIT = model.predict(X)
    return xgyro_RAW_IMU_PREDCIT[0]

def predict_ygyro_RAW_IMU(sensor_data):
    X = sensor_data.copy()
    del X['ygyroRAWIMU']
    model = joblib.load('./ml/Lasso_regression_ygyro_RAW_IMU.pkl')
    ygyro_RAW_IMU_PREDCIT = model.predict(X)
    return ygyro_RAW_IMU_PREDCIT[0]

def predict_zgyro_RAW_IMU(sensor_data):
    X = sensor_data.copy()
    del X['zgyroRAWIMU']
    model = joblib.load('./ml/Lasso_regression_zgyro_RAW_IMU.pkl')
    zgyro_RAW_IMU_PREDCIT = model.predict(X)
    return zgyro_RAW_IMU_PREDCIT[0]

def predict_xmag_RAW_IMU(sensor_data):
    X = sensor_data.copy()
    del X['xmagRAWIMU']
    model = joblib.load('./ml/MLP_regression_yacc_RAW_IMU.pkl')
    xmag_RAW_IMU_PREDCIT = model.predict(X)
    return xmag_RAW_IMU_PREDCIT[0]

def predict_ymag_RAW_IMU(sensor_data):
    X = sensor_data.copy()
    del X['ymagRAWIMU']
    model = joblib.load('./ml/Gradient_boosting_regression_ymag_RAW_IMU.pkl')
    ymag_RAW_IMU_PREDCIT = model.predict(X)
    return ymag_RAW_IMU_PREDCIT[0]

def predict_zmag_RAW_IMU(sensor_data):
    X = sensor_data.copy()
    del X['zmagRAWIMU']
    model = joblib.load('./ml/Gradient_boosting_regression_zmag_RAW_IMU.pkl')
    zmag_RAW_IMU_PREDCIT = model.predict(X)
    return zmag_RAW_IMU_PREDCIT[0]

def predict_vibration_x_VIBRATION(sensor_data):
    X = sensor_data.copy()
    del X['vibrationXVIBRATION']
    model = joblib.load('./ml/Gradient_boosting_regression_vibration_x_VIBRATION.pkl')
    vibration_x_VIBRATION_PREDCIT = model.predict(X)
    return vibration_x_VIBRATION_PREDCIT[0]

def predict_vibration_y_VIBRATION(sensor_data):
    X = sensor_data.copy()
    del X['vibrationYVIBRATION']
    model = joblib.load('./ml/Gradient_boosting_regression_vibration_y_VIBRATION.pkl')
    vibration_y_VIBRATION_PREDCIT = model.predict(X)
    return vibration_y_VIBRATION_PREDCIT[0]

def predict_vibration_z_VIBRATION(sensor_data):
    X = sensor_data.copy()
    del X['vibrationZVIBRATION']
    model = joblib.load('./ml/MLP_regression_vibration_z_VIBRATION.pkl')
    vibration_z_VIBRATION_PREDCIT = model.predict(X)
    return vibration_z_VIBRATION_PREDCIT[0]
