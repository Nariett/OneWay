using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace OneWay.Services
{
    class YandexGeocoderService
    {
        public string selectToken;
        public string coordinateX = null;
        public string coordinateY = null;
        private readonly string apiKey;
        private readonly HttpClient httpClient;
        public YandexGeocoderService(string apiKey)
        {
            this.apiKey = apiKey;
            httpClient = new HttpClient();
        }
        public async Task<List<Tuple<string, string>>> GetCityAsync(string city)
        {
            string url = $"https://geocode-maps.yandex.ru/1.x/?apikey={apiKey}&geocode={city}&format=json";

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    JObject jsonObject = JObject.Parse(jsonResponse);
                    return GetPosKeys(jsonObject);
                }
                else
                {
                    throw new Exception($"Ошибка: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении данных от Yandex Geocoder API: {ex.Message}");
            }
        }
        public List<Tuple<string, string>> GetPosKeys(JToken token)
        {
            List<string> posKeys = new List<string>();
            List<Tuple<string, string>> data = new List<Tuple<string, string>>();

            if (token.Type == JTokenType.Object)
            {
                foreach (JProperty property in token.Children<JProperty>())
                {
                    JToken currentObject = property.Parent;
                    if (property.Name == "text" && (property.Parent?.SelectToken("kind")?.Value<string>() == "locality" || property.Parent?.SelectToken("kind")?.Value<string>() == "province" || property.Parent?.SelectToken("kind")?.Value<string>() == "house"))
                    {
                        coordinateX = property.Value.ToString();
                        selectToken = GetParentPathUntil(currentObject, "GeoObject");
                    }

                    if (property.Name == "pos")
                    {
                        string objectName = GetParentPathUntil(currentObject, "GeoObject");
                        if (selectToken == objectName)
                        {
                            coordinateY = ReverseString(property.Value.ToString());
                        }
                    }

                    data.AddRange(GetPosKeys(property.Value));

                    if (!string.IsNullOrEmpty(coordinateX) && !string.IsNullOrEmpty(coordinateY))
                    {
                        data.Add(Tuple.Create(coordinateX, coordinateY));
                        coordinateX = null;
                        coordinateY = null;
                    }
                }
            }
            else if (token.Type == JTokenType.Array)
            {
                foreach (JToken arrayItem in token.Children())
                {
                    data.AddRange(GetPosKeys(arrayItem));
                }
            }
            return data;
        }
        private string GetParentPathUntil(JToken token, string stopAt)
        {
            var path = token.Path;
            var index = path.LastIndexOf(stopAt, StringComparison.Ordinal);
            return index >= 0 ? path.Substring(0, index + stopAt.Length) : path;
        }
        public string ReverseString(string input)
        {
            string[] parts = input.Split(' ');
            return $"{parts[1]} {parts[0]}";
        }
    }
}
