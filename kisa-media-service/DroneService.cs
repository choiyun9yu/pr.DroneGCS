using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

public class DroneService
{
    private readonly IMongoCollection<DroneModel> _droneCollection;

    public DroneService(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        _droneCollection = database.GetCollection<DroneModel>("Drone");
    }

    public List<DroneModel> GetDrones()
    {
        return _droneCollection.Find(d => true).ToList();
    }

    public DroneModel GetDroneById(string id)
    {
        return _droneCollection.Find(d => d.Id == id).FirstOrDefault();
    }

    // 다른 데이터 액세스 메서드 추가
}


//using MongoDB.Driver;
//using Microsoft.AspNetCore.Mvc;
//using System.Collections.Generic;
//
//using kisa_gcs_service.Models;
//
//namespace kisa_gcs_service.Services
//{
//    public class DroneService
//    {
//        private readonly IMongoCollection<DroneModel> _droneCollection;
//
//        public DroneService(IMongoDatabase database)
//        {
//            // MongoDB 데이터베이스와의 연결 설정
//            var client = new MongoClient("mongodb://localhost:27017"); // MongoDB 연결 문자열
//            var databaseName = "gcs_drone"; // 데이터베이스 이름
//            var db = client.GetDatabase(databaseName);
//            _droneCollection = db.GetCollection<DroneModel>("Drone"); // 컬렉션 이름
//        }
//
//        public ActionResult<IEnumerable<DroneModel>> GetDrones()
//        {
//            // MongoDB에서 드론 목록 가져오기
//            var droneList = new List<DroneModel>
//            {
//                new DroneModel { DroneId = "12345678CD" },
//                //            return _droneCollection.Find(d => true).ToList();
//            };
//
//            return Ok(droneList);
//        }
//
//        public DroneModel GetDroneById(string id)
//        {
//            // MongoDB에서 특정 ID의 드론 가져오기
//            var filter = Builders<DroneModel>.Filter.Eq("DroneId", id); // DroneId 필드로 필터링
//            var sort = Builders<DroneModel>.Sort.Descending(d => d.LastHeartbeatMessage);   // LastHeartbeatMessage로 정렬
//            return _droneCollection.Find(filter).FirstOrDefault();  // FirstOrDefault메소드는 컬렉션에서 첫번째 요소 반
//        }
//
//    }
//}
