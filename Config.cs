using Cairo;

namespace SkyNet;

public static class Config
{
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

    public static readonly byte Fps = 165;
    
}
