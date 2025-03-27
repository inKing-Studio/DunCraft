using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    
    [Header("Inventory Setup")]
    public GameObject inventoryPanel;
    public Transform slotsParent;
    public GameObject slotPrefab;
    public int inventorySize = 25;
    public int slotsPerRow = 5;
    
    [Header("Dragging Setup")]
    public Canvas canvas;
    public GameObject draggedItemPrefab;
    
    private List<InventorySlot> slots = new List<InventorySlot>();
    private DraggedItem currentDraggedItem;
    private ItemTooltip tooltip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SetupInventory();
        tooltip = FindAnyObjectByType<ItemTooltip>();
    }

    private void SetupInventory()
    {
        // Crear slots
        for (int i = 0; i < inventorySize; i++)
        {
            GameObject slotGO = Instantiate(slotPrefab, slotsParent);
            InventorySlot slot = slotGO.GetComponent<InventorySlot>();
            slots.Add(slot);
        }
    }

    public bool AddItem(ItemData item)
    {
        // Buscar un slot vacío
        foreach (var slot in slots)
        {
            if (slot.GetItem() == null)
            {
                slot.SetItem(item);
                return true;
            }
        }
        return false;
    }

    public void StartDragging(InventorySlot fromSlot)
    {
        if (fromSlot.GetItem() == null)
            return;

        // Crear objeto arrastrado
        GameObject draggedObj = Instantiate(draggedItemPrefab, canvas.transform);
        currentDraggedItem = draggedObj.GetComponent<DraggedItem>();
        
        if (currentDraggedItem != null)
        {
            currentDraggedItem.Initialize(fromSlot.GetItem(), fromSlot);
            fromSlot.ClearSlot();
        }
    }

    public void StopDragging(InventorySlot toSlot)
    {
        if (currentDraggedItem == null)
            return;

        // Si el slot destino está vacío
        if (toSlot.GetItem() == null)
        {
            toSlot.SetItem(currentDraggedItem.ItemData);
        }
        // Si el slot destino tiene un item
        else
        {
            // Intercambiar items
            ItemData tempItem = toSlot.GetItem();
            toSlot.SetItem(currentDraggedItem.ItemData);
            currentDraggedItem.OriginalSlot.SetItem(tempItem);
        }

        Destroy(currentDraggedItem.gameObject);
        currentDraggedItem = null;
    }

    public void CancelDragging()
    {
        if (currentDraggedItem != null)
        {
            currentDraggedItem.OriginalSlot.SetItem(currentDraggedItem.ItemData);
            Destroy(currentDraggedItem.gameObject);
            currentDraggedItem = null;
        }
    }

    public void ShowTooltip(ItemData item)
    {
        if (tooltip != null && item != null)
        {
            tooltip.ShowTooltip(item);
        }
    }

    public void HideTooltip()
    {
        if (tooltip != null)
        {
            tooltip.HideTooltip();
        }
    }
}