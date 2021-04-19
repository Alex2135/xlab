using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewQuestionModel;

public class FacesTestPresenter : ATestPresenter<FacesQuestModel, FacesAdaptedQuestModel>, ITestPresenter<FacesAdaptedQuestToViewModel>
{
    protected override Dictionary<int, FacesAdaptedQuestModel> AdaptedQuestionData { get; set; }

    public FacesTestPresenter(ATestModel<FacesQuestModel> model, NewQuestionModel.ITestView view, WordsPanelUIController _wordsPanel)
    {
        testModel = model;
        testView = view;
        AdaptedQuestionData = new Dictionary<int, FacesAdaptedQuestModel>();

        testView.OnAnswerDid += view_OnAnswerDid;
        testView.OnAnswering += view_OnAnswering;
    }

    public FacesAdaptedQuestToViewModel GetAdaptedQuest(Action<object> _onAnswerClick)
    {
        var result = new FacesAdaptedQuestToViewModel();

        return result;
    }

    public float GetTestTime()
    {
        return testModel.GetTestTime();
    }

    public void view_OnAnswerDid(object _userData)
    {
        
    }

    public void view_OnAnswering(object _userAnswer)
    {
        
    }

    public void view_OnQuestTimeout(object _obj, EventArgs _eventArgs)
    {
        
    }

    protected override void GenerateAnswersId()
    {
        
    }
}
