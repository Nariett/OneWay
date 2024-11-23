using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OneWay.Services
{
    class GraphHopperRoutingService
    {
        private readonly string apiKey;
        private readonly HttpClient httpClient;
        public GraphHopperRoutingService(string apiKey)
        {
            this.apiKey = apiKey;
            httpClient = new HttpClient();
        }
        public async Task<JToken> GetRouteInfo(string routePoint)
        {
            HttpResponseMessage request;
            if (routePoint.Split(',').Length == 3) 
            {
                request = await httpClient.GetAsync($"https://graphhopper.com/api/1/route?{routePoint}profile=car&calc_points=false&algorithm=alternative_route&key={apiKey}");
            }
            else
            {
                request = await httpClient.GetAsync($"https://graphhopper.com/api/1/route?{routePoint}profile=car&calc_points=false&key={apiKey}");
            }
            var responseContent = await request.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(responseContent);
            return jsonResponse["paths"];
        }
    }
}
