using UnityEngine;

public class InventoryTester : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public ItemData[] testItems; // Arrastra aquí los ItemData que quieras probar

    [Header("Test Controls")]
    [SerializeField] private KeyCode addRandomItemKey = KeyCode.Space;
    [SerializeField] private KeyCode clearInventoryKey = KeyCode.C;
    [SerializeField] private KeyCode fillInventoryKey = KeyCode.F;

    private void Update()
    {
        // Añadir un item aleatorio al presionar Espacio
        if (Input.GetKeyDown(addRandomItemKey) && testItems.Length > 0)
        {
            int randomIndex = Random.Range(0, testItems.Length);
            inventoryManager.AddItem(testItems[randomIndex]);
            Debug.Log($"Añadido item: {testItems[randomIndex].itemName}");
        }

        // Limpiar todo el inventario al presionar C
        if (Input.GetKeyDown(clearInventoryKey))
        {
            ClearAllSlots();
            Debug.Log("Inventario limpiado");
        }

        // Llenar el inventario con items aleatorios al presionar F
        if (Input.GetKeyDown(fillInventoryKey))
        {
            FillInventory();
            Debug.Log("Inventario llenado con items aleatorios");
        }
    }

    private void ClearAllSlots()
    {
        InventorySlot[] slots = inventoryManager.GetComponentsInChildren<InventorySlot>();
        foreach (var slot in slots)
        {
            slot.ClearSlot();
        }
    }

    private void FillInventory()
    {
        if (testItems.Length == 0) return;

        InventorySlot[] slots = inventoryManager.GetComponentsInChildren<InventorySlot>();
        foreach (var slot in slots)
        {
            int randomIndex = Random.Range(0, testItems.Length);
            slot.SetItem(testItems[randomIndex]);
        }
    }
}