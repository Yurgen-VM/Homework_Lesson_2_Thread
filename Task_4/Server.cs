using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Task_4
{
    internal class Server
    {
        private static bool IsRunning = true;

        public static void AcceptMess()
        {
            UdpClient udpClient = new UdpClient(12345);
            IPEndPoint serverEp = new IPEndPoint(IPAddress.Any, 0);

            Console.WriteLine("Сервер ожидает сообщение от клиента.");
            Console.WriteLine("Для завершения работы нажмите \"Esc\"");

            Thread ProgrammClose = new Thread(() =>
            {
                while (IsRunning)
                {
                    var key = Console.ReadKey(intercept: true);
                    if (key.Key == ConsoleKey.Escape)
                    {
                        IsRunning = false;
                        Console.WriteLine("Заврешение работы сервера");
                        return;
                    }
                }
            });
            ProgrammClose.Start();


            while (IsRunning)
            {
                try
                {
                    if (udpClient.Available > 0)
                    {
                        byte[] buffer = udpClient.Receive(ref serverEp);
                        string data = Encoding.UTF8.GetString(buffer);

                        Thread threadServer = new Thread(() =>
                        {
                            Message? msg = Message.FromJson(data);
                            if (msg != null)
                            {
                                Console.WriteLine($"{msg.ToString()}\n"); // Вывод в консоль сообщения от клиента

                                Message servResp = new Message("Server", "Сообщение получено"); // Формируем ответ клиенту, о доставке сообщения
                                string strServResp = servResp.ToJson(); // Конвертируем наше сообщение в JSON
                                byte[] byteServResp = Encoding.UTF8.GetBytes(strServResp); // Кодируем JSON в массив байтов
                                udpClient.Send(byteServResp, serverEp); // Отправляем пакет клиенту

                            }
                            else { Console.WriteLine("Некорректное сообщение"); }
                        });
                        threadServer.IsBackground = true;
                        threadServer.Start();
                    }
                    else { Thread.Sleep(100); } 
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Ошибка {e.Message}");
                }
            }           
        }
    }
}
