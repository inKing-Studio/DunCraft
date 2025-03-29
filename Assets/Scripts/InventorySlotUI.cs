using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class InventorySlotUI : MonoBehaviour
{
    public Image backgroundImage;
    public Color normalColor = new Color(1, 1, 1, 0.1f);
    public Color highlightedColor = new Color(1, 1, 1, 0.2f);

    void Awake()
    {
        // Asegurar que tenemos la referencia del background
        if (backgroundImage == null)
            backgroundImage = GetComponent<Image>();
            
        // Configurar el color inicial del fondo
        backgroundImage.color = normalColor;
    }

    public void SetHighlighted(bool highlighted)
    {
        backgroundImage.color = highlighted ? highlightedColor : normalColor;
    }
}