using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CraftingSlot : MonoBehaviour, IDropHandler
{
    public enum SlotType
    {
        Material,
        Recipe,
        Result
    }

    public SlotType slotType;
    public Image icon;
    public TextMeshProUGUI itemNameText;
    private ItemData currentItem;
    private Image slotImage;
    private Color slotDefaultColor;

    void Awake()
    {
        slotImage = GetComponent<Image>();
        if (slotImage != null)
        {
            slotDefaultColor = slotImage.color;
        }
        icon.gameObject.SetActive(false);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        DraggedItem draggedItem = eventData.pointerDrag.GetComponent<DraggedItem>();
        if (draggedItem == null)
            return;

        // Validar según el tipo de slot
        switch (slotType)
        {
            case SlotType.Recipe:
                if (draggedItem.ItemData is RecipeScroll)
                {
                    HandleItemPlacement(draggedItem);
                    CraftingStation.Instance.OnRecipeSlotUpdated(draggedItem.ItemData);
                }
                break;

            case SlotType.Material:
                if (IsValidMaterial(draggedItem.ItemData))
                {
                    HandleItemPlacement(draggedItem);
                    CraftingStation.Instance.OnMaterialSlotUpdated();
                }
                break;

            case SlotType.Result:
                // El slot de resultado no acepta items arrastrados
                break;
        }
    }

    private bool IsValidMaterial(ItemData item)
    {
        // Verificar si el item es un material refinado
        return item.Category == MaterialCategory.MetalIngot ||
               item.Category == MaterialCategory.ProcessedLeather ||
               item.Category == MaterialCategory.ProcessedGem ||
               item.Category == MaterialCategory.ProcessedFabric ||
               item.Category == MaterialCategory.WoodPlank;
    }

    private void HandleItemPlacement(DraggedItem draggedItem)
    {
        if (currentItem != null)
        {
            // Intercambiar items
            ItemData tempItem = currentItem;
            SetItem(draggedItem.ItemData);
            draggedItem.OriginalSlot.SetItem(tempItem);
        }
        else
        {
            // Colocar nuevo item
            SetItem(draggedItem.ItemData);
            draggedItem.OriginalSlot.ClearSlot();
        }
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
    }

    private void UpdateSlotColor()
    {
        if (currentItem != null && slotImage != null)
        {
            Color rarityColor = GetRarityColor(currentItem.Rarity);
            slotImage.color = new Color(rarityColor.r, rarityColor.g, rarityColor.b, 0.3f);
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