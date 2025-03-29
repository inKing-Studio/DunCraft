using UnityEngine;

public class TestInventorySetup : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public ItemData itemTemplate; // Ahora es una plantilla
    public int numberOfItemsToGenerate = 10;

    void Start()
    {
        if (inventoryManager == null)
            inventoryManager = Object.FindAnyObjectByType<InventoryManager>();

        if (itemTemplate != null)
        {
            // Generar varios items con diferentes calidades y rarezas
            for (int i = 0; i < numberOfItemsToGenerate; i++)
            {
                ItemData newItem = ScriptableObject.CreateInstance<ItemData>();
                // Copiar propiedades del template
                newItem.itemName = itemTemplate.itemName;
                newItem.description = itemTemplate.description;
                newItem.icon = itemTemplate.icon;
                newItem.rarity = itemTemplate.rarity;
                newItem.quality = Random.Range(0.5f, 1.5f);
                newItem.category = itemTemplate.category;
                newItem.statModifiers = new System.Collections.Generic.List<StatModifier>(itemTemplate.statModifiers);

                inventoryManager.AddItem(newItem);
            }
        }
        else
        {
            Debug.LogWarning("Por favor asigna un ItemData template en el inspector");
        }
    }
}