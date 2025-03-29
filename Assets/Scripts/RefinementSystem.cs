using UnityEngine;
using System.Collections.Generic;

public class RefinementSystem : MonoBehaviour
{
    public static RefinementSystem Instance { get; private set; }

    [Header("Prefabs de Items Refinados")]
    public RefinedMaterialData metalIngotPrefab;
    public RefinedMaterialData processedLeatherPrefab;
    public RefinedMaterialData processedGemPrefab;
    public RefinedMaterialData processedFabricPrefab;
    public RefinedMaterialData woodPlankPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool CanRefine(RawMaterialData item1, RawMaterialData item2)
    {
        if (item1 == null || item2 == null) return false;

        // Si son del mismo material exacto, siempre se pueden refinar
        if (item1.itemName == item2.itemName) return true;

        // Para materiales diferentes, verificar reglas específicas
        switch (item1.category)
        {
            case MaterialCategory.MetalOre:
            case MaterialCategory.FiberRaw:
                // Metales y fibras pueden ser diferentes tipos
                return item1.category == item2.category;

            case MaterialCategory.WoodTrunk:
            case MaterialCategory.AnimalSkin:
            case MaterialCategory.CrystalRaw:
                // Estos deben ser exactamente el mismo tipo
                return item1.itemName == item2.itemName;

            default:
                return false;
        }
    }

    public ItemData RefineItems(RawMaterialData item1, RawMaterialData item2)
    {
        if (!CanRefine(item1, item2)) return null;

        // Calcular la calidad promedio
        float averageQuality = QualitySystem.CalculateAverageQuality(item1.quality, item2.quality);

        // Obtener el tipo de material refinado basado en los materiales crudos
        RefinedMaterialData refinedPrefab = GetRefinedPrefab(item1.category);
        if (refinedPrefab == null) return null;

        // Crear una nueva instancia del material refinado
        RefinedMaterialData refinedItem = Instantiate(refinedPrefab);
        refinedItem.usedMaterials.Add(item1);
        refinedItem.usedMaterials.Add(item2);

        // Establecer la calidad y rareza basada en la calidad
        refinedItem.quality = averageQuality;
        refinedItem.rarity = QualitySystem.GetRarityFromQuality(averageQuality);

        // Si los materiales son diferentes, personalizar el nombre y descripción
        if (item1.itemName != item2.itemName)
        {
            switch (item1.category)
            {
                case MaterialCategory.MetalOre:
                    refinedItem.itemName = $"Aleación de {item1.itemName.Replace(" Ore", "")} y {item2.itemName.Replace(" Ore", "")}";
                    refinedItem.description = $"Una aleación {QualitySystem.GetQualityDescription(averageQuality).ToLower()} " +
                                           $"creada a partir de {item1.itemName} y {item2.itemName}.\n" +
                                           $"Calidad: {averageQuality:F1}";
                    break;
                case MaterialCategory.FiberRaw:
                    refinedItem.itemName = $"Tejido Mixto de {item1.itemName} y {item2.itemName}";
                    refinedItem.description = $"Un tejido {QualitySystem.GetQualityDescription(averageQuality).ToLower()} " +
                                           $"creado mezclando {item1.itemName} y {item2.itemName}.\n" +
                                           $"Calidad: {averageQuality:F1}";
                    break;
            }
        }
        else
        {
            refinedItem.description = $"Material refinado {QualitySystem.GetQualityDescription(averageQuality).ToLower()} " +
                                    $"creado a partir de {item1.itemName}.\n" +
                                    $"Calidad: {averageQuality:F1}";
        }
        
        // Actualizar las propiedades del item refinado
        refinedItem.UpdateFromRawMaterials();

        return refinedItem;
    }

    private RefinedMaterialData GetRefinedPrefab(MaterialCategory category)
    {
        switch (category)
        {
            case MaterialCategory.MetalOre:
                return metalIngotPrefab;
            case MaterialCategory.AnimalSkin:
                return processedLeatherPrefab;
            case MaterialCategory.CrystalRaw:
                return processedGemPrefab;
            case MaterialCategory.FiberRaw:
                return processedFabricPrefab;
            case MaterialCategory.WoodTrunk:
                return woodPlankPrefab;
            default:
                return null;
        }
    }

    private MaterialCategory GetRefinedCategory(MaterialCategory rawCategory)
    {
        switch (rawCategory)
        {
            case MaterialCategory.MetalOre:
                return MaterialCategory.MetalIngot;
            case MaterialCategory.AnimalSkin:
                return MaterialCategory.ProcessedLeather;
            case MaterialCategory.CrystalRaw:
                return MaterialCategory.ProcessedGem;
            case MaterialCategory.FiberRaw:
                return MaterialCategory.ProcessedFabric;
            case MaterialCategory.WoodTrunk:
                return MaterialCategory.WoodPlank;
            default:
                return rawCategory;
        }
    }
}