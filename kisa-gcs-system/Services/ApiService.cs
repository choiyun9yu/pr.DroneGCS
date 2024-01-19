using kisa_gcs_system.Model;

namespace kisa_gcs_system.Services
{
    public class ApiService
    {
        private readonly ILogger<ApiService> _logger;
        private readonly IMongoCollection<AnomalyDetectionAPI> _dronePredict;
        private readonly IMongoCollection<AnomalyDetectionAPI> _flightData;
        private readonly IMongoCollection<AnomalyDetectionAPI> _localPoint;

        public ApiService(ILogger<ApiService> logger, IConfiguration configuration)
        {
            // Looger
            _logger = logger;

            // MongoDB 연결
            var connectionString = configuration.GetConnectionString("MongoDB");
            var mongoClient = new MongoClient(connectionString);
            var database = mongoClient.GetDatabase("drone");
            _dronePredict = database.GetCollection<AnomalyDetectionAPI>("drone_predict");

        }

        // public List<DronePredict> Get()
        // {
        //     try
        //     {
        //         ProjectionDefinition<DronePredict> projection = Builders<DronePredict>.Projection
        //             .Exclude(d => d._id);
        //                                                                                         // _droneCollection 으로 모든 Document 가져와서 .Find() 메서드에 빈 BsonDcoumet를 사용하여 모든 문서 선택
        //         List<DronePredict> drones = _dronePredict.Find(new BsonDocument())      // BsonDocument는 MongoDB형식으로(Binary JSON) 데이터를 나타내는 클래스
        //             .Project<DronePredict>(projection)                                        // 선택한 필드만 가져오기
        //             .ToList();                                                                  // 선택한 필드를 포함하는 Drone 문서의 목록을 List 형태로 반환
        //     
        //         return drones;
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "An error occurred while fetching drone data from MongoDB.");
        //         throw;
        //     }
        // }

        public List<string> GetDroneIds()
        {
            try
            {
                var distinctiDroneIds = _dronePredict.Distinct<string>("DroneId", new BsonDocument()).ToList();
                return distinctiDroneIds;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching drone data from MongoDB.");
                throw;
            }
        }

        public List<string> GetFlightIds()
        {
            try
            {
                var distinctiFlightIds = _dronePredict.Distinct<string>("FlightId", new BsonDocument()).ToList();
                return distinctiFlightIds;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching drone data from MongoDB.");
                throw;
            }
        }

        public AnomalyDetectionAPI GetRealtimeByDroneId(string droneId)
        {
            try
            {
                ProjectionDefinition<AnomalyDetectionAPI> projection = Builders<AnomalyDetectionAPI>.Projection
                    .Exclude(d => d._id);
                FilterDefinition<AnomalyDetectionAPI> filter =
                    Builders<AnomalyDetectionAPI>.Filter.Eq("DroneId", droneId);
                AnomalyDetectionAPI anomalyDetectionApi =
                    _dronePredict.Find(filter).Project<AnomalyDetectionAPI>(projection).FirstOrDefault();
                return anomalyDetectionApi;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching drone data from MongoDB.");
                throw;
            }
        }

        public List<AnomalyDetectionAPI> GetLogDataByForm(string DroneId, string FlightId, string periodFrom,
            string periodTo)
        {
            try
            {
                ProjectionDefinition<AnomalyDetectionAPI> projection = Builders<AnomalyDetectionAPI>.Projection
                    .Exclude(d => d._id);

                FilterDefinition<AnomalyDetectionAPI> filter = Builders<AnomalyDetectionAPI>.Filter.And(
                    Builders<AnomalyDetectionAPI>.Filter.Eq("DroneId", DroneId),
                    Builders<AnomalyDetectionAPI>.Filter.Eq("FlightId", FlightId),
                    Builders<AnomalyDetectionAPI>.Filter.Gte("PredictTime", DateTime.Parse(periodFrom)),
                    Builders<AnomalyDetectionAPI>.Filter.Lte("PredictTime", DateTime.Parse(periodTo))
                );

                List<AnomalyDetectionAPI> logdataList = _dronePredict.Find(filter)
                    .Project<AnomalyDetectionAPI>(projection)
                    .Sort(Builders<AnomalyDetectionAPI>.Sort.Descending("PredictTime")) // PredictTime을 기준으로 내림차순 정렬
                    .ToList();

                return logdataList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public List<AnomalyDetectionAPI> GetPredictDataByForm(string DroneId, string FlightId, string periodFrom, string periodTo, string SelectData)
        {
            try
            {
                ProjectionDefinition<AnomalyDetectionAPI> projection = Builders<AnomalyDetectionAPI>.Projection
                    .Exclude(d => d._id);

                FilterDefinition<AnomalyDetectionAPI> filter = Builders<AnomalyDetectionAPI>.Filter.And(
                    Builders<AnomalyDetectionAPI>.Filter.Eq("DroneId", DroneId),
                    Builders<AnomalyDetectionAPI>.Filter.Eq("FlightId", FlightId),
                    Builders<AnomalyDetectionAPI>.Filter.Gte("PredictTime", DateTime.Parse(periodFrom)),
                    Builders<AnomalyDetectionAPI>.Filter.Lte("PredictTime", DateTime.Parse(periodTo))
                );

                List<AnomalyDetectionAPI> predictionList = _dronePredict.Find(filter)
                    .Project<AnomalyDetectionAPI>(projection)
                    .Sort(Builders<AnomalyDetectionAPI>.Sort.Descending("PredictTime"))
                    .ToList();

                Console.WriteLine(predictionList.Count);
                
                return predictionList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}