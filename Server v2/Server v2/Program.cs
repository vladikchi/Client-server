using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class Program
    {
        public int answer;

        static void Main(string[] args)
        {
            Program program = new Program();

            //Получаем информацию о хосте по имени "localhost"
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            //Получаем IP-адрес из списка адресов хоста и выводим в консоль 
            IPAddress ipAddr = ipHost.AddressList[1];
            Console.WriteLine("IP Адрес - " + ipAddr);
            //Создаем конечную точку для соединения
            IPEndPoint EndPoint = new IPEndPoint(ipAddr, 2222);
            //Создаем сокет для прослушивания входящих соединений
            Socket sListen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                //Устанавливаем соединение с сервером
                sListen.Bind(EndPoint);
                sListen.Listen(10);
                while (true)
                {
                    //Ждем подключения клиента
                    Console.WriteLine("Ждем соединение " + EndPoint);
                    Socket handler = sListen.Accept();

                    //Получаем первое число от клиента
                    string Number1;
                    byte[] bNumb1 = new byte[512];
                    int RecNumber1 = handler.Receive(bNumb1);
                    Number1 = Encoding.UTF8.GetString(bNumb1);
                    Console.WriteLine("Первое число: " + Number1);
                    //Отправляем ответ клиету
                    string sOtvetNumber1 = "Информация получена успешно! " + Number1.Length.ToString();
                    byte[] bOtvetNumber1 = Encoding.UTF8.GetBytes(sOtvetNumber1);
                    handler.Send(bOtvetNumber1);

                    //Получаем знак операции от клиента
                    string Sign;
                    byte[] bSign = new byte[256];
                    int RecSign = handler.Receive(bSign);
                    Sign = Encoding.UTF8.GetString(bSign);
                    Console.WriteLine("Знак: " + Sign);
                    //Отправляем ответ клиету
                    string sOtvetSign = "Информация получена успешно! " + Sign.Length.ToString();
                    byte[] bOtvetSign = Encoding.UTF8.GetBytes(sOtvetSign);
                    handler.Send(bOtvetSign);

                    // получаем второе число от клиента
                    string Number2;
                    byte[] bNumb2 = new byte[512];
                    int RecNumber2 = handler.Receive(bNumb2);
                    Number2 = Encoding.UTF8.GetString(bNumb2);
                    Console.WriteLine("Второе число: " + Number2);

                    //Считем полученные данные
                    try
                    {
                        if (Sign[0] == '+')
                        {
                            int inumb = Convert.ToInt32(Number1);
                            int inumb2 = Convert.ToInt32(Number2);
                            program.answer = inumb + inumb2;
                            Console.Write("Ответ: ");
                            Console.WriteLine(program.answer);

                        }
                        else if (Sign[0] == '-')
                        {
                            int inumb = Convert.ToInt32(Number1);
                            int inumb2 = Convert.ToInt32(Number2);
                            program.answer = inumb - inumb2;
                            Console.Write("Ответ: ");
                            Console.WriteLine(program.answer);

                        }
                        else if (Sign[0] == '*')
                        {
                            int inumb = Convert.ToInt32(Number1);
                            int inumb2 = Convert.ToInt32(Number2);
                            program.answer = inumb * inumb2;
                            Console.Write("Ответ: ");
                            Console.WriteLine(program.answer);
                        }
                        else if (Sign[0] == '/')
                        {
                            int inumb = Convert.ToInt32(Number1);
                            int inumb2 = Convert.ToInt32(Number2);
                            program.answer = inumb / inumb2;
                            Console.Write("Ответ: ");
                            Console.WriteLine(program.answer);
                        }
                        else
                        {
                            Console.WriteLine("Ошибка, Введите пример правильно.");
                        }
                    }
                    catch
                    {
                        string serror = "Вы ввели неправильный или недопустимый пример, попробуйте снова";
                        Console.WriteLine(serror);
                    }

                    //Отправляем ответ клиенту
                    string sOtvetNumber2 = "Информация получена успешно! " + Number2.Length.ToString() + "\nОтвет: " + program.answer;
                    byte[] bOtvetNumber2 = Encoding.UTF8.GetBytes(sOtvetNumber2);
                    handler.Send(bOtvetNumber2);

                    if (Number1.IndexOf("<TheEnd>") > 1)
                    {
                        Console.WriteLine("Завершено!");
                        break;
                    }

                    //Закрываем соединение с клиентом
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }

            }
            catch
            {
                Console.WriteLine("Cоединение разорвано");
            }

            Console.ReadKey();
        }
    }
}
