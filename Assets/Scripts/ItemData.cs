using UnityEngine;
using System.Collections.Generic;

public class ItemData : ScriptableObject
{
    [Header("Información Básica")]
    public string itemName;
    public string description;
    public Sprite icon;
    public Rarity rarity;
    public float quality = 1f;
    public MaterialCategory category;

    [Header("Modificadores de Stats")]
    public List<StatModifier> statModifiers = new List<StatModifier>();

    public virtual string GetTooltipText()
    {
        string text = $"{itemName}\n";
        text += $"Calidad: {quality:P0}\n";
        text += $"Rareza: {rarity}\n";
        text += $"Categoría: {category}\n\n";
        
        if (!string.IsNullOrEmpty(description))
        {
            text += $"{description}\n\n";
        }

        if (statModifiers.Count > 0)
        {
            text += "Modificadores:\n";
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