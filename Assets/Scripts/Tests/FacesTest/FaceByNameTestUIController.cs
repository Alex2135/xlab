using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FaceByNameTestUIController : MonoBehaviour, IResetableScreenController
{
    // Screen objects
    public TextMeshProUGUI nameAndLastnameTMP;
    public TextMeshProUGUI scoreTMP;
    public TextMeshProUGUI questNumberTMP;

    // Screen logic objects
    public string _screenName;
    public List<LoadedImage> loadedImages;
    public List<LoadedImage> additionalImages;
    private List<LoadedImage> merge;
    public QuestionView questionView;
    public FaceByNameTestView testView;
    private bool isButtonPressed;
    private int _score;

    public string ScreenName { get => _screenName; set => _screenName = value; }
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }

    public void Awake()
    {
        isButtonPressed = false;
        testView = new FaceByNameTestView();
        testView.CurrentQuestionView = questionView;
        testView.onFaceButtonClick = OnFaceClick;
        merge = new List<LoadedImage>();
        foreach (var img in loadedImages)
            merge.Add(img);
        foreach (var img in additionalImages)
            merge.Add(img);
    }

    void OnEnable()
    {
        questNumberTMP.text = $"{1} из {4}";
        testView.test = SimulateData();
        testView.GetNextQuestion();
        testView.RefreshQuestDataOnQuestionView(merge);
    }

    public Test SimulateData()
    {
        Test result = new Test();

        for (int i = 0; i < loadedImages.Count; i++)
        {
            loadedImages[i]._name += $"_{i}";
        }

        List<Question> questions = new List<Question>();
        // Images was downloaded with indexes in suffix.
        // Generation sameles answers for questions.
        List<Answer> answers = new List<Answer>();
        foreach (var img in loadedImages)
        {
            var nameLastname = img._name.Split('_')?[0];
            var ans = new Answer();
            ans.content = nameLastname;
            ans.isRight = false;
            answers.Add(ans);
        }

        // Create answers by additional images
        foreach (var img in additionalImages)
        {
            var ans = new Answer();
            ans.content = img._name;
            ans.isRight = false;
            answers.Add(ans);
        }

        // Create quests with generated answers
        foreach (var img in loadedImages)
        {
            // Copy answers for quest
            answers.ShuffleItems();
            var ansArr = new Answer[answers.Count];
            for(int i = 0; i < answers.Count; i++)
                ansArr[i] = (Answer)answers[i].Clone();

            // Create new quest
            var nameLastname = img._name.Split('_')?[0];
            var quest = new Question();
            quest.question = nameLastname;
            quest.file = null;
            quest.answers = ansArr;
            questions.Add(quest);
        }
        result.quests = questions;

        return result;
    }

    private void OnFaceClick(string faceName, int selectedId)
    {
        if (!isButtonPressed && testView.CurrentQuestion != null)
        {
            int rightId = 0;
            var screenText = questionView._quest.GetText();
            var size = screenText.Length > faceName.Length ? faceName.Length : screenText.Length;
            size -= 1;
            if (screenText.Substring(0, size) == faceName.Substring(0, size))
            {
                Debug.Log("Yes");
                rightId = selectedId;
                _score += 10;
            }
            else
            {
                Debug.Log("No");
                rightId = -1;
            }
            //var answers = testView.CurrentQuestionView._answers;
            //for (int i = 0; i < answers.Count; i++)
            //{
            //    if (answers[i].isRight)
            //        rightId = i;
            //}

            StartCoroutine(ShowResult(selectedId, rightId));
        }
    }

    private IEnumerator ShowResult(int _selectedId, int _rightId)
    {
        isButtonPressed = true;

        Debug.Log($"{_selectedId} == {_rightId}");
        questNumberTMP.text = $"{(testView.test as Test).quesitonIdx + 1} из {(testView.test as Test).quests.Count}";
        scoreTMP.text = $"{_score}";
        float delay = 0.3f;
        if (_selectedId == _rightId)
        {
            yield return new WaitForSeconds(delay);
        }
        else
        {
            yield return new WaitForSeconds(delay);
        }

        testView.GetNextQuestion();
        if (testView.CurrentQuestion != null)
        {
            testView.RefreshQuestDataOnQuestionView(merge);
        }
        else
        {
            gameObject.SetActive(false);
            (NextScreen as MonoBehaviour).gameObject.SetActive(true);
        }

        isButtonPressed = false;
    }

    void OnDisable()
    {
        ResetScreenState();
    }

    public void ResetScreenState()
    {
        testView.ResetTestAndQuestView();
    }
}


public class FaceByNameTestView : ITestView, ITest
{
    public QuestionView CurrentQuestionView { get; set; }
    public ITest test { get; set; }
    public Result ResultScore { get => test.ResultScore; set => test.ResultScore = value; }

    public Question CurrentQuestion => test.CurrentQuestion;
    public Action<string, int> onFaceButtonClick;

    public FaceByNameTestView() {}

    public Question GetNextQuestion()
    {
        return test.GetNextQuestion();
    }

    public float GetTime()
    {
        return test.GetTime();
    }

    public void RefreshQuestDataOnQuestionView(List<LoadedImage> _netImages)
    {
        if (_netImages == null) throw new Exception("Images are not set");
        if (CurrentQuestionView == null) throw new Exception("Question view is null");

        _netImages.ShuffleItems();
        SetQuestImages(_netImages);
        SetQuestText(_netImages);
    }

    public void SetQuestImages(List<LoadedImage> _images)
    {
        var answersDataUI = CurrentQuestionView._answers;
        for (int i = 0; i < answersDataUI.Count; i++)
        {
            var ans = answersDataUI[i];
            LoadedImage.SetTextureToImage(ref ans._image, _images[i]._image);
            var imageButton = ans._image.gameObject.GetComponent<Button>();
            imageButton.onClick.AddListener(() => onFaceButtonClick(ans._text.text, i));
        }
    }

    public void SetQuestText(List<LoadedImage> _netImages)
    {
        var answersDataUI = CurrentQuestionView._answers;
        for (int i = 0; i < answersDataUI.Count; i++)
        {
            answersDataUI[i]._text.text = _netImages[i]._name;
        }
        CurrentQuestionView._quest._text.text = CurrentQuestion.question;
    }

    public void ResetTestAndQuestView()
    {
        CurrentQuestionView._quest.ResetText();
        CurrentQuestionView._answers.ForEach((data) => data.ResetImageAndText());
    }
}