# GCS Service

## 1. Project Architecture

    kisa-gcs-service/
    │
    ├── kisa-gcs-service.csproj
    ├── Controllers/            
    │   ├── DroneController.cs
    ├── Hubs/                   
    │   ├── DroneHub.cs              
    ├── Models/                 
    │   ├── DroneModel.cs              
    ├── Services/               
    │   ├── DroneService.cs
    ├── Startup.cs
    └── Program.cs              

## 2. Dependency management
kisa-gcs-service.csproj 파일만으로 실행되지 않는 경우

    % dotnet add package MongoDB.Driver
    % dotnet add package MongoDB.Bson
    % dotnet add package Newtonsoft.Json
    % dotnet add package Microsoft.AspNetCore.Cors  // CORS 정책을 위해 설치
    % dotnet add package Microsoft.AspNetCore.SignalR   // SignalR 설치

## 3. Database
### [MongoDB Specification](https://docs.google.com/spreadsheets/d/1H0tCsqDfMZ2z4MZ82Cf29FznkBN-HQiu5DckXepfIy8/edit?usp=sharing)

## 4. URLs

### 4-1. GCS 관제 시스템: hostip:5000/api/drones

#### GET
> request: url

> response

    [
        DroneId, DroneId ...
    ]

#### POST
> request: form(DroneId)
 
> response

    {
        _id,
        lastHeartbeatMessage,
        droneId,
        droneLogger: [],
        isOnline,
        hasDeliveryPlan,
        waypointsDistance: [],
        droneRawState: {
            DR_ID,
            DR_STATE,
            DR_STATE_SUB,
            POWER_V,
            GPS_STATE,
            FWD_CAM_STATE,
            TEMPERATURE_C,
            LOADER_LOAD,
            LOADER_LOCK,
            WP_NO,
            DR_LAT,
            DR_LON,
            DR_ALT,
            DR_SPEED,
            HOVERING_STATE,
            DR_ROLL,
            DR_PITCH,
            DR_YAW,
            HDOP,
            FLIGHT_MODE,
            SAT_COUNT,
            MAV_SYS_STATUS,
            SENSOR_STATUSES: [],
            MAVLINK_INFO: {
                FrameType,
                ROS,
                FC_HARDWARE,
                Autopilot,
                CommunicationOut,
            }
        },
        pathIndex,
        droneTrails,
        droneProgress,
        droneProgressPercentages,
        elapsedDistance,
        remainDistance,
        totalDistance,
        cameraIP,
        cameraURL1,
        cameraURL,
        cameraProtocolType,
        mavMission,
        startTime,
        completeTime,
        communicationLink: {
            ConnectionProtocol,
            MessageProtocol,
            Address,
        },
        droneSpeed,
    }