using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotSetup : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject slotPrefab;
    public GameObject itemNamePrefab;
    
    [Header("References")]
    public Transform slotsParent;
    public int inventorySize = 25;
    public int slotsPerRow = 5;
    
    void Start()
    {
        SetupInventorySlots();
    }

    void SetupInventorySlots()
    {
        for (int i = 0; i < inventorySize; i++)
        {
            GameObject slotGO = Instantiate(slotPrefab, slotsParent);
            InventorySlot slot = slotGO.GetComponent<InventorySlot>();
            
            // Configurar el RectTransform para el grid
            RectTransform rectTransform = slotGO.GetComponent<RectTransform>();
            float slotSize = 100f; // Tamaño base del slot
            float spacing = 10f;   // Espacio entre slots
            
            int row = i / slotsPerRow;
            int col = i % slotsPerRow;
            
            rectTransform.anchoredPosition = new Vector2(
                col * (slotSize + spacing),
                -row * (slotSize + spacing)
            );
            
            // Crear y configurar el texto del nombre del item
            if (itemNamePrefab != null)
            {
                GameObject nameGO = Instantiate(itemNamePrefab, slotGO.transform);
                TextMeshProUGUI nameText = nameGO.GetComponent<TextMeshProUGUI>();
                if (nameText != null)
                {
                    slot.itemNameText = nameText;
                }
            }
        }
        
        // Ajustar el tamaño del contenedor padre
        if (slotsParent != null)
        {
            RectTransform parentRect = slotsParent.GetComponent<RectTransform>();
            if (parentRect != null)
            {
                int rows = (inventorySize + slotsPerRow - 1) / slotsPerRow;
                float totalWidth = slotsPerRow * 110f;
                float totalHeight = rows * 110f;
                parentRect.sizeDelta = new Vector2(totalWidth, totalHeight);
            }
        }
    }
}