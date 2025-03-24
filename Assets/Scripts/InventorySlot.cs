using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public int slotIndex; // Índice del slot en la grid
    public Item currentItem;
    public Image icon;
    public Button button; // Opcional, si quieres interacción por botón

    private InventoryManager inventoryManager;

    public void Initialize(int index, InventoryManager manager)
    {
        slotIndex = index;
        inventoryManager = manager;
        UpdateSlot();

        if (button != null)
        {
            button.onClick.AddListener(OnSlotClicked);
        }
    }

    public void SetItem(Item item)
    {
        currentItem = item;
        UpdateSlot();
    }

    public void ClearSlot()
    {
        currentItem = null;
        UpdateSlot();
    }

    private void UpdateSlot()
    {
        if (currentItem != null)
        {
            icon.sprite = currentItem.icon;
            icon.enabled = true;
        }
        else
        {
            icon.sprite = null;
            icon.enabled = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (inventoryManager != null)
        {
            inventoryManager.SlotClicked(this);
        }
    }

    private void OnSlotClicked()
    {
        if (inventoryManager != null)
        {
            inventoryManager.SlotClicked(this);
        }
    }
}