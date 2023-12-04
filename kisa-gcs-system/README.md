# GCS Service

## 1. Project Directory Structure

    kisa-gcs-service/
    ├── Controllers/
    ├── Hubs/                   
    │   ├── SignalRHub.cs
    ├── MAVSDK/                   
    │   ├── mavlink.cs
    │   ├── MavlinkCRC.cs
    │   ├── MAVLinkMessage.cs
    │   ├── MavlinkParse.cs
    │   ├── MavlinkUtil.cs
    ├── Models/                 
    │   ├── Drone.cs              
    ├── Services/               
    │   ├── DroneServiceMavlink.cs
    │   ├── DroneState.cs
    │   ├── MavLinkUdpMEssageDecoder.cs
    ├── Program.cs
    ├── Startup.cs
    ├── appsettings.json
    └── kisa-gcs-service.csproj

## 2. Project Architecture
![img.png](img.png)

### 2-1. Undeveloped List
③ Parsing MAVLink message by MAVSDK 
⑧ Send mission message from React.js to .NET through SignalR
⑨ Convert mission message to MAVLink message by MAVSDK
⑩ Send MAVLink message from .NET to Ardupilot through DotNetty