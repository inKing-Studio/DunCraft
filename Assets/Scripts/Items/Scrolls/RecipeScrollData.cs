using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Recipe Scroll", menuName = "DunCraft/Scrolls/Recipe Scroll")]
public class RecipeScrollData : ConsumableData
{
    [Header("Receta")]
    public List<Vector2Int> materialPositions = new List<Vector2Int>();
    public List<RawMaterialData> requiredMaterials = new List<RawMaterialData>();
    public ItemData resultItem;

    public void RandomizeRecipePositions(int gridSize = 5)
    {
        materialPositions.Clear();
        List<Vector2Int> possiblePositions = new List<Vector2Int>();

        // Crear lista de todas las posiciones posibles
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                possiblePositions.Add(new Vector2Int(x, y));
            }
        }

        // Asegurarse de que hay suficientes posiciones
        int requiredPositions = requiredMaterials.Count;
        if (possiblePositions.Count < requiredPositions)
        {
            Debug.LogError("No hay suficientes posiciones disponibles para todos los materiales");
            return;
        }

        // Seleccionar posiciones aleatorias
        for (int i = 0; i < requiredPositions; i++)
        {
            int randomIndex = Random.Range(0, possiblePositions.Count);
            materialPositions.Add(possiblePositions[randomIndex]);
            possiblePositions.RemoveAt(randomIndex);
        }
    }

    public override string GetTooltipText()
    {
        string text = base.GetTooltipText();
        text += "\n\nReceta para: " + (resultItem != null ? resultItem.itemName : "???");
        text += "\nMateriales necesarios:";
        foreach (var material in requiredMaterials)
        {
            text += $"\n- {material.itemName}";
        }
        return text;
    }
}