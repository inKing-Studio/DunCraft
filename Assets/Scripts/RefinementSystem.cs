using UnityEngine;
using System.Collections.Generic;

public class RefinementSystem : MonoBehaviour
{
    public static RefinementSystem Instance { get; private set; }

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

    public ItemData RefineItems(ItemData item1, ItemData item2)
    {
        if (item1 == null || item2 == null)
            return null;

        // Verificar que ambos items son del mismo tipo de material sin refinar
        if (!CanRefine(item1, item2))
            return null;

        // Calcular la calidad promedio
        float averageQuality = (item1.Quality + item2.Quality) / 2f;
        
        // Determinar la rareza basada en la calidad
        Rarity newRarity;
        if (averageQuality <= 25f)
            newRarity = Rarity.Common;
        else if (averageQuality <= 50f)
            newRarity = Rarity.Uncommon;
        else if (averageQuality <= 75f)
            newRarity = Rarity.Rare;
        else
            newRarity = Rarity.Epic;

        // Crear el nuevo item refinado
        ItemData refinedItem = CreateRefinedItem(item1.Category, averageQuality, newRarity);
        
        return refinedItem;
    }

    private bool CanRefine(ItemData item1, ItemData item2)
    {
        // Verificar que ambos items son del mismo tipo base
        bool sameBaseCategory = AreInSameBaseCategory(item1.Category, item2.Category);
        
        // Para pieles, cristales y troncos, deben ser exactamente del mismo tipo
        bool needsSameType = IsCategoryRequiringSameType(item1.Category);
        
        if (needsSameType)
            return sameBaseCategory && item1.Category == item2.Category;
            
        return sameBaseCategory;
    }

    private bool AreInSameBaseCategory(MaterialCategory cat1, MaterialCategory cat2)
    {
        // Metales
        if (IsMetalOre(cat1) && IsMetalOre(cat2)) return true;
        // Pieles
        if (IsSkin(cat1) && IsSkin(cat2)) return true;
        // Cristales
        if (IsCrystal(cat1) && IsCrystal(cat2)) return true;
        // Hilos
        if (IsThread(cat1) && IsThread(cat2)) return true;
        // Troncos
        if (IsTrunk(cat1) && IsTrunk(cat2)) return true;

        return false;
    }

    private bool IsCategoryRequiringSameType(MaterialCategory category)
    {
        return IsSkin(category) || IsCrystal(category) || IsTrunk(category);
    }

    private bool IsMetalOre(MaterialCategory category)
    {
        return category == MaterialCategory.MetalOre;
    }

    private bool IsSkin(MaterialCategory category)
    {
        return category == MaterialCategory.AnimalSkin;
    }

    private bool IsCrystal(MaterialCategory category)
    {
        return category == MaterialCategory.CrystalRaw;
    }

    private bool IsThread(MaterialCategory category)
    {
        return category == MaterialCategory.FiberRaw;
    }

    private bool IsTrunk(MaterialCategory category)
    {
        return category == MaterialCategory.WoodTrunk;
    }

    private ItemData CreateRefinedItem(MaterialCategory baseCategory, float quality, Rarity rarity)
    {
        MaterialCategory refinedCategory = GetRefinedCategory(baseCategory);
        
        // Crear nuevo ScriptableObject
        ItemData refinedItem = ScriptableObject.CreateInstance<ItemData>();
        
        // Configurar propiedades básicas
        refinedItem.Quality = quality;
        refinedItem.Rarity = rarity;
        refinedItem.Category = refinedCategory;
        
        // Actualizar nombre y descripción
        refinedItem.UpdateDescription();
        
        return refinedItem;
    }

    private MaterialCategory GetRefinedCategory(MaterialCategory baseCategory)
    {
        // Convertir categoría base a refinada
        if (IsMetalOre(baseCategory)) return MaterialCategory.MetalIngot;
        if (IsSkin(baseCategory)) return MaterialCategory.ProcessedLeather;
        if (IsCrystal(baseCategory)) return MaterialCategory.ProcessedGem;
        if (IsThread(baseCategory)) return MaterialCategory.ProcessedFabric;
        if (IsTrunk(baseCategory)) return MaterialCategory.WoodPlank;
        
        return baseCategory; // Por defecto retorna la misma categoría
    }
}