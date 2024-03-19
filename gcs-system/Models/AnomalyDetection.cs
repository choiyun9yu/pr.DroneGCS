using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace gcs_system.Models;

public class AnomalyDetection
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)] // MongoDB의 ObjectId를 문자열로 표현
    public string? _id { get; set; }
    public DateTime PredictTime { get; set; }
    public string? DroneId { get; set; }
    public string? FlightId { get; set; }
    public SensorData2? SensorData { get; set; }
    public PredictData? PredictData { get; set; }
    public WarningData? WarningData { get; set; }
}

public class SensorData2
{
    public double rollATTITUDE { get; set; }
    public double pitchATTITUDE { get; set; }
    public double yawATTITUDE { get; set; }
    public double xaccRAWIMU { get; set; }
    public double yaccRAWIMU { get; set; }
    public double zaccRAWIMU { get; set; }
    public double xgyroRAWIMU { get; set; }
    public double ygyroRAWIMU { get; set; }
    public double zgyroRAWIMU { get; set; }
    public double xmagRAWIMU { get; set; }
    public double ymagRAWIMU { get; set; }
    public double zmagRAWIMU { get; set; }
    public double vibrationXVIBRATION { get; set; }
    public double vibrationYVIBRATION { get; set; }
    public double vibrationZVIBRATION { get; set; }
    public double accelCalXSENSOROFFSETS { get; set; }
    public double accelCalYSENSOROFFSETS { get; set; }
    public double accelCalZSENSOROFFSETS { get; set; }
    public double magOfsXSENSOROFFSETS { get; set; }
    public double magOfsYSENSOROFFSETS { get; set; }
    public double vxGLOBALPOSITIONINT { get; set; }
    public double vyGLOBALPOSITIONINT { get; set; }
    public double xLOCALPOSITIONNED { get; set; }
    public double vxLOCALPOSITIONNED { get; set; }
    public double vyLOCALPOSITIONNED { get; set; }
    public double navPitchNAVCONTROLLEROUTPUT { get; set; }
    public double navBearingNAVCONTROLLEROUTPUT { get; set; }
    public double servo3RawSERVOOUTPUTRAW { get; set; }
    public double servo8RawSERVOOUTPUTRAW { get; set; }
    public double groundspeedVFRHUD { get; set; }
    public double airspeedVFRHUD { get; set; }
    public double pressAbsSCALEDPRESSURE { get; set; }
    public double VservoPOWERSTATUS { get; set; }
    public double voltages1BATTERYSTATUS { get; set; }
    public double chancountRCCHANNELS { get; set; }
    public double chan12RawRCCHANNELS { get; set; }
    public double chan13RawRCCHANNELS { get; set; }
    public double chan14RawRCCHANNELS { get; set; }
    public double chan15RawRCCHANNELS { get; set; }
    public double chan16RawRCCHANNELS { get; set; }
}

public class PredictData
{
    public double rollATTITUDE_PREDICT { get; set; }
    public double pitchATTITUDE_PREDICT { get; set; }
    public double yawATTITUDE_PREDICT { get; set; }
    public double xaccRAWIMU_PREDICT { get; set; }
    public double yaccRAWIMU_PREDICT { get; set; }
    public double zaccRAWIMU_PREDICT { get; set; }
    public double xgyroRAWIMU_PREDICT { get; set; }
    public double ygyroRAWIMU_PREDICT { get; set; }
    public double zgyroRAWIMU_PREDICT { get; set; }
    public double xmagRAWIMU_PREDICT { get; set; }
    public double ymagRAWIMU_PREDICT { get; set; }
    public double zmagRAWIMU_PREDICT { get; set; }
    public double vibrationXVIBRATION_PREDICT { get; set; }
    public double vibrationYVIBRATION_PREDICT { get; set; }
    public double vibrationZVIBRATION_PREDICT { get; set; }
}

public class WarningData
{
    public int warning_count { get; set; }
    public bool rollATTITUDE_WARNING { get; set; }
    public bool pitchATTITUDE_WARNING { get; set; }
    public bool yawATTITUDE_WARNING { get; set; }
    public bool xaccRAWIMU_WARNING { get; set; }
    public bool yaccRAWIMU_WARNING { get; set; }
    public bool zaccRAWIMU_WARNING { get; set; }
    public bool xgyroRAWIMU_WARNING { get; set; }
    public bool ygyroRAWIMU_WARNING { get; set; }
    public bool zgyroRAWIMU_WARNING { get; set; }
    public bool xmagRAWIMU_WARNING { get; set; }
    public bool ymagRAWIMU_WARNING { get; set; }
    public bool zmagRAWIMU_WARNING { get; set; }
    public bool vibrationXVIBRATION_WARNING { get; set; }
    public bool vibrationYVIBRATION_WARNING { get; set; }
    public bool vibrationZVIBRATION_WARNING { get; set; }
}