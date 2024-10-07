using Cairo;
using Rectangle = SixLabors.ImageSharp.Rectangle;

namespace SkyNet.Vision.Objects;


public class VisionObject()
{
    
    public readonly ObjectType ObjectType;
    public readonly Rectangle Bounds;
    public Color StrokeColor;
    public readonly float Percents;

    private readonly bool _isAlly; // false by default
    

    public VisionObject(ObjectType objectType, Rectangle bounds, float percents, bool isAlly = false) : this()
    {
        ObjectType = objectType;
        Bounds = Config.ImageResizing ? new Rectangle((int)(Bounds.X * Config.ScaleFactor), (int)(Bounds.Y * Config.ScaleFactor), (int)(Bounds.Width * Config.ScaleFactor), (int)(Bounds.Height * Config.ScaleFactor)) : bounds;
        _isAlly = isAlly;
        Percents = percents;
        
        SetColor();
    }
    
    private void SetColor()
    {
        switch (ObjectType)
        {
            case ObjectType.Flashbang:
                ProcessGadget();
                // разворот на 180
                return;
            case ObjectType.AntiPersonnelMine:
                ProcessGadget();
                return;
            case ObjectType.Enemy:
                StrokeColor = Config.EnemyColor;
                CheckSize();
                // play sound
                return;
            case ObjectType.EnemyHead:
                StrokeColor = Config.EnemyColor;
                return;
            case ObjectType.DeadEnemy:
                StrokeColor = Config.DeadBodyColor;
                return;
            case ObjectType.Ally:
                StrokeColor = Config.AllyColor;
                return;
            case ObjectType.AllyHead:
                StrokeColor = Config.AllyColor;
                return;
            case ObjectType.DeadAlly:
                StrokeColor = Config.DeadBodyColor;
                return;
            case ObjectType.FragGrenade:
                ProcessGadget();
                // play sound
                return;
            case ObjectType.ImpactGrenade:
                ProcessGadget();
                return;
            case ObjectType.SmokeGrenade:
                ProcessGadget();
                return;
            case ObjectType.Claymore:
                ProcessGadget();
                // play sound
                return;
            case ObjectType.AntiGrenadeTrophy:
                ProcessGadget();
                return;
            case ObjectType.SniperFlashlight:
                break;
            case ObjectType.GasTigerUnarmed:
                break;
            case ObjectType.GasTigerCrows:
                break;
            case ObjectType.GasTigerStandardM2Browning:
                break;
            case ObjectType.GasTigerUnarmedDestroyed:
                break;
            case ObjectType.GasTigerCrowsDestroyed:
                break;
            case ObjectType.GasTigerStandardM2BrowningDestroyed:
                break;
            case ObjectType.HumveeUnarmedDestroyed:
                break;
            case ObjectType.HumveeCrowsDestroyed:
                break;
            case ObjectType.HumveeStandardM2BrowningDestroyed:
                break;
            case ObjectType.M1Abrams:
                break;
            case ObjectType.T90A:
                break;
            case ObjectType.M1AbramsDestroyed:
                break;
            case ObjectType.T90ADestroyed:
                break;
            case ObjectType.Pwc:
                break;
            case ObjectType.PwcDestroyed:
                break;
            case ObjectType.Rhib:
                break;
            case ObjectType.RhibDestroyed:
                break;
            case ObjectType.Rcb90:
                break;
            case ObjectType.Rcb90Destroyed:
                break;
            case ObjectType.Lav25A1:
                break;
            case ObjectType.Lav25A1Destroyed:
                break;
            case ObjectType.Btr82:
                break;
            case ObjectType.Btr82Destroyed:
                break;
            case ObjectType.BlackHawk:
                break;
            case ObjectType.BlackHawkDestroyed:
                break;
            case ObjectType.Ka90:
                break;
            case ObjectType.Ka90Destroyed:
                break;
            case ObjectType.LittleBird:
                break;
            case ObjectType.LittleBirdDestroyed:
                break;
            case ObjectType.LittleBirdMilitaryArmed:
                break;
            case ObjectType.LittleBirdMilitaryArmedDestroyed:
                break;
            case ObjectType.Hermit:
                break;
            case ObjectType.HermitDestroyed:
                break;
            case ObjectType.HermitMilitary:
                break;
            case ObjectType.HermitMilitaryDestroyed:
                break;
            case ObjectType.HermitHermitMilitaryArmed:
                break;
            case ObjectType.HermitHermitMilitaryArmedDestroyed:
                break;
            case ObjectType.Quad:
                break;
            case ObjectType.QuadDestroyed:
                break;
            case ObjectType.AirDrone:
                break;
            case ObjectType.AirDroneDestroyed:
                break;
            case ObjectType.AntiVehicleMine:
                break;
            case ObjectType.GreenLaser:
                break;
            case ObjectType.RedLaser:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ProcessGadget()
    {
        if (_isAlly)
        {
            StrokeColor = Config.AllyGadgetsColor;
            return;
        }
        
        // Enemy
        if(ObjectType == ObjectType.Flashbang)
        {
            StrokeColor = Config.GrenadeColor;
            CheckSize();
            return;
        }

        if(ObjectType == ObjectType.ImpactGrenade)
        {
            StrokeColor = Config.GrenadeColor;
            CheckSize();
            return;
        }

        if(ObjectType == ObjectType.FragGrenade)
        {
            StrokeColor = Config.GrenadeColor;
            CheckSize();
            return;
        }

        StrokeColor = Config.EnemyGadgetsColor;
        CheckSize();
    }
    
    private void CheckSize()
    {
        
    }
    
}
