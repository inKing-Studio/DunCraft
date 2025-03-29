using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Scroll", menuName = "DunCraft/Scroll")]
public class ScrollData : ItemData
{
    [Header("Scroll Properties")]
    public ScrollType scrollType;

    [Header("Recipe Properties")]
    [Tooltip("Solo se usa si el scroll es de tipo Recipe")]
    public List<Vector2Int> materialPositions = new List<Vector2Int>();
    public List<RefinedMaterialData> requiredMaterials = new List<RefinedMaterialData>();
    
    [Header("Identification Properties")]
    [Tooltip("Porcentaje de información que revela el scroll de identificación")]
    [Range(0, 100)]
    public float identificationPercentage = 25f;

    [Header("Skill Properties")]
    [Tooltip("ID de la habilidad que contiene el scroll")]
    public int skillID;

    public void RandomizeRecipePositions()
    {
        if (scrollType != ScrollType.Recipe) return;

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
        int requiredPositions = Mathf.Min(materialPositions.Count, 8);
        materialPositions.Clear();

        // Seleccionar posiciones aleatorias
        for (int i = 0; i < requiredPositions; i++)
        {
            int randomIndex = Random.Range(0, allPositions.Count);
            materialPositions.Add(allPositions[randomIndex]);
            allPositions.RemoveAt(randomIndex);
        }
    }

    public float GetIdentificationRevealPercentage()
    {
        return scrollType == ScrollType.Identification ? identificationPercentage : 0f;
    }
}