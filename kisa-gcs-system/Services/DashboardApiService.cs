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

    public TimeSpan GetFlightTime(int year, int month)
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

            return totalFlightTime;
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

        var filter = Builders<Dashboard>.Filter.And(
            Builders<Dashboard>.Filter.Gte("_id", startDate),
            Builders<Dashboard>.Filter.Lte("_id", endDate)
        );

        var projection = Builders<Dashboard>.Projection.Include(d => d._id).Include(d => d.FlightTime);

        var result = _dashboard.Aggregate()
            .Match(filter)
            .Project<Dashboard>(projection)
            .ToList();

        var flightTimeSummary = new List<DailyFlightTime>();

        foreach (var doc in result)
        {
            var date = doc._id.Date;
            var flightTime = doc.FlightTime;

            var existingSummary = flightTimeSummary.Find(summary => summary.Date == date);
            if (existingSummary != null)
            {
                existingSummary.FlightTime += flightTime;
            }
            else
            {
                flightTimeSummary.Add(new DailyFlightTime { Date = date, FlightTime = flightTime });
            }
        }

        return flightTimeSummary;
    }
    
    public void GetDailyAnomalyCount(int year, int month)
    {
        var startDate = new DateTime(year, month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);
        
        FilterDefinition<AnomalyDetection> filter = Builders<AnomalyDetection>.Filter.And(
            Builders<AnomalyDetection>.Filter.Gte("PredictTime", startDate),
            Builders<AnomalyDetection>.Filter.Lte("PredictTime", endDate)
        );
    }

}