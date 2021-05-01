using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewQuestionModel;
using System;

class NumbersTestPresenter : ATestPresenter<NumbersQuestModel, NumbersAdaptedQuestModel>, ITestPresenter<NumbersQuestView>
{
    protected override Dictionary<int, NumbersAdaptedQuestModel> AdaptedQuestionData { get; set; }
    public NumbersPanelCreator numbersPanelCreator;
    public bool isRememberScreenState;

    public NumbersTestPresenter(NewQuestionModel.ITestView _view, ATestModel<NumbersQuestModel> _model)
    {
        testView = _view;
        testModel = _model;
        testView.OnAnswerDidEvent += view_OnAnswerDid;
        testView.OnAnsweringEvent += view_OnAnswering;
        testView.OnQuestTimeoutEvent += view_OnQuestTimeout;
        AdaptedQuestionData = new Dictionary<int, NumbersAdaptedQuestModel>();

        GenerateAnswersId();
    }

    protected override void GenerateAnswersId()
    {
        var quest = testModel.GetNextQuestion();
        if (quest == null) return;
        var (questData, questIndex) = quest.Value;

        var adaptedQuest = new NumbersAdaptedQuestModel();
        var clonedList = new List<int>();

        foreach(var numb in questData.RightAnswers)
        {
            clonedList.Add(numb);
        }
        adaptedQuest.RightAnswers.Add(questIndex, clonedList);
        AdaptedQuestionData.Add(0, adaptedQuest);
    }

    public NumbersQuestView GetAdaptedQuest(Action<object> _onAnswerClick)
    {
        NumbersQuestView questView = new NumbersQuestView();

        if (isRememberScreenState)
        {
            var adaptedQuest = AdaptedQuestionData[0];
            var (_, questIndex) = testModel.GetCurrentQuestion().Value;
            
            var buttons = numbersPanelCreator.CreatePanel(adaptedQuest.RightAnswers[questIndex]);
            for (int i = 0; i < buttons.Count; i++)
            {
                questView.RightAnswers.Add(i, buttons[i]);
            }
        }
        else
        {

        }

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
}