using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe Scroll", menuName = "Inventory/Recipe Scroll")]
public class RecipeScroll : ItemData
{
    public CraftingRecipe recipe;
    public bool isOneTimeUse = true;
}