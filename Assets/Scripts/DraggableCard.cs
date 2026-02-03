using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Text wordText;
    public int originalIndex; // The index corresponding to the correct answer

    private Transform parentToReturnTo = null;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Canvas canvas;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        // Find the root canvas (needed for delta movement logic if we used delta)
        // or just to ensure we have one.
        Canvas[] canvases = GetComponentsInParent<Canvas>();
        if (canvases.Length > 0)
        {
            canvas = canvases[canvases.Length - 1]; // Usually the root canvas
        }
    }

    public void Initialize(string text, int index)
    {
        if (wordText != null)
        {
            wordText.text = text;
        }
        originalIndex = index;
    }

    public void SetText(string text)
    {
         if (wordText != null)
        {
            wordText.text = text;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentToReturnTo = this.transform.parent;

        // Move to the root canvas or a high-level container so it draws over everything
        if (canvas != null)
        {
            this.transform.SetParent(canvas.transform);
        }

        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas != null)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
        else
        {
            // Fallback
            transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(parentToReturnTo);
        this.transform.localPosition = Vector3.zero; // Center in slot

        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
    }

    public void SetParentSlot(Transform newParent)
    {
        parentToReturnTo = newParent;
    }

    public Transform GetParentSlot()
    {
        return parentToReturnTo;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Simple click detection, works for touch too
        if (eventData.dragging) return; // Ignore if it was a drag

        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.OnCardClicked(this);
        }
    }
}
