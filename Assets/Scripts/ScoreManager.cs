using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int CurrentScore { get; private set; }
    public int HighScore { get; private set; }

    const string HIGH_SCORE_KEY = "HIGH_SCORE";

    [SerializeField]
    int matchValue = 100, mismatchValue = 20, winValue = 500;
    int combo = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadHighScore();
    }

    void LoadHighScore()
    {
        HighScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
    }

    void saveHighScore()
    {
        PlayerPrefs.SetInt(HIGH_SCORE_KEY, HighScore);
        PlayerPrefs.Save();
    }

    public void ResetScore()
    {
        CurrentScore = 0;
        combo = 0;
    }

    public void SetScore(int score)
    {
        CurrentScore = score;
        combo = 0;
    }

    public void AddMatch()
    {
        combo++;
        int comboMultiplier = Mathf.Clamp(combo, 1, 10);
        int score = matchValue * comboMultiplier;

        if (comboMultiplier > 1)
        {
            AlertManager.Instance.Show("COMBO");
            AudioManager.Instance.Play(SoundType.Combo);
        }
        CurrentScore += score;
        checkHighScore();
    }

    public void AddMismatch()
    {
        combo = 0;
        CurrentScore = Mathf.Max(0, CurrentScore - mismatchValue);
    }

    public void AddWinBonus()
    {
        CurrentScore += winValue;
        checkHighScore();
    }

    void checkHighScore()
    {
        if (CurrentScore > HighScore)
        {
            HighScore = CurrentScore;
            saveHighScore();
        }
    }
}
