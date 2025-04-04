using UnityEngine;
using System.Collections.Generic;
using System.Linq; // Added for LINQ operations if needed later, and for clarity
using System; // Added for Enum.HasFlag if needed

public struct MaterialsNeeded 
{
    public MaterialCategory category;
    public Vector2Int relativeSlotPosition;
} 

[CreateAssetMenu(fileName = "New Scroll", menuName = "DunCraft/Scroll")]
public class ScrollData : ConsumableData
{

    [Header("Recipe Properties")]
    [Tooltip("Solo se usa si el scroll es de tipo RecipeScroll")]
    public List<MaterialsNeeded> requiredMaterials = new List<MaterialsNeeded>();

    [Header("Identification Properties")]
    [Tooltip("Porcentaje de información que revela el scroll de identificación")]
    [Range(0, 100)]
    public float identificationPercentage = 25f;

    public void RandomizeRecipePositions()
    {
        if (consumableType != ConsumableType.RecipeScroll) return;

        // Crear una lista de todas las posiciones posibles (3x3 excluyendo el centro)
        List<Vector2Int> allPositions = new List<Vector2Int>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue; // Excluir el centro
                allPositions.Add(new Vector2Int(x, y));
            }
        }

        // Asegurarse de que tenemos suficientes posiciones
        // Use requiredMaterials.Count as the source of truth for required positions
        int requiredPositions = Mathf.Min(requiredMaterials.Count, 8); 
        requiredMaterials.Clear(); // Clear existing positions before assigning new ones

        // Seleccionar posiciones aleatorias
        for (int i = 0; i < requiredPositions; i++)
        {
            if (allPositions.Count == 0) break; // Avoid error if more materials than positions
            int randomIndex = UnityEngine.Random.Range(0, allPositions.Count);
            requiredMaterials.Add(new MaterialsNeeded { relativeSlotPosition = allPositions[randomIndex] });
            allPositions.RemoveAt(randomIndex);
        }
    }

    public float GetIdentificationRevealPercentage()
    {
        return consumableType == ConsumableType.IdentificationScroll ? identificationPercentage : 0f;
    }

    public override string GetTooltipText()
    {
        string text = base.GetTooltipText();

        // Agregar información específica según el tipo de scroll
        switch (consumableType)
        {
            case ConsumableType.RecipeScroll:
                text += "\nMateriales requeridos:";
                if (requiredMaterials.Count == 0)
                {
                    text += "\n- Ninguno";
                }
                else
                {
                    // Iterate through required materials (which are now flags)
                    foreach (var materialFlags in requiredMaterials)
                    {
                        // Get the string representation (e.g., "MetalIngot, WoodPlank")
                        string categories = materialFlags.ToString();
                        
                        // Replace comma with " or " for better readability
                        categories = categories.Replace(", ", " o "); 

                        text += $"\n- {categories}";
                    }
                }
                break;

            case ConsumableType.IdentificationScroll:
                text += $"\nRevela {identificationPercentage:F0}% de la información";
                break;
        }

        return text;
    }
}