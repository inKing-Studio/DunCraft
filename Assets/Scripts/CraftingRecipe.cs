using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Crafting/Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public string recipeName;
    public Sprite recipeIcon;
    
    [System.Serializable]
    public class MaterialRequirement
    {
        public MaterialCategory category;
        public int quantity;
        public bool requiresSameType;
    }
    
    public List<MaterialRequirement> requirements = new List<MaterialRequirement>();
    public GameObject resultPrefab;
    
    [Header("Sinergia")]
    public float baseQualityMultiplier = 1f;
    public float rarityPowerMultiplier = 1f;
    
    public bool CanCraft(List<ItemData> materials)
    {
        if (materials.Count != requirements.Count)
            return false;
            
        // Verificar que cada material cumple con los requisitos
        foreach (var req in requirements)
        {
            int found = 0;
            MaterialCategory lastCategory = MaterialCategory.MetalOre; // valor por defecto
            bool firstFound = true;
            
            foreach (var mat in materials)
            {
                if (IsValidMaterial(mat.category, req.category))
                {
                    if (req.requiresSameType)
                    {
                        if (firstFound)
                        {
                            lastCategory = mat.category;
                            firstFound = false;
                        }
                        else if (mat.category != lastCategory)
                        {
                            continue;
                        }
                    }
                    found++;
                }
            }
            
            if (found < req.quantity)
                return false;
        }
        
        return true;
    }
    
    private bool IsValidMaterial(MaterialCategory material, MaterialCategory requirement)
    {
        // Verificar si el material es del tipo requerido
        switch (requirement)
        {
            case MaterialCategory.MetalIngot:
                return material == MaterialCategory.MetalIngot;
            case MaterialCategory.ProcessedLeather:
                return material == MaterialCategory.ProcessedLeather;
            case MaterialCategory.ProcessedGem:
                return material == MaterialCategory.ProcessedGem;
            case MaterialCategory.ProcessedFabric:
                return material == MaterialCategory.ProcessedFabric;
            case MaterialCategory.WoodPlank:
                return material == MaterialCategory.WoodPlank;
            default:
                return false;
        }
    }
    
    public float CalculateSynergy(List<ItemData> materials)
    {
        float synergyMultiplier = 1f;
        float qualityAverage = 0f;
        float rarityPower = 0f;
        
        // Calcular promedio de calidad y poder de rareza
        foreach (var material in materials)
        {
            qualityAverage += material.quality;
            rarityPower += GetRarityPower(material.rarity);
        }
        
        qualityAverage /= materials.Count;
        rarityPower /= materials.Count;
        
        // Aplicar multiplicadores
        synergyMultiplier *= (qualityAverage / 100f) * baseQualityMultiplier;
        synergyMultiplier *= rarityPower * rarityPowerMultiplier;
        
        return synergyMultiplier;
    }
    
    private float GetRarityPower(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common:
                return 1f;
            case Rarity.Uncommon:
                return 1.5f;
            case Rarity.Rare:
                return 2f;
            case Rarity.Epic:
                return 3f;
            default:
                return 1f;
        }
    }
}