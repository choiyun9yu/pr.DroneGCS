using kisa_gcs_system.Model;
using kisa_gcs_system.Services;

namespace kisa_gcs_service.Controllers;

[ApiController] // 이 특성을 사용하면 컨트롤러 클래스를 간소하게 정이할 수 있음, 별도의 설정없이도 컨트롤러가 API 엔드포인트 동작을 하게 됨
[EnableCors("CorsPolicy")]            
[Route("/api")]                        
public class DroneController : ControllerBase  
{
    private readonly ApiService _apiService; 

    public DroneController(ApiService apiService)  
    {
        _apiService = apiService; 
    }


    [HttpGet("realtime")]
    public IActionResult GetRealTime()
    {
        try
        {
            List<String> drones = _apiService.GetDroneIds();

            var JsonObject = new
            {
                drones = drones
            };
            
            string jsonString = JsonConvert.SerializeObject(JsonObject);
            
            if (drones.Count == 0) { return NotFound(); }
            return Ok(jsonString);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost("realtime")]
    public IActionResult PostRealTime()
    {
        try
        {
            IFormCollection form = Request.Form;
            string? DroneId = form["DroneId"];
            if (DroneId == null) { return BadRequest("Invalid request data"); }
            AnomalyDetectionAPI anomalyDetectionApi = _apiService.GetRealtimeByDroneId(DroneId);

            if (anomalyDetectionApi != null)
            {
                var response = new 
                {
                    DroneId,
                    anomalyDetectionApi.PredictData,  
                    anomalyDetectionApi.SensorData,
                };
                return Ok(response);
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

    [HttpGet("logdata")]
    public IActionResult GetLogData()
    {
        try
        {
            List<String> drones = _apiService.GetDroneIds();
            List<String> flights = _apiService.GetFlightIds();
            
            var JsonObject = new
            {
                drones = drones,
                flights = flights
            };
            
            string jsonString = JsonConvert.SerializeObject(JsonObject);
            
            if (drones.Count == 0) { return NotFound(); }
            return Ok(jsonString);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost("logdata")] // form(DroneId, FlightId, periodFrom, periodTo) -> PredictTime, DroneId, FlightId, SensorData
    public IActionResult PostLogData()
    {
        try
        {
            IFormCollection form = Request.Form;
            string? DroneId = form["DroneId"];
            string? FlightId = form["FlightId"];
            string? periodFrom = form["periodFrom"];
            string? periodTo = form["periodTo"];
            if (DroneId == null)
            {
                return BadRequest("Invalid request data");
            }

            List<AnomalyDetectionAPI> anomalyDetectionApi =
                _apiService.GetLogDataByForm(DroneId, FlightId, periodFrom, periodTo);
            if (anomalyDetectionApi != null)
            {
                var response = new
                {
                    logPage = anomalyDetectionApi
                };
                return Ok(response);
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

    [HttpGet("predict")] 
    public IActionResult GetPredict()
    {
        try
        {
            List<String> drones = _apiService.GetDroneIds();
            List<String> flights = _apiService.GetFlightIds();
            
            var JsonObject = new
            {
                drones = drones,
                flights = flights
            };
            
            string jsonString = JsonConvert.SerializeObject(JsonObject);
            
            if (drones.Count == 0) { return NotFound(); }
            return Ok(jsonString);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost("predict")] //  form(DroneId, FlightId, periodFrom, periodTo, SelectData) -> PredictTime, DroneId, FlightId, SelectData, PredictData, SensorData
    public IActionResult PostPrediction()
    {
        try
        {
            IFormCollection form = Request.Form;
            string? DroneId = form["DroneId"];
            string? FlightId = form["FlightId"];
            string? periodFrom = form["periodFrom"];
            string? periodTo = form["periodTo"];
            string? SelectData = form["SelectData"];
            if (DroneId == null)
            {
                return BadRequest("Invalid request data");
            }

            List<AnomalyDetectionAPI> anomalyDetectionApi =
                _apiService.GetPredictDataByForm(DroneId, FlightId, periodFrom, periodTo, SelectData);
            if (anomalyDetectionApi != null)
            {
                var response = new
                {
                    SelectData,
                    predictPage = anomalyDetectionApi,
   
                };
                return Ok(response);
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