using UnityEngine;
using UnityEngine.UI;
using System; // Needed for Enum.HasFlag

[RequireComponent(typeof(Image))]
public class InventorySlotUI : MonoBehaviour
{
    public Image backgroundImage; // Assign the Image component of the slot background

    // --- Default and Recipe Requirement Sprites ---
    [Header("Slot Sprites")]
    public Sprite defaultSprite; // Assign the default slot background sprite in Inspector
    public Sprite metalRequiredSprite; // Assign sprite for metal requirement
    public Sprite woodRequiredSprite;  // Assign sprite for wood requirement
    public Sprite clothRequiredSprite; // Assign sprite for cloth requirement
    public Sprite gemRequiredSprite;   // Assign sprite for gem requirement
    public Sprite leatherRequiredSprite; // Assign sprite for leather requirement
    public Sprite crystalRequiredSprite; // Assign sprite for crystal requirement
    // Add more Sprite fields here for other categories as needed (Ore, Fiber, etc.)
    public Sprite fallbackRequirementSprite; // Optional: A generic sprite if no specific one matches

    private bool isRecipeHighlighted = false; // Track if currently recipe highlighted

    void Awake()
    {
        if (backgroundImage == null)
            backgroundImage = GetComponent<Image>();

        // Ensure a default sprite is assigned to avoid errors
        if (defaultSprite == null)
        {
            Debug.LogWarning($"Default sprite not assigned on {gameObject.name}. Using background image's current sprite as default.", this);
            defaultSprite = backgroundImage.sprite;
        }
        ResetToDefaultAppearance(); // Start with the default sprite
    }

    // --- Methods for Recipe Highlighting ---

    /// <summary>
    /// Sets the background sprite based on the required material category for a recipe.
    /// </summary>
    /// <param name="requiredCategoryFlags">The MaterialCategory flags required by the recipe for this slot.</param>
    public void UpdateRecipeRequirementHighlight(MaterialCategory requiredCategoryFlags)
    {
        isRecipeHighlighted = true; // Mark as recipe highlighted
        Sprite targetSprite = GetSpriteForCategory(requiredCategoryFlags);
        if (targetSprite != null)
        {
            backgroundImage.sprite = targetSprite;
        }
        else
        {
            // Optionally fallback to default or a generic requirement sprite if no specific one found
            backgroundImage.sprite = fallbackRequirementSprite != null ? fallbackRequirementSprite : defaultSprite;
            Debug.LogWarning($"No specific requirement sprite found for category {requiredCategoryFlags} on {gameObject.name}. Using fallback/default.", this);
        }
    }

    /// <summary>
    /// Resets the slot's background sprite to the default appearance.
    /// </summary>
    public void ResetToDefaultAppearance()
    {
        isRecipeHighlighted = false; // Unmark recipe highlight
        if (defaultSprite != null)
        {
            backgroundImage.sprite = defaultSprite;
        }
    }

    /// <summary>
    /// Helper method to determine the sprite based on MaterialCategory flags.
    /// Prioritizes specific categories if multiple flags are set.
    /// </summary>
    private Sprite GetSpriteForCategory(MaterialCategory categoryFlags)
    {
        // Check for specific categories (adjust priority and checks based on your needs)
        if (categoryFlags.HasFlag(MaterialCategory.MetalIngot) && metalRequiredSprite != null) return metalRequiredSprite;
        if (categoryFlags.HasFlag(MaterialCategory.WoodPlank) && woodRequiredSprite != null) return woodRequiredSprite; // Corrected HasFlag usage
        if (categoryFlags.HasFlag(MaterialCategory.Cloth) && clothRequiredSprite != null) return clothRequiredSprite; // Corrected HasFlag usage
        if (categoryFlags.HasFlag(MaterialCategory.Gem) && gemRequiredSprite != null) return gemRequiredSprite; // Corrected HasFlag usage
        if (categoryFlags.HasFlag(MaterialCategory.Leather) && leatherRequiredSprite != null) return leatherRequiredSprite; // Corrected HasFlag usage
        if (categoryFlags.HasFlag(MaterialCategory.Crystal) && crystalRequiredSprite != null) return crystalRequiredSprite; // Corrected HasFlag usage
        // Add checks for Ore, Fiber, WoodLog, Skin, etc. if you have specific sprites for them

        // Return null if no specific sprite matches the flags
        return null;
    }

    // --- Optional: Original highlight for hover/selection ---
    // You might want to disable this or change how it interacts with recipe sprites
    // For example, maybe add a separate highlight Image on top instead of changing the background sprite/color.
    /*
    public Color highlightedColor = new Color(1, 1, 1, 0.2f); // Example if using color tint for hover
    public void SetHighlighted(bool highlighted)
    {
        // Example: Tint the background image slightly if hovered
        backgroundImage.color = highlighted ? highlightedColor : Color.white; // Assuming default tint is white
    }
    */
}