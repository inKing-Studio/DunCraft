using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public int gridWidth = 5;
    public int gridHeight = 5;
    public GameObject slotPrefab;
    public Transform inventoryParent;
    public List<InventorySlot> slots;

    private InventorySlot selectedSlot; // Slot actualmente seleccionado

    void Start()
    {
        CreateInventoryGrid();
    }

    void CreateInventoryGrid()
    {
        slots = new List<InventorySlot>();
        for (int i = 0; i < gridWidth * gridHeight; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, inventoryParent);
            InventorySlot slot = newSlot.GetComponent<InventorySlot>();
            slot.Initialize(i, this);
            slots.Add(slot);
        }
    }

    public bool AddItem(Item item)
    {
        // Buscar un slot vacío
        foreach (var slot in slots)
        {
            if (slot.currentItem == null)
            {
                slot.SetItem(item);
                return true;
            }
        }
        Debug.Log("Inventario lleno!");
        return false; // Inventario lleno
    }

    public void SlotClicked(InventorySlot slot)
    {
        Debug.Log("Slot clicked: " + slot.slotIndex);
        selectedSlot = slot;
        // Aquí implementaremos la lógica de selección de items para crafting, equipamiento, etc.
    }

    // Funciones futuras para remover items, mover items, lógica de crafting, etc.
}