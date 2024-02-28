from signalrcore.hub_connection_builder import HubConnectionBuilder
from datetime import datetime, timedelta

import asyncio

from .ml_helper import *
from .mongodb_helper import save_prediction_to_mongodb

last_save_time_dict = {}

async def HubConnection():
    hub_connection = HubConnectionBuilder() \
            .with_url("ws://localhost:5000/droneHub/") \
            .configure_logging(logging_level=30) \
            .with_automatic_reconnect({
                "type": "raw",
                "keep_alive_interval": 10,
                "reconnect_interval": 5,
                "max_attempts": 10
            }).build()

    close_event = asyncio.Event()

    hub_connection.on_open(lambda: print("connection opened and handshake received ready to send messages"))
    hub_connection.on_close(lambda: close_event.set())
    hub_connection.on_reconnect(lambda: print("reconnection in progress"))
    hub_connection.on("droneState", lambda data: pipeline(data[0]))

    hub_connection.start()

    try:
        await close_event.wait()
    finally:
        await hub_connection.stop()

def pipeline(data):
    global last_save_time_dict

    drone_id, flight_id, sensor_data = parser(data)

    if (last_save_time_dict.get(drone_id) is None):
        last_save_time_dict[f"{drone_id}"] = datetime.now()

    predict_time = datetime.now()

    if ((predict_time - last_save_time_dict[f"{drone_id}"]) > timedelta(seconds=1)):
        last_save_time_dict[f"{drone_id}"] = datetime.now()

        df = dict_to_df([sensor_data])
        predict_data, warning_data = predict(df)
        save_prediction_to_mongodb(drone_id, flight_id, sensor_data, predict_data, warning_data, predict_time)
