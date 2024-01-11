import json
from datetime import datetime
def DroneMessagePaser(data):
    droneMessage= json.loads(data[0])
    PredictTime= datetime.now()
    DroneId= droneMessage['DroneId']
    FlightId= 'none'
    Alt= droneMessage['DroneStt']['Alt']
    SensorData= droneMessage['SensorData']
    print(PredictTime, DroneId, FlightId, Alt, SensorData)