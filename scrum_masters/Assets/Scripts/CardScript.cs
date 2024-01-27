using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerClickHandler
{
    public int cardID; 
    public bool isFlipped = false;
    public Image image;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isFlipped)
        {
            Flip();
        }
    }

    private void OnMouseDown()
    {
        if (!isFlipped)
        {
            Flip();
        }
    }

    public void Flip()
    {
        isFlipped = true;
        GetComponent<SpriteRenderer>().enabled = true;
        GameManager.Instance.CheckMatch(this);
    }

    // Reset the card to its initial state
    public void ResetCard()
    {
        isFlipped = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }
}