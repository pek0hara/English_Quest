using UnityEngine;
using UnityEngine.EventSystems;

public class CardSlot : MonoBehaviour, IDropHandler
{
    public int slotIndex; // 0-14, set by GameManager during initialization

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        DraggableCard droppedCard = eventData.pointerDrag.GetComponent<DraggableCard>();
        if (droppedCard != null)
        {
            // The slot where the dragged card came from
            Transform originalSlot = droppedCard.GetParentSlot();

            // If this slot already has a card, we swap them
            if (transform.childCount > 0)
            {
                Transform currentCardTransform = transform.GetChild(0);
                DraggableCard currentCard = currentCardTransform.GetComponent<DraggableCard>();

                if (currentCard != null && originalSlot != null)
                {
                    // Move the current card to the original slot of the dropped card
                    currentCardTransform.SetParent(originalSlot);
                    currentCardTransform.localPosition = Vector3.zero;

                    // Update the swapped card's internal reference
                    // so it knows its new home is the original slot of the other card
                    currentCard.SetParentSlot(originalSlot);
                }
            }

            // Set the dropped card's target parent to be this slot
            // The actual reparenting happens in OnEndDrag of DraggableCard
            droppedCard.SetParentSlot(this.transform);

            // Notify GameManager to update feedback colors (for Stage 2)
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                gm.OnCardDropped();
            }
        }
    }
}
