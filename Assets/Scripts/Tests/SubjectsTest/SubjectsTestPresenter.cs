using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewQuestionModel;

public class SubjectsTestPresenter : ATestPresenter<SubjectsQuestModel, AdaptedSubjectsQuestModel>, NewQuestionModel.ITestPresenter<SubjectsQuestView>
{
    public SubjectsPanelUIController QuestPanel { get; set; }
    public SubjectsPanelUIController AnswerPanel { get; set; }
    protected override Dictionary<int, AdaptedSubjectsQuestModel> AdaptedQuestionData { get; set; }
    private bool isRememberState;

    public SubjectsTestPresenter(ATestModel<SubjectsQuestModel> _model, NewQuestionModel.ITestView _view)
    {
        testModel = _model;
        testView = _view;
        testView.OnAnswerDid += view_OnAnswerDid;
        testView.OnAnswering += view_OnAnswering;
        testView.OnQuestTimeout += view_OnQuestTimeout;

        AdaptedQuestionData = new Dictionary<int, AdaptedSubjectsQuestModel>();
        isRememberState = true;
    }

    protected override void GenerateAnswersId()
    {
        // Get next question model and index
        var quest = testModel.GetNextQuestion();
        if (quest == null) return;
        var (questData, questIndex) = quest.Value;
        int answerIndex = 0;

        // Create adapted quest model
        var adaptedQuest = new AdaptedSubjectsQuestModel();
        for (int i = 0; i < questData.Quest.Count; i++)
        {
            adaptedQuest.Quest.Add(i, questData.Quest[i]);
        }
        for (int i = 0; i < questData.RightAnswers.Count; i++)
        {
            adaptedQuest.RightAnswers.Add(answerIndex, questData.RightAnswers[i]);
            answerIndex++;
        }
        for (int i = 0; i < questData.AdditionalAnswers.Count; i++)
        {
            adaptedQuest.AdditionalAnswers.Add(answerIndex, questData.AdditionalAnswers[i]);
            answerIndex++;
        }

        AdaptedQuestionData.Add(0, adaptedQuest);
    }

    public SubjectsQuestView GetAdaptedQuest(Action<object> _onAnswerClick)
    {
        var adaptedQuest = AdaptedQuestionData[0];

        var allAnswers = new Dictionary<int, GameObject>();
        foreach (var ans in adaptedQuest.RightAnswers)
        {
            //allAnswers.Add(ans.Key, new);
        }


        return null;
    }

    public void view_OnAnswering(object _userAnswer)
    {
        
    }

    public void view_OnAnswerDid(object _userData)
    {
        
    }

    public void view_OnQuestTimeout(object _obj, EventArgs _eventArgs)
    {
        
    }

    public float GetTestTime()
    {
        return testModel.GetTestTime();
    }
}
