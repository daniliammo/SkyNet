// using ImGuiSharp;
using SFML.Graphics;
using SFML.Window;


namespace SkyNet;

internal abstract class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Привет, Мир!");
        // Console.WriteLine(ImGui.GetVersion());

        var mode = new VideoMode(1920, 1080);
        var window = new RenderWindow(mode, "SkyNet", Styles.Close);
        
        window.SetVerticalSyncEnabled(true);

        window.Closed += (_, _) => window.Close();

        // ImGui.CreateContext(new ImFontAtlas());
        // ImGui.NewFrame();
        // ImGui.Render();

        var shape = new CircleShape(360);
        shape.FillColor = Color.Green;
        
        while (window.IsOpen)
        {
            window.DispatchEvents();
            
            window.Clear(Color.Transparent);
            window.Draw(shape);
            window.Display();
            
            // ImGui.NewFrame();
            // var unused2 = false;
            // ImGui.ShowDemoWindow(ref unused2);
            // ImGui.Render();
        }

        // ImGui.DestroyContext(null);
        
        
        new SkyNet();

        // var buffer = new SoundBuffer("/home/daniliammo/HookCanceled.wav");
        // var sound = new Sound();
        // sound.SoundBuffer = buffer;
        // sound.Play();
    }
}
