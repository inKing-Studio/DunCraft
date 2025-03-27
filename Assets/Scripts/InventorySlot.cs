using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image icon;
    public TextMeshProUGUI itemNameText;
    private ItemData currentItem;
    private Color slotDefaultColor;
    private Image slotImage;

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
        if (eventData.pointerDrag != null)
        {
            DraggedItem draggedItem = eventData.pointerDrag.GetComponent<DraggedItem>();
            if (draggedItem != null)
            {
                // Si este slot ya tiene un item
                if (currentItem != null)
                {
                    // Intentar refinamiento si ambos items son materiales sin refinar
                    if (IsRefinementPossible(currentItem, draggedItem.ItemData))
                    {
                        PerformRefinement(currentItem, draggedItem.ItemData);
                        // Eliminar el item arrastrado de su slot original
                        draggedItem.OriginalSlot.ClearSlot();
                        return;
                    }
                    else
                    {
                        // Si no se puede refinar, intercambiar items
                        ItemData tempItem = currentItem;
                        SetItem(draggedItem.ItemData);
                        draggedItem.OriginalSlot.SetItem(tempItem);
                    }
                }
                else
                {
                    // Si el slot está vacío, colocar el item
                    SetItem(draggedItem.ItemData);
                    draggedItem.OriginalSlot.ClearSlot();
                }
            }
        }
    }

    private bool IsRefinementPossible(ItemData item1, ItemData item2)
    {
        // Verificar si ambos items son materiales sin refinar
        return IsUnrefinedMaterial(item1.Category) && 
               IsUnrefinedMaterial(item2.Category) && 
               AreCompatibleForRefinement(item1, item2);
    }

    private bool IsUnrefinedMaterial(MaterialCategory category)
    {
        return category == MaterialCategory.MetalOre ||
               category == MaterialCategory.AnimalSkin ||
               category == MaterialCategory.CrystalRaw ||
               category == MaterialCategory.FiberRaw ||
               category == MaterialCategory.WoodTrunk;
    }

    private bool AreCompatibleForRefinement(ItemData item1, ItemData item2)
    {
        // Para pieles, cristales y troncos, deben ser del mismo tipo exacto
        bool requiresSameType = item1.Category == MaterialCategory.AnimalSkin ||
                              item1.Category == MaterialCategory.CrystalRaw ||
                              item1.Category == MaterialCategory.WoodTrunk;

        if (requiresSameType)
        {
            return item1.Category == item2.Category;
        }

        // Para metales y fibras, solo necesitan ser del mismo tipo base
        return GetBaseCategory(item1.Category) == GetBaseCategory(item2.Category);
    }

    private MaterialCategory GetBaseCategory(MaterialCategory category)
    {
        if (category == MaterialCategory.MetalOre)
            return MaterialCategory.MetalOre;
        if (category == MaterialCategory.FiberRaw)
            return MaterialCategory.FiberRaw;
        return category;
    }

    private void PerformRefinement(ItemData item1, ItemData item2)
    {
        // Usar el RefinementSystem para crear el nuevo item
        ItemData refinedItem = RefinementSystem.Instance.RefineItems(item1, item2);
        if (refinedItem != null)
        {
            SetItem(refinedItem);
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