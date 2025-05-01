using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    public TextMeshProUGUI recordText;
    public TextMeshProUGUI lastScoreText;

    private void Start()
    {
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        int lastScore = PlayerPrefs.GetInt("LastScore", 0);

        recordText.text = "Best Score: " + bestScore;
        lastScoreText.text = "Last Score: " + lastScore;
    }

    public void StartGame()
    {
        GameManager.Instance.ResetScore();
        GameManager.Instance.LoadScene("Level1_MultipleChoice");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
