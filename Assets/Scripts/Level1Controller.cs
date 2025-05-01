using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;
using System.Collections.Generic;

[System.Serializable]
public class MultipleChoiceQuestion
{
    public string question;
    public List<string> options;
    public int correctIndex;
    public string image; // путь к картинке
    public string video; // путь к видео
}

public class Level1Controller : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public List<Button> optionButtons;
    public Button nextButton;
    public TextMeshProUGUI feedbackText;

    public Image questionImage;          // Картинка для вопросов
    public RawImage videoScreen;          // Куда выводится видео через RenderTexture
    public VideoPlayer questionVideo;     // Сам плеер видео

    private List<MultipleChoiceQuestion> questions;
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
        TextAsset file = Resources.Load<TextAsset>("Level1_MultipleChoice");
        questions = JsonUtility.FromJson<MultipleChoiceWrapper>("{\"questions\":" + file.text + "}").questions;
    }

    void ShowQuestion()
    {
        answered = false;
        feedbackText.text = "";
        nextButton.gameObject.SetActive(false);

        MultipleChoiceQuestion q = questions[currentQuestionIndex];
        questionText.text = q.question;

        // Настройка вариантов ответов
        for (int i = 0; i < optionButtons.Count; i++)
        {
            optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = q.options[i];
            int index = i;
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => SelectAnswer(index));
            optionButtons[i].interactable = true;
        }

        // Управление мультимедиа
        if (!string.IsNullOrEmpty(q.image))
        {
            // Показать картинку
            questionImage.gameObject.SetActive(true);
            videoScreen.gameObject.SetActive(false);
            questionVideo.gameObject.SetActive(false);

            Sprite sprite = Resources.Load<Sprite>("Images/" + q.image);
            if (sprite != null)
                questionImage.sprite = sprite;
            else
                Debug.LogWarning("Image not found: " + q.image);
        }
        else if (!string.IsNullOrEmpty(q.video))
        {
            // Показать видео
            questionImage.gameObject.SetActive(false);
            videoScreen.gameObject.SetActive(true);
            questionVideo.gameObject.SetActive(true);

            VideoClip clip = Resources.Load<VideoClip>("Videos/" + q.video);
            if (clip != null)
            {
                questionVideo.clip = clip;
                questionVideo.Play();
            }
            else
            {
                Debug.LogWarning("Video not found: " + q.video);
            }
        }
        else
        {
            // Нет медиа
            questionImage.gameObject.SetActive(false);
            videoScreen.gameObject.SetActive(false);
            questionVideo.gameObject.SetActive(false);
        }
    }

    void SelectAnswer(int selectedIndex)
    {
        if (answered) return;
        answered = true;

        MultipleChoiceQuestion q = questions[currentQuestionIndex];

        if (selectedIndex == q.correctIndex)
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

        foreach (Button btn in optionButtons)
        {
            btn.interactable = false;
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
            GameManager.Instance.LoadScene("Level2_TrueFalse");
        }
        else
        {
            ShowQuestion();
        }
    }

    [System.Serializable]
    private class MultipleChoiceWrapper
    {
        public List<MultipleChoiceQuestion> questions;
    }
}
