from concurrent import futures
import logging
import json

# python -m grpc_tools.protoc -I=./protos --python_out=./generation --pyi_out=./generation --grpc_python_out=./generation ./protos/drone.proto

import grpc
from google.protobuf.json_format import MessageToDict

from generation import drone_pb2, drone_pb2_grpc

class Drone(drone_pb2_grpc.DroneStatusUpdateServicer):
    def UpdateDroneStatus(self, request, context):
        print(request)

        # dict = MessageToDict(request)

        # print(dict)

        return drone_pb2.StatusResponse(success=True)

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