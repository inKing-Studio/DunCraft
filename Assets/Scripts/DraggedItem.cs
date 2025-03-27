using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggedItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 originalPosition;
    private Transform originalParent;
    
    public ItemData ItemData { get; private set; }
    public InventorySlot OriginalSlot { get; private set; }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Initialize(ItemData itemData, InventorySlot originalSlot)
    {
        ItemData = itemData;
        OriginalSlot = originalSlot;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = transform.position;
        originalParent = transform.parent;
        
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (eventData.pointerCurrentRaycast.gameObject == null)
        {
            // Si no se soltó sobre ningún slot válido, volver a la posición original
            transform.position = originalPosition;
            transform.SetParent(originalParent);
        }
    }
}