using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewQuestionModel;
using System;

public class WordsColorTestPresenter : ATestPresenter<WordsColorQuestModel, WordsAdaptedQuestModel>, ITestPresenter<WordsColorTestView>
{
    protected override Dictionary<int, WordsAdaptedQuestModel> AdaptedQuestionData { get; set; }

    public WordsColorTestPresenter(NewQuestionModel.ITestView _view, ATestModel<WordsColorQuestModel> _model)
    {
        testView = _view;
        testModel = _model;
    }

    protected override void GenerateAnswersId()
    {
        
    }

    public WordsColorTestView GetAdaptedQuest(Action<object> _onAnswerAction)
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