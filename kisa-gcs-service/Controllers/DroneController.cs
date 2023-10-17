using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using kisa_gcs_service.Model;
using kisa_gcs_service.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace kisa_gcs_service.Controllers;

[ApiController]
[EnableCors("CorsPolicy")]    // CORS 정책을 컨트롤러에 적용
[Route("/api")]
public class DroneController : ControllerBase
{
    private readonly DroneService _droneService; // DroneService를 사용하기 위한 멤버 변수

    public DroneController(ILogger<DroneController> logger, IConfiguration configuration, DroneService droneService)
    {
        _droneService = droneService; // DroneService 주입
    }

    [HttpGet("test")]
    public IActionResult Get()
    {
        try
        {
            List<Drone> drones = _droneService.Get(); // DroneService를 사용하여 데이터 가져오기

            if (drones.Count == 0) { return NotFound(); }
            return Ok(drones);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("drones")]
    public IActionResult GetDrones()    // 동기 메서드, 메서드가 실행되면 결과를 즉시 반환
    {
        try
        {
            List<String> drones = _droneService.GetDroneIds(); // DroneService를 사용하여 데이터 가져오기

            if (drones.Count == 0) { return NotFound(); }
            return Ok(drones);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost("drones")]        // 비동기 메서드, 메서드가 비동기 작업 수행이 완료될 때까지 기다리지 않음 
    public IActionResult GetDroneByDroneId()
    {
        try
        {
            IFormCollection form = Request.Form;
            string? DroneId = form["DroneId"];
            if (DroneId == null) { return BadRequest("Invalid request data"); }
            Drone drone = _droneService.GetDroneByDroneId(DroneId);

            if (drone != null)
            {
                // var response = new { monitorPage = drone }; // JSON 형식의 응답을 생성
                // return Ok(response);
                return Ok(drone);
            }
            else
            {
                return NotFound();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
    
    
}