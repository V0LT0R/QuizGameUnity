using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class WordPuzzleQuestion
{
    public string scrambled;
    public string answer;
}

public class Level3Controller : MonoBehaviour
{
    public TextMeshProUGUI scrambledText;
    public TMP_InputField answerInput;
    public Button submitButton;
    public Button nextButton;
    public TextMeshProUGUI feedbackText;

    private List<WordPuzzleQuestion> questions;
    private int currentQuestionIndex = 0;
    private bool answered = false;

    private void Start()
    {
        LoadQuestions();
        ShowQuestion();
        nextButton.gameObject.SetActive(false);
    }

    void LoadQuestions()
    {
        TextAsset file = Resources.Load<TextAsset>("Level3_WordPuzzle");
        questions = JsonUtility.FromJson<WordPuzzleWrapper>("{\"questions\":" + file.text + "}").questions;
    }

    void ShowQuestion()
    {
        answered = false;
        feedbackText.text = "";
        answerInput.text = "";
        nextButton.gameObject.SetActive(false);

        WordPuzzleQuestion q = questions[currentQuestionIndex];
        scrambledText.text = q.scrambled;

        submitButton.onClick.RemoveAllListeners();
        submitButton.onClick.AddListener(CheckAnswer);
    }

    void CheckAnswer()
    {
        if (answered) return;
        answered = true;

        WordPuzzleQuestion q = questions[currentQuestionIndex];
        if (answerInput.text.Trim().ToUpper() == q.answer.ToUpper())
        {
            feedbackText.color = Color.green;
            feedbackText.text = "Correct!";
            GameManager.Instance.AddScore(10);
        }
        else
        {
            feedbackText.color = Color.red;
            feedbackText.text = "Wrong!";
        }

        nextButton.gameObject.SetActive(true);
        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(NextQuestion);
    }

    void NextQuestion()
    {
        currentQuestionIndex++;
        if (currentQuestionIndex >= questions.Count)
        {
            GameManager.Instance.SaveScore();
            GameManager.Instance.LoadScene("MainMenu");
        }
        else
        {
            ShowQuestion();
        }
    }

    [System.Serializable]
    private class WordPuzzleWrapper
    {
        public List<WordPuzzleQuestion> questions;
    }
}
