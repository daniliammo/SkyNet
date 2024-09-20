namespace SkyNet;

public enum ObjectType
{
    #region Players
    Enemy,
    EnemyHead,
    DeadEnemy,
    
    Ally,
    AllyHead,
    DeadAlly,
    #endregion

    #region Vehicles

    #region GasTiger
    GasTigerUnarmed,
    GasTigerCrows,
    GasTigerStandardM2Browning,
    #endregion

    #region Humvee
    HumveeUnarmed,
    HumveeCrows,
    HumveeStandardM2Browning,
    #endregion
    
    Quad,
    
    #endregion
    
    #region Gadgets
    FragGrenade,
    ImpactGrenade,
    Flashbang,
    SmokeGrenade,
    AntiPersonnelMine,
    Claymore,
    AntiGrenadeTrophy
    #endregion
}