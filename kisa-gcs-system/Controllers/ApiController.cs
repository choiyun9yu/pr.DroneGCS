using kisa_gcs_system.Model;
using kisa_gcs_system.Services;

namespace kisa_gcs_service.Controllers;

[ApiController] // 이 특성을 사용하면 컨트롤러 클래스를 간소하게 정이할 수 있음, 별도의 설정없이도 컨트롤러가 API 엔드포인트 동작을 하게 됨
[EnableCors("CorsPolicy")]            
[Route("/api")]                        
public class ApiController : ControllerBase  
{
    private readonly AnomalyDetectionApiService _anomalyDetectionApiService;
    private readonly GcsApiService _gcsApiService;

    public ApiController(AnomalyDetectionApiService anomalyDetectionApiService, GcsApiService gcsApiService)  
    {
        _anomalyDetectionApiService = anomalyDetectionApiService;
        _gcsApiService = gcsApiService;
    }

    [HttpPost("createmission")]
    public IActionResult PostGenerateMission()
    {
        IFormCollection form = Request.Form;
        string? StartPoint = form["StartPoint"];
        string? TargetPoint = form["TargetPoint"];
        string? FlightAlt = form["FlightAlt"];

        List<string> TransitPointsList = new List<string>();
        
        for (int i = 1; i <= 9; i++)
        {
            string? transitPoint = form[$"TransitPoint{i}"];
            if (!string.IsNullOrEmpty(transitPoint))
            {
                TransitPointsList.Add(transitPoint);
            }
        }
        
        try
        {
            _gcsApiService.CreateMission(StartPoint, TargetPoint, FlightAlt, TransitPointsList);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "서버 에러");
        }
    }
    
    [HttpGet("selectmission")]
    public IActionResult GetSelectMission()
    {
        try
        {
            return Ok(_gcsApiService.GetAllMissionLoad());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "서버 에러");
        }
    }
    
    [HttpDelete("deletemissionload")]
    public IActionResult DeleteMissionLoad()
    {
        IFormCollection form = Request.Form;
     
        string? MissionName = form["MissionName"];

        try
        {
            _gcsApiService.deleteMissionName(MissionName);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "서버 에러");
        }
    }
    
    [HttpPost("addwaypoint")]
    public IActionResult PostAddWayPoint()
    {
        IFormCollection form = Request.Form;
     
        string? LocalName = form["LocalName"];
        string? LocalLat = form["LocalLat"];
        string? LocalLon = form["LocalLon"];
        
        try
        {
            _gcsApiService.AddWayPoint(LocalName, LocalLat, LocalLon);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "서버 에러");
        }
    }
    
    [HttpGet("localpoints")]
    public IActionResult GetLocalPoint()
    {
        try
        {
            List<string> localPointList = _gcsApiService.getLocalPointList();

            var JsonObject = new
            {
                localPointList
            };
        
            string jsonString = JsonConvert.SerializeObject(JsonObject);
        
            if (localPointList.Count == 0) { return NotFound(); }
            return Ok(jsonString);
        }        
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "서버 에러");
        }
    }
    
    [HttpDelete("deletelocalpoint")]
    public IActionResult DeleteLocalPoint()
    {
        IFormCollection form = Request.Form;
     
        string? LocalName = form["LocalName"];

        try
        {
            _gcsApiService.deleteLocalName(LocalName);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "서버 에러");
        }
    }

    [HttpGet("getid")]
    public IActionResult GetDroneId()
    {
        try
        {
            List<String> drones = _anomalyDetectionApiService.GetDroneIds();

            var JsonObject = new
            {
                drones
            };
            
            string jsonString = JsonConvert.SerializeObject(JsonObject);
            
            if (drones.Count == 0) { return NotFound(); }
            return Ok(jsonString);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "서버 에러");
        }
    }
    
    [HttpPost("getid")]
    public IActionResult PostGetFlightId()
    {
        IFormCollection form = Request.Form;
        string? DroneId = form["DroneId"];
        string? periodFrom = form["periodFrom"];
        string? periodTo = form["periodTo"];
        try
        {
            List<String> flights = _anomalyDetectionApiService.GetFlightIds(DroneId, periodFrom, periodTo);
            var JsonObject = new
            {
                flights
            };
            
            string jsonString = JsonConvert.SerializeObject(JsonObject);
            
            if (flights.Count == 0) { return NotFound(); }
            return Ok(jsonString);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "서버 에러");
        }
    }

    [HttpPost("realtime")]
    public IActionResult PostRealTime()
    {
        try
        {
            IFormCollection form = Request.Form;
            string? DroneId = form["DroneId"];
            if (DroneId == null) { return BadRequest("유효하지 않은 요청"); }
            AnomalyDetectionAPI anomalyDetectionApi = _anomalyDetectionApiService.GetRealtimeByDroneId(DroneId);

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
            return StatusCode(500, "서버 에러");
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
                return BadRequest("유효하지 않은 요청");
            }

            List<AnomalyDetectionAPI> anomalyDetectionApi =
                _anomalyDetectionApiService.GetLogDataByForm(DroneId, FlightId, periodFrom, periodTo);
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
            return StatusCode(500, "서버 에러");
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
                return BadRequest("유효하지 않은 요청");
            }

            List<AnomalyDetectionAPI> anomalyDetectionApi =
                _anomalyDetectionApiService.GetPredictDataByForm(DroneId, FlightId, periodFrom, periodTo, SelectData);
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
            return StatusCode(500, "서버 에러");
        }
    
    }
}