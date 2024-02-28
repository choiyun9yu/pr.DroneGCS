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



def dict_to_df(dict_data):
    try:
        df = pd.DataFrame(dict_data)
        return df
    except Exception as e:
        print(e)

warnings.filterwarnings(action='ignore')

def predict(sensor_data):

    predict_data = {
        'roll_ATTITUDE_PREDICT': predict_roll_ATTITUDE(sensor_data),
        'yaw_ATTITUDE_PREDICT': predict_pitch_ATTITUDE(sensor_data),
        'pitch_ATTITUDE_PREDICT': predict_yaw_ATTITUDE(sensor_data),
        'xacc_RAW_IMU_PREDICT': predict_xacc_RAW_IMU(sensor_data),
        'yacc_RAW_IMU_PREDICT': predict_yacc_RAW_IMU(sensor_data),
        'zacc_RAW_IMU_PREDICT': predict_zacc_RAW_IMU(sensor_data),
        'xgyro_RAW_IMU_PREDICT': predict_xgyro_RAW_IMU(sensor_data),
        'ygyro_RAW_IMU_PREDICT': predict_ygyro_RAW_IMU(sensor_data),
        'zgyro_RAW_IMU_PREDICT': predict_zgyro_RAW_IMU(sensor_data),
        'xmag_RAW_IMU_PREDICT': predict_xmag_RAW_IMU(sensor_data),
        'ymag_RAW_IMU_PREDICT': predict_ymag_RAW_IMU(sensor_data),
        'zmag_RAW_IMU_PREDICT': predict_zmag_RAW_IMU(sensor_data),
        'vibration_x_VIBRATION_PREDICT': predict_vibration_x_VIBRATION(sensor_data),
        'vibration_y_VIBRATION_PREDICT': predict_vibration_y_VIBRATION(sensor_data),
        'vibration_z_VIBRATION_PREDICT': predict_vibration_z_VIBRATION(sensor_data),
    }
    roll_ATTITUDE_WARNING = True if (abs(sensor_data['roll_ATTITUDE'][0] - predict_data['roll_ATTITUDE_PREDICT']) - 0.519040579) > 0 else False
    pitch_ATTITUDE_WARNING = True if (abs(sensor_data['pitch_ATTITUDE'][0] - predict_data['pitch_ATTITUDE_PREDICT']) - 0.330952408) > 0 else False
    yaw_ATTITUDE_WARNING = True if (abs(sensor_data['yaw_ATTITUDE'][0] - predict_data['yaw_ATTITUDE_PREDICT']) - 4.426681577) > 0 else False
    xacc_RAW_IMU_WARNING = True if (abs(sensor_data['xacc_RAW_IMU'][0] - predict_data['xacc_RAW_IMU_PREDICT']) - 505.059753) > 0 else False
    yacc_RAW_IMU_WARNING = True if (abs(sensor_data['yacc_RAW_IMU'][0] - predict_data['yacc_RAW_IMU_PREDICT']) - 253.607117) > 0 else False
    zacc_RAW_IMU_WARNING = True if (abs(sensor_data['zacc_RAW_IMU'][0] - predict_data['zacc_RAW_IMU_PREDICT']) - 592.7912866) > 0 else False
    xgyro_RAW_IMU_WARNING = True if (abs(sensor_data['xgyro_RAW_IMU'][0] - predict_data['xgyro_RAW_IMU_PREDICT']) - 2419.060251) > 0 else False
    ygyro_RAW_IMU_WARNING = True if (abs(sensor_data['ygyro_RAW_IMU'][0] - predict_data['ygyro_RAW_IMU_PREDICT']) - 1655.598837) > 0 else False
    zgyro_RAW_IMU_WARNING = True if (abs(sensor_data['zgyro_RAW_IMU'][0] - predict_data['zgyro_RAW_IMU_PREDICT']) - 1430.775122) > 0 else False
    xmag_RAW_IMU_WARNING = True if (abs(sensor_data['xmag_RAW_IMU'][0] - predict_data['xmag_RAW_IMU_PREDICT']) - 432.8393278) > 0 else False
    ymag_RAW_IMU_WARNING = True if (abs(sensor_data['ymag_RAW_IMU'][0] - predict_data['ymag_RAW_IMU_PREDICT']) - 365.9837014) > 0 else False
    zmag_RAW_IMU_WARNING = True if (abs(sensor_data['zmag_RAW_IMU'][0] - predict_data['zmag_RAW_IMU_PREDICT']) - 566.2303151) > 0 else False
    vibration_x_VIBRATION_WARNING = True if (abs(sensor_data['vibration_x_VIBRATION'][0] - predict_data['vibration_x_VIBRATION_PREDICT']) - 4.997481432) > 0 else False
    vibration_y_VIBRATION_WARNING = True if (abs(sensor_data['vibration_y_VIBRATION'][0] - predict_data['vibration_y_VIBRATION_PREDICT']) - 3.934628874) > 0 else False
    vibration_z_VIBRATION_WARNING =True if (abs(sensor_data['vibration_z_VIBRATION'][0] - predict_data['vibration_z_VIBRATION_PREDICT']) - 9.405666076) > 0 else False

    warning_count = sum([
        roll_ATTITUDE_WARNING, pitch_ATTITUDE_WARNING, yaw_ATTITUDE_WARNING,
        xacc_RAW_IMU_WARNING, yacc_RAW_IMU_WARNING, zacc_RAW_IMU_WARNING,
        xgyro_RAW_IMU_WARNING, ygyro_RAW_IMU_WARNING, zgyro_RAW_IMU_WARNING,
        xmag_RAW_IMU_WARNING, ymag_RAW_IMU_WARNING, zmag_RAW_IMU_WARNING,
        vibration_x_VIBRATION_WARNING, vibration_y_VIBRATION_WARNING, vibration_z_VIBRATION_WARNING
    ])

    warning_data = {
        'warning_count': warning_count,
        'roll_ATTITUDE_WARNING': roll_ATTITUDE_WARNING,
        'pitch_ATTITUDE_WARNING': pitch_ATTITUDE_WARNING,
        'yaw_ATTITUDE_WARNING': yaw_ATTITUDE_WARNING,
        'xacc_RAW_IMU_WARNING': xacc_RAW_IMU_WARNING,
        'yacc_RAW_IMU_WARNING': yacc_RAW_IMU_WARNING,
        'zacc_RAW_IMU_WARNING': zacc_RAW_IMU_WARNING,
        'xgyro_RAW_IMU_WARNING': xgyro_RAW_IMU_WARNING,
        'ygyro_RAW_IMU_WARNING': ygyro_RAW_IMU_WARNING,
        'zgyro_RAW_IMU_WARNING': zgyro_RAW_IMU_WARNING,
        'xmag_RAW_IMU_WARNING': xmag_RAW_IMU_WARNING,
        'ymag_RAW_IMU_WARNING': ymag_RAW_IMU_WARNING,
        'zmag_RAW_IMU_WARNING': zmag_RAW_IMU_WARNING,
        'vibration_x_VIBRATION_WARNING': vibration_x_VIBRATION_WARNING,
        'vibration_y_VIBRATION_WARNING': vibration_y_VIBRATION_WARNING,
        'vibration_z_VIBRATION_WARNING': vibration_z_VIBRATION_WARNING
    }

    return predict_data, warning_data

def predict_roll_ATTITUDE(sensor_data):
    try:
        # pandas 데이터 프레임은 객체참조 이기 때문에 원본 데이터 프레임에도 영향을 미침
        X = sensor_data.copy()
        del X['roll_ATTITUDE']
        model = joblib.load('./ml/Gradient_boosting_regression_roll_ATTITUDE.pkl')
        roll_ATTITUDE_PREDCIT = model.predict(X)
        return roll_ATTITUDE_PREDCIT[0]
    except Exception as e:
        print(e)
def predict_pitch_ATTITUDE(sensor_data):
    X = sensor_data.copy()
    del X['pitch_ATTITUDE']
    model = joblib.load('./ml/Gradient_boosting_regression_pitch_ATTITUDE.pkl')
    pitch_ATTITUDE_PREDCIT = model.predict(X)
    return pitch_ATTITUDE_PREDCIT[0]

def predict_yaw_ATTITUDE(sensor_data):
    X = sensor_data.copy()
    del X['yaw_ATTITUDE']
    model = joblib.load('./ml/Gradient_boosting_regression_yaw_ATTITUDE.pkl')
    yaw_ATTITUDE_PREDCIT = model.predict(X)
    return yaw_ATTITUDE_PREDCIT[0]

def predict_xacc_RAW_IMU(sensor_data):
    X = sensor_data.copy()
    del X['xacc_RAW_IMU']
    model = joblib.load('./ml/Gradient_boosting_regression_xacc_RAW_IMU.pkl')
    xacc_RAW_IMU_PREDCIT = model.predict(X)
    return xacc_RAW_IMU_PREDCIT[0]

def predict_yacc_RAW_IMU(sensor_data):
    X = sensor_data.copy()
    del X['yacc_RAW_IMU']
    model = joblib.load('./ml/MLP_regression_yacc_RAW_IMU.pkl')
    yacc_RAW_IMU_PREDCIT = model.predict(X)
    return yacc_RAW_IMU_PREDCIT[0]

def predict_zacc_RAW_IMU(sensor_data):
    X = sensor_data.copy()
    del X['zacc_RAW_IMU']
    model = joblib.load('./ml/Gradient_boosting_regression_zacc_RAW_IMU.pkl')
    zacc_RAW_IMU_PREDCIT = model.predict(X)
    return zacc_RAW_IMU_PREDCIT[0]

def predict_xgyro_RAW_IMU(sensor_data):
    X = sensor_data.copy()
    del X['xgyro_RAW_IMU']
    model = joblib.load('./ml/Lasso_regression_xgyro_RAW_IMU.pkl')
    xgyro_RAW_IMU_PREDCIT = model.predict(X)
    return xgyro_RAW_IMU_PREDCIT[0]

def predict_ygyro_RAW_IMU(sensor_data):
    X = sensor_data.copy()
    del X['ygyro_RAW_IMU']
    model = joblib.load('./ml/Lasso_regression_ygyro_RAW_IMU.pkl')
    ygyro_RAW_IMU_PREDCIT = model.predict(X)
    return ygyro_RAW_IMU_PREDCIT[0]

def predict_zgyro_RAW_IMU(sensor_data):
    X = sensor_data.copy()
    del X['zgyro_RAW_IMU']
    model = joblib.load('./ml/Lasso_regression_zgyro_RAW_IMU.pkl')
    zgyro_RAW_IMU_PREDCIT = model.predict(X)
    return zgyro_RAW_IMU_PREDCIT[0]

def predict_xmag_RAW_IMU(sensor_data):
    X = sensor_data.copy()
    del X['xmag_RAW_IMU']
    model = joblib.load('./ml/MLP_regression_yacc_RAW_IMU.pkl')
    xmag_RAW_IMU_PREDCIT = model.predict(X)
    return xmag_RAW_IMU_PREDCIT[0]

def predict_ymag_RAW_IMU(sensor_data):
    X = sensor_data.copy()
    del X['ymag_RAW_IMU']
    model = joblib.load('./ml/Gradient_boosting_regression_ymag_RAW_IMU.pkl')
    ymag_RAW_IMU_PREDCIT = model.predict(X)
    return ymag_RAW_IMU_PREDCIT[0]

def predict_zmag_RAW_IMU(sensor_data):
    X = sensor_data.copy()
    del X['zmag_RAW_IMU']
    model = joblib.load('./ml/Gradient_boosting_regression_zmag_RAW_IMU.pkl')
    zmag_RAW_IMU_PREDCIT = model.predict(X)
    return zmag_RAW_IMU_PREDCIT[0]

def predict_vibration_x_VIBRATION(sensor_data):
    X = sensor_data.copy()
    del X['vibration_x_VIBRATION']
    model = joblib.load('./ml/Gradient_boosting_regression_vibration_x_VIBRATION.pkl')
    vibration_x_VIBRATION_PREDCIT = model.predict(X)
    return vibration_x_VIBRATION_PREDCIT[0]

def predict_vibration_y_VIBRATION(sensor_data):
    X = sensor_data.copy()
    del X['vibration_y_VIBRATION']
    model = joblib.load('./ml/Gradient_boosting_regression_vibration_y_VIBRATION.pkl')
    vibration_y_VIBRATION_PREDCIT = model.predict(X)
    return vibration_y_VIBRATION_PREDCIT[0]

def predict_vibration_z_VIBRATION(sensor_data):
    X = sensor_data.copy()
    del X['vibration_z_VIBRATION']
    model = joblib.load('./ml/MLP_regression_vibration_z_VIBRATION.pkl')
    vibration_z_VIBRATION_PREDCIT = model.predict(X)
    return vibration_z_VIBRATION_PREDCIT[0]
