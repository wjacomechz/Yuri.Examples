// See https://aka.ms/new-console-template for more information
using System;
using System.Net.WebSockets;
using System.Text;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        string wsUri = "ws://127.0.0.1/";
        using (var ws = new ClientWebSocket())
        {
            await ws.ConnectAsync(new Uri(wsUri), CancellationToken.None);
            var buffer = new byte[256];
            while (ws.State == WebSocketState.Open)
            {
                var result = await ws.ReceiveAsync(buffer, CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
                }
                else
                {
                    Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, result.Count));
                }
            }
        }
    }
}