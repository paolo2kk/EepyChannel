using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<Sprite> cardImages;
    public Transform cardsParent; 
    public GameObject cardPrefab; 

    private List<Card> cards = new List<Card>();
    private Card firstCard, secondCard;
    private int pairsFound = 0;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InitializeGame();
    }


    void InitializeGame()
    {
        int id = 0;

        // Number of rows and columns
        int numRows = 2;
        int numCols = 4;

        // Spacing between cards
        float cardSpacingX = 150f;
        float cardSpacingY = 150f;

        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                // Calculate position for each card
                float posX = col * cardSpacingX;
                float posY = -row * cardSpacingY; // Negative to place cards from top to bottom

                CreateCard(id, cardImages[id], posX, posY);
                id++;
            }
        }

        ShuffleCards();
    }

    void CreateCard(int id, Sprite image, float posX, float posY)
    {
        GameObject cardGO = Instantiate(cardPrefab, cardsParent);
        RectTransform cardRectTransform = cardGO.GetComponent<RectTransform>();

        // Set RectTransform properties based on layout preferences and position parameters
        cardRectTransform.anchoredPosition = new Vector2(posX, posY);
        cardRectTransform.sizeDelta = new Vector2(100f, 100f); // Adjust the size based on your preferences

        Image cardImage = cardGO.GetComponent<Image>();
        cardImage.sprite = image;

        Card card = cardGO.GetComponent<Card>();
        card.cardID = id;
        card.image = cardImage;
        card.ResetCard();
        cards.Add(card);
        Debug.Log($"Creating card {id} at position ({posX}, {posY})");

    }

    void ShuffleCards()
    {
        int count = cards.Count;
        while (count > 1)
        {
            count--;
            int randIndex = Random.Range(0, count + 1);
            Card temp = cards[randIndex];
            cards[randIndex] = cards[count];
            cards[count] = temp;
        }
    }

    public void CheckMatch(Card card)
    {
        if (firstCard == null)
        {
            firstCard = card;
        }
        else
        {
            secondCard = card;
            StartCoroutine(CheckMatchDelay());
        }
    }

    IEnumerator CheckMatchDelay()
    {
        yield return new WaitForSeconds(1f);

        if (firstCard.cardID == secondCard.cardID)
        {
            pairsFound++;
            if (pairsFound == cardImages.Count)
            {
                Debug.Log("Game Over! All pairs found.");
            }
        }
        else
        {
            firstCard.ResetCard();
            secondCard.ResetCard();
        }

        firstCard = null;
        secondCard = null;
    }
}
