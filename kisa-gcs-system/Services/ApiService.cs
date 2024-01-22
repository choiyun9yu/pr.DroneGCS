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

        public List<string> GetFlightIds(string droneId, string periodFrom, string periodTo)
        {
            try
            {
                var filter = Builders<AnomalyDetectionAPI>.Filter.And(
                    Builders<AnomalyDetectionAPI>.Filter.Eq(api => api.DroneId, droneId),
                    Builders<AnomalyDetectionAPI>.Filter.Gte(api => api.PredictTime, DateTime.Parse(periodFrom)),
                    Builders<AnomalyDetectionAPI>.Filter.Lte(api => api.PredictTime, DateTime.Parse(periodTo))
                );

                var distinctFlightIds = _dronePredict
                    .Find(filter)
                    .Project(api => api.FlightId)
                    .ToList()
                    .Distinct()
                    .ToList();

                return distinctFlightIds;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MongoDB에서 드론 데이터를 가져오는 중에 오류가 발생했습니다.");
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
                
                SortDefinition<AnomalyDetectionAPI> sort = 
                    Builders<AnomalyDetectionAPI>.Sort.Descending(api => api.PredictTime);
                
                AnomalyDetectionAPI anomalyDetectionApi =
                    _dronePredict
                        .Find(filter)
                        .Sort(sort)
                        .Project<AnomalyDetectionAPI>(projection)
                        .FirstOrDefault();
                return anomalyDetectionApi;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching drone data from MongoDB.");
                throw;
            }
        }

        public List<AnomalyDetectionAPI> GetLogDataByForm(string DroneId, string FlightId, string periodFrom, string periodTo)
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