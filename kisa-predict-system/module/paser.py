import json
def DroneMessagePaser(data):
    droneMessage = json.loads(data[0])
    predictionMessage = droneMessage['SensorData']
    print(predictionMessage)