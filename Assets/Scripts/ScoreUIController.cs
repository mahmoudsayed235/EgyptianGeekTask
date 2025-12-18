using TMPro;
using UnityEngine;

public class ScoreUIController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI currentScoreText, highScoreText;

    void Update()
    {
        currentScoreText.text = $"Score: {ScoreManager.Instance.CurrentScore}";
        highScoreText.text = $"High Score: {ScoreManager.Instance.HighScore}";
    }
}
