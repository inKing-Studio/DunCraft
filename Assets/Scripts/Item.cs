using UnityEngine;

public enum ItemType
{
    Material,
    Consumable,
    Accessory,
    Weapon,
    Armor,
    Blueprint
}

public enum MaterialType
{
    None,
    Metal,
    Crystal,
    Skin,
    Wood,
    Thread
}

public class Item : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite icon;
    public ItemType itemType;
    public MaterialType materialType; // Solo para items de tipo Material
    public int quality; // Porcentaje de calidad (1-100)

    // Otras propiedades específicas del item pueden ir aquí (ej: stats de arma, efectos de consumible)
}