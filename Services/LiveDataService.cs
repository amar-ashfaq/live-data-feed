
using System.Text.Json;

namespace LiveDataFeed.Services
{
    public class LiveDataService : BackgroundService
    {
        private readonly ILogger<LiveDataService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly WebSockets.WebSocketHandler _webSocketHandler;
        private readonly string _apiUrl = "http://dev-sample-api.tsl-timing.com/sessions";

        public LiveDataService(ILogger<LiveDataService> logger, IHttpClientFactory httpClientFactory, WebSockets.WebSocketHandler webSocketHandler)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _webSocketHandler = webSocketHandler;

            Console.WriteLine("LiveDataService CONSTRUCTOR called");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation("LiveDataService has started running...");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during LiveDataService startup");
            }

            var client = _httpClientFactory.CreateClient();

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Example: call the base endpoint or a specific one
                    var response = await client.GetAsync(_apiUrl, stoppingToken);

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync(stoppingToken);

                        // Optional: parse or transform the data here
                        using var doc = JsonDocument.Parse(json);
                        var formatted = JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true });

                        // Broadcast the JSON to all connected WebSocket clients
                        await _webSocketHandler.BroadcastMessageAsync(formatted);

                        _logger.LogInformation("Broadcasted data to clients at {time}", DateTime.Now);
                    }
                    else
                    {
                        _logger.LogWarning("Failed to fetch data: {status}", response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error fetching data");
                }

                // Wait a bit before polling again
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}