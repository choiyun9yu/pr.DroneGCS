using kisa_gcs_system.Models;
using System.Collections.Generic;
using MongoClient = MongoDB.Driver.MongoClient;

namespace kisa_gcs_system.Services;

public class DashboardApiService
{
    private readonly IMongoCollection<Dashboard> _dashboard;
    private readonly IMongoCollection<AnomalyDetection> _prediction;

    public DashboardApiService(IConfiguration configuration)
    {
        // MongoDB 연결
        var connectionString = configuration.GetConnectionString("MongoDB");
        var mongoClient = new MongoClient(connectionString);
        var database = mongoClient.GetDatabase("drone");
        _dashboard = database.GetCollection<Dashboard>("dashboard_info");
        _prediction = database.GetCollection<AnomalyDetection>("drone_predict");
    }

    public long GetFlightCount(int year, int month)
    {
        try
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            
            FilterDefinition<Dashboard> filter = Builders<Dashboard>.Filter.And(
                Builders<Dashboard>.Filter.Gte("_id", startDate),
                Builders<Dashboard>.Filter.Lte("_id", endDate)
            );

            long count = _dashboard.CountDocuments(filter);
            
            return count;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public string GetFlightTime(int year, int month)
    {
        try
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            
            FilterDefinition<Dashboard> filter = Builders<Dashboard>.Filter.And(
                Builders<Dashboard>.Filter.Gte("_id", startDate),
                Builders<Dashboard>.Filter.Lte("_id", endDate)
            );

            var flights = _dashboard.Find(filter).ToList();

            TimeSpan totalFlightTime = TimeSpan.Zero;

            foreach (var flight in flights)
            {
                if (TimeSpan.TryParse(flight.FlightTime, out TimeSpan flightTime))
                {
                    totalFlightTime += flightTime;
                }
            }

            string totlaFlightTimeString = totalFlightTime.ToString("hh\\:mm\\:ss");
            
            return totlaFlightTimeString;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
    
    public double GetFlightDistance(int year, int month)
    {
        try
        {
            
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            
            FilterDefinition<Dashboard> filter = Builders<Dashboard>.Filter.And(
                Builders<Dashboard>.Filter.Gte("_id", startDate),
                Builders<Dashboard>.Filter.Lte("_id", endDate)
            );
        
            var flights = _dashboard.Find(filter).ToList();

            double totalFlightDistance = 0;

            foreach (var flight in flights)
            {
                if (flight.FlightDistance.HasValue) // FlightDistance 값이 null이 아닌 경우에만 더합니다.
                {
                    totalFlightDistance += flight.FlightDistance.Value;
                }
            }

            return totalFlightDistance;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public long GetLogCount(int year, int month)
    {
        try
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            
            FilterDefinition<AnomalyDetection> filter = Builders<AnomalyDetection>.Filter.And(
                Builders<AnomalyDetection>.Filter.Gte("PredictTime", startDate),
                            Builders<AnomalyDetection>.Filter.Lte("PredictTime", endDate)
            );
            
            long count = _prediction.CountDocuments(filter);
            
            return count;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
    
    public long GetAnomlayCount(int year, int month)
    {
        try
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
        
            FilterDefinition<AnomalyDetection> filter = Builders<AnomalyDetection>.Filter.And(
                Builders<AnomalyDetection>.Filter.Gte("PredictTime", startDate),
                Builders<AnomalyDetection>.Filter.Lte("PredictTime", endDate),
                Builders<AnomalyDetection>.Filter.Gte("WarningData.warning_count", 10)
            );

            var count = _prediction.CountDocuments(filter);
        
            return count;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public FlightRate GetFlightRate(int year, int month)
    {
        try
        {

            return new FlightRate();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
    
    public List<DailyFlightTime> GetDailyFlightTime(int year, int month)
    {
        var startDate = new DateTime(year, month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);
    
        // MongoDB 에서 조회할 필터 정의 
        FilterDefinition<Dashboard> filter = Builders<Dashboard>.Filter.And(
            Builders<Dashboard>.Filter.Gte("_id", startDate),
            Builders<Dashboard>.Filter.Lte("_id", endDate)
        );

        // 조회할 필드 지정 
        var projection = Builders<Dashboard>.Projection.Include(x => x._id).Include(x => x.FlightTime);
        
        // 데이터 조회 
        var cursor = _dashboard.Find(filter).Project<Dashboard>(projection).ToList();
        
        // 각 날짜별 비행 시간을 저장할 Dictionary 생성 
        Dictionary<int, TimeSpan> dailyFlightTimes = new Dictionary<int, TimeSpan>();
        foreach (var flight in cursor)
        {
            // 비행 날짜 추출 
            DateTime date = flight._id.ToUniversalTime().Date;
            TimeSpan parsedTimeSpan;

            // 비행 시간을 TimeSpan으로 파싱하여 처리 
            if (TimeSpan.TryParse(flight.FlightTime, out parsedTimeSpan))
            {
                // Drionary 에 날짜별 비행 시간 누적 
                if (dailyFlightTimes.ContainsKey(date.Day))
                {
                    dailyFlightTimes[date.Day] += parsedTimeSpan;
                }
                else
                {
                    dailyFlightTimes[date.Day] = parsedTimeSpan;
                }
            }
        }
        
        // 모든 날짜에 대해 비행 시간을 저장 
        var res = new List<DailyFlightTime>();
        for (int day = 1; day <= endDate.Day; day++)
        {
            if (dailyFlightTimes.ContainsKey(day))
            {
                res.Add(new DailyFlightTime
                {
                    FlightDay = day,
                    FlightTime = (int)dailyFlightTimes[day].TotalMinutes // TimeSpan을 분 단위로 변환하여 저장 (막대 그래프에서 분단위로 표시)
                });
            }
            else
            {
                // 비행 시간이 없는 경우 0으로 처리 
                res.Add(new DailyFlightTime
                {
                    FlightDay = day,
                    FlightTime = 0
                });
            }
        }

        return res;
    }
    
    public List<DailyAnomalyCount> GetDailyAnomalyCount(int year, int month)
    {
        var startDate = new DateTime(year, month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        var filter = Builders<AnomalyDetection>.Filter.And(
            Builders<AnomalyDetection>.Filter.Gte(x => x.PredictTime, startDate),
            Builders<AnomalyDetection>.Filter.Lte(x => x.PredictTime, endDate),
            Builders<AnomalyDetection>.Filter.Gte(x => x.WarningData.warning_count, 10)
        );

        var projection = Builders<AnomalyDetection>.Projection.Include(x => x.PredictTime).Include(x => x.WarningData.warning_count);
        var cursor = _prediction.Find(filter).Project<AnomalyDetection>(projection).ToList();

        Dictionary<int, int> dailyAnomalyCount = new Dictionary<int, int>();
        foreach (var log in cursor)
        {
            DateTime date = log.PredictTime.ToUniversalTime().Date;
            var count = log.WarningData.warning_count >= 10;

            if (dailyAnomalyCount.ContainsKey(date.Day) && count)
            {
                dailyAnomalyCount[date.Day]++;
            }
            else
            {
                dailyAnomalyCount[date.Day] = 0;
            }

        }
        
        // 결과를 저장할 List 생성
        var res = new List<DailyAnomalyCount>();
        for (int day = 1; day <= endDate.Day; day++)
        {
            // 모든 날짜에 대해 이상징후 발생 횟수를 저장
            if (dailyAnomalyCount.ContainsKey(day))
            {
                res.Add(new DailyAnomalyCount { FlightDay = day, AnomalyCount = dailyAnomalyCount[day] });
            }
            else
            {
                // 조회되지 않은 날짜에 대한 처리 (이상징후 발생 횟수는 0)
                res.Add(new DailyAnomalyCount { FlightDay = day, AnomalyCount = 0 });
            }
        }

        return res;
    }

    public Dictionary<string, List<DateTime>> GetDroneFlightDay(int year, int month)
    {
        var startDate = new DateTime(year, month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        var filter = Builders<Dashboard>.Filter.And(
            Builders<Dashboard>.Filter.Gte(x => x._id, startDate),
            Builders<Dashboard>.Filter.Lte(x => x._id, endDate)
        );

        var projection = Builders<Dashboard>.Projection.Expression(x => new { x.DroneId, x._id });

        var result = _dashboard.Find(filter).Project(projection).ToList();

        var flightTimesByDroneId = new Dictionary<string, List<DateTime>>();

        foreach (var item in result)
        {
            string droneId = item.DroneId;
            DateTime flightTime = item._id.Date;

            if (!flightTimesByDroneId.ContainsKey(droneId))
            {
                flightTimesByDroneId[droneId] = new List<DateTime>();
            }

            flightTimesByDroneId[droneId].Add(flightTime);
        }

        return flightTimesByDroneId;
    }

}