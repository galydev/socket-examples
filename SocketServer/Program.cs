// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World! Server");
        RunTcpServer();
        // RunUdpServer();
        Console.ReadKey();
    }


    private static void RunTcpServer() 
    {
        // Establecer la dirección IP y el número de puerto del servidor
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        int port = 8080;

        // Crear el socket TCP
        TcpListener listener = new TcpListener(ipAddress, port);
        
        // Iniciar la escucha
        listener.Start();
        Console.WriteLine("Servidor TCP iniciado. Esperando conexiones...");

        // Aceptar conexiones entrantes
        TcpClient client = listener.AcceptTcpClient();
        Console.WriteLine("Cliente conectado.");

        // Obtener el stream de datos de la conexión
        NetworkStream stream = client.GetStream();

        // Buffer para los datos recibidos
        byte[] buffer = new byte[1024];
        int bytesRead;

        // Recibir datos del cliente y mostrarlos en la consola
        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0) {
            string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Mensaje del cliente: {dataReceived}");

            string responseData = string.Empty;
            if(string.IsNullOrEmpty(dataReceived)){
                responseData = "Lo siento pero no he recibido algún mensaje";
            }
            else{
                Operation data = JsonSerializer.Deserialize<Operation>(dataReceived);
                responseData = $"Resutado operación {data.Operator} : {data.Execute()}";
                Console.WriteLine($"Respuesta al cliente: {responseData}");
            }
            
            // Enviar una respuesta al cliente
            byte[] responseDataBytes = Encoding.ASCII.GetBytes(responseData);
            stream.Write(responseDataBytes, 0, responseDataBytes.Length);
        }

        // Cerrar la conexión
        client.Close();
        listener.Stop();
    }

    private static void RunUdpServer(){
        // Establecer el número de puerto del servidor
        int port = 8080;

        UdpClient udpServer = new UdpClient(port);
        Console.WriteLine("Servidor UDP esperando conexiones...");

        while (true)
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, port);
            byte[] receivedBytes = udpServer.Receive(ref remoteEP);
            string receivedString = Encoding.ASCII.GetString(receivedBytes);
            Console.WriteLine($"Mensaje recibido del cliente: {receivedString}");

            // Generar cadena aleatoria
            Random random = new Random();
            string randomString = "";
            for (int i = 0; i < 7; i++)
            {
                randomString += (char)('a' + random.Next(0, 26));
            }

            Console.WriteLine($"Enviando cadena aleatoria al cliente: {randomString}");
            byte[] sendBytes = Encoding.ASCII.GetBytes(randomString);
            udpServer.Send(sendBytes, sendBytes.Length, remoteEP);
        }

        // Crear el socket UDP
        // UdpClient listener = new UdpClient(port);

        // Console.WriteLine("Servidor UDP iniciado. Esperando datagramas...");

        // // Esperar la recepción de datos
        // IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, port);
        // byte[] dataReceived = listener.Receive(ref remoteEP);

        // // Convertir los datos recibidos a una cadena y mostrarlos en la consola
        // string message = Encoding.ASCII.GetString(dataReceived);
        // Console.WriteLine($"Mensaje recibido del cliente: {message}");

        // // Enviar una respuesta al cliente
        // string responseData = "Mensaje recibido por el servidor.";
        // byte[] responseDataBytes = Encoding.ASCII.GetBytes(responseData);
        // listener.Send(responseDataBytes, responseDataBytes.Length, remoteEP);

        // // Cerrar el socket
        // listener.Close();
    }
}