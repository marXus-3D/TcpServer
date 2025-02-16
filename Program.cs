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

            TcpListener server = new(IPAddress.Any, port);
            server.Start();

            Console.WriteLine($"Server started on port {port}. Waiting for connections...");

            try
            {
                while (true)
                {
                    TcpClient client = await server.AcceptTcpClientAsync();
                    Console.WriteLine($"Client connected: {client.Client.RemoteEndPoint}");

                    _ = Task.Run(() => HandleClient(client));
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

        static async Task HandleClient(TcpClient client)
        {
            try
            {
                using (client)
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] buffer = new byte[1024];

                    while (true)
                    {
                        int bytesRead = await stream.ReadAsync(buffer);
                        if (bytesRead == 0) break; // Client disconnected

                        string command = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        command = command.Trim('\uFEFF').Trim(); // had to use this to remove BOM cause of UTF 
                        Console.Write($"Received from {client.Client.RemoteEndPoint}: ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(command);
                        Console.ResetColor();

                        string response = ProcessCommand(command.Trim());
                        byte[] responseBytes = Encoding.UTF8.GetBytes(response + "\n");
                        await stream.WriteAsync(responseBytes);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error with {client.Client.RemoteEndPoint}: {ex.Message}");
            }
            finally
            {
                Console.WriteLine($"Client disconnected: {client.Client.RemoteEndPoint}");
            }
        }

        static string ProcessCommand(string command)
        {
            switch (command.ToUpper())
            {
                case "GET_TEMP":
                    Random rnd = new();
                    double temp = 30 + rnd.NextDouble() * 10;
                    return $"Temperature: {temp:F1}°C";
                case "GET_STATUS":
                    return "Status: Active";
                default:
                    return "Error: Invalid command";
            }
        }
    }
}