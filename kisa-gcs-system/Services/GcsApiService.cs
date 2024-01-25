using kisa_gcs_system.Interfaces;
using kisa_gcs_system.Services.Helper;

namespace kisa_gcs_system.Services;

public class GcsApiService
{
    private readonly ILogger<GcsApiService> _logger;
    private readonly IMongoCollection<LocalPointAPI> _localPoint;
    private readonly IMongoCollection<MissionLoadAPI> _missionLoad;
    private readonly VincentyCalculator _vincentyCalculator;
    
    public GcsApiService(ILogger<GcsApiService> logger, IConfiguration configuration)
    {
        // Looger
        _logger = logger;

        // MongoDB 연결
        var connectionString = configuration.GetConnectionString("MongoDB");
        var mongoClient = new MongoClient(connectionString);
        var database = mongoClient.GetDatabase("drone");
        _localPoint = database.GetCollection<LocalPointAPI>("local_point");
        _missionLoad = database.GetCollection<MissionLoadAPI>("mission_load");
        _vincentyCalculator = new VincentyCalculator();
    }

    public void CreateMission(string startPoint, string targetPoint, string flirghtAlt, List<string> transitPointsList)
    {
        try
        {
            StringBuilder idBuilder = new StringBuilder();
            idBuilder.Append(startPoint)
                .Append("-")
                .AppendJoin("-", transitPointsList.Take(9).Where(point => !string.IsNullOrEmpty(point)))
                .Append("-")
                .Append(targetPoint);
            
            string id = idBuilder.ToString();
            
            List<double> startLocalPoint = getLocalPoint(startPoint);
            List<double> endLocalPoint = getLocalPoint(targetPoint);

            double flightDistance = 0;

            if (transitPointsList.Count == 0)
            {
                flightDistance += _vincentyCalculator.DistanceCalculater(
                    startLocalPoint[0], startLocalPoint[1], 
                    endLocalPoint[0], endLocalPoint[1]);   
            }
            else
            {
                // startPoint에서 각 transitPoint를 거쳐 endPoint까지의 거리를 누적하여 계산
                for (int i = 0; i < transitPointsList.Count; i++)
                {
                    List<double> transitLocalPoint = getLocalPoint(transitPointsList[i]);
            
                    // 핸재 경유지에서 다음 경유지 또는 목적지까지의 거리를 계산하여 누적
                    flightDistance += _vincentyCalculator.DistanceCalculater(
                        (i == 0) ? startLocalPoint[0] : getLocalPoint(transitPointsList[i - 1])[0],
                        (i == 0) ? startLocalPoint[1] : getLocalPoint(transitPointsList[i - 1])[1],
                        transitLocalPoint[0],
                        transitLocalPoint[1]
                    );
            
                }
                // 마지막 경유지에서 목적지까지의 거리를 누적하여 계산
                flightDistance += _vincentyCalculator.DistanceCalculater(
                    getLocalPoint(transitPointsList.Last())[0],
                    getLocalPoint(transitPointsList.Last())[1],
                    endLocalPoint[0],
                    endLocalPoint[1]
                );
            }
            
            double takeTime = flightDistance / 600 + 1;
            
            var missionLoad = new MissionLoadAPI
            {
                _id = id,
                StartPoint = startPoint,
                TargetPoint = targetPoint,
                FlightAlt = int.Parse(flirghtAlt),
                TransitPoints = transitPointsList,
                FlightDistance = $"{Math.Round(flightDistance)/1000} km",
                TakeTime = $"{(int)takeTime}분 {(int)(Math.Round(takeTime,1)%1*60)}초"
            };
            
            // Console.WriteLine(missionLoad.FlightDistance);
            // Console.WriteLine(missionLoad.TakeTime);
            
            _missionLoad.InsertOne(missionLoad);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MongoDB에서 드론 데이터를 가져오는 중에 오류가 발생했습니다.");
            throw;
        }    
    }
    
    public object GetAllMissionLoad()
    {
        var mission = _missionLoad
            .Find(Builders<MissionLoadAPI>.Filter.Empty)
            .ToList();

        if (mission.Count == 0)
        {
            throw new Exception("미션을 찾을 수 없습니다.");
        }

        // Console.WriteLine(mission);
        
        return mission;
    }
    
    public void deleteMissionName(string missionName)
    {
        try
        {
            var filter = Builders<MissionLoadAPI>.Filter.Eq(api => api._id, missionName);

            var result = _missionLoad.DeleteOne(filter);

            if (result.DeletedCount == 0)
            {
                Console.WriteLine($"LocalName이 '{missionName}'인 문서를 찾지 못했습니다.");
            }
            else
            {
                Console.WriteLine($"LocalName이 '{missionName}'인 문서를 삭제했습니다.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MongoDB에서 문서를 삭제하는 중에 오류가 발생했습니다.");
            throw;
        }
    }

    public void AddWayPoint(string localName, string localLat, string localLon)
    {
        try
        {
            string id = localName.Replace(" ", "");
            
            var wayPoint = new LocalPointAPI
            {
                _id = id,
                LocalLat = double.Parse(localLat),
                LocalLon = double.Parse(localLon),
                // LocalAlt = double.Parse(localAlt)
            };
            
            _localPoint.InsertOne(wayPoint);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MongoDB에서 드론 데이터를 가져오는 중에 오류가 발생했습니다.");
            throw;
        }    
    }

    public List<string> getLocalPointList()
    {
        try
        {
            var localPosition = _localPoint
                .AsQueryable()
                .Select(api => api._id)
                .ToList();

            // Console.WriteLine(string.Join(", ", localPosition));

            return localPosition;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MongoDB에서 데이터를 가져오는 중에 오류가 발생했습니다.");
            throw;
        }    
    }

    public void deleteLocalName(string localName)
    {
        try
        {
            var filter = Builders<LocalPointAPI>.Filter.Eq(api => api._id, localName);

            var result = _localPoint.DeleteOne(filter);

            if (result.DeletedCount == 0)
            {
                Console.WriteLine($"LocalName이 '{localName}'인 문서를 찾지 못했습니다.");
            }
            else
            {
                Console.WriteLine($"LocalName이 '{localName}'인 문서를 삭제했습니다.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MongoDB에서 문서를 삭제하는 중에 오류가 발생했습니다.");
            throw;
        }
    }
    
    private List<double>? getLocalPoint(string localName)
    {
        try
        {
            var localPoint = _localPoint
                .AsQueryable()
                .Where(api => api._id == localName)
                .Select(api => new { api.LocalLat, api.LocalLon })
                .FirstOrDefault();
        
            return new List<double>() { (double)localPoint.LocalLat, (double)localPoint.LocalLon };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MongoDB에서 데이터를 가져오는 중에 오류가 발생했습니다.");
            throw;
        }    
    }
}