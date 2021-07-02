using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewQuestionModel;
using TMPro;

public class TongueTwistersTestView : MonoBehaviour, IScreenController, NewQuestionModel.ITestView, NewQuestionModel.ITestScreenController
{
    public TextMeshProUGUI scoreTMP;
    public TextMeshProUGUI questTMP;
    public VideoRecorderUIController videoRecorder;
    public GameObject sendButton;
    public GameObject testSendPanel;
    public string screenName;

    public string ScreenName { get => screenName; set => screenName = value; }
    private IScreenController _nextScreen;
    public IScreenController NextScreen
    {
        get { return _nextScreen; }
        set
        {
            value.PrevScreen = this;
            _nextScreen = value;
        }
    }
    public IScreenController PrevScreen { get; set; }
    public IAdaptedQuestToView QuestionToView { get; set; }

    public string TestName => "TongueTwisters";

    public event Action<object> OnAnsweringEvent;
    public event Action<object> OnAnswerDidEvent;
    public event Action<object, EventArgs> OnQuestTimeoutEvent;

    private TongueTwistersTestPresenter _presenter;

    void OnEnable()
    {
        var data = gameObject.GetComponent<TongueTwistersTestDataProvider>() ?? 
            throw new Exception("No data provider for tongue twisters test");
        var model = new TongueTwistersTestModel(data);
        _presenter = new TongueTwistersTestPresenter(model, this);
        QuestionToView = new TongueTwistersQuestView();
        QuestionToView.Quest.Add(0, questTMP.gameObject);
        testSendPanel.SetActive(false);

        ShowQuestion();
    }

    public void ResetView()
    {
        
    }

    public void SetScore(float _score)
    {
        scoreTMP.text = $"{_score}";
    }

    public void ShowQuestion()
    {
        _presenter.GetAdaptedQuest(null);
    }

    public void ShowQuestResult()
    {
        OnAnswerDidEvent?.Invoke(videoRecorder.FilePath);

        testSendPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void GoToNextScreen()
    {
        if (NextScreen != null)
        {
            var screensController = ScreensUIController.GetInstance();
            screensController.DiactivateScreens();
            screensController.Activate(NextScreen);
        }
        else
        {
            Debug.Log("Next screen not set!");
        }
    }
}
