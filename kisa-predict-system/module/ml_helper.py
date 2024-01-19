import pandas as pd
import warnings
import joblib
import json

def parser(data):
    droneMessage= json.loads(data[0])
    DroneId = droneMessage['DroneId']
    # FligthId = droneMessage['FligthId']
    FlightId= '123456'  # 임시
    SensorData = droneMessage['SensorData']
    return DroneId, FlightId, SensorData

def dict_to_df(dict_data):
    try:
        df = pd.DataFrame(dict_data)
        return df
    except Exception as e:
        print(e)

warnings.filterwarnings(action='ignore')

def predict(df):
    predict_data = {
        'roll_ATTITUDE_PREDICT': predict_roll_ATTITUDE(df),
        'yaw_ATTITUDE_PREDICT': predict_pitch_ATTITUDE(df),
        'pitch_ATTITUDE_PREDICT': predict_yaw_ATTITUDE(df),
        'xacc_RAW_IMU_PREDICT': predict_xacc_RAW_IMU(df),
        'yacc_RAW_IMU_PREDICT': predict_yacc_RAW_IMU(df),
        'zacc_RAW_IMU_PREDICT': predict_zacc_RAW_IMU(df),
        'xgyro_RAW_IMU_PREDICT': predict_xgyro_RAW_IMU(df),
        'ygyro_RAW_IMU_PREDICT': predict_ygyro_RAW_IMU(df),
        'zgyro_RAW_IMU_PREDICT': predict_zgyro_RAW_IMU(df),
        'xmag_RAW_IMU_PREDICT': predict_xmag_RAW_IMU(df),
        'ymag_RAW_IMU_PREDICT': predict_ymag_RAW_IMU(df),
        'zmag_RAW_IMU_PREDICT': predict_zmag_RAW_IMU(df),
        'vibration_x_VIBRATION_PREDICT': predict_vibration_x_VIBRATION(df),
        'vibration_y_VIBRATION_PREDICT': predict_vibration_y_VIBRATION(df),
        'vibration_z_VIBRATION_PREDICT': predict_vibration_z_VIBRATION(df),
    }
    # print(predict_data)
    return predict_data

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
