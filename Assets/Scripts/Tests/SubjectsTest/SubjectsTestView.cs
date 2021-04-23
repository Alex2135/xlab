using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NewQuestionModel;
using System;

public class SubjectsTestView : MonoBehaviour, IScreenController, NewQuestionModel.ITestView
{
    public string screenName;
    public TextMeshProUGUI instruct;
    public GameObject subjectsGrid;
    public GameObject answersGrid;

    public event Action<object> OnAnswering;
    public event Action<object> OnAnswerDid;
    public event Action<object, EventArgs> OnQuestTimeout;

    public string ScreenName { get => screenName; set => screenName = value; }
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }
    public IAdaptedQuestToView QuestionToView { get; set; }
    private SubjectsTestPresenter presenter;

    void OnEnable()
    {
        var model = new SubjectsTestModel(new SubjectsTestGeneratedDataProvider());
        presenter = new SubjectsTestPresenter(model, this);
        ShowQuestion();
    }

    void OnDisable()
    {
        ResetView();
    }

    public void ShowQuestion()
    {
        presenter.GetAdaptedQuest(obj => { });
    }

    public void ShowQuestResult()
    {
        
    }

    public void ResetView()
    {
        
    }
}
