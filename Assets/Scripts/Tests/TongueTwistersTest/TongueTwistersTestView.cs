using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewQuestionModel;

public class TongueTwistersTestView : MonoBehaviour, IScreenController, NewQuestionModel.ITestView
{
    public string screenName;

    public string ScreenName { get => screenName; set => screenName = value; }
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }
    public IAdaptedQuestToView QuestionToView { get; set; }

    public event Action<object> OnAnsweringEvent;
    public event Action<object> OnAnswerDidEvent;
    public event Action<object, EventArgs> OnQuestTimeoutEvent;

    private TongueTwistersTestPresenter _presenter;

    void OnEnable()
    {
        var model = new TongueTwistersTestModel(null);
        _presenter = new TongueTwistersTestPresenter(null, this);
    }

    public void ResetView()
    {
        
    }

    public void SetScore(float _score)
    {
        
    }

    public void ShowQuestion()
    {
        
    }

    public void ShowQuestResult()
    {
        
    }
}
