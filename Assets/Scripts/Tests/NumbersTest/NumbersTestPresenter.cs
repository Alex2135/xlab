using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewQuestionModel;
using System;

class NumbersTestPresenter : ATestPresenter<NumbersQuestModel, NumbersAdaptedQuestModel>, ITestPresenter<NumbersQuestView>
{
    protected override Dictionary<int, NumbersAdaptedQuestModel> AdaptedQuestionData { get; set; }

    public NumbersTestPresenter(NewQuestionModel.ITestView _view, ATestModel<NumbersQuestModel> _model)
    {
        testView = _view;
        testModel = _model;
    }

    public NumbersQuestView GetAdaptedQuest(Action<object> _onAnswerClick)
    {
        NumbersQuestView questView = new NumbersQuestView();



        return questView;
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