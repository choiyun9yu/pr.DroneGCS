# Back-end Source Code

## 1. Start .NET Application

    % cd gcs-system
   
    % dotnet restore

    % dotnet run

## 2. Project Directory Structure

    gcs-system/
    ├── Controller/                   
    │   └── ApiController.cs
    ├── Interfaces/                   
    │   ├── Helper/
    │   │   └── FixedSizedQueue.cs
    │   ├── DroneState.cs
    │   ├── IDroneControl.cs
    │   └── Joystick.cs
    ├── MAVSDK/                   
    │   ├── mavlink.cs
    │   ├── MavlinkCRC.cs
    │   ├── MAVLinkMessage.cs
    │   ├── MavlinkParse.cs
    │   └── MavlinkUtil.cs
    ├── Models/         
    │   ├── AnomalyDetection.cs         
    │   ├── Dashboard.cs         
    │   └── Mission.cs       
    ├── protos/          
    │   └── drone.proto
    ├── Services/         
    │   ├── Helper/
    │   │   ├── GoogleMapHelper.cs
    │   │   ├── MavlinkDecoder.cs
    │   │   ├── MavlinkEncoder.cs
    │   │   ├── MavlinkHandler.cs
    │   │   ├── MavlinkMapper.cs
    │   │   ├── MavlinkNetty.cs
    │   │   └── VincentyCalculator.cs
    │   ├── AnomalyDetectionApiService.cs
    │   ├── ArduCopterControl.cs
    │   ├── DashboardApiService.cs
    │   └── GcsApiService.cs
    ├── Program.cs
    ├── Startup.cs
    ├── appsettings.json
    └── gcs-system.csproj

## 3. Create Project

      % dotnet new webapi -f net8.0 -o kisa-gcs-system
