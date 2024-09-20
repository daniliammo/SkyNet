namespace SkyNet;

public class SkyNet
{

    private static XdgSessionType _sessionType;

    private static VisionObject[]? _visionObjects;
    
    
    public SkyNet()
    {
        PrepareForStart();
    }

    private static void PrepareForStart()
    {
        _sessionType = GetSessionType();
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
