

using System.Web;

namespace kisa_gcs_system.Interfaces;

// Google Maps API에서 반환된 고도 및 위치 정보를 나타내는 클래스 
public class GoogleWaypoint
{
    public float Elevation { get; set; }
    
    public LatLong Location { get; set; }
    
    public class LatLong
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}

// Google Maps API에서 반환된 고도 및 위치 정보를 갖는 리스트를 나타내는 클래스 
class GoogleWaypointResult{
    public List<GoogleWaypoint> Results { get; set; }
}

class GoogleElevationResult
{
    public List<float> ElevationList { get; set; }
}

// Google Maps API를 사용하여 고도 및 위치 정보를 가져오는 도우미 클래스 
public class GoogleMapHelper
{
    private static GoogleMapHelper? instance;
    private JObject _configureationObj = new();
    private string? API_KEY;
    private string BASE_URL;

    private HttpClient httpClient = new HttpClient();

    // 생성자에서 Google API 키 및 기본 URL 초기화 
    private GoogleMapHelper()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
        string? apiKey = configuration["AppSettings:API_KEY"];
        
        API_KEY = GetValue<String>("GOOGLE_API_KEY", apiKey);
        BASE_URL = GetValue<String>("GOOGLE_BASE_URL", "https://maps.googleapis.com/maps/api/elevation/json");
    }

    // 인스턴스의 싱글톤 패턴을 위한 속성 
    public static GoogleMapHelper? Instance
    {
        get => instance;
        set => instance = value;
    }

    // 싱글톤 패턴을 사용하여 인스턴스 가져오기 
    public static GoogleMapHelper GetInstace()
    {
        if (Instance == null)
        {
            Instance = new GoogleMapHelper();
        }

        return Instance;
    }
    
    public async Task<float> FetchElevation(double startLat, double startLng)
    {
        var uriBuilder = new UriBuilder(BASE_URL);
        var parameters = HttpUtility.ParseQueryString(string.Empty);
        parameters["locations"] = $"{startLat:0.0000000},{startLng:0.0000000}";
        parameters["key"] = API_KEY;
        uriBuilder.Query = parameters.ToString();
        var responseBody = await httpClient.GetStringAsync(uriBuilder.Uri);
        var results = JsonConvert.DeserializeObject<GoogleWaypointResult>(responseBody);
        return float.Parse(results.Results[0].Elevation.ToString());
    }
    
    public async Task<List<GoogleWaypoint>> FetchElevations(
        double startLat,
        double startLng,
        double endLat,
        double endLng,
        int samples,
        bool sensor = true
    )
    {
        var uriBuilder = new UriBuilder(BASE_URL);
        var parameters = HttpUtility.ParseQueryString(string.Empty);
        parameters["path"] = String.Format("{0:0.0000000},{1:0.0000000}|{2:0.0000000},{3:0.0000000}",
            startLat, startLng, endLat, endLng);
        parameters["samples"] = samples.ToString();
        parameters["key"] = API_KEY;
        parameters["sensor"] = sensor.ToString();
        uriBuilder.Query = parameters.ToString();

        string responseBody = await httpClient.GetStringAsync(uriBuilder.Uri);

        var results = JsonConvert.DeserializeObject<GoogleWaypointResult>(responseBody);
        return results!.Results;
    }
    
    public async Task<List<float>> FetchElevations2(
        double startLat, 
        double startLng, 
        double targetLat, 
        double targetLng, 
        int samples)
    {
        var uriBuilder = new UriBuilder(BASE_URL);
        var parameters = HttpUtility.ParseQueryString(string.Empty);
        
        parameters["path"] = String.Format("{0:0.0000000},{1:0.0000000}|{2:0.0000000},{3:0.0000000}", startLat, startLng, targetLat, targetLng);
        parameters["samples"] = samples.ToString(); // 경로상에 몇개의 우치에서 고도 값을 샘플링할지 결정하는 값 (보통 몇십개에서 몇백개 샘플 얻는 편 )
        parameters["key"] = API_KEY;
        // UriBuilder의 쿼리 문자열을 설정  
        uriBuilder.Query = parameters.ToString();
        
        try
        {
            string responseBody = await httpClient.GetStringAsync(uriBuilder.Uri);
            var results = JsonConvert.DeserializeObject<GoogleElevationResult>(responseBody);
            return results!.ElevationList;
        }
        catch (Exception e)
        {
            Console.WriteLine("check error");
            Console.WriteLine($"고도를 가져오는 중 오류 발생: {e.Message}");
            throw;
        }
    }

    public T GetValue<T>(string key, T def)
    {
        try
        {
            return _configureationObj.GetValue(key).Value<T>();
        }
        catch (Exception)
        {
            return def;
        }
    }
}