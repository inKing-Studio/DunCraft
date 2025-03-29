using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemTooltip : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public TextMeshProUGUI itemStatsText;
    public CanvasGroup canvasGroup;

    private void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
            
        HideTooltip();
    }

    public void ShowTooltip(ItemData item)
    {
        if (item == null) return;

        // Mostrar nombre y rareza
        itemNameText.text = $"{item.itemName}";
        itemNameText.color = GetRarityColor(item.rarity);

        // Mostrar descripción
        itemDescriptionText.text = item.description;

        // Mostrar stats
        string statsText = "";
        
        // Mostrar todos los modificadores
        foreach (var mod in item.statModifiers)
        {
            string prefix = mod.value >= 0 ? "+" : "";
            statsText += $"{prefix}{mod.value}{(mod.valueType == ModifierValueType.Percentage ? "%" : "")} {mod.statType}\n";
        }

        itemStatsText.text = statsText;

        // Mostrar el tooltip
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }

    public void HideTooltip()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
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
}