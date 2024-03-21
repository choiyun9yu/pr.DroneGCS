# gRPC

## [Python] Generating client and server code 

    % sudo pip install --upgrade pip
    % pip install grpcio grpcio-tools

    // .proto 파일 작성

    // proto stub 생성 
    % python -m grpc_tools.protoc -I=./protos --python_out=./generation --pyi_out=./generation --grpc_python_out=./generation ./protos/drone.proto

## [C#] Generating client code
- .csproj 파일을 설정하고, protos 디렉토리 안에 .proto 파일을 작성한다. 
- 빌드 시 자동으로 obj 디렉토리에 생성된다.

    
    