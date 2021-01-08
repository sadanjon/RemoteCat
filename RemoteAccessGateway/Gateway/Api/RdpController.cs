using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Api
{
    public class FooResult
    {
        public string Bar { get; set; }
        public int Baz { get; set; } 
    }

    [Route("api/[controller]")]
    [ApiController]
    public class RdpController : ControllerBase
    {
        [Route("connect")]
        public async Task Connect()
        {
            if (!HttpContext.WebSockets.IsWebSocketRequest)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            await HandleWebsocketAsync(webSocket);
        }

        [Route("foo")]
        public FooResult Foo()
        {
            return new FooResult
            {
                Bar = "Hello",
                Baz = 42,
            };
        }

        private async Task HandleWebsocketAsync(WebSocket webSocket)
        {
            var arraySegment = new ArraySegment<byte>(new byte[4096]);

            while (webSocket.State == WebSocketState.Open)
            {
                var message = await webSocket.ReceiveAsync(arraySegment, CancellationToken.None);
                if (message.MessageType == WebSocketMessageType.Text)
                {
                    var str = Encoding.UTF8.GetString(arraySegment);
                    var newStr = $"Echo: {str}";
                    await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(newStr)), WebSocketMessageType.Text, endOfMessage: true, CancellationToken.None);
                }
            }

        }
    }
}
