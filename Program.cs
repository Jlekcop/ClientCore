//ClientCore
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace ClientCore
{

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SendMessageFromSocket(11000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadLine();
            }
        }

        static void SendMessageFromSocket(int port)
        {
            //Буфер для входящих данных
            byte[] bytes = new byte[1024];
            
                //Соединение с сервером
                //Установку удаленной точки для сокета
                IPHostEntry ipHost = Dns.GetHostEntry("localhost");
                IPAddress ipAddr = ipHost.AddressList[0];
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);
                
                Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                
                //Соединение сокета с удаленной точкой

                sender.Connect(ipEndPoint);
                
                Console.Write("Введите команду: ");
                string message = Console.ReadLine();
                
                Console.WriteLine("Connection with {0}", sender.RemoteEndPoint.ToString());
                byte[] msg = Encoding.UTF8.GetBytes(message);
                
                //отправка данных
                int bytesSent = sender.Send(msg);
                
                //Ответ от сервера
                int bytesRec = sender.Receive(bytes);
                
                Console.WriteLine("\nОтвет от сервера {0}\n\n", Encoding.UTF8.GetString(bytes, 0, bytesRec));
                
                //Рекурсивный вызов SendMessageFromSocket()
                if (message.IndexOf("<TheEnd>") == -1)
                    SendMessageFromSocket(port);
                
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
        }
    }
}