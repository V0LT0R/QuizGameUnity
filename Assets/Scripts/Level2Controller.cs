using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class TrueFalseQuestion
{
    public string question;
    public bool answer;
}

public class Level2Controller : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public Button trueButton;
    public Button falseButton;
    public Button nextButton;
    public TextMeshProUGUI feedbackText;

    private List<TrueFalseQuestion> questions;
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
        TextAsset file = Resources.Load<TextAsset>("Level2_TrueFalse");
        questions = JsonUtility.FromJson<TrueFalseWrapper>("{\"questions\":" + file.text + "}").questions;
    }

    void ShowQuestion()
    {
        answered = false;
        feedbackText.text = "";
        nextButton.gameObject.SetActive(false);

        TrueFalseQuestion q = questions[currentQuestionIndex];
        questionText.text = q.question;

        trueButton.interactable = true;
        falseButton.interactable = true;
        trueButton.onClick.RemoveAllListeners();
        falseButton.onClick.RemoveAllListeners();
        trueButton.onClick.AddListener(() => SelectAnswer(true));
        falseButton.onClick.AddListener(() => SelectAnswer(false));
    }

    void SelectAnswer(bool selectedAnswer)
    {
        if (answered) return;
        answered = true;

        TrueFalseQuestion q = questions[currentQuestionIndex];

        if (selectedAnswer == q.answer)
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

        trueButton.interactable = false;
        falseButton.interactable = false;

        nextButton.gameObject.SetActive(true);
        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(NextQuestion);
    }

    void NextQuestion()
    {
        currentQuestionIndex++;
        if (currentQuestionIndex >= questions.Count)
        {
            GameManager.Instance.LoadScene("Level3_WordPuzzle");
        }
        else
        {
            ShowQuestion();
        }
    }

    [System.Serializable]
    private class TrueFalseWrapper
    {
        public List<TrueFalseQuestion> questions;
    }
}
