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
        public RefinedMaterialData refinedData;
        public List<RawMaterialData> requiredMaterials = new List<RawMaterialData>();
    }

    // Lista única de todas las recetas de refinamiento
    public List<RefinedMaterialEntry> refinementRecipes = new List<RefinedMaterialEntry>();

    public bool CanRefine(RawMaterialData material1, RawMaterialData material2)
    {
        if (material1 == null || material2 == null)
        {
            Debug.Log("[RefinementSystem] CanRefine - Uno de los materiales es null");
            return false;
        }

        Debug.Log($"[RefinementSystem] CanRefine - Verificando materiales:");
        Debug.Log($"Material 1: {material1.itemName} (Categoría: {material1.materialCategory})");
        Debug.Log($"Material 2: {material2.itemName} (Categoría: {material2.materialCategory})");

        // Buscar una receta que coincida con estos materiales en cualquier orden
        var recipe = FindRecipeForMaterials(material1, material2);
        if (recipe != null)
        {
            Debug.Log($"[RefinementSystem] CanRefine - Receta encontrada para {recipe.refinedData.itemName}");
            return true;
        }

        Debug.Log("[RefinementSystem] CanRefine - No se encontró receta para esta combinación");
        return false;
    }

    public ItemData RefineItems(RawMaterialData material1, RawMaterialData material2)
    {
        Debug.Log($"[RefinementSystem] RefineItems - Intentando refinar:");
        Debug.Log($"Material 1: {material1.itemName} (Categoría: {material1.materialCategory})");
        Debug.Log($"Material 2: {material2.itemName} (Categoría: {material2.materialCategory})");

        var recipe = FindRecipeForMaterials(material1, material2);
        if (recipe == null)
        {
            Debug.Log("[RefinementSystem] RefineItems - No se encontró receta válida");
            return null;
        }

        Debug.Log($"[RefinementSystem] RefineItems - Creando {recipe.refinedData.itemName}");
        return CreateRefinedMaterial(material1, material2, recipe.refinedData);
    }

    private RefinedMaterialEntry FindRecipeForMaterials(RawMaterialData material1, RawMaterialData material2)
    {
        foreach (var recipe in refinementRecipes)
        {
            if (recipe.requiredMaterials.Count != 2) continue;

            // Verificar si los materiales coinciden en cualquier orden
            bool match1 = recipe.requiredMaterials[0].itemName == material1.itemName &&
                         recipe.requiredMaterials[1].itemName == material2.itemName;

            bool match2 = recipe.requiredMaterials[0].itemName == material2.itemName &&
                         recipe.requiredMaterials[1].itemName == material1.itemName;

            if (match1 || match2)
            {
                Debug.Log($"[RefinementSystem] FindRecipeForMaterials - Receta encontrada: {recipe.refinedData.itemName}");
                return recipe;
            }
        }

        Debug.Log("[RefinementSystem] FindRecipeForMaterials - No se encontró receta");
        return null;
    }

    private ItemData CreateRefinedMaterial(RawMaterialData material1, RawMaterialData material2, RefinedMaterialData baseData)
    {
        Debug.Log($"[RefinementSystem] CreateRefinedMaterial - Creando nuevo material refinado");
        
        var refinedItem = ScriptableObject.CreateInstance<RefinedMaterialData>();
        
        // Copiar propiedades básicas
        refinedItem.itemName = baseData.itemName;
        refinedItem.description = baseData.description;
        refinedItem.icon = baseData.icon;
        refinedItem.rarity = baseData.rarity;
        refinedItem.statModifiers = new List<StatModifier>(baseData.statModifiers);
        
        // Establecer categorías
        refinedItem.itemCategory = ItemCategory.Material;
        refinedItem.materialCategory = baseData.materialCategory;
        
        // Calcular calidad promedio
        refinedItem.quality = (material1.quality + material2.quality) / 2;
        
        // Agregar materiales usados
        refinedItem.usedMaterials = new List<RawMaterialData> { material1, material2 };

        Debug.Log($"[RefinementSystem] CreateRefinedMaterial - Material creado: {refinedItem.itemName}");
        Debug.Log($"Categoría: {refinedItem.materialCategory}");
        Debug.Log($"Calidad: {refinedItem.quality}");
        
        return refinedItem;
    }
}