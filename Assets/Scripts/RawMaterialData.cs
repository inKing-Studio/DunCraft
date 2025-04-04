using UnityEngine;

[CreateAssetMenu(fileName = "New Raw Material", menuName = "Inventory/Raw Material")]
public class RawMaterialData : ItemData
{
    private void OnEnable()
    {
        // Generar calidad aleatoria al crear una nueva instancia
        if (quality <= 0)
        {
            quality = QualitySystem.GenerateRandomQuality();
            rarity = QualitySystem.GetRarityFromQuality(quality);
        }
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
            text += "\\nModificadores:\\n";
            foreach (var mod in statModifiers)
            {
                string valueText = mod.valueType == ModifierValueType.Percentage 
                    ? $"{mod.value:P0}" 
                    : $"{mod.value:F0}";
                    
                text += $"- {mod.statType}: {(mod.value >= 0 ? "+" : "")}{valueText}\\n";
            }
        }

        return text;
    }
}