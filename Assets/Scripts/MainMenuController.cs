using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] GameObject recordText;

    private void Start()
    {
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        recordText.GetComponent<TMPro.TMP_Text>().text = "Best Score: " + bestScore;
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
