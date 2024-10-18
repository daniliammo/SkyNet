using SkyNet.Vision.Objects;
using  Compunet.YoloV8;
using Compunet.YoloV8.Data;
using SixLabors.ImageSharp;
using SkyNet.Vision;

namespace SkyNet;


public static class SkyNet
{

    public static XdgSessionType SessionType;

    public static List<VisionObject> VisionObjects = [];

    private static YoloPredictor _yoloPredictor;

    
    public static void Init()
    {
        SessionType = GetSessionType();
        LoadYoloPredictor(Config.PathToOnnxModel);
        Start();
    }

    private static void LoadYoloPredictor(string modelPath)
    {
        var x = new YoloPredictorOptions
        {
            UseCuda = false,
            // SessionOptions = SessionOptions.MakeSessionOptionWithRocmProvider() // AMD GPU
        };
        _yoloPredictor = new YoloPredictor(modelPath, x);
        Console.WriteLine("YoloPredictor ready!");
    }

    private static void Start()
    {
        if(SessionType == XdgSessionType.Wayland)
            WaylandScreenshot.Init();
    }
    
    public static void DetectAndDraw(Image image)
    {
        // _yoloPredictor.DetectAndSaveAsync("/home/daniliammo/RiderProjects/SkyNet/SkyNet/test2.jpg", "/home/daniliammo/RiderProjects/SkyNet/SkyNet/out.png");
        DrawAllObjects(_yoloPredictor!.Detect(image));
    }

    private static void DrawAllObjects(YoloResult<Detection> visionObjects)
    {
        // Clear the list
        // VisionObjects = [];
        
        foreach (var visionObject in visionObjects)
            VisionObjects.Add(new VisionObject(ObjectType.Ally, visionObject.Bounds, visionObject.Confidence));
        
        Console.WriteLine($"Объектов: {VisionObjects.Count}");
        WindowController.DrawAllObjects();
    }
    
    private static XdgSessionType GetSessionType()
    {
        var sessionType = Environment.GetEnvironmentVariable("XDG_SESSION_TYPE");

        return sessionType switch
        {
            "wayland" or "x11" => XdgSessionType.Wayland,
            _ => throw new ArgumentOutOfRangeException(
                $"Неизвестный тип XDGSessionType. Environment.GetEnvironmentVariable(\"XDG_SESSION_TYPE\") вернул значение: {sessionType}")
        };
    }
    
}
