using Newtonsoft.Json;
using OneReportServer.Client.Interface;
using OneReportServer.DB.Redis;
using OneReportServer.Model;
using StackExchange.Redis;

namespace OneReportServer.Client.Implementation
{
    public class RedisClient : BaseHttpClient, IRedisClient, IHostedService
    {
        private ILogger<RedisClient> _logger;
        private readonly IDatabase _cache;
        private bool shouldContinue = true;
        private bool IsRun = false;



        public RedisClient(ILogger<RedisClient> logger, IDatabase cache) : base(logger, "")
        {
            _logger = logger;
            _cache = cache;
        }

        public async Task SetOTPToken(OTPModel otpModel)
        {
            await _cache.StringSetAsync(RedisKeys.GetOTPKey(otpModel.OTPToken), JsonConvert.SerializeObject(otpModel), TimeSpan.FromMinutes(5));
        }

        public async Task SetOTPAvailableToken(OTPModel otpModel)
        {
            await _cache.StringSetAsync(RedisKeys.GetOTPKey(otpModel.AvailableToken), JsonConvert.SerializeObject(otpModel), TimeSpan.FromMinutes(5));
        }
        public async Task<OTPModel> GetOTPToken(string token)
        {
            var res = await _cache.StringGetAsync(RedisKeys.GetOTPKey(token));
            if (string.IsNullOrEmpty(res))
            {
                return null;
            }
            return JsonConvert.DeserializeObject<OTPModel>(res);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("StartAsync for Redis");
            if (!IsRun)
            {
                IsRun = true;
                //InfiniteLoop(cancellationToken);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"StopAsync for Redis. cancellationToken: [{cancellationToken}]");
            shouldContinue = false;
            Task.Delay(100);
            return Task.CompletedTask;
        }
    }
}
