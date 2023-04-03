using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Yuri.WorkerService.WebSocket
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string server_ip = "127.0.0.1";
            int server_puerto = 80;
            IPAddress localAdd = IPAddress.Parse(server_ip);
            TcpListener listener = new TcpListener(localAdd, server_puerto);
            Console.WriteLine("Listening...");
            listener.Start();
            while (!stoppingToken.IsCancellationRequested)
            {
                TcpClient client = listener.AcceptTcpClient();

                //---get the incoming data through a network stream---
                NetworkStream nwStream = client.GetStream();
                byte[] buffer = new byte[client.ReceiveBufferSize];

                //---read incoming stream---
                int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

                //---convert the data received into a string---
                string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Received : " + dataReceived);

                //---write back the text to the client---
                Console.WriteLine("Sending back : " + dataReceived);
                nwStream.Write(buffer, 0, bytesRead);
                client.Close();


                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                //await Task.Delay(1000, stoppingToken);
            }
            listener.Stop();
            Console.ReadLine();
        }
    }
}