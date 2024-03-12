from concurrent import futures
import logging

import grpc

from generation import drone_pb2, drone_pb2_grpc

class Drone(drone_pb2_grpc.DroneStatusUpdateServicer):
    def UpdateDroneStatus(self, request, context):
        print(request)

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