
namespace LiveDataFeed.Services
{
    public class LiveDataService : BackgroundService
    {
        private readonly ILogger<LiveDataService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly WebSockets.WebSocketHandler _webSocketHandler;
        private readonly string _apiUrl = "http://dev-sample-api.tsl-timing.com/";

        public LiveDataService(ILogger<LiveDataService> logger, IHttpClientFactory httpClientFactory, WebSockets.WebSocketHandler webSocketHandler)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _webSocketHandler = webSocketHandler;  
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
