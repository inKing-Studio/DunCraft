using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class InventorySlotUI : MonoBehaviour
{
    public Image backgroundImage;
    public Image itemIconImage;
    public Color normalColor = new Color(1, 1, 1, 0.1f);
    public Color highlightedColor = new Color(1, 1, 1, 0.2f);

    void Awake()
    {
        // Asegurar que tenemos las referencias necesarias
        if (backgroundImage == null)
            backgroundImage = GetComponent<Image>();
            
        if (itemIconImage == null)
        {
            // Crear el objeto para el icono del item
            GameObject iconObj = new GameObject("ItemIcon");
            iconObj.transform.SetParent(transform);
            itemIconImage = iconObj.AddComponent<Image>();
            
            // Configurar el RectTransform del icono
            RectTransform iconRect = iconObj.GetComponent<RectTransform>();
            iconRect.anchorMin = Vector2.zero;
            iconRect.anchorMax = Vector2.one;
            iconRect.sizeDelta = Vector2.zero;
            iconRect.anchoredPosition = Vector2.zero;
            
            // Configurar la imagen del icono
            itemIconImage.raycastTarget = false;
            itemIconImage.enabled = false;
        }

        // Configurar el color inicial del fondo
        backgroundImage.color = normalColor;
    }

    public void SetHighlighted(bool highlighted)
    {
        backgroundImage.color = highlighted ? highlightedColor : normalColor;
    }
}