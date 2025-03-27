public enum MaterialCategory
{
    // Materiales sin refinar
    MetalOre,        // Mineral metálico
    AnimalSkin,      // Piel de animal
    CrystalRaw,      // Cristal en bruto
    FiberRaw,        // Fibra natural
    WoodTrunk,       // Tronco de madera
    
    // Materiales refinados
    MetalIngot,      // Lingote de metal
    ProcessedLeather, // Cuero procesado
    ProcessedGem,     // Gema procesada
    ProcessedFabric,  // Tela procesada
    WoodPlank        // Tablón de madera
}

public enum Rarity
{
    Common,     // 1-25 calidad (40% probabilidad)
    Uncommon,   // 26-50 calidad (30% probabilidad)
    Rare,       // 51-75 calidad (20% probabilidad)
    Epic        // 76-100 calidad (10% probabilidad)
}

public enum StatType
{
    HP,
    MP,
    PhysicalAttack,
    PhysicalDefense,
    MagicalAttack,
    MagicalDefense,
    Speed,
    WaterAffinity,
    FireAffinity,
    EnergyAffinity,
    AirAffinity,
    EarthAffinity,
    LightAffinity,
    DarkAffinity
}

public struct StatModifier
{
    public StatType Stat { get; set; }
    public float Value { get; set; }
    public bool IsPercentage { get; set; }

    public StatModifier(StatType stat, float value, bool isPercentage = false)
    {
        Stat = stat;
        Value = value;
        IsPercentage = isPercentage;
    }
}