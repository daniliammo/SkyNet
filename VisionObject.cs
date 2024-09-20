using System.Drawing;


namespace SkyNet;

public class VisionObject()
{
    
    private readonly ObjectType _objectType;
    public required Transform Transform;
    public Color StrokeColor;
    // public byte Percents = 52;

    private readonly bool _isAlly; // false by default


    
    
    public VisionObject(ObjectType objectType, Transform transform) : this()
    {
        _objectType = objectType;
        Transform = transform;
        
        CheckObject();
    }

    public VisionObject(ObjectType objectType, Transform transform, bool isAlly) : this()
    {
        _objectType = objectType;
        Transform = transform;
        _isAlly = isAlly;
        
        CheckObject();
    }
    
    private void CheckObject()
    {
        switch (_objectType)
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
        if(_objectType == ObjectType.Flashbang)
        {
            StrokeColor = Config.GrenadeColor;
            CheckSize();
            return;
        }

        if(_objectType == ObjectType.ImpactGrenade)
        {
            StrokeColor = Config.GrenadeColor;
            CheckSize();
            return;
        }

        if(_objectType == ObjectType.FragGrenade)
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
