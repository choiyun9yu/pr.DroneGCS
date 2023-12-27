
namespace kisa_gcs_system.Models;

[DataContract] 
public struct DroneGcs
{
    [DataMember] 
    public string? DroneId;
    
    [DataMember] 
    public string[]? DroneLogger;
    
    [DataMember] 
    public bool? IsOnline;
    
    [DataMember] 
    public bool? HasDeliverPlan;
    
    [DataMember] 
    public double[]? WayPointsDistance;
    
    [DataMember] 
    public DroneStt? DroneStt;
    
    [DataMember] 
    public DroneTrack? DroneTrack;
    
    [DataMember] 
    public DroneCamera? DroneCamera;
    
    [DataMember] 
    public DroneMission? DroneMission;
    
    [DataMember] 
    public CommunicationLink? CommunicationLink;

    public DroneGcs()
    {
        DroneId = "";
        
        DroneLogger = [];
        
        IsOnline = false;
        
        HasDeliverPlan = false;
        
        WayPointsDistance = [];
        
        DroneStt = new DroneStt();
        
        DroneTrack = new DroneTrack();
        
        DroneCamera = new DroneCamera();
        
        DroneMission = new DroneMission();
        
        CommunicationLink = new CommunicationLink();
    }
}

[DataContract]
public class DroneStt
{
    [DataMember] 
    public double? WayPointNum;
    
    [DataMember] 
    public double? PowerV;
    
    [DataMember] 
    public char? GpsStt;
    
    [DataMember]
    public double? TempC;
    
    [DataMember] 
    public char? LoaderLoad;
    
    [DataMember] 
    public char? LoaderLock;
    
    [DataMember] 
    public double? Lat;
    
    [DataMember] 
    public double? Lon;
    
    [DataMember] 
    public double? Alt;
    
    [DataMember] 
    public double? Speed;
    
    [DataMember] 
    public double? ROLL_ATTITUDE;
    
    [DataMember] 
    public double? PITCH_ATTITUDE;
    
    [DataMember] 
    public double? YAW_ATTITUDE;
    
    [DataMember] 
    public char? HoverStt;
    
    [DataMember] 
    public double? HODP;
    
    [DataMember] 
    public char? FlightMode;
    
    [DataMember] 
    public double? SatCount;
    
    [DataMember]
    public double? MabSysStt;
    
    [DataMember] 
    public SensorStt SensorStt;
    
    [DataMember] 
    public Mavlinkinfo Mavlinkinfo;

    public DroneStt()
    {
        WayPointNum = 0.0;
        
        PowerV = 0.0;
        
        GpsStt = ' ';
        
        TempC = 0.0;
        
        LoaderLoad = ' ';
        
        LoaderLock = ' ';
        
        Lat = 0.0;
        
        Lon = 0.0;
        
        Alt = 0.0;
        
        Speed = 0.0;
        
        ROLL_ATTITUDE = 0.0;
        
        PITCH_ATTITUDE = 0.0;
        
        YAW_ATTITUDE = 0.0;
        
        HoverStt = ' ';
        
        HODP = 0.0;
        
        FlightMode = ' ';
        
        SatCount = 0.0;
        
        MabSysStt = 0.0;
        
        SensorStt = new SensorStt();
        
        Mavlinkinfo = new Mavlinkinfo();
    }
}

[DataContract] 

public class SensorStt
{
    [DataMember] 
    public string? Name;
    
    [DataMember] 
    public bool? Enabled;
    
    [DataMember] 
    public bool? Present;
    
    [DataMember] 
    public bool? Health;
}

[DataContract] 
public class Mavlinkinfo
{
    [DataMember] 
    public string? FrameType;
    
    [DataMember] 
    public string? Ros;
    
    [DataMember] 
    public string? FC_HARDWAR;
    
    [DataMember] 
    public string? Autopilot;
    
    [DataMember] 
    public string? CommunicationOut;
}


[DataContract] 
public class DroneCamera
{
    [DataMember] 
    public char? FWD_CAM_STATE;
    
    [DataMember] 
    public string? CameraIp;
    
    [DataMember] 
    public string? CameraUrl1;
    
    [DataMember] 
    public string? CameraUrl2;
    
    [DataMember] 
    public string? CameraProtocolType;
}

[DataContract] 
public class DroneMission
{
    [DataMember] 
    public string? MavMission;
    
    [DataMember] 
    public DateTime? StartTime;
    
    [DataMember] 
    public DateTime? CompleteTime;
}

[DataContract] 
public class DroneTrack
{
    [DataMember] 
    public double? PathIndex;
    
    [DataMember] 
    public double[]? DroneTrails;
    
    [DataMember] 
    public double[]? DroneProgress;
    
    [DataMember] 
    public double[]? DroneProgressPresentation;
    
    [DataMember] 
    public double? TotalDistance;
    
    [DataMember] 
    public double? ElapsedDistance;
    
    [DataMember] 
    public double? RemainDistance;
    
}

[DataContract] 
public class CommunicationLink
{
    [DataMember] 
    public double? ConnectionProtocol;
    
    [DataMember] 
    public double? MessageProtocol;
    
    [DataMember] 
    public string? Address;
}