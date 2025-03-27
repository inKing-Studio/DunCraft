using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI rarityText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI statsText;
    public Image itemIcon;
    public TextMeshProUGUI costText;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Canvas canvas;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        HideTooltip();
    }

    public void ShowTooltip(ItemData item)
    {
        if (item == null) return;

        // Actualizar contenido
        itemNameText.text = item.itemName;
        rarityText.text = item.Rarity.ToString();
        descriptionText.text = item.description;
        
        if (itemIcon != null && item.icon != null)
        {
            itemIcon.sprite = item.icon;
            itemIcon.gameObject.SetActive(true);
        }
        
        // Stats
        string statsString = "";
        foreach (var mod in item.baseStatModifiers)
        {
            statsString += $"{mod.Stat}: {(mod.IsPercentage ? mod.Value + "%" : mod.Value.ToString("F1"))}\n";
        }
        statsText.text = statsString;

        // Costo
        if (costText != null)
        {
            costText.text = $"Costo: {item.cost}";
        }

        // Mostrar tooltip
        canvasGroup.alpha = 1;
        
        // Posicionar cerca del cursor
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            Input.mousePosition,
            canvas.worldCamera,
            out mousePos
        );

        // Ajustar posición para que no se salga de la pantalla
        Vector2 tooltipSize = rectTransform.sizeDelta;
        Vector2 canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;
        
        // Asegurar que el tooltip no se salga por la derecha o abajo
        mousePos.x = Mathf.Min(mousePos.x, canvasSize.x - tooltipSize.x);
        mousePos.y = Mathf.Max(mousePos.y, -canvasSize.y + tooltipSize.y);
        
        rectTransform.anchoredPosition = mousePos;
    }

    public void HideTooltip()
    {
        canvasGroup.alpha = 0;
    }
}