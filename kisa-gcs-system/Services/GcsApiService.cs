using kisa_gcs_system.Model;

namespace kisa_gcs_system.Services;

public class GcsApiService
{
    private readonly ILogger<GcsApiService> _logger;
    private readonly IMongoCollection<LocalPointAPI> _localPoint;
    private readonly IMongoCollection<MissionLoadAPI> _missionLoad;

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
            // Console.WriteLine($"saved {id}");
            
            float flightDistance = 1.0f;
            // To Do 
            // 지점 이름으로 지점 위도와 고도를 조회하는 함수로 시작점과 출발점의 좌표를 얻고,
            // 그 좌표를 매개변수로 받아서 그 사이 거리를 계산하는 함수에 넣으면 됨
            // 그리고 그걸 미션 불러 오기에 넘겨주고
            // 미션 불러오기는 불러온 데이터를 보여주면 됨 
            // 그리고 미션 불러오기 시작 버튼을 누르면 미션을 생성해서 
            
            var missionLoad = new MissionLoadAPI
            {
                _id = id,
                StartPoint = startPoint,
                TargetPoint = targetPoint,
                FlightAlt = int.Parse(flirghtAlt),
                TransitPoints = transitPointsList,
                FlightDistance = flightDistance,
                TakeTime = ((flightDistance * 1000) / 600) + 2  // 이착륙에 1분씩 할당해서 +2, 거리는 km로 저장하니까 미터로 환산해주고 이동 속도 10m/s는 600m/m 이니까 600으로 계산, 걸리는 시간 단위가 분이라서
            };
        
            _missionLoad.InsertOne(missionLoad);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MongoDB에서 드론 데이터를 가져오는 중에 오류가 발생했습니다.");
            throw;
        }    
    }

    public List<string> getMissionLoad()
    {
        try
        {
            var missionLoad = _missionLoad
                .AsQueryable()
                .Select(api => api._id)
                .ToList();

            return missionLoad;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MongoDB에서 데이터를 가져오는 중에 오류가 발생했습니다.");
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
}