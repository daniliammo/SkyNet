namespace SkyNet;

internal abstract class Program
{
    private static void Main(string[] args)
    {
        var sessionType = Environment.GetEnvironmentVariable("XDG_SESSION_TYPE");
        Console.WriteLine("Hello, World!");
        new SkyNet();
    }
}
