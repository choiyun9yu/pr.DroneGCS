# AI System

## 1. Start Python Application

    % cd predict-sytem

    % poetry shell

    % python run.py

## 2. Project Directory Structure

    predict-system/
    ├── ml/...
    ├── module/                   
    │   ├── drone_pb2.py
    │   ├── drone_pb2.pyi
    │   ├── drone_pb2_grpc.py
    │   ├── ml_helper.py
    │   ├── mongodb_helper.py
    │   └── signalr_connection_builder.py
    ├── protos/          
    │   └── drone.proto
    ├── poetroy.lock
    ├── pyproject.toml
    ├── requirements.txt
    └── run.py