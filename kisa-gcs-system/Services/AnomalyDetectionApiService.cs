using kisa_gcs_system.Model;

namespace kisa_gcs_system.Services
{
    public class AnomalyDetectionApiService
    {
        private readonly ILogger<AnomalyDetectionApiService> _logger;
        private readonly IMongoCollection<AnomalyDetection> _dronePredict;

        public AnomalyDetectionApiService(ILogger<AnomalyDetectionApiService> logger, IConfiguration configuration)
        {
            // Looger
            _logger = logger;

            // MongoDB 연결
            var connectionString = configuration.GetConnectionString("MongoDB");
            var mongoClient = new MongoClient(connectionString);
            var database = mongoClient.GetDatabase("drone");
            _dronePredict = database.GetCollection<AnomalyDetection>("drone_predict");
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
                _logger.LogError(ex, "MongoDB에서 드론 데이터를 가져오는 중에 오류가 발생했습니다.");
                throw;
            }
        }

        public List<string> GetFlightIds(string droneId, string periodFrom, string periodTo)
        {
            try
            {
                
                DateTime periodToDate = DateTime.Parse(periodTo).AddDays(2);
                
                var filter = Builders<AnomalyDetection>.Filter.And(
                    Builders<AnomalyDetection>.Filter.Eq(api => api.DroneId, droneId),
                    Builders<AnomalyDetection>.Filter.Gte(api => api.PredictTime, DateTime.Parse(periodFrom)),
                    Builders<AnomalyDetection>.Filter.Lte(api => api.PredictTime, periodToDate)
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
    

    public AnomalyDetection GetRealtimeByDroneId(string droneId)
        {
            try
            {
                ProjectionDefinition<AnomalyDetection> projection = Builders<AnomalyDetection>.Projection
                    .Exclude(d => d._id);
                FilterDefinition<AnomalyDetection> filter =
                    Builders<AnomalyDetection>.Filter.Eq("DroneId", droneId);
                
                SortDefinition<AnomalyDetection> sort = 
                    Builders<AnomalyDetection>.Sort.Descending(api => api.PredictTime);
                
                AnomalyDetection anomalyDetection =
                    _dronePredict
                        .Find(filter)
                        .Sort(sort)
                        .Project<AnomalyDetection>(projection)
                        .FirstOrDefault();
                return anomalyDetection;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MongoDB에서 드론 데이터를 가져오는 중에 오류가 발생했습니다.");
                throw;
            }
        }

        public List<AnomalyDetection> GetLogDataByForm(string DroneId, string FlightId, string periodFrom, string periodTo)
        {
            try
            {
                ProjectionDefinition<AnomalyDetection> projection = Builders<AnomalyDetection>.Projection
                    .Exclude(d => d._id);

                DateTime periodToDate = DateTime.Parse(periodTo).AddDays(2);
                
                FilterDefinition<AnomalyDetection> filter = Builders<AnomalyDetection>.Filter.And(
                    Builders<AnomalyDetection>.Filter.Eq("DroneId", DroneId),
                    Builders<AnomalyDetection>.Filter.Eq("FlightId", FlightId),
                    Builders<AnomalyDetection>.Filter.Gte("PredictTime", DateTime.Parse(periodFrom)),
                    Builders<AnomalyDetection>.Filter.Lte("PredictTime", periodToDate)
                );

                List<AnomalyDetection> logdataList = _dronePredict.Find(filter)
                    .Project<AnomalyDetection>(projection)
                    .Sort(Builders<AnomalyDetection>.Sort.Descending("PredictTime")) // PredictTime을 기준으로 내림차순 정렬
                    .ToList();

                return logdataList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public List<AnomalyDetection> GetPredictDataByForm(string DroneId, string FlightId, string periodFrom, string periodTo, string SelectData)
        {
            try
            {
                ProjectionDefinition<AnomalyDetection> projection = Builders<AnomalyDetection>.Projection
                    .Exclude(d => d._id);

                DateTime periodToDate = DateTime.Parse(periodTo).AddDays(2);
                
                FilterDefinition<AnomalyDetection> filter = Builders<AnomalyDetection>.Filter.And(
                    Builders<AnomalyDetection>.Filter.Eq("DroneId", DroneId),
                    Builders<AnomalyDetection>.Filter.Eq("FlightId", FlightId),
                    Builders<AnomalyDetection>.Filter.Gte("PredictTime", DateTime.Parse(periodFrom)),
                    Builders<AnomalyDetection>.Filter.Lte("PredictTime", periodToDate)
                );

                List<AnomalyDetection> predictionList = _dronePredict.Find(filter)
                    .Project<AnomalyDetection>(projection)
                    .Sort(Builders<AnomalyDetection>.Sort.Descending("PredictTime"))
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