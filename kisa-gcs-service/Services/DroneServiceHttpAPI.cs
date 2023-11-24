using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;

using kisa_gcs_service.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver.Core.Configuration;

namespace kisa_gcs_service.Service
{
    public class DroneServiceHttpAPI
    {
        private readonly ILogger<DroneServiceHttpAPI> _logger;
        private readonly IMongoCollection<DroneMongo> _droneCollection;
        public DroneServiceHttpAPI(ILogger<DroneServiceHttpAPI> logger, IConfiguration configuration)
        {
            // Looger
            _logger = logger;
            
            // MongoDB 연결,  // null인지 확인
            // var connectionString = configuration.GetConnectionString("MongoDB");
            var connectionString = "mongodb://localhost:27017";     // local에서 실행하면 문제가 없는데 Docker로 실행하면 null 값을 자꾸 넘겨 받아서 직접 입력함..
            var mongoClient = new MongoClient(connectionString);
            var database = mongoClient.GetDatabase("gcs_drone");
            _droneCollection = database.GetCollection<DroneMongo>("Drone");
        }
        
        public List<DroneMongo> Get()
        {
            try
            {
                // 필드 선택 정의, 원하는 필드만 선택하기 위한 목적으로 사용
                ProjectionDefinition<DroneMongo> projection = Builders<DroneMongo>.Projection
                    .Exclude(d => d._id)
                    .Exclude(d => d.DroneTrails);
            
                // _droneCollection 으로 모든 Document 가져와서 .Find() 메서드에 빈 BsonDcoumet를 사용하여 모든 문서 선택
                List<DroneMongo> drones = _droneCollection.Find(new BsonDocument())  // BsonDocument는 MongoDB형식으로(Binary JSON) 데이터를 나타내는 클래스
                    .Project<DroneMongo>(projection) //선택한 필드만 가져오기
                    .ToList();  // 선택한 필드를 포함하는 Drone 문서의 목록을 List 형태로 반환
            
                return drones;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching drone data from MongoDB.");
                throw;  // 오류를 호출자에게 전달
            }
        }

        public List<string> GetDroneIds()
        {
            try
            {
                var distinctiDroneIds = _droneCollection.Distinct<string>("DroneId", new BsonDocument()).ToList();
                return distinctiDroneIds;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching drone data from MongoDB.");
                throw;
            }
        }
        
        public DroneMongo GetDroneByDroneId(string droneId)
        {
            try
            {
                ProjectionDefinition<DroneMongo> projection = Builders<DroneMongo>.Projection
                    .Exclude(d => d._id)
                    .Exclude(d => d.DroneTrails);
                
                FilterDefinition<DroneMongo> filter = Builders<DroneMongo>.Filter.Eq("DroneId", droneId);
                DroneMongo drone = _droneCollection.Find(filter) .Project<DroneMongo>(projection).FirstOrDefault();
                return drone;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching drone data from MongoDB.");
                throw;
            }
        }
    }
}
