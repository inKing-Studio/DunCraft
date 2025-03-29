using UnityEngine;
using UnityEngine.UI;

public class DraggedItemDisplay : MonoBehaviour
{
    private Image itemImage;
    private RectTransform rectTransform;

    void Awake()
    {
        itemImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        
        if (itemImage != null)
        {
            itemImage.preserveAspect = true;
            itemImage.raycastTarget = false;
        }
        
        if (rectTransform != null)
        {
            rectTransform.sizeDelta = new Vector2(80, 80);
            rectTransform.localScale = Vector3.one * 0.77f;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
        }
    }
}