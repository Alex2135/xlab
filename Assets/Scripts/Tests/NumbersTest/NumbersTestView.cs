using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NewQuestionModel;
using System;
using TMPro;

public class NumbersTestView : MonoBehaviour, NewQuestionModel.ITestView, IScreenController
{
    public TextMeshProUGUI timeTMP;
    public TextMeshProUGUI scoreTMP;
    public TextMeshProUGUI instructionTMP;
    public Button rememberButton;
    public string screenName;

    public IAdaptedQuestToView QuestionToView { get; set; }
    public string ScreenName { get => screenName; set => screenName = value; }
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }

    public event Action<object> OnAnsweringEvent;
    public event Action<object> OnAnswerDidEvent;
    public event Action<object, EventArgs> OnQuestTimeoutEvent;

    private NumbersTestPresenter presenter;

    void OnEnable()
    {
        var data = gameObject.GetComponent<NumbersTestDataProvider>() ?? throw new Exception("Numbers test have no data provider");
        var model = new NumbersTestModel(data);
        presenter = new NumbersTestPresenter(this, model);
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