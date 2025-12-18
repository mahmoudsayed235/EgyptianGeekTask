using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [SerializeField]
    GameObject card;

    [SerializeField]
    GridLayoutGroup grid;
    [SerializeField]
    GameObject mainScreen,gameMenu;
    [SerializeField]
    CardDeck[] cardDecks;
    CardDeck cardDeck;
    int numberOfSelectedCards;

    const string CURRENT_STATE_KEY = "CURRENT_STATE";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        cardDeck = cardDecks[0];
        
    }
    public void SetDifficulty(Int32 index)
    {
        cardDeck = cardDecks[index];
    }
    public void SelectCard(GameObject card)
    {
        AudioManager.Instance.Play(SoundType.Flip);
        card.GetComponent<CardController>().FlipCard();
        selectedCards.Add(card);
        if (selectedCards.Count % 2 == 0 && selectedCards.Count >= 2)
        {
            StartCoroutine(CheckMatch(selectedCards[0], selectedCards[1]));
        }
    }
    List<GameObject> selectedCards=new List<GameObject>();
    public IEnumerator CheckMatch(GameObject firstCard, GameObject secondCard)
    {
        AudioManager.Instance.Play(SoundType.Flip);
        yield return new WaitForSeconds(0.5f);
        if (firstCard.GetComponent<CardController>().id
            .Equals(secondCard.GetComponent<CardController>().id))
        {
            firstCard.GetComponent<CardController>().Hide();
            secondCard.GetComponent<CardController>().Hide();
            AudioManager.Instance.Play(SoundType.Match);
            ScoreManager.Instance.AddMatch();
            numberOfSelectedCards += 1;
            if (numberOfSelectedCards == cardDeck.cards.Count)
            {
                ScoreManager.Instance.AddWinBonus();
                endGame();
            }
        }
        else
        {
            AudioManager.Instance.Play(SoundType.Mismatch);
            firstCard.GetComponent<CardController>().FlipCard();
            secondCard.GetComponent<CardController>().FlipCard();
            ScoreManager.Instance.AddMismatch();
        }

        selectedCards.RemoveAt(1);
        selectedCards.RemoveAt(0);


    }
    public void StartGame()
    {
        ResetGameState();
        ClearGrid();
        BuildPlayDeck(cardDeck);
        ScoreManager.Instance.ResetScore();

    }
    
    void ResetGameState()
    {
        selectedCards = new List<GameObject>();
        numberOfSelectedCards = 0;
        mainScreen.SetActive(false);
        grid.gameObject.SetActive(true);
    }
    void ClearGrid()
    {
        foreach (Transform child in grid.transform)
        {
            Destroy(child.gameObject);
        }
    }
    void endGame()
    {

        AlertManager.Instance.Show("YOU WIN");
        AudioManager.Instance.Play(SoundType.Win);
        grid.gameObject.SetActive(false);
        gameMenu.SetActive(false);
        mainScreen.SetActive(true);
    }
    public void SaveGame()
    {
        List<CardInfo> cardsData = new List<CardInfo>();

        for (int i = 0; i < grid.transform.childCount; i++)
        {
            CardController card = grid.transform.GetChild(i).GetComponent<CardController>();
            if (card != null)
            {
                cardsData.Add(new CardInfo(
                    card.id,
                    card.cardName,
                    card.Face,
                    i,
                    card.isActive
                ));
            }
        }

        GridSaveData saveData = new GridSaveData { Cards = cardsData ,Score=ScoreManager.Instance.CurrentScore};
        string json = JsonUtility.ToJson(saveData, true);
        PlayerPrefs.SetString(CURRENT_STATE_KEY, json);
    }
    public void LoadGame()
    {
        GridSaveData savedData = JsonUtility.FromJson<GridSaveData>(PlayerPrefs.GetString(CURRENT_STATE_KEY));
        StartCoroutine(ConfigureGrid(savedData.Cards.Count));
        loadCards(savedData.Cards);
        ScoreManager.Instance.SetScore(savedData.Score);
    }
    void loadCards(List<CardInfo> cardsContainer)
    {
        for (int i = 0; i < cardsContainer.Count; i++)
        {
            GameObject newCard = Instantiate(card, grid.transform);
            newCard.GetComponent<CardController>().CreateCard(cardsContainer[i].id, cardsContainer[i].CardName, cardsContainer[i].CardSprite, cardsContainer[i].Index, cardsContainer[i].IsActive);
        }
    }
    void BuildPlayDeck(CardDeck sourceDeck)
    {
        List<CardData> playDeck = new List<CardData>();

        foreach (CardData card in sourceDeck.cards)
        {
            playDeck.Add(card);
            playDeck.Add(card);
        }

        for (int i = playDeck.Count - 1; i > 0; i--)
        {
            int rnd = UnityEngine.Random.Range(0, i + 1);
            (playDeck[i], playDeck[rnd]) = (playDeck[rnd], playDeck[i]);
        }

        StartCoroutine(ConfigureGrid(playDeck.Count));

        fetchCards(playDeck);
    }
    private IEnumerator ConfigureGrid(int cardsContainerCounter,float spacing = 10f,float padding = 20f)
    {
        gameMenu.SetActive(true);
        mainScreen.SetActive(false);
        grid.gameObject.SetActive(true);
        RectTransform rect = grid.GetComponent<RectTransform>();
        float width = rect.rect.width - padding * 2;
        float height = rect.rect.height - padding * 2;

        int bestRows = 1;
        int bestCols = cardsContainerCounter;
        float bestCellSize = 0f;

        for (int rows = 1; rows <= cardsContainerCounter; rows++)
        {
            int cols = Mathf.CeilToInt((float)cardsContainerCounter / rows);

            float cellWidth =
                (width - spacing * (cols - 1)) / cols;
            float cellHeight =
                (height - spacing * (rows - 1)) / rows;

            float cellSize = Mathf.Min(cellWidth, cellHeight);

            if (cellSize > bestCellSize)
            {
                bestCellSize = cellSize;
                bestRows = rows;
                bestCols = cols;
            }
        }

        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = bestCols;

        grid.cellSize = new Vector2(bestCellSize, bestCellSize);
        grid.spacing = new Vector2(spacing, spacing);
        grid.padding = new RectOffset(
            (int)padding, (int)padding,
            (int)padding, (int)padding
        );
        yield return null;
    }

    void fetchCards(List<CardData> cardsContainer)
    {
        for (int i = 0; i < cardsContainer.Count; i++)
        {
            GameObject newCard = Instantiate(card, grid.transform);
            newCard.GetComponent<CardController>().CreateCard(cardsContainer[i], i);
        }
    }
    public void ExitGame()
    {
        ResetGameState();
        ClearGrid();
        ScoreManager.Instance.ResetScore();
        gameMenu.SetActive(false);
        mainScreen.SetActive(true);
        grid.gameObject.SetActive(false);
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

}

[System.Serializable]
public class CardInfo
{
    public string id;
    public string CardName;
    public Sprite CardSprite;
    public int Index;
    public bool IsActive;

    public CardInfo(string id, string cardName, Sprite cardSprite, int index, bool isActive = true)
    {
        this.id = id;
        CardName = cardName;
        CardSprite = cardSprite;
        Index = index;
        IsActive = isActive;
    }
}
[System.Serializable]
public class GridSaveData
{
    public List<CardInfo> Cards;
    public int Score;
}