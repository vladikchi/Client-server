using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Вызываем функцию SendMessageFromSocket
                SendMessageFromSocket(2222);
            }
            catch (Exception ex)
            {
                // Выводим сообщение об ошибке в консоль
                Console.WriteLine(ex.ToString());

                Console.ReadKey();
            }

        }
        static void SendMessageFromSocket(int port)
        {
            // Получаем IP-адрес локального хоста
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            // Получаем второй адрес из списка адресов хоста
            IPAddress ipAddr = ipHost.AddressList[1];
            Console.WriteLine(ipAddr);
            // Создаем конечную точку для подключения
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);
            // Создаем сокет для отправки сообщений
            Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            // Подключаемся к серверу
            sender.Connect(ipEndPoint);
            try
            {
                // Отправляем первое число на сервер
                byte[] bytesNumber1 = new byte[512];
                Console.Write("Введите первое число: ");
                string sMessageNumber1 = Console.ReadLine();
                Console.WriteLine("Сокет соединяется с {0} ", sender.RemoteEndPoint.ToString());
                byte[] msgNumber1 = Encoding.UTF8.GetBytes(sMessageNumber1);
                int SentNumber1 = sender.Send(msgNumber1);
                // Получаем ответ от сервера
                int RecNumber1 = sender.Receive(bytesNumber1);
                Console.WriteLine("\nОтвет от сервера: {0}\n\n", Encoding.UTF8.GetString(bytesNumber1, 0, RecNumber1));

                // Отправляем знак на сервер
                byte[] bytesSign = new byte[256];
                Console.Write("Введите знак: ");
                string sMessageSign = Console.ReadLine();
                Console.WriteLine("Сокет соединяется с {0} ", sender.RemoteEndPoint.ToString());
                byte[] msgSign = Encoding.UTF8.GetBytes(sMessageSign);
                int SentSign = sender.Send(msgSign);
                // Получаем ответ от сервера
                int RecSign = sender.Receive(bytesSign);
                Console.WriteLine("\nОтвет от сервера: {0}\n\n", Encoding.UTF8.GetString(bytesSign, 0, RecSign));

                // Отправляем второе число на сервер
                byte[] bytesNumber2 = new byte[512];
                Console.Write("Введите второе число: ");
                string smessageNumber2 = Console.ReadLine();
                Console.WriteLine("Сокет соединяется с {0} ", sender.RemoteEndPoint.ToString());
                byte[] msgNumber2 = Encoding.UTF8.GetBytes(smessageNumber2);
                int SentNumber2 = sender.Send(msgNumber2);
                // Получаем ответ от сервера
                int RecNumber2 = sender.Receive(bytesNumber2);
                Console.WriteLine("\nОтвет от сервера: {0}\n\n", Encoding.UTF8.GetString(bytesNumber2, 0, RecNumber2));

                if (smessageNumber2.IndexOf("<TheEnd>") == -1)
                    SendMessageFromSocket(port);
                // Закрываем соединение с сервером
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
            }
            catch
            {
                // Если происходит ошибка при отправке или получении сообщения
                Console.WriteLine(" Error!!!!!");
            }

            Console.ReadKey();
        }
    }
}
