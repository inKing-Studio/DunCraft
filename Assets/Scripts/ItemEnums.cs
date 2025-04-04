using System; // Added for [Flags] attribute

public enum ItemCategory
{
    Material,
    Equipment,
    Consumable
}

[Flags] // Added Flags attribute
public enum MaterialCategory
{
    None = 0, // Explicitly set None to 0
    // Raw Materials
    MetalOre       = 1 << 0,  // 1
    AnimalSkin     = 1 << 1,  // 2
    CrystalRaw     = 1 << 2,  // 4
    FiberRaw       = 1 << 3,  // 8
    WoodTrunk      = 1 << 4,  // 16
    // Processed Materials
    MetalIngot     = 1 << 5,  // 32
    ProcessedLeather = 1 << 6,  // 64
    ProcessedGem   = 1 << 7,  // 128
    ProcessedFabric= 1 << 8,  // 256
    WoodPlank      = 1 << 9,   // 512
    // Add more categories here with increasing powers of two
    Cloth = 1 << 10, // 1024 - Renamed to Cloth
    Gem = 1 << 11, // 2048 - Renamed to Gem
    Leather = 1 << 12, // 4096 - Renamed to Leather
    Crystal = 1 << 13  // 8192 - Renamed to Crystal
}

public enum ConsumableType
{
    HealthPotion,
    ManaPotion,
    PoisonPotion,
    SkillScroll,
    RecipeScroll,
    IdentificationScroll
}

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

public enum StatType
{
    // Base Stats
    Health,
    Mana,
    Strength,
    Dexterity,
    Intelligence,
    Defense,
    MagicResistance,
    Attack,
    Speed,
    // Elemental Resistances
    FireResistance,
    WaterResistance,
    EarthResistance,
    WindResistance,
    LightResistance,
    DarkResistance
}

public enum ModifierValueType
{
    Flat,
    Percentage
}