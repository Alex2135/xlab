using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewQuestionModel;
using System;

public class TongueTwistersTestPresenter :
    ATestPresenter<TongueTwistersQuestModel, AdaptedTongueTwistersQuestModel>,
    ITestPresenter<TongueTwistersAdaptedQuestToView>
{
    protected override Dictionary<int, AdaptedTongueTwistersQuestModel> AdaptedQuestionData { get; set; }

    public TongueTwistersTestPresenter(ATestModel<TongueTwistersQuestModel> _model, NewQuestionModel.ITestView _view)
    {
        testModel = _model;
        testView = _view;
        testView.OnAnsweringEvent += view_OnAnswering;
        testView.OnAnswerDidEvent += view_OnAnswerDid;
        testView.OnQuestTimeoutEvent += view_OnQuestTimeout;
    }

    protected override void GenerateAnswersId()
    {

    }

    public TongueTwistersAdaptedQuestToView GetAdaptedQuest(Action<object> _onAnswerAction)
    {
        return null;
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
}
