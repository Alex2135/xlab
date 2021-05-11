using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewQuestionModel;
using TMPro;

public class TongueTwistersTestView : MonoBehaviour, IScreenController, NewQuestionModel.ITestView
{
    public TextMeshProUGUI scoreTMP;
    public TextMeshProUGUI questTMP;
    public GameObject sendButton;
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

    public event Action<object> OnAnsweringEvent;
    public event Action<object> OnAnswerDidEvent;
    public event Action<object, EventArgs> OnQuestTimeoutEvent;

    private TongueTwistersTestPresenter _presenter;

    void OnEnable()
    {
        var data = gameObject.GetComponent<TongueTwistersTestDataProvider>() ?? 
            throw new Exception("No data provider for tongue twisters test");
        var model = new TongueTwistersTestModel(data);
        _presenter = new TongueTwistersTestPresenter(null, this);

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
        
    }

    public void ShowQuestResult()
    {
        
    }
}
