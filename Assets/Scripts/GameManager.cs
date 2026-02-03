using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public WordList wordList;
    public GameObject slotPrefab;
    public GameObject cardPrefab;
    public Transform gridParent;

    public Text timerText;
    public Text statusText;
    public Button checkButton;

    private List<CardSlot> slots = new List<CardSlot>();
    private List<DraggableCard> cards = new List<DraggableCard>();

    private float timer = 10f;
    private bool isTimerRunning = false;

    private enum GameState { Memorize, Solve, Result }
    private GameState currentState;

    void Start()
    {
        // Setup button listener
        if (checkButton != null)
        {
            checkButton.onClick.AddListener(CheckAnswer);
        }

        InitializeGame();
    }

    void Update()
    {
        if (isTimerRunning)
        {
            timer -= Time.deltaTime;
            if (timerText != null) timerText.text = "Time: " + Mathf.Ceil(timer).ToString();

            if (timer <= 0)
            {
                isTimerRunning = false;
                StartSolvePhase();
            }
        }
    }

    void InitializeGame()
    {
        currentState = GameState.Memorize;
        if (statusText != null) statusText.text = "Memorize the positions!";
        if (checkButton != null) checkButton.interactable = false;

        // Clear existing children if any
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }
        slots.Clear();
        cards.Clear();

        // Ensure we have enough words
        if (wordList == null || wordList.words.Count < 15)
        {
            Debug.LogError("Not enough words in WordList! Need at least 15.");
            return;
        }

        // Create 15 slots and cards
        for (int i = 0; i < 15; i++)
        {
            // Instantiate Slot
            GameObject slotObj = Instantiate(slotPrefab, gridParent);
            CardSlot slot = slotObj.GetComponent<CardSlot>();
            if (slot == null) slot = slotObj.AddComponent<CardSlot>();

            slot.slotIndex = i;
            slots.Add(slot);

            // Instantiate Card inside Slot
            GameObject cardObj = Instantiate(cardPrefab, slotObj.transform);
            DraggableCard card = cardObj.GetComponent<DraggableCard>();
            if (card == null) card = cardObj.AddComponent<DraggableCard>();

            card.Initialize(wordList.words[i].japanese, i);

            // Disable interaction during memorization
            CanvasGroup cg = card.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.blocksRaycasts = false; // Disable drag
            }

            cards.Add(card);
        }

        timer = 10f;
        isTimerRunning = true;
    }

    void StartSolvePhase()
    {
        currentState = GameState.Solve;
        if (statusText != null) statusText.text = "Arrange the English words!";
        if (checkButton != null) checkButton.interactable = true;

        // Switch to English
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].SetText(wordList.words[cards[i].originalIndex].english);

            // Enable interaction
            CanvasGroup cg = cards[i].GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.blocksRaycasts = true;
            }
        }

        ShuffleCards();
    }

    void ShuffleCards()
    {
        // Shuffle the cards list first
        for (int i = 0; i < cards.Count; i++)
        {
            DraggableCard temp = cards[i];
            int randomIndex = Random.Range(i, cards.Count);
            cards[i] = cards[randomIndex];
            cards[randomIndex] = temp;
        }

        // Re-parent them to slots based on new order
        for (int i = 0; i < 15; i++)
        {
            DraggableCard card = cards[i];
            CardSlot slot = slots[i];

            card.transform.SetParent(slot.transform);
            card.transform.localPosition = Vector3.zero;
            card.SetParentSlot(slot.transform);
        }
    }

    void CheckAnswer()
    {
        if (currentState != GameState.Solve) return;

        int correctCount = 0;

        foreach (CardSlot slot in slots)
        {
            if (slot.transform.childCount > 0)
            {
                DraggableCard card = slot.transform.GetChild(0).GetComponent<DraggableCard>();
                Image cardImage = card.GetComponent<Image>();

                if (card != null && card.originalIndex == slot.slotIndex)
                {
                    correctCount++;
                    // Optional: Visual feedback for correct cards
                    if (cardImage != null) cardImage.color = Color.green;
                }
                else
                {
                    if (cardImage != null) cardImage.color = Color.red;
                }
            }
        }

        if (statusText != null) statusText.text = $"Result: {correctCount} / 15 Correct!";
        currentState = GameState.Result;
        if (checkButton != null) checkButton.interactable = false;
    }
}
