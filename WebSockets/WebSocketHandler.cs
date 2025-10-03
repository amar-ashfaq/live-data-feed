namespace LiveDataFeed.WebSockets
{
    public class WebSocketHandler
    {
        // Store connected WebSocket clients
        private readonly List<System.Net.WebSockets.WebSocket> _sockets = new();

        // Handle incoming WebSocket requests
        public async Task HandleAsync(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                _sockets.Add(webSocket);
                await ReceiveMessagesAsync(webSocket);
            }
            else
            {
                context.Response.StatusCode = 400;
            }
        }

        // Receive messages from a client and broadcast to all connected clients
        private async Task ReceiveMessagesAsync(System.Net.WebSockets.WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!result.CloseStatus.HasValue)
            {
                foreach (var socket in _sockets)
                {
                    if (socket.State == System.Net.WebSockets.WebSocketState.Open)
                    {
                        await socket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
                    }
                }
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            _sockets.Remove(webSocket);
        }

        // Broadcast a message to all connected clients
        public async Task BroadcastMessageAsync(string message)
        {
            var buffer = System.Text.Encoding.UTF8.GetBytes(message);
            var tasks = new List<Task>();

            foreach (var socket in _sockets)
            {
                if (socket.State == System.Net.WebSockets.WebSocketState.Open)
                {
                    tasks.Add(socket.SendAsync(new ArraySegment<byte>(buffer), System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None));
                }
            }

            await Task.WhenAll(tasks);
        }
    }
}
