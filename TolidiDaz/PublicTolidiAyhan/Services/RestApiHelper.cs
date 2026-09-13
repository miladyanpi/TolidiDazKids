using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PublicTolidiAyhan.Services
{
    public class RestApiHelper<T> where T : class
    {
        private string FullUrlApi = string.Empty;
        private readonly HttpClient _http;

        public RestApiHelper(HttpClient http)
        {
            _http = http;
        }

        public void SetUrl(string baseUrl, string endpoint)
        {
            FullUrlApi = Path.Combine(baseUrl, endpoint);
        }

        public async Task<HttpResponseMessage> Post(object data)
            => await _http.PostAsJsonAsync(FullUrlApi, data);

        public async Task<HttpResponseMessage> Patch(object data)
            => await _http.PatchAsJsonAsync(FullUrlApi, data);

        public async Task<HttpResponseMessage> Delete()
            => await _http.DeleteAsync(FullUrlApi);

        public async Task<HttpResponseMessage> Get(object parameters)
        {
            string url = FullUrlApi;

            if (parameters != null)
            {
                var dict = JObject.FromObject(parameters);
                var qs = string.Join("&", dict.Properties().Select(x =>
                    $"{x.Name}={Uri.EscapeDataString(x.Value!.ToString())}"));

                url += "?" + qs;
            }

            return await _http.GetAsync(url);
        }


        public async Task<TOut?> ReadContent<TOut>(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TOut>(json);
        }
    }

}
