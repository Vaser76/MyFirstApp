using System;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace DeliveyApp.Services
{
    public class CostCalculator
    {        
        public int Calculate(string pointA, string pointB)
        {
            string apiKey = "f6b3f13a-a1d8-423e-8a9d-e68fe0f22840";
            //string formURL = "https://geocode-maps.yandex.ru/1.x/?apikey=&geocode=";
            string url1 = $"https://geocode-maps.yandex.ru/1.x/?apikey={apiKey}&geocode={pointA}&format=json";
            string url2 = $"https://geocode-maps.yandex.ru/1.x/?apikey={apiKey}&geocode={pointB}&format=json";
            

            var responseString = getResponseString(url1);
            var cordinatesPointA = getCoordinates(responseString);

            responseString = getResponseString(url2);
            var cordinatesPointB = getCoordinates(responseString);
            var distanse = getDistance(cordinatesPointA, cordinatesPointB);



            int MINIMUM_COST = 100;
            var DELIVERY_TARIFF = 30;

            return Math.Max( (int)Math.Round(DELIVERY_TARIFF * Convert.ToDouble(distanse.Replace(".", ",")), 0), MINIMUM_COST);

        }

        private string getResponseString(string url)
        {
            WebRequest webRequest;
            webRequest = WebRequest.Create(url);
            Stream response = webRequest.GetResponse().GetResponseStream();
            StreamReader reader = new StreamReader(response);
            return reader.ReadToEnd();
        }

        private string getCoordinates(string json)
        {
            var jObj = JObject.Parse(json);
            string id = (string)jObj.Descendants()
                .OfType<JProperty>()
                .Where(p => p.Name == "pos")
                .First()
                .Value;
            var output = id.Split(" ").Reverse();
            return String.Join(", ", output).Replace(" ", "");
        }

        private string getDistance(string coordinatesStart, string coordinatesEnd)
        {
            var url = $"http://open.mapquestapi.com/directions/v2/route?key=5KlwY5ro8gKQipTebp4zunO8BjeShXyH&from={coordinatesStart}&to={coordinatesEnd}&unit=k";
            var responseString =  getResponseString(url);

            var jObj = JObject.Parse(responseString);
            string distance = (string)jObj.Descendants()
                .OfType<JProperty>()
                .Where(p => p.Name == "distance")
                .First()
                .Value;
            return distance;
        }
    }
}
