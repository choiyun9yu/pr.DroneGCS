using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

using kisa_gcs_service.Models;
using kisa_gcs_service.Services;

namespace kisa_gcs_service.Controllers
{
    [ApiController]
    [Route("api/drones")] // 라우팅 정의 'localhost:port/api/drones'
    public class DroneController : ControllerBase
    {
        private readonly DroneService _droneService;

        public DroneController(DroneService droneService)
        {
            _droneService = droneService;
        }

        [HttpGet]   // GET 요청
        public ActionResult<IEnumerable<int>> GetDrones()    // ActionResult를 사용하면 JSON, XML 등 다양한 응답 동작 처리 가능
        {
            var drones = _droneService.GetDrones();

            if (drones == null)
            {
                return NotFound();
            }

            return Ok(drones);
        }

//        [HttpGet("{id}")]
//                public ActionResult<DroneModel> GetDroneStatus(string id)
//                {
//                    // 드론의 상태 정보를 조회하는 코드
//                    var drone = _droneService.GetDroneById(id);
//
//                    if (drone == null)
//                    {
//                        return NotFound(); // 드론을 찾을 수 없음
//                    }
//
//                    return Ok(drone); // 드론 상태 정보 반환
//                }

//        [HttpPost]  // POST 요청
//        public ActionResult<DroneModel> AddDrone(DroneModel drone){}
//
//        [HttpPatch("{id}")]
//        public ActionResult<DroneModel> UpdateDrone(string id, DroneModel drone){}
//
//        [HttpDelete("{id}")]
//        public ActionResult DeleteDrone(string id){}

    }
}
