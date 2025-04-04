using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Refined Material", menuName = "Inventory/Refined Material")]
public class RefinedMaterialData : ItemData
{
    [Header("Propiedades de Material Refinado")]
    public List<RawMaterialData> usedMaterials = new List<RawMaterialData>();

    public void UpdateFromRawMaterials()
    {
        if (usedMaterials.Count == 0) return;

        // Calcular promedios de las propiedades
        float totalQuality = 0f;

        foreach (var material in usedMaterials)
        {
            totalQuality += material.quality;
        }
        
        // La calidad final es un promedio ponderado de la calidad de los materiales y la pureza
        quality = (totalQuality / usedMaterials.Count);
    }

    private void UpdateStatModifiers(float density, float hardness)
    {
        statModifiers.Clear();

        // Ejemplo de cómo las propiedades afectan los stats
        // Densidad afecta el peso y la defensa
        statModifiers.Add(new StatModifier(
            StatType.Defense,
            ModifierValueType.Percentage,
            (density - 1f) * 0.2f // 20% de bonus por cada punto de densidad sobre 1
        ));

        // Dureza afecta la durabilidad y el daño
        statModifiers.Add(new StatModifier(
            StatType.Attack,
            ModifierValueType.Percentage,
            (hardness - 1f) * 0.15f // 15% de bonus por cada punto de dureza sobre 1
        ));
    }

    public override string GetTooltipText()
    {
        string text = $"{itemName} ({QualitySystem.GetQualityDescription(quality)})\\n";
        text += $"Calidad: {quality:F1}\\n";
        text += $"Rareza: {rarity}\\n";
        text += $"Categoría: {materialCategory}\\n\\n";
        
        if (!string.IsNullOrEmpty(description))
        {
            text += $"{description}\\n\\n";
        }

        if (statModifiers.Count > 0)
        {
            text += "Modificadores:\\n";
            foreach (var mod in statModifiers)
            {
                string valueText = mod.valueType == ModifierValueType.Percentage 
                    ? $"{mod.value:P0}" 
                    : $"{mod.value:F0}";
                    
                text += $"- {mod.statType}: {(mod.value >= 0 ? "+" : "")}{valueText}\\n";
            }
        }
        
        if (usedMaterials.Count > 0)
        {
            text += "\\nMateriales Utilizados:\\n";
            foreach (var material in usedMaterials)
            {
                text += $"- {material.itemName} (Calidad: {material.quality:F1})\\n";
            }
        }
        
        return text;
    }
}