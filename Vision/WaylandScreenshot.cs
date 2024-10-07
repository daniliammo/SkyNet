using System.Net;
using System.Net.Sockets;
using SixLabors.ImageSharp;

namespace SkyNet.Vision;

public static class WaylandScreenshot
{

    private static Thread _thread;
    private static TcpListener _listener;
    private static bool _isRunning;

    private static readonly byte[] ExitSequence = "IEND"u8.ToArray(); // PNG End


    public static void Init()
    {
        // Запускаем сервер в отдельном потоке
        _thread = new Thread(StartTcpServer);
        _isRunning = true; // Устанавливаем флаг для запуска
        _thread.Start();
    }

    private static async void StartTcpServer()
    {
        using var tcpListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        
        tcpListener.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), Config.Port));
        tcpListener.Listen();    // запускаем сервер
        Console.WriteLine("Сервер запущен. Ожидание подключений... ");

        while (true)
        {
            // Получаем подключение в виде TcpClient
            var tcpClient = await tcpListener.AcceptAsync();
            _ = HandleClientAsync(tcpClient); // Обрабатываем клиента асинхронно
        }
    }

    private static async Task HandleClientAsync(Socket tcpClient)
    {
        var buffer = new List<byte>();
        var bytesRead = new byte[1]; // Читаем по 1 байту

        while (true)
        {
            var count = await tcpClient.ReceiveAsync(bytesRead);

            if (count == 0) break; // Если не прочитано ничего, выходим из цикла

            // Добавляем прочитанные байты в буфер
            buffer.Add(bytesRead[0]);

            // Проверяем наличие конца сообщения в буфере
            if (buffer.Count >= ExitSequence.Length &&
                buffer.Skip(buffer.Count - ExitSequence.Length).SequenceEqual(ExitSequence))
            {
                // Убираем конечный символ из буфера
                // buffer.RemoveRange(buffer.Count - ExitSequence.Length, ExitSequence.Length);

                // Сохраняем данные в файл
                Console.WriteLine("Получено сообщение!");

                SkyNet.DetectAndDraw(Config.ImageResizing
                    ? Image.Load(buffer.ToArray()).ResizeWithConfig()
                    : Image.Load(buffer.ToArray()));
                // const string filePath = "/home/daniliammo/RiderProjects/SkyNet/SkyNet/outTest.png"; // Уникальное имя файла
                //await File.WriteAllBytesAsync(filePath, buffer.ToArray());
                // Очищаем буфер для следующего сообщения
                buffer.Clear();
            }
        }
        // if (buffer.Count > 0) // Обработка остатков (если они есть)
        // {
        //     var filePath = "/home/daniliammo/RiderProjects/SkyNet/SkyNet/outTestОстатки.png"; // Уникальное имя файла
        //     await File.WriteAllBytesAsync(filePath, buffer.ToArray());
        //     Console.WriteLine("Сохранены остатки!");
        // }
    }

    public static void Stop()
    {
        // Остановить сервер
        _isRunning = false;
        _listener.Stop();
        _thread.Join(); // Дождаться завершения потока сервера

        Console.WriteLine("Сервер остановлен.");
    }
    
}