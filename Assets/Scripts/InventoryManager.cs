using UnityEngine;
using UnityEngine.UI;
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
    private GameObject draggedItemObj;
    private Image draggedItemImage;
    private RectTransform draggedItemRect;
    private MonoBehaviour sourceSlot;
    private MonoBehaviour hoveredSlot;
    private ItemTooltip tooltip;
    private bool isDragging = false;
    private ItemData draggedItem;

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

    private void Update()
    {
        if (isDragging && draggedItemRect != null)
        {
            draggedItemRect.position = Input.mousePosition;
        }
    }

    private void SetupInventory()
    {
        for (int i = 0; i < inventorySize; i++)
        {
            GameObject slotGO = Instantiate(slotPrefab, slotsParent);
            InventorySlot slot = slotGO.GetComponent<InventorySlot>();
            slots.Add(slot);
        }
    }

    public bool AddItem(ItemData item)
    {
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

    public void BeginDragOperation(MonoBehaviour slot, PointerEventData eventData)
    {
        Debug.Log("BeginDragOperation called");
        draggedItem = GetItemFromSlot(slot);
        if (draggedItem == null || isDragging) return;

        sourceSlot = slot;
        isDragging = true;

        // Ocultar el item en el slot original
        if (slot is InventorySlot invSlot)
            invSlot.HideIcon();
        else if (slot is CraftingSlot craftSlot)
            craftSlot.icon.gameObject.SetActive(false);

        // Crear la imagen de arrastre
        draggedItemObj = Instantiate(draggedItemPrefab, canvas.transform);
        draggedItemRect = draggedItemObj.GetComponent<RectTransform>();
        draggedItemImage = draggedItemObj.GetComponent<Image>();

        // Configurar la imagen
        draggedItemImage.sprite = draggedItem.icon;
        draggedItemImage.preserveAspect = true;
        draggedItemRect.localScale = Vector3.one * 0.77f;
        draggedItemRect.position = eventData.position;
        
        Debug.Log("Drag image created");
    }

    public void EndDragOperation(PointerEventData eventData)
    {
        Debug.Log("EndDragOperation called");
        if (!isDragging) return;

        bool validDrop = false;
        if (hoveredSlot != null)
        {
            Debug.Log($"Hovered slot: {hoveredSlot.GetType().Name}");
            
            // Verificar si el drop es válido
            if (hoveredSlot is CraftingSlot craftSlot)
            {
                validDrop = craftSlot.IsValidForItem(draggedItem);
            }
            else if (hoveredSlot is InventorySlot)
            {
                validDrop = true;
            }

            if (validDrop)
            {
                ItemData targetItem = GetItemFromSlot(hoveredSlot);
                SetItemToSlot(hoveredSlot, draggedItem);

                if (targetItem != null)
                {
                    SetItemToSlot(sourceSlot, targetItem);
                }
                else
                {
                    ClearSlot(sourceSlot);
                }
                Debug.Log("Item swapped successfully");
            }
        }

        if (!validDrop)
        {
            Debug.Log("Invalid drop, returning item");
            if (sourceSlot is InventorySlot invSlot)
                invSlot.ShowIcon();
            else if (sourceSlot is CraftingSlot craftSlot)
                craftSlot.icon.gameObject.SetActive(true);
        }

        // Limpiar
        if (draggedItemObj != null)
        {
            Destroy(draggedItemObj);
        }
        isDragging = false;
        draggedItemObj = null;
        draggedItemRect = null;
        draggedItemImage = null;
        draggedItem = null;
        sourceSlot = null;
        hoveredSlot = null;
    }

    private ItemData GetItemFromSlot(MonoBehaviour slot)
    {
        if (slot is InventorySlot invSlot)
            return invSlot.GetItem();
        if (slot is CraftingSlot craftSlot)
            return craftSlot.GetItem();
        return null;
    }

    private void SetItemToSlot(MonoBehaviour slot, ItemData item)
    {
        if (slot is InventorySlot invSlot)
            invSlot.SetItem(item);
        else if (slot is CraftingSlot craftSlot)
            craftSlot.SetItem(item);
    }

    private void ClearSlot(MonoBehaviour slot)
    {
        if (slot is InventorySlot invSlot)
            invSlot.ClearSlot();
        else if (slot is CraftingSlot craftSlot)
            craftSlot.ClearSlot();
    }

    public void OnSlotHovered(MonoBehaviour slot)
    {
        Debug.Log($"Slot hovered: {slot.GetType().Name}");
        hoveredSlot = slot;
    }

    public void OnSlotExited(MonoBehaviour slot)
    {
        Debug.Log($"Slot exited: {slot.GetType().Name}");
        if (hoveredSlot == slot)
        {
            hoveredSlot = null;
        }
    }

    public void ShowTooltip(ItemData item)
    {
        if (tooltip != null && item != null && !isDragging)
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