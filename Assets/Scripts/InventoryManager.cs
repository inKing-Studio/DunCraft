using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Linq; // Needed for FirstOrDefault

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("Inventory Setup")]
    public GameObject inventoryPanel;
    public Transform slotsParent;
    public GameObject slotPrefab;
    public int inventorySize = 25;
    public int gridWidth = 5; // IMPORTANT: Set this to match your inventory grid width

    [Header("Dragging Setup")]
    public Canvas canvas;

    private List<InventorySlot> slots = new List<InventorySlot>();
    private List<InventorySlotUI> slotUIs = new List<InventorySlotUI>(); // Store UI references
    private GameObject draggedItemObj;
    private Image draggedItemImage;
    private RectTransform draggedItemRect;
    private InventorySlot sourceSlot; // Changed type to InventorySlot
    private InventorySlot hoveredSlot; // Changed type to InventorySlot
    private ItemTooltip tooltip;
    private bool isDragging = false;
    private ItemData draggedItem;

    // Keep track of which slots are highlighted by which scroll source
    private Dictionary<InventorySlot, InventorySlot> recipeHighlightSources = new Dictionary<InventorySlot, InventorySlot>();

    private void Awake()
    {
        Instance = this;
        SetupInventory();
        tooltip = FindAnyObjectByType<ItemTooltip>(); // Use FindAnyObjectByType for modern Unity
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
        // Clear existing lists if any (e.g., during hot reload)
        slots.Clear();
        slotUIs.Clear();
        recipeHighlightSources.Clear();

        for (int i = 0; i < inventorySize; i++)
        {
            GameObject slotGO = Instantiate(slotPrefab, slotsParent);
            InventorySlot slot = slotGO.GetComponent<InventorySlot>();
            InventorySlotUI slotUI = slotGO.GetComponent<InventorySlotUI>(); // Get UI component

            if (slot != null && slotUI != null)
            {
                slot.slotIndex = i; // Assign index to the slot itself
                slots.Add(slot);
                slotUIs.Add(slotUI);
            }
            else
            {
                Debug.LogError($"Slot prefab is missing InventorySlot or InventorySlotUI component at index {i}", slotGO);
            }
        }
    }

    public bool AddItem(ItemData item)
    {
        InventorySlot emptySlot = slots.FirstOrDefault(s => s.GetItem() == null);
        if (emptySlot != null)
        {
            SetItemToSlot(emptySlot, item); // Use SetItemToSlot to handle highlighting
            return true;
        }
        return false;
    }

    // --- Drag and Drop Logic ---

    public void BeginDragOperation(InventorySlot slot, PointerEventData eventData) // Changed parameter type
    {
        draggedItem = slot.GetItem();
        if (draggedItem == null) return;

        // Clear highlights caused by this item before dragging
        ClearRecipeHighlightsAroundSlot(slot);

        sourceSlot = slot;
        isDragging = true;
        slot.HideIcon(); // Assumes InventorySlot has HideIcon

        // Create dragged item visual
        draggedItemObj = new GameObject("DraggedItem");
        draggedItemObj.transform.SetParent(canvas.transform, false); // Set worldPositionStays to false
        draggedItemObj.transform.SetAsLastSibling(); // Ensure it renders on top

        draggedItemRect = draggedItemObj.AddComponent<RectTransform>();
        draggedItemRect.sizeDelta = new Vector2(80, 80); // Adjust size as needed
        draggedItemRect.localScale = Vector3.one; // Use scale 1

        draggedItemImage = draggedItemObj.AddComponent<Image>();
        draggedItemImage.sprite = draggedItem.icon;
        draggedItemImage.preserveAspect = true;
        draggedItemImage.raycastTarget = false; // Prevent blocking raycasts to slots

        // Initial position update
        draggedItemRect.position = Input.mousePosition;

        if (tooltip != null) tooltip.HideTooltip();
    }

    public void EndDragOperation(PointerEventData eventData)
    {
        if (!isDragging) return;

        ItemData originalSourceItem = draggedItem; // Store before potential changes
        InventorySlot originalSourceSlot = sourceSlot; // Store before potential changes

        bool refinementOccurred = false;
        bool swapOccurred = false;

        if (hoveredSlot != null && hoveredSlot != originalSourceSlot) // Check if dropped on a valid, different slot
        {
            ItemData targetItem = hoveredSlot.GetItem();

            // --- Refinement Check ---
            if (draggedItem is RawMaterialData sourceRaw && targetItem is RawMaterialData targetRaw)
            {
                if (RefinementSystem.Instance != null && RefinementSystem.Instance.CanRefine(sourceRaw, targetRaw))
                {
                    ItemData refinedItem = RefinementSystem.Instance.RefineItems(sourceRaw, targetRaw);
                    if (refinedItem != null)
                    {
                        // Refinement successful
                        ClearSlotInternal(originalSourceSlot); // Clear source slot data
                        ClearSlotInternal(hoveredSlot);      // Clear target slot data
                        SetItemInternal(hoveredSlot, refinedItem); // Place refined item

                        refinementOccurred = true;
                        Debug.Log($"Refinement successful: {refinedItem.itemName}");
                    }
                    else { Debug.LogError("Refinement failed unexpectedly."); }
                }
            }

            // --- Normal Swap (if no refinement) ---
            if (!refinementOccurred)
            {
                // Clear highlights around target slot before placing new item
                ClearRecipeHighlightsAroundSlot(hoveredSlot);

                // Perform the swap internally first
                SetItemInternal(originalSourceSlot, targetItem);
                SetItemInternal(hoveredSlot, originalSourceItem);
                swapOccurred = true;
                Debug.Log("Performed item swap.");
            }
        }

        // --- Update Visuals and Highlights ---
        if (refinementOccurred)
        {
            originalSourceSlot.UpdateVisuals(); // Update source slot (now empty)
            hoveredSlot.UpdateVisuals();      // Update target slot (now has refined item)
            UpdateRecipeHighlightsAroundSlot(hoveredSlot); // Apply highlights for refined item if it's a scroll
        }
        else if (swapOccurred)
        {
            originalSourceSlot.UpdateVisuals(); // Update source slot (now has target item)
            hoveredSlot.UpdateVisuals();      // Update target slot (now has source item)
            // Update highlights for both slots involved in the swap
            UpdateRecipeHighlightsAroundSlot(originalSourceSlot);
            UpdateRecipeHighlightsAroundSlot(hoveredSlot);
        }
        else // Dragged ended outside or on the same slot
        {
            originalSourceSlot.ShowIcon(); // Make original item visible again
            // Re-apply highlights for the original item since it didn't move successfully
            UpdateRecipeHighlightsAroundSlot(originalSourceSlot);
        }

        // --- Cleanup ---
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
        // Keep hoveredSlot null unless explicitly set by OnSlotHovered
    }

    // --- Slot Interaction Callbacks ---

    public void OnSlotHovered(InventorySlot slot) // Changed parameter type
    {
        hoveredSlot = slot;
        if (!isDragging && slot.GetItem() != null)
        {
            ShowTooltip(slot.GetItem());
        }
    }

    public void OnSlotExited(InventorySlot slot) // Changed parameter type
    {
        if (hoveredSlot == slot)
        {
            hoveredSlot = null;
        }
        HideTooltip();
    }

    // --- Tooltip ---

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

    // --- Item Management (Internal Helpers for Swap/Refine) ---

    // Sets item data internally WITHOUT triggering highlight updates immediately
    private void SetItemInternal(InventorySlot slot, ItemData item)
    {
        if (slot != null)
        {
            slot.SetItem(item); // Assumes InventorySlot has SetItemInternal
        }
    }

    // Clears slot data internally WITHOUT triggering highlight updates immediately
    private void ClearSlotInternal(InventorySlot slot)
    {
        if (slot != null)
        {
            slot.ClearSlot(); // Assumes InventorySlot has ClearSlotInternal
        }
    }


    // --- Public Item Management (Use these for adding/removing items normally) ---

    public void SetItemToSlot(InventorySlot slot, ItemData item)
    {
        if (slot == null) return;

        // Clear any highlights potentially caused by an item previously in this slot
        ClearRecipeHighlightsAroundSlot(slot);

        // Set the item and update visuals
        slot.SetItem(item); // Assumes SetItem updates visuals

        // Apply new highlights if the placed item is a recipe scroll
        UpdateRecipeHighlightsAroundSlot(slot);
    }

    public void ClearSlot(InventorySlot slot)
    {
        if (slot == null) return;

        // Clear highlights caused by the item in this slot before clearing
        ClearRecipeHighlightsAroundSlot(slot);

        // Clear the item and update visuals
        slot.ClearSlot(); // Assumes ClearSlot updates visuals
    }


    // --- Recipe Highlight Logic ---

    private void UpdateRecipeHighlightsAroundSlot(InventorySlot sourceSlot)
    {
        if (sourceSlot == null) return;

        ItemData item = sourceSlot.GetItem();
        if (item is ScrollData scroll && scroll.consumableType == ConsumableType.RecipeScroll)
        {
            int sourceIndex = sourceSlot.slotIndex;
            int sourceRow = sourceIndex / gridWidth;
            int sourceCol = sourceIndex % gridWidth;

            foreach (var materialNeeded in scroll.requiredMaterials)
            {
                int targetCol = sourceCol + materialNeeded.relativeSlotPosition.x;
                int targetRow = sourceRow + materialNeeded.relativeSlotPosition.y;

                // Check bounds
                if (targetCol >= 0 && targetCol < gridWidth && targetRow >= 0 && targetRow < (inventorySize / gridWidth))
                {
                    int targetIndex = targetRow * gridWidth + targetCol;
                    InventorySlot targetSlot = GetSlotByIndex(targetIndex);
                    InventorySlotUI targetSlotUI = GetSlotUIByIndex(targetIndex);

                    if (targetSlot != null && targetSlotUI != null)
                    {
                        // Check if another scroll is already highlighting this slot
                        if (recipeHighlightSources.TryGetValue(targetSlot, out InventorySlot existingSource) && existingSource != sourceSlot)
                        {
                            // Another scroll has priority, maybe log a warning or decide on override logic
                            // Debug.LogWarning($"Slot {targetIndex} already highlighted by scroll in slot {existingSource.slotIndex}. Ignoring highlight from slot {sourceIndex}.");
                        }
                        else
                        {
                            // Apply highlight and record the source
                            targetSlotUI.UpdateRecipeRequirementHighlight(materialNeeded.category);
                            recipeHighlightSources[targetSlot] = sourceSlot; // Record which scroll highlighted this slot
                        }
                    }
                }
            }
        }
    }

    private void ClearRecipeHighlightsAroundSlot(InventorySlot sourceSlot)
    {
        if (sourceSlot == null) return;

        ItemData item = sourceSlot.GetItem(); // Get item *before* clearing/moving
        // Check if the item *was* a recipe scroll OR if we are clearing highlights regardless
         if (item is ScrollData scroll && scroll.consumableType == ConsumableType.RecipeScroll)
         {
            // Iterate through all slots that *might* have been highlighted by this source scroll
            List<InventorySlot> slotsToClear = new List<InventorySlot>();
            foreach (var kvp in recipeHighlightSources)
            {
                if (kvp.Value == sourceSlot) // If this source scroll was responsible for the highlight
                {
                    slotsToClear.Add(kvp.Key);
                }
            }

            // Now clear the highlights and remove the tracking entries
            foreach (InventorySlot slotToClear in slotsToClear)
            {
                InventorySlotUI slotUIToClear = GetSlotUIByIndex(slotToClear.slotIndex);
                if (slotUIToClear != null)
                {
                    slotUIToClear.ResetToDefaultAppearance();
                }
                recipeHighlightSources.Remove(slotToClear); // Remove tracking entry
            }
         }
         // Also handle the case where the sourceSlot itself was highlighted by *another* scroll
         if (recipeHighlightSources.ContainsKey(sourceSlot))
         {
             InventorySlotUI sourceSlotUI = GetSlotUIByIndex(sourceSlot.slotIndex);
             if (sourceSlotUI != null)
             {
                 sourceSlotUI.ResetToDefaultAppearance();
             }
             recipeHighlightSources.Remove(sourceSlot);
         }
    }


    // --- Helper Methods ---

    private InventorySlot GetSlotByIndex(int index)
    {
        if (index >= 0 && index < slots.Count)
        {
            return slots[index];
        }
        return null;
    }

     private InventorySlotUI GetSlotUIByIndex(int index)
    {
        if (index >= 0 && index < slotUIs.Count)
        {
            return slotUIs[index];
        }
        return null;
    }
}