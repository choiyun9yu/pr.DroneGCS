from concurrent import futures
from datetime import datetime, timedelta
import logging
import grpc
from google.protobuf.json_format import MessageToDict

from module import ml_helper, mongodb_helper, drone_pb2, drone_pb2_grpc

# python -m grpc_tools.protoc -I=./protos --python_out=./generation --pyi_out=./generation --grpc_python_out=./generation ./protos/drone.proto

last_save_time_dict = {}
class Drone(drone_pb2_grpc.DroneStatusUpdateServicer):
    def UpdateDroneStatus(self, request, context, catch=None):
        # print(request)
        global last_save_time_dict
        predict_time = datetime.now()

        drone_id, flight_id, sensor_data = ml_helper.gRPCparser(MessageToDict(request)["droneStatuses"][0])

        last_save_time = last_save_time_dict.get(str(drone_id))

        if last_save_time is None or (predict_time - last_save_time) > timedelta(seconds=1):
            last_save_time_dict[str(drone_id)] = predict_time
            df = ml_helper.dict_to_df([sensor_data[0]])
            predict_data, warning_data = ml_helper.predict(df)
            mongodb_helper.save_prediction_to_mongodb(drone_id[0], flight_id[0], sensor_data[0], predict_data, warning_data, predict_time)

            if warning_data["warning_count"] is not None:
                return drone_pb2.StatusResponse(
                    DroneId=drone_id[0],
                    PredictData=predict_data,
                    WarningData=warning_data,
                )

        return drone_pb2.StatusResponse()



def serve():
    port = "50051"
    server = grpc.server(futures.ThreadPoolExecutor(max_workers=10))
    drone_pb2_grpc.add_DroneStatusUpdateServicer_to_server(Drone(), server)
    server.add_insecure_port("[::]:" + port)
    server.start()
    print("Server started, listening on " + port)
    server.wait_for_termination()

if __name__ == "__main__":
    logging.basicConfig()
    serve()