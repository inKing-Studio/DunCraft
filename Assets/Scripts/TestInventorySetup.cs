using UnityEngine;
using System.Collections.Generic;

public class TestInventorySetup : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public List<ItemData> testItems = new List<ItemData>();
    
    [Range(1, 25)]
    public int numberOfItemsToGenerate = 10;

    [Header("Controles")]
    [SerializeField] private KeyCode generateItemsKey = KeyCode.G;
    [SerializeField] private KeyCode clearInventoryKey = KeyCode.C;

    void Start()
    {
        if (inventoryManager == null)
            inventoryManager = Object.FindAnyObjectByType<InventoryManager>();

        if (testItems.Count > 0)
        {
            GenerateRandomItems();
        }
        else
        {
            Debug.LogWarning("Por favor asigna algunos ItemData en el inspector");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(generateItemsKey))
        {
            GenerateRandomItems();
            Debug.Log($"Generados {numberOfItemsToGenerate} items aleatorios");
        }

        if (Input.GetKeyDown(clearInventoryKey))
        {
            ClearInventory();
            Debug.Log("Inventario limpiado");
        }
    }

    private void GenerateRandomItems()
    {
        if (testItems.Count == 0) return;

        ClearInventory();

        for (int i = 0; i < numberOfItemsToGenerate; i++)
        {
            int randomIndex = Random.Range(0, testItems.Count);
            ItemData selectedItem = testItems[randomIndex];

            // Crear una instancia del mismo tipo que el item seleccionado
            ItemData newItem;
            if (selectedItem is RawMaterialData)
            {
                var newRawMaterial = ScriptableObject.CreateInstance<RawMaterialData>();
                newRawMaterial.category = (selectedItem as RawMaterialData).category;
                newItem = newRawMaterial;
                Debug.Log($"Creado material en bruto: {selectedItem.itemName} (Categoría: {newRawMaterial.category})");
            }
            else
            {
                newItem = ScriptableObject.CreateInstance<ItemData>();
                Debug.Log($"Creado item normal: {selectedItem.itemName}");
            }

            // Copiar propiedades comunes
            newItem.itemName = selectedItem.itemName;
            newItem.description = selectedItem.description;
            newItem.icon = selectedItem.icon;
            newItem.rarity = selectedItem.rarity;
            newItem.quality = Random.Range(0.5f, 1.5f);
            newItem.statModifiers = new List<StatModifier>(selectedItem.statModifiers);

            if (!inventoryManager.AddItem(newItem))
            {
                Debug.Log("Inventario lleno - No se pueden generar más items");
                break;
            }
        }
    }

    private void ClearInventory()
    {
        InventorySlot[] slots = inventoryManager.GetComponentsInChildren<InventorySlot>();
        foreach (var slot in slots)
        {
            slot.ClearSlot();
        }
    }
}