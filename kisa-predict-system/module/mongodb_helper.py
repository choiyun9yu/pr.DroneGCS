from pymongo import MongoClient
from datetime import datetime, timedelta

last_save_time = datetime.now()

def save_prediction_to_mongodb(drone_id, flight_id, alt, sensor_data, predict_data):
    global last_save_time
    predict_time = datetime.now()

    if ((predict_time - last_save_time) > timedelta(seconds=1)):

        last_save_time = datetime.now()

        client = MongoClient("mongodb://localhost:27017/")
        db = client["drone"]
        collection = db['drone_predict']

        data_to_insert = {
            "PredictTime": predict_time,
            "DroneId": drone_id,
            "FlightId": flight_id,
            # "Alt": alt,
            "SensorData": sensor_data,
            "PredictData": predict_data
        }

        result = collection.insert_one(data_to_insert)

        if (result.inserted_id is not None):
            print(data_to_insert)
        else:
            print("can't save")

        client.close()
