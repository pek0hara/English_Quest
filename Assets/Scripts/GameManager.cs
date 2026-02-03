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

    public bool reverseMode = false;

    private List<CardSlot> slots = new List<CardSlot>();
    private List<DraggableCard> cards = new List<DraggableCard>();
    private List<WordData> currentRoundWords;

    private float timer = 10f;
    private bool isTimerRunning = false;

    private enum GameState { Memorize, Solve, Check1, Solve2, Result }
    private GameState currentState;
    private DraggableCard selectedSwapCard = null;
    private int stage1Score = 0;

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

        // Prepare words for this round
        currentRoundWords = new List<WordData>(wordList.words);
        // Shuffle currentRoundWords
        for (int i = 0; i < currentRoundWords.Count; i++)
        {
            WordData temp = currentRoundWords[i];
            int randomIndex = Random.Range(i, currentRoundWords.Count);
            currentRoundWords[i] = currentRoundWords[randomIndex];
            currentRoundWords[randomIndex] = temp;
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

            string text = reverseMode ? currentRoundWords[i].english : currentRoundWords[i].japanese;
            card.Initialize(text, i);

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
            string text = reverseMode ? currentRoundWords[cards[i].originalIndex].japanese : currentRoundWords[cards[i].originalIndex].english;
            cards[i].SetText(text);

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

    public void ResetFeedback()
    {
        foreach (DraggableCard card in cards)
        {
            Image cardImage = card.GetComponent<Image>();
            if (cardImage != null) cardImage.color = Color.white; // Reset to default
        }
        if (currentState == GameState.Solve2)
        {
            if (statusText != null) statusText.text = "Stage 2: Arrange again using hints!";
        }
        else
        {
            if (statusText != null) statusText.text = reverseMode ? "Arrange the Japanese words!" : "Arrange the English words!";
        }
    }

    public void OnCardClicked(DraggableCard clickedCard)
    {
        if (currentState != GameState.Solve && currentState != GameState.Solve2) return;

        ResetFeedback();

        if (selectedSwapCard == null)
        {
            // Select first card
            selectedSwapCard = clickedCard;
            // Visual feedback: Scale up
            clickedCard.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
        }
        else if (selectedSwapCard == clickedCard)
        {
            // Deselect same card
            clickedCard.transform.localScale = Vector3.one;
            selectedSwapCard = null;
        }
        else
        {
            // Swap logic

            // Get current parent slots
            Transform parent1 = selectedSwapCard.GetParentSlot();
            Transform parent2 = clickedCard.GetParentSlot();

            // Reset scale of selected card
            selectedSwapCard.transform.localScale = Vector3.one;

            // Perform Swap
            // Move selectedSwapCard to parent2
            selectedSwapCard.transform.SetParent(parent2);
            selectedSwapCard.transform.localPosition = Vector3.zero;
            selectedSwapCard.SetParentSlot(parent2);

            // Move clickedCard to parent1
            clickedCard.transform.SetParent(parent1);
            clickedCard.transform.localPosition = Vector3.zero;
            clickedCard.SetParentSlot(parent1);

            // Clear selection
            selectedSwapCard = null;
        }
    }

    void CheckAnswer()
    {
        if (currentState == GameState.Solve)
        {
            // Stage 1: Show results with problem and answer
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
                        if (cardImage != null) cardImage.color = Color.green;
                    }
                    else
                    {
                        if (cardImage != null) cardImage.color = Color.red;
                    }

                    // Show answer with original question below
                    if (card != null)
                    {
                        string answerText = reverseMode ? currentRoundWords[card.originalIndex].japanese : currentRoundWords[card.originalIndex].english;
                        string questionText = currentRoundWords[card.originalIndex].japanese;
                        card.SetText($"{answerText}\n({questionText})");
                    }
                }
            }

            stage1Score = correctCount;
            currentState = GameState.Check1;
            if (statusText != null) statusText.text = $"Stage 1: {correctCount} / 15 Correct!  Press button to start Stage 2";

            // Change button text to Stage 2
            if (checkButton != null)
            {
                Text buttonText = checkButton.GetComponentInChildren<Text>();
                if (buttonText != null) buttonText.text = "Start Stage 2";
            }
        }
        else if (currentState == GameState.Check1)
        {
            // Transition to Stage 2
            StartSolve2Phase();
        }
        else if (currentState == GameState.Solve2)
        {
            // Stage 2: Final check
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
                        if (cardImage != null) cardImage.color = Color.green;
                    }
                    else
                    {
                        if (cardImage != null) cardImage.color = Color.red;
                    }

                    // Show answer with original question below
                    if (card != null)
                    {
                        string answerText = reverseMode ? currentRoundWords[card.originalIndex].japanese : currentRoundWords[card.originalIndex].english;
                        string questionText = currentRoundWords[card.originalIndex].japanese;
                        card.SetText($"{answerText}\n({questionText})");
                    }
                }
            }

            currentState = GameState.Result;
            if (statusText != null) statusText.text = $"Final: Stage1 {stage1Score}/15 → Stage2 {correctCount}/15";

            // Change button text to Restart
            if (checkButton != null)
            {
                Text buttonText = checkButton.GetComponentInChildren<Text>();
                if (buttonText != null) buttonText.text = "Restart Game";
            }
        }
        else if (currentState == GameState.Result)
        {
            // Restart the game
            if (checkButton != null)
            {
                Text buttonText = checkButton.GetComponentInChildren<Text>();
                if (buttonText != null) buttonText.text = "Check Answer";
            }
            InitializeGame();
        }
    }

    void StartSolve2Phase()
    {
        currentState = GameState.Solve2;
        if (statusText != null) statusText.text = "Stage 2: Arrange again using hints!";

        // Change button text back to Check Answer
        if (checkButton != null)
        {
            Text buttonText = checkButton.GetComponentInChildren<Text>();
            if (buttonText != null) buttonText.text = "Check Answer";
        }

        // Reset card colors and keep showing answer with question
        foreach (DraggableCard card in cards)
        {
            Image cardImage = card.GetComponent<Image>();
            if (cardImage != null) cardImage.color = Color.white;

            // Keep showing answer with question as hint
            string answerText = reverseMode ? currentRoundWords[card.originalIndex].japanese : currentRoundWords[card.originalIndex].english;
            string questionText = currentRoundWords[card.originalIndex].japanese;
            card.SetText($"{answerText}\n({questionText})");
        }

        // Shuffle cards for second attempt
        ShuffleCards();
    }
}
