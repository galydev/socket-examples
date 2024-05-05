// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World! Client");
        RunTcpClient();
        // RunUdpClient();
        Console.ReadKey();
    }

    private static void RunTcpClient() 
    {
         // Establecer la dirección IP y el número de puerto del servidor
        string serverIP = "127.0.0.1";
        int port = 8080;

        // Crear el socket TCP
        TcpClient client = new TcpClient();

        // Conectar al servidor
        client.Connect(serverIP, port);
        Console.WriteLine("Conexión establecida con el servidor.");
        
        string operationBePerformed = string.Empty;
        while(true){

            Console.WriteLine("Qué operecion deseas realizar? (+,-,*,/)");
            operationBePerformed = Console.ReadLine();
            if(operationBePerformed == "exit"){
                break;
            }
            if(operationBePerformed != "+" && operationBePerformed != "-" && operationBePerformed != "*" && operationBePerformed != "/"){
                Console.WriteLine("Operacion no valida");
                continue;
            }

            // Obtener el stream de datos de la conexión
            NetworkStream stream = client.GetStream();

            // Enviar un mensaje al servidor
            Operation operation = new Operation {
                Operator = operationBePerformed,
                FirtsValue = 10,
                SecondValue = 20
            };
            
            string message = JsonSerializer.Serialize(operation);
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            stream.Write(messageBytes, 0, messageBytes.Length);
            Console.WriteLine($"Mensaje enviado al servidor: {message}");

            // Recibir la respuesta del servidor
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string responseData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Respuesta del servidor: {responseData}");
            
        }

        // Cerrar la conexión
        client.Close();
    }

    static void RunUdpClient() {
        string serverIP = "127.0.0.1";
        int port = 8080;

        UdpClient udpClient = new UdpClient();
        udpClient.Connect(IPAddress.Parse(serverIP), port);

        Console.WriteLine("Cliente UDP conectado al servidor.");

        while (true)
        {
            byte[] sendBytes = Encoding.ASCII.GetBytes("Solicitando cadena aleatoria.");
            udpClient.Send(sendBytes, sendBytes.Length);

            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, port);
            byte[] receivedBytes = udpClient.Receive(ref remoteEP);
            string receivedString = Encoding.ASCII.GetString(receivedBytes);

            Console.WriteLine($"Cadena aleatoria recibida: {receivedString}");

            // Organizar alfabéticamente
            char[] charArray = receivedString.ToCharArray();
            Array.Sort(charArray);
            string sortedString = new string(charArray);

            Console.WriteLine($"Cadena ordenada alfabéticamente: {sortedString}");
            Console.WriteLine();
            Thread.Sleep(2000); // Esperar 2 segundos antes de solicitar otra cadena
        }
    }
}
