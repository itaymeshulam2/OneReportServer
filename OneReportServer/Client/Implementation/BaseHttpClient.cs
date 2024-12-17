using Newtonsoft.Json;

namespace OneReportServer.Client.Implementation
{
    public abstract class BaseHttpClient
    {
        protected readonly ILogger<BaseHttpClient> _logger;
        private readonly string BaseUrl;

        protected BaseHttpClient(ILogger<BaseHttpClient> logger, string url)
        {
            _logger = logger;
            BaseUrl = url;

            if (!BaseUrl.EndsWith('/'))
            {
                BaseUrl += "/";
            }
        }

        protected async Task<string> Get(string endpoint, string? jwt = null)
        {
            var url = BaseUrl + endpoint;
            var res = "";
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                if (!string.IsNullOrEmpty(jwt))
                {
                    request.Headers.Add("Authorization", $"Bearer {jwt}");
                }

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                res = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Http Get Exception. url: [{url}], response: [{res}], jwt: [{jwt}]", ex);
                throw;
            }

            return res;
        }

        protected async Task<string> Post(string endpoint, object body, string? jwt = null,
            Dictionary<string, string>? headers = null, string? baseUrl = null)
        {
            var url = (baseUrl ?? BaseUrl) + endpoint;
            var res = "";
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                if (!string.IsNullOrEmpty(jwt))
                {
                    request.Headers.Add("Authorization", $"Bearer {jwt}");
                }

                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        request.Headers.Add(header.Key, header.Value);
                    }
                }

                var content = new StringContent(JsonConvert.SerializeObject(body), null, "application/json");

                request.Content = content;
                var response = await client.SendAsync(request);
                res = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Http Post Exception. url: [{url}], response: [{res}], jwt: [{jwt}]", ex);
                throw;
            }

            return res;
        }

        protected async Task<string> Delete(string endpoint, string? jwt = null)
        {
            var url = BaseUrl + endpoint;
            var res = "";
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Delete, url);
                if (!string.IsNullOrEmpty(jwt))
                {
                    request.Headers.Add("Authorization", $"Bearer {jwt}");
                }

                var response = await client.SendAsync(request);
                res = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Http Delete Exception. url: [{url}], response: [{res}], jwt: [{jwt}]", ex);
                throw;
            }

            return res;
        }

        protected async Task<string> Patch(string endpoint, object body, string? jwt = null)
        {
            var url = BaseUrl + endpoint;
            var res = "";
            try
            {
                var bodyJson = JsonConvert.SerializeObject(body, Formatting.None,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Patch, url);
                if (!string.IsNullOrEmpty(jwt))
                {
                    request.Headers.Add("Authorization", $"Bearer {jwt}");
                }

                var content = new StringContent(bodyJson, null, "application/json");

                request.Content = content;
                var response = await client.SendAsync(request);
                res = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Http Patch Exception. url: [{url}], response: [{res}], jwt: [{jwt}]", ex);
                throw;
            }

            return res;
        }
    }
}