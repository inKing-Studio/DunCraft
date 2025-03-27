using UnityEngine;
using System.Collections.Generic;

public class CraftingStation : MonoBehaviour
{
    public static CraftingStation Instance { get; private set; }
    
    [Header("Referencias")]
    public GameObject craftingPanel;
    public Transform materialSlotsParent;
    public CraftingSlot[] materialSlots;
    public CraftingSlot recipeSlot;
    public CraftingSlot resultSlot;

    private CraftingRecipe currentRecipe;
    private bool isRecipeValid;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Inicializar slots
        materialSlots = materialSlotsParent.GetComponentsInChildren<CraftingSlot>();
    }

    public void OnRecipeSlotUpdated(ItemData recipeItem)
    {
        if (recipeItem != null && recipeItem is RecipeScroll)
        {
            RecipeScroll scroll = (RecipeScroll)recipeItem;
            currentRecipe = scroll.recipe;
            ValidateRecipe();
        }
        else
        {
            currentRecipe = null;
            isRecipeValid = false;
            resultSlot.ClearSlot();
        }
    }

    public void OnMaterialSlotUpdated()
    {
        if (currentRecipe != null)
        {
            ValidateRecipe();
        }
    }

    private void ValidateRecipe()
    {
        if (currentRecipe == null)
        {
            isRecipeValid = false;
            return;
        }

        List<ItemData> materials = new List<ItemData>();
        foreach (var slot in materialSlots)
        {
            if (slot.GetItem() != null)
            {
                materials.Add(slot.GetItem());
            }
        }

        isRecipeValid = currentRecipe.CanCraft(materials);
        
        if (isRecipeValid)
        {
            // Calcular sinergias y crear el item resultante
            float synergyMultiplier = currentRecipe.CalculateSynergy(materials);
            // Aquí crearías el item resultante con las estadísticas modificadas por la sinergia
            // y lo mostrarías en el resultSlot
        }
        else
        {
            resultSlot.ClearSlot();
        }
    }

    public void OnCraftButtonPressed()
    {
        if (!isRecipeValid || currentRecipe == null)
            return;

        // Recolectar materiales
        List<ItemData> materials = new List<ItemData>();
        foreach (var slot in materialSlots)
        {
            if (slot.GetItem() != null)
            {
                materials.Add(slot.GetItem());
                slot.ClearSlot();
            }
        }

        // Crear el item
        float synergyMultiplier = currentRecipe.CalculateSynergy(materials);
        // Aquí crearías el item final y lo darías al jugador

        // Limpiar el slot de receta si es un scroll de un solo uso
        RecipeScroll scroll = recipeSlot.GetItem() as RecipeScroll;
        if (scroll != null && scroll.isOneTimeUse)
        {
            recipeSlot.ClearSlot();
        }
        resultSlot.ClearSlot();
    }
}