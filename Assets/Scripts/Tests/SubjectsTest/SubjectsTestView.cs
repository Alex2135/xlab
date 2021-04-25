using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;

using NewQuestionModel;

public class SubjectsTestView : MonoBehaviour, IScreenController, NewQuestionModel.ITestView
{
    public string screenName;
    public TextMeshProUGUI instruct;
    public GameObject rememberButton;
    public SubjectsPanelUIController questPanelUIC;
    public SubjectsPanelUIController answerPanelUIC;

    public event Action<object> OnAnsweringEvent;
    public event Action<object> OnAnswerDidEvent;
    public event Action<object, EventArgs> OnQuestTimeoutEvent;

    public string ScreenName { get => screenName; set => screenName = value; }
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }
    public IAdaptedQuestToView QuestionToView { get; set; }
    private SubjectsTestPresenter presenter;

    void OnEnable()
    {
        var data = gameObject.GetComponent<SubjectsTestGeneratedDataProvider>() ?? throw new Exception("No data provider");
        var model = new SubjectsTestModel(data);
        presenter = new SubjectsTestPresenter(model, this);
        presenter.QuestPanel = questPanelUIC;
        presenter.AnswerPanel = answerPanelUIC;
        ShowQuestion();
    }

    void OnDisable()
    {
        ResetView();
    }

    public void OnRememberClick()
    {
        presenter.isRememberState = false;
        instruct.gameObject.SetActive(false);
        rememberButton.SetActive(false);
        ShowQuestion();
    }

    public void ShowQuestion()
    {
        if (presenter.isRememberState)
        {
            presenter.GetAdaptedQuest(obj => { });
        }
        else
        {
            presenter.GetAdaptedQuest(obj => { OnAnsweringEvent.Invoke(obj); });
        }
    }

    public void ShowQuestResult()
    {
        
    }

    public void ResetView()
    {
        
    }
}
