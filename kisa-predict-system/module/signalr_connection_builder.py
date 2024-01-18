from signalrcore.hub_connection_builder import HubConnectionBuilder
import asyncio
from .ml_helper import *

# async def HubConnection():
#     hub_connection = HubConnectionBuilder() \
#         .with_url("ws://localhost:5000/droneHub/") \
#         .configure_logging(logging_level=30) \
#         .with_automatic_reconnect({
#         "type": "raw",
#         "keep_alive_interval": 10,
#         "reconnect_interval": 5,
#         "max_attempts": 10
#     }).build()
#
#     close_event = asyncio.Event()
#
#     hub_connection.on_open(lambda: print("connection opened and handshake received ready to send messages"))
#     hub_connection.on_close(lambda: close_event.set())
#     hub_connection.on_reconnect(lambda: print("reconnection in progress"))
#     hub_connection.on("droneMessage", lambda data: asyncio.create_task(pipeline(data)))
#     hub_connection.start()
#
#     try:
#         await close_event.wait()
#     finally:
#         await hub_connection.stop()
#
# def pipeline(data):
#     drone_id, flight_id, sensor_data = parser(data)
#     df = dict_to_df([sensor_data])
#     predict_data = predict(df)



from signalrcore.hub_connection_builder import HubConnectionBuilder

import asyncio

from .ml_helper import *
from .mongodb_helper import save_prediction_to_mongodb

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
    hub_connection.on("droneMessage", lambda data: pipeline(data))
    hub_connection.start()

    try:
        await close_event.wait()
    finally:
        await hub_connection.stop()

def pipeline(data):
    drone_id, flight_id, sensor_data = parser(data)
    df = dict_to_df([sensor_data])
    predict_data = predict(df)
    save_prediction_to_mongodb(drone_id, flight_id, sensor_data, predict_data)
