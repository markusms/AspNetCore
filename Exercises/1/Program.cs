using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace _1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var realTimeCityBikeFetcher = new RealTimeCityBikeDataFetcher();
            var offlineCityBikeFetcher = new OfflineCityBikeDataFetcher();
            string stationName = "";
            bool realtime = false;
            try
            {
                stationName = args[0];
                if(args[1] == "realtime")
                {
                    realtime = true;
                }
            }
            catch
            {
                stationName = "Rajasaarentie";
            }
            try
            {
                if(stationName.Any(char.IsDigit))
                    throw new ArgumentException("There is a number in the station name.");

                int numOfBikes = 0;
                if(realtime)
                {
                    numOfBikes = await realTimeCityBikeFetcher.GetBikeCountInStation(stationName);
                }
                else
                {
                    numOfBikes = await offlineCityBikeFetcher.GetBikeCountInStation(stationName);
                }
                Console.WriteLine("Number of bikes: " + numOfBikes);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Argument exception: " + e);
            }
        }
    }

    public class RealTimeCityBikeDataFetcher : ICityBikeDataFetcher
    {
        public async Task<int> GetBikeCountInStation(string stationName)
        {
            var url = new Uri("http://api.digitransit.fi/routing/v1/routers/hsl/bike_rental");
            var httpClient = new System.Net.Http.HttpClient();
            try
            {
                bool stationFound = false;
                var result = await httpClient.GetStringAsync(url);
                var bikeRentalStationList = JsonConvert.DeserializeObject<BikeRentalStationList>(result);
                foreach (var station in bikeRentalStationList.Stations)
                {
                    if(station.Name == stationName)
                    {
                        stationFound = true;
                        return station.BikesAvailable;
                    }
                }
                if(!stationFound)
                {
                    throw new NotFoundException("Station not found");
                }
            }
            catch (NotFoundException e)
            {
                Console.WriteLine("Not found: " + e);
            }
            catch
            {
                Console.WriteLine("error");
            }
            return 0;
        }
    }

    public interface ICityBikeDataFetcher
    {
        Task<int> GetBikeCountInStation(string stationName);
    }

    public class Stations
    {
        public int Id {get; set;}
        public string Name {get; set;}
        public double X {get;set;}
        public double Y {get;set;}
        public int BikesAvailable {get;set;}
        public int SpacesAvailable {get;set;}
        public bool AllowDropOff {get;set;}
        public bool IsFloatinBike {get;set;}
        public bool IsCarStation {get;set;}
        public string State {get;set;}
        public string[] Networks {get;set;}
        public bool RealTimeData {get;set;}
    }

    public class BikeRentalStationList
    {
        public Stations[] Stations { get; set; }
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {

        }
    }

    public class OfflineCityBikeDataFetcher : ICityBikeDataFetcher
    {
        public const string fileName = "bikedata.txt";
        public readonly string filePath = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);
        public Task<int> GetBikeCountInStation(string stationName)
        {
            try
            {
                //string[] lines = System.IO.File.ReadAllLines(@"C:\Users\marku\OneDrive\Metropolia\3 vuosi\1 periodi\Taustajärjestelmät\Tehtävät\1\bikedata.txt");
                string[] lines = System.IO.File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    var splitted = line.Split(" : ");
                    if(splitted[0] == stationName)
                    {
                        int bikes = Int32.Parse(splitted[1]);
                        return Task.FromResult(bikes);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error reading the file: " + e);
            }

            return Task.FromResult(0);
        }
    }
}
