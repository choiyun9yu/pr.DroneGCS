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
- Parsing MAVLink message to Drone State with MAVSDK
- Send Drone State from .NET Server to React.js with SignalR
- Send Drone State from .NET Server to Flask Server with Socket.IO
- Convert Mssion message to MAVLink message with MAVSDK
- Send MAVLink message from .NET Server to Ardupilot with dotNetty