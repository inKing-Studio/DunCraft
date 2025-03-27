using UnityEngine;

public class TestInventorySetup : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public ItemData itemTemplate; // Ahora es una plantilla
    public int numberOfItemsToGenerate = 10;

    void Start()
    {
        if (inventoryManager == null)
            inventoryManager = FindAnyObjectByType<InventoryManager>();

        if (itemTemplate != null)
        {
            // Generar varios items con diferentes calidades y rarezas
            for (int i = 0; i < numberOfItemsToGenerate; i++)
            {
                ItemData newItem = itemTemplate.CreateInstance();
                inventoryManager.AddItem(newItem);
            }
        }
        else
        {
            Debug.LogWarning("Por favor asigna un ItemData template en el inspector");
        }
    }
}