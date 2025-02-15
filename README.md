
# Simple TCP Server in C#

This is a simple TCP Server made using C# made for simulating communication between devices with mock(Sample) data.

## Features
- **Handles multiple connections**: Each client is handled concurrently using asynchronous tasks.
- **Command Protocol**: Supports custom commands (`GET_TEMP`, `GET_STATUS`)
- **Mock Sensor Data**:
  - **GET_TEMP**: Returns a random temperature reading.
  - **GET_STATUS**: Returns the server status ("Active").
  - Any other command results in an "Invalid command" error.
- **Cross-Platform**: Runs on any OS with .NET 6+

## Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download) or newer
- IDE/Runtime (choose one):
  - Visual Studio 2022+
  - VS Code with C# extension
- (Has only been tested on Windows)

## Getting Started

### Clone the Repository
```bash
git clone https://github.com/marXus-3D/TcpServer.git
cd TcpServer
```
### Server Setup
```bash
cd TcpServer
dotnet restore
dotnet run
```
### Client Setup
```bash
git clone https://github.com/marXus-3D/TcpClient.git

cd TcpClient
dotnet run
```
you can edit the commands sent by the client as much as you want all you need to do is edit the commands array then run the project again.

`string[] commands = { "GET_STATUS", "GET_TEMP", "GET_STATUS", "INVALID_CMD" };`
