using Cairo;

namespace SkyNet;

public static class Config
{
    #region Rendering
    #region Colors
    public static readonly Color AllyGadgetsColor = new (0, 255, 255); // rgb(0, 255, 255)
    public static readonly Color EnemyGadgetsColor = new (210, 105, 30); // rgb(210, 105, 30)
    public static readonly Color GrenadeColor = new (255, 0, 255); // rgb(255, 0, 255)
    public static readonly Color EnemyColor = new (178, 34, 34); // rgb(178, 34, 34)
    public static readonly Color AllyColor = new (127, 255, 0); // rgb(127, 255, 0)
    public static readonly Color DeadBodyColor = new(0, 0, 0); // rgb(0, 0, 0)
    #endregion
    
    public static readonly Color CrosshairColor = new(0, 255, 127); // rgb(0, 255, 127)
    public const bool DrawCrosshair = true;
    #endregion

    public const ushort Port = 5254;
    public const float ScaleFactor = 4.75f;
    public const bool ImageResizing = true;
    public const string PathToOnnxModel = "/home/daniliammo/RiderProjects/SkyNet/SkyNet/yolov10b/onnx/model_quantized.onnx";
}
