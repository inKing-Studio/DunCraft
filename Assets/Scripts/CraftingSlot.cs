using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using TMPro; // Added back for TextMeshProUGUI
public class CraftingSlot : InventorySlot, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public enum SlotType
    {
        Material,
        Recipe,
        Result
    }    

    public SlotType slotType;
    public TextMeshProUGUI itemNameText; // Added back
    
    new void Awake() // Use 'new' to hide the base class Awake
    {
        base.Awake(); // Call base class Awake to initialize base members
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (currentItem != null && !isDragging && slotType != SlotType.Result)
        {
            isPointerDown = true;
            isDragging = true;
            InventoryManager.Instance.BeginDragOperation(this, eventData);
            if (icon != null)
            {
                icon.gameObject.SetActive(false);
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isPointerDown)
        {
            isPointerDown = false;
            isDragging = false;
            InventoryManager.Instance.EndDragOperation(eventData);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentItem != null)
        {
            InventoryManager.Instance.ShowTooltip(currentItem);
        }
        InventoryManager.Instance.OnSlotHovered(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.Instance.HideTooltip();
        InventoryManager.Instance.OnSlotExited(this);
    }

    public bool IsValidForItem(ItemData item)
    {
        switch (slotType)
        {
            case SlotType.Recipe:
                return item is RecipeScroll;
            case SlotType.Material:
                return IsValidMaterial(item);
            case SlotType.Result:
                return false; // El slot de resultado no acepta items arrastrados
            default:
                return false;
        }
    }

    private bool IsValidMaterial(ItemData item)
    {
        return item.materialCategory == MaterialCategory.MetalIngot ||
               item.materialCategory == MaterialCategory.ProcessedLeather ||
               item.materialCategory == MaterialCategory.ProcessedGem ||
               item.materialCategory == MaterialCategory.ProcessedFabric ||
               item.materialCategory == MaterialCategory.WoodPlank;
    }

    public void SetItem(ItemData item)
    {
        currentItem = item;
        if (item != null)
        {
            icon.sprite = item.icon;
            icon.gameObject.SetActive(true);
            if (itemNameText != null)
            {
                itemNameText.text = item.itemName;
            }
            UpdateSlotColor();

            // Notificar a la estación de crafteo
            if (slotType == SlotType.Recipe)
            {
                CraftingStation.Instance.OnRecipeSlotUpdated(item);
            }
            else if (slotType == SlotType.Material)
            {
                CraftingStation.Instance.OnMaterialSlotUpdated();
            }
        }
    }

    public void ClearSlot()
    {
        currentItem = null;
        icon.sprite = null;
        icon.gameObject.SetActive(false);
        if (itemNameText != null)
        {
            itemNameText.text = "";
        }
        if (slotImage != null)
        {
            slotImage.color = slotDefaultColor;
        }

        // Notificar a la estación de crafteo
        if (slotType == SlotType.Recipe)
        {
            CraftingStation.Instance.OnRecipeSlotUpdated(null);
        }
        else if (slotType == SlotType.Material)
        {
            CraftingStation.Instance.OnMaterialSlotUpdated();
        }
    }

    private void UpdateSlotColor()
    {
        if (currentItem != null && slotImage != null)
        {
            Color rarityColor = GetRarityColor(currentItem.rarity);
            slotImage.color = new Color(rarityColor.r, rarityColor.g, rarityColor.b, slotDefaultColor.a);
        }
    }

    private Color GetRarityColor(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common:
                return Color.white;
            case Rarity.Uncommon:
                return Color.green;
            case Rarity.Rare:
                return Color.blue;
            case Rarity.Epic:
                return new Color(0.5f, 0f, 0.5f); // Púrpura
            default:
                return Color.white;
        }
    }

    public ItemData GetItem()
    {
        return currentItem;
    }
}