using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TcpServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const int port = 8888;
            TcpListener server = new TcpListener(IPAddress.Any, port);
            server.Start();
            Console.WriteLine($"Server started on port {port}. Waiting for connections...");

            try
            {
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine($"Client connected: {client.Client.RemoteEndPoint}");
                    
                    Thread clientThread = new Thread(() => HandleClient(client));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Server error: {ex.Message}");
            }
            finally
            {
                server.Stop();
            }
        }

        static void HandleClient(TcpClient client)
        {
            try
            {
                using (client)
                using (NetworkStream stream = client.GetStream())
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true })
                {
                    while (true)
                    {
                        string command = reader.ReadLine();
                        if (command == null) break;

                        Console.WriteLine($"Received command: {command}");
                        string response = ProcessCommand(command);
                        writer.WriteLine(response);
                        Console.WriteLine($"Sent response: {response}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling client: {ex.Message}");
            }
        }

        static string ProcessCommand(string command)
        {
            switch (command?.ToUpper())
            {
                case "GET_TEMP":
                    Random rnd = new();
                    double temp = 20 + rnd.NextDouble() * 10;
                    return $"Temperature: {temp:F1}°C";
                case "GET_STATUS":
                    return "Status: Active";
                default:
                    return "Error: Invalid command";
            }
        }
    }
}