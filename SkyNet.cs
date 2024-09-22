namespace SkyNet;

public static class SkyNet
{

    public static XdgSessionType SessionType;

    public static VisionObject[]? VisionObjects;
    

    private static void PrepareForStart()
    {
        SessionType = GetSessionType();
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
