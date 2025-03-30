using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class RefinementSystem : MonoBehaviour
{
    private static RefinementSystem instance;
    public static RefinementSystem Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<RefinementSystem>();
            }
            return instance;
        }
    }

    [System.Serializable]
    public class RefinedMaterialEntry
    {
        public ItemData refinedData;
        public List<RawMaterialData> requiredMaterials = new List<RawMaterialData>();
    }

    public List<RefinedMaterialEntry> metalIngots = new List<RefinedMaterialEntry>();
    public List<RefinedMaterialEntry> processedLeather = new List<RefinedMaterialEntry>();
    public List<RefinedMaterialEntry> cutGems = new List<RefinedMaterialEntry>();
    public List<RefinedMaterialEntry> processedFabrics = new List<RefinedMaterialEntry>();
    public List<RefinedMaterialEntry> processedWood = new List<RefinedMaterialEntry>();

    public bool CanRefine(RawMaterialData material1, RawMaterialData material2)
    {
        if (material1 == null || material2 == null)
            return false;

        if (material1.category != material2.category)
            return false;

        List<RefinedMaterialEntry> possibleResults = GetPossibleResults(material1.category);
        if (possibleResults == null || possibleResults.Count == 0)
            return false;

        return possibleResults.Any(r => 
            r.requiredMaterials.Count == 2 &&
            ((r.requiredMaterials[0].itemName == material1.itemName && r.requiredMaterials[1].itemName == material2.itemName) ||
             (r.requiredMaterials[0].itemName == material2.itemName && r.requiredMaterials[1].itemName == material1.itemName)));
    }

    public ItemData RefineItems(RawMaterialData material1, RawMaterialData material2)
    {
        List<RefinedMaterialEntry> possibleResults = GetPossibleResults(material1.category);
        if (possibleResults == null || possibleResults.Count == 0)
            return null;

        var recipe = possibleResults.FirstOrDefault(r => 
            r.requiredMaterials.Count == 2 &&
            ((r.requiredMaterials[0].itemName == material1.itemName && r.requiredMaterials[1].itemName == material2.itemName) ||
             (r.requiredMaterials[0].itemName == material2.itemName && r.requiredMaterials[1].itemName == material1.itemName)));

        if (recipe != null)
            return CreateRefinedMaterial(material1, material2, recipe.refinedData);

        return null;
    }

    private List<RefinedMaterialEntry> GetPossibleResults(MaterialCategory category)
    {
        return category switch
        {
            MaterialCategory.MetalOre => metalIngots,
            MaterialCategory.AnimalSkin => processedLeather,
            MaterialCategory.CrystalRaw => cutGems,
            MaterialCategory.FiberRaw => processedFabrics,
            MaterialCategory.WoodTrunk => processedWood,
            _ => null
        };
    }

    private ItemData CreateRefinedMaterial(RawMaterialData material1, RawMaterialData material2, ItemData baseData)
    {
        if (baseData == null)
            return null;

        ItemData refinedItem = ScriptableObject.CreateInstance<ItemData>();
        refinedItem.icon = baseData.icon;
        refinedItem.rarity = baseData.rarity;
        refinedItem.statModifiers = new List<StatModifier>(baseData.statModifiers);
        refinedItem.itemName = baseData.itemName;
        refinedItem.description = baseData.description;
        refinedItem.quality = (material1.quality + material2.quality) / 2;

        return refinedItem;
    }
}