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
    
    [Header("Dragging Setup")]
    public Canvas canvas;
    
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
        Instance = this;
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
        draggedItem = GetItemFromSlot(slot);
        if (draggedItem == null) return;

        Debug.Log($"Comenzando arrastre de: {draggedItem.itemName} (Tipo: {draggedItem.GetType().Name})");
        if (draggedItem is RawMaterialData rawMat)
        {
            Debug.Log($"Es material en bruto. Categoría: {rawMat.category}");
        }
        
        sourceSlot = slot;
        isDragging = true;

        if (slot is InventorySlot invSlot)
            invSlot.HideIcon();

        draggedItemObj = new GameObject("DraggedItem");
        draggedItemObj.transform.SetParent(canvas.transform);
        
        draggedItemRect = draggedItemObj.AddComponent<RectTransform>();
        draggedItemRect.sizeDelta = new Vector2(80, 80);
        draggedItemRect.localScale = Vector3.one * 0.77f;
        draggedItemRect.position = eventData.position;

        draggedItemImage = draggedItemObj.AddComponent<Image>();
        draggedItemImage.sprite = draggedItem.icon;
        draggedItemImage.preserveAspect = true;
        draggedItemImage.raycastTarget = false;
    }

    public void EndDragOperation(PointerEventData eventData)
    {
        if (!isDragging) return;

        Debug.Log("Finalizando operación de arrastre");

        if (hoveredSlot != null && hoveredSlot is InventorySlot)
        {
            ItemData targetItem = GetItemFromSlot(hoveredSlot);
            Debug.Log($"Item en slot destino: {(targetItem != null ? targetItem.itemName : "vacío")} (Tipo: {(targetItem != null ? targetItem.GetType().Name : "null")})");

            // Verificar si ambos items son materiales en bruto
            bool isSourceRaw = draggedItem is RawMaterialData;
            bool isTargetRaw = targetItem is RawMaterialData;
            
            Debug.Log($"Source es RawMaterial: {isSourceRaw}");
            Debug.Log($"Target es RawMaterial: {isTargetRaw}");

            if (sourceSlot != hoveredSlot && isSourceRaw && isTargetRaw)
            {
                var sourceRawMaterial = draggedItem as RawMaterialData;
                var targetRawMaterial = targetItem as RawMaterialData;

                Debug.Log($"Intentando refinar materiales:");
                Debug.Log($"Material 1: {sourceRawMaterial.itemName} (Categoría: {sourceRawMaterial.category})");
                Debug.Log($"Material 2: {targetRawMaterial.itemName} (Categoría: {targetRawMaterial.category})");

                if (RefinementSystem.Instance == null)
                {
                    Debug.LogError("¡RefinementSystem no encontrado!");
                }
                else
                {
                    Debug.Log("RefinementSystem encontrado - Verificando si se pueden refinar");
                    if (RefinementSystem.Instance.CanRefine(sourceRawMaterial, targetRawMaterial))
                    {
                        Debug.Log("Los materiales pueden ser refinados - Iniciando refinamiento");
                        ItemData refinedItem = RefinementSystem.Instance.RefineItems(sourceRawMaterial, targetRawMaterial);
                        
                        if (refinedItem != null)
                        {
                            Debug.Log($"Refinamiento exitoso - Creado: {refinedItem.itemName}");
                            ClearSlot(sourceSlot);
                            ClearSlot(hoveredSlot);
                            SetItemToSlot(hoveredSlot, refinedItem);
                            
                            CleanupDragOperation();
                            return;
                        }
                        else
                        {
                            Debug.LogError("El refinamiento falló - refinedItem es null");
                        }
                    }
                    else
                    {
                        Debug.Log("Los materiales no pueden ser refinados");
                    }
                }
            }

            Debug.Log("Realizando swap normal de items");
            SwapItems(hoveredSlot, targetItem);
        }
        else
        {
            if (sourceSlot is InventorySlot invSlot)
                invSlot.ShowIcon();
        }

        CleanupDragOperation();
    }

    private void CleanupDragOperation()
    {
        if (draggedItemObj != null)
            Destroy(draggedItemObj);
            
        isDragging = false;
        draggedItemObj = null;
        draggedItemRect = null;
        draggedItemImage = null;
        draggedItem = null;
        sourceSlot = null;
        hoveredSlot = null;
    }

    private void SwapItems(MonoBehaviour targetSlot, ItemData targetItem)
    {
        SetItemToSlot(targetSlot, draggedItem);
        if (targetItem != null)
            SetItemToSlot(sourceSlot, targetItem);
        else
            ClearSlot(sourceSlot);
    }

    private ItemData GetItemFromSlot(MonoBehaviour slot)
    {
        if (slot is InventorySlot invSlot)
            return invSlot.GetItem();
        return null;
    }

    private void SetItemToSlot(MonoBehaviour slot, ItemData item)
    {
        if (slot is InventorySlot invSlot)
            invSlot.SetItem(item);
    }

    private void ClearSlot(MonoBehaviour slot)
    {
        if (slot is InventorySlot invSlot)
            invSlot.ClearSlot();
    }

    public void OnSlotHovered(MonoBehaviour slot)
    {
        hoveredSlot = slot;
    }

    public void OnSlotExited(MonoBehaviour slot)
    {
        if (hoveredSlot == slot)
            hoveredSlot = null;
    }

    public void ShowTooltip(ItemData item)
    {
        if (tooltip != null && item != null && !isDragging)
            tooltip.ShowTooltip(item);
    }

    public void HideTooltip()
    {
        if (tooltip != null)
            tooltip.HideTooltip();
    }
}