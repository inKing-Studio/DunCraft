using UnityEngine;
using System.Collections.Generic;

public class ItemData : ScriptableObject
{
    [Header("Información Básica")]
    public string itemName;
    public string description;
    public Sprite icon;
    public Rarity rarity;
    public ItemCategory itemCategory;
    
    [Header("Propiedades de Material")]
    [Tooltip("Solo se usa si itemCategory es Material")]
    public MaterialCategory materialCategory;
    [Tooltip("Solo se usa si itemCategory es Material")]
    [ReadOnlyAttribute] public float quality = 1f;

    [Header("Modificadores de Stats")]
    public List<StatModifier> statModifiers = new List<StatModifier>();

    public virtual string GetTooltipText()
    {
        string text = $"{itemName}\n";
        text += $"Rareza: {rarity}\n";
        
        if (itemCategory == ItemCategory.Material)
        {
            text += $"Calidad: {quality:P0}\n";
            text += $"Categoría: {materialCategory}\n";
        }
        
        if (!string.IsNullOrEmpty(description))
        {
            text += $"\n{description}\n";
        }

        if (statModifiers.Count > 0)
        {
            text += "\nModificadores:\n";
            foreach (var mod in statModifiers)
            {
                string valueText = mod.valueType == ModifierValueType.Percentage 
                    ? $"{mod.value:P0}" 
                    : $"{mod.value:F0}";
                    
                text += $"- {mod.statType}: {(mod.value >= 0 ? "+" : "")}{valueText}\n";
            }
        }

        return text;
    }
}