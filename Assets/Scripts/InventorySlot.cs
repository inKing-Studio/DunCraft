using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;
    protected ItemData currentItem; // Changed to protected
    public int slotIndex { get; set; } // Added slotIndex property
    protected Image slotImage; // Changed to protected
    protected Color slotDefaultColor; // Changed to protected
    protected bool isPointerDown = false; // Changed to protected
    protected bool isDragging = false; // Changed to protected

    protected virtual void Awake() // Changed to protected and virtual
    {
        slotImage = GetComponent<Image>();
        if (slotImage != null)
        {
            slotDefaultColor = slotImage.color;
        }
        icon.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (currentItem != null && !isDragging)
        {
            isPointerDown = true;
            isDragging = true;
            InventoryManager.Instance.BeginDragOperation(this, eventData);
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

    public void SetItem(ItemData item)
    {
        currentItem = item;
        if (item != null)
        {
            icon.sprite = item.icon;
            icon.gameObject.SetActive(true);
            UpdateSlotColor();
        }
        else
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        currentItem = null;
        icon.sprite = null;
        icon.gameObject.SetActive(false);
        if (slotImage != null)
        {
            slotImage.color = slotDefaultColor;
        }
    }

    public void HideIcon()
    {
        if (icon != null)
        {
            icon.gameObject.SetActive(false);
        }
    }

    public void ShowIcon()
    {
        if (icon != null && currentItem != null)
        {
            icon.gameObject.SetActive(true);
        }
    }

    public void UpdateVisuals() // Added UpdateVisuals method
    {
        ShowIcon();
        UpdateSlotColor();
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