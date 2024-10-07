namespace SkyNet.Vision.Objects;

public enum ObjectType
{
    #region Players
    Enemy,
    EnemyHead,
    DeadEnemy,
    
    SniperFlashlight,
    
    Ally,
    AllyHead,
    DeadAlly,
    #endregion

    #region Vehicles

    #region GasTiger
    GasTigerUnarmed,
    GasTigerCrows,
    GasTigerStandardM2Browning,
    
    GasTigerUnarmedDestroyed,
    GasTigerCrowsDestroyed,
    GasTigerStandardM2BrowningDestroyed,
    #endregion

    #region Humvee
    HumveeUnarmedDestroyed,
    HumveeCrowsDestroyed,
    HumveeStandardM2BrowningDestroyed,
    #endregion

    #region Tanks
    M1Abrams,
    T90A,
    M1AbramsDestroyed,
    T90ADestroyed,
    #endregion

    #region Sea Vehicles
    Pwc,
    PwcDestroyed,
    Rhib,
    RhibDestroyed,
    Rcb90,
    Rcb90Destroyed,
    #endregion

    #region Armored Personnel Carrier
    Lav25A1,
    Lav25A1Destroyed,
    Btr82,
    Btr82Destroyed,
    #endregion
    
    #region Hehicopters
    BlackHawk,
    BlackHawkDestroyed,
    
    Ka90,
    Ka90Destroyed,
    
    LittleBird,
    LittleBirdDestroyed,
    LittleBirdMilitaryArmed,
    LittleBirdMilitaryArmedDestroyed,
    
    Hermit,
    HermitDestroyed,
    HermitMilitary,
    HermitMilitaryDestroyed,
    HermitHermitMilitaryArmed,
    HermitHermitMilitaryArmedDestroyed,
    #endregion
    
    Quad,
    QuadDestroyed,
    
    #endregion
    
    #region Gadgets

    #region Grenades
    FragGrenade,
    ImpactGrenade,
    Flashbang,
    SmokeGrenade,
    #endregion
    
    AirDrone,
    AirDroneDestroyed,
    
    AntiVehicleMine,
    AntiPersonnelMine,
    Claymore,
    AntiGrenadeTrophy,
    #endregion

    #region Lasers

    GreenLaser,
    RedLaser

    #endregion
}
