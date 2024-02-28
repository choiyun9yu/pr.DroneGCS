from pymongo import MongoClient

def save_prediction_to_mongodb(drone_id, flight_id, sensor_data, predict_data, warning_data, predict_time):

    client = MongoClient("mongodb://localhost:27017/")
    db = client["drone"]
    collection = db['drone_predict']

    data_to_insert = {
        "PredictTime": predict_time,
        "DroneId": drone_id,
        "FlightId": flight_id,
        "SensorData": sensor_data,
        "PredictData": predict_data,
        "WarningData": warning_data
    }

    collection.insert_one(data_to_insert)

    # result = collection.insert_one(data_to_insert)

    # if (result.inserted_id is not None):
    #     print(data_to_insert)
    # else:
    #     print("can't save")

    client.close()
