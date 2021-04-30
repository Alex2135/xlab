using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FaceByNameTestUIController : MonoBehaviour, IResetableScreenController, NewQuestionModel.ITestView
{
    // Screen objects
    public TextMeshProUGUI nameAndLastnameTMP;
    public TextMeshProUGUI scoreTMP;
    public TextMeshProUGUI questNumberTMP;

    // Show result images
    public Texture2D rightAnswerImage;
    public Texture2D wrongAnswerImage;

    // Screen logic objects
    public string _screenName;
    public List<LoadedImage> loadedImages;
    public List<LoadedImage> additionalImages;
    private List<LoadedImage> merge;
    public QuestionView questionView;
    public FaceByNameTestView testView;
    private bool isButtonPressed;
    private int _score;

    public event Action<object> OnAnsweringEvent;
    public event Action<object> OnAnswerDidEvent;
    public event Action<object, EventArgs> OnQuestTimeoutEvent;

    public string ScreenName { get => _screenName; set => _screenName = value; }
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }
    public NewQuestionModel.IAdaptedQuestToView QuestionToView { get; set; }

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
            answers.Shuffle();
            var ansArr = new Answer[answers.Count];
            var nameLastname = img._name.Split('_')?[0];
            for (int i = 0; i < answers.Count; i++)
            {
                ansArr[i] = (Answer)answers[i].Clone();
                if (ansArr[i].content == nameLastname)
                    ansArr[i].isRight = true;
            }

            // Create new quest
            var quest = new Question();
            quest.question = nameLastname;
            quest.file = null;
            quest.answers = ansArr;
            questions.Add(quest);
        }
        result.quests = questions;

        return result;
    }
    
    private void OnFaceClick(string faceName)
    {
        if (!isButtonPressed && testView.CurrentQuestion != null)
        {
            int rightId = 0;
            int selectedId = 0;
            var screenText = questionView._quest.GetText();
            var size = screenText.Length > faceName.Length ? faceName.Length : screenText.Length;
            size -= 1;

            var id = (testView.test as Test).quesitonIdx;
            var answers = (testView.test as Test).quests[id].answers;
            for (int i = 0; i < answers.Length; i++)
            {
                if (answers[i].isRight)
                    rightId = i;
                if (faceName.StartsWith(answers[i].content))
                    selectedId = i;
            }

            StartCoroutine(ShowResult(selectedId, rightId));
        }
    }

    private IEnumerator ShowResult(int _selectedId, int _rightId)
    {
        isButtonPressed = true;

        Debug.Log($"{_selectedId} == {_rightId}");
        questNumberTMP.text = $"{(testView.test as Test).quesitonIdx + 1} из {(testView.test as Test).quests.Count}";
        float delay = 0.3f;
        if (_selectedId == _rightId)
        {
            _score += (testView.test as IRewarder).GetReward();
            scoreTMP.text = $"{_score}";
            yield return new WaitForSeconds(delay);
        }
        else
        {
            _score += (testView.test as IRewarder).GetPenaltie();
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

    public void ShowQuestion()
    {
        throw new NotImplementedException();
    }

    public void ShowQuestResult()
    {
        throw new NotImplementedException();
    }

    public void ResetView()
    {
        throw new NotImplementedException();
    }

    public void SetScore(float _score)
    {
        throw new NotImplementedException();
    }

    public void SetQuestionTime(float _time, object _params = null)
    {
        throw new NotImplementedException();
    }
}


public class FaceByNameTestView : ITestView, ITest
{
    public QuestionView CurrentQuestionView { get; set; }
    public ITest test { get; set; }
    public Result ResultScore { get => test.ResultScore; set => test.ResultScore = value; }

    public Question CurrentQuestion => test.CurrentQuestion;
    public Action<string> onFaceButtonClick;

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

        _netImages.Shuffle();
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
            imageButton.onClick.AddListener(() => onFaceButtonClick(ans._text.text));
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