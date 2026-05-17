using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rect;
    private Vector2 originalPosition;
    private CanvasGroup canvasGroup;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        originalPosition = rect.anchoredPosition;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.anchoredPosition += eventData.delta;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        rect.anchoredPosition = originalPosition;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
