using ImGuiNET;
using SFML.Audio;


namespace SkyNet;

internal abstract class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Привет, Мир!");
        Console.WriteLine(ImGui.GetVersion());
        var x = false;
        ImGui.ShowDemoWindow(ref x);
        new SkyNet();

        // var buffer = new SoundBuffer("/home/daniliammo/HookCanceled.wav");
        // var sound = new Sound();
        // sound.SoundBuffer = buffer;
        // sound.Play();
    }
}
