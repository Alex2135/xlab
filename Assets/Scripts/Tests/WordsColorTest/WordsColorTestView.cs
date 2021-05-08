using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewQuestionModel;
using TMPro;

public class WordsColorTestView : MonoBehaviour, IScreenController, NewQuestionModel.ITestView, NewQuestionModel.ITestScreenController
{
    public TextMeshProUGUI scoreTMP;
    public TextMeshProUGUI timeTMP;
    public string screenName;

    public string ScreenName { get; set; }
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }
    public IAdaptedQuestToView QuestionToView { get; set; }
    public string TestName => "WordsColor";

    public event Action<object> OnAnsweringEvent;
    public event Action<object> OnAnswerDidEvent;
    public event Action<object, EventArgs> OnQuestTimeoutEvent;

    private WordsColorTestPresenter presenter;

    void OnEnable()
    {
        var dataProvider = gameObject.GetComponent<WordsColorTestGeneratedDataProvider>() ?? throw new Exception("Game object have not data provider");
        var model = new WordsColorTestModel(dataProvider);
        presenter = new WordsColorTestPresenter(this, model);

        ShowQuestion();
    }

    public void ShowQuestion()
    {
        QuestionToView = presenter.GetAdaptedQuest(obj => { });
    }

    public void ShowQuestResult()
    {
        
    }

    public void ResetView()
    {
        
    }

    public void SetScore(float _score)
    {
        
    }
}
