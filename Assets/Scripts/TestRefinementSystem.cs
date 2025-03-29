using UnityEngine;

public class TestRefinementSystem : MonoBehaviour
{
    public RawMaterialData ironOre;
    public RawMaterialData goldOre;
    public RawMaterialData oakTrunk;
    public RawMaterialData curvyTrunk;
    public RawMaterialData cotton;
    public RawMaterialData silk;

    void Start()
    {
        TestRefinements();
    }

    void TestRefinements()
    {
        // Prueba 1: Refinar dos Iron Ores (debería funcionar)
        Debug.Log("Prueba 1: Refinando dos Iron Ores");
        TestRefine(ironOre, ironOre);

        // Prueba 2: Refinar Iron Ore con Gold Ore (debería funcionar y crear una aleación)
        Debug.Log("Prueba 2: Refinando Iron Ore con Gold Ore");
        TestRefine(ironOre, goldOre);

        // Prueba 3: Refinar Oak Trunk con Curvy Trunk (no debería funcionar)
        Debug.Log("Prueba 3: Refinando Oak Trunk con Curvy Trunk");
        TestRefine(oakTrunk, curvyTrunk);

        // Prueba 4: Refinar dos Oak Trunks (debería funcionar)
        Debug.Log("Prueba 4: Refinando dos Oak Trunks");
        TestRefine(oakTrunk, oakTrunk);

        // Prueba 5: Refinar Cotton con Silk (debería funcionar y crear un tejido mixto)
        Debug.Log("Prueba 5: Refinando Cotton con Silk");
        TestRefine(cotton, silk);
    }

    void TestRefine(RawMaterialData item1, RawMaterialData item2)
    {
        if (RefinementSystem.Instance.CanRefine(item1, item2))
        {
            ItemData result = RefinementSystem.Instance.RefineItems(item1, item2);
            if (result != null)
            {
                Debug.Log($"Refinamiento exitoso: {result.itemName}");
                Debug.Log($"Descripción: {result.description}");
                Debug.Log($"Calidad: {result.quality:P0}");
            }
            else
            {
                Debug.LogError("Error al refinar los materiales");
            }
        }
        else
        {
            Debug.Log($"No se pueden refinar {item1.itemName} con {item2.itemName}");
        }
        Debug.Log("-------------------");
    }
}