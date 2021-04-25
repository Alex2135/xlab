using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewQuestionModel;

public class FacesTestPresenter : ATestPresenter<FacesQuestModel, FacesAdaptedQuestModel>, ITestPresenter<FacesAdaptedQuestToViewModel>
{
    protected override Dictionary<int, FacesAdaptedQuestModel> AdaptedQuestionData { get; set; }
    private NewQuestionModel.ITestView nameByFaceView;
    private NewQuestionModel.ITestView faceByNameView;

    public FacesTestPresenter(
        ATestModel<FacesQuestModel> model, 
        NewQuestionModel.ITestView _nameByFaceView, 
        NewQuestionModel.ITestView _faceByNameView, 
        WordsPanelUIController _wordsPanel)
    {
        testModel = model;
        nameByFaceView = _nameByFaceView;
        faceByNameView = _faceByNameView;
        AdaptedQuestionData = new Dictionary<int, FacesAdaptedQuestModel>();

        testView.OnAnswerDidEvent += view_OnAnswerDid;
        testView.OnAnsweringEvent += view_OnAnswering;
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
