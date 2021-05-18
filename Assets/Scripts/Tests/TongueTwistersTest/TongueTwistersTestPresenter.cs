using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewQuestionModel;
using TMPro;

public class TongueTwistersTestPresenter :
    ATestPresenter<TongueTwistersQuestModel, AdaptedTongueTwistersQuestModel>,
    ITestPresenter<TongueTwistersQuestView>
{
    protected override Dictionary<int, AdaptedTongueTwistersQuestModel> AdaptedQuestionData { get; set; }

    public TongueTwistersTestPresenter(ATestModel<TongueTwistersQuestModel> _model, NewQuestionModel.ITestView _view)
    {
        testModel = _model;
        testView = _view;
        testView.OnAnsweringEvent += view_OnAnswering;
        testView.OnAnswerDidEvent += view_OnAnswerDid;
        testView.OnQuestTimeoutEvent += view_OnQuestTimeout;
        testView.SetScore(UserModel.GetLastScore("TongueTwisters"));
    }

    protected override void GenerateAnswersId()
    {
        var quest = testModel.GetNextQuestion();
        if (quest == null) throw new Exception("No quests");
        var (questData, questId) = quest.Value;

        var adaptedQuest = new AdaptedTongueTwistersQuestModel();
        adaptedQuest.Quest.Add(questId, questData.Quest);
    }

    public TongueTwistersQuestView GetAdaptedQuest(Action<object> _onAnswerAction)
    {
        GenerateAnswersId();

        var (questData, questId) = testModel.GetCurrentQuestion().Value;
        var questText = questData.Quest[questId];
        var questTMP = testView.QuestionToView.Quest[0].GetComponent<TextMeshProUGUI>();
        questTMP.text = questText;

        return null;
    }

    public float GetTestTime()
    {
        return testModel.GetTestTime();
    }

    public void view_OnAnswerDid(object _userData)
    {
        string filePath = (string)_userData;
        //Debug.Log($"Result path: {filePath}");
    }

    public void view_OnAnswering(object _userAnswer)
    {
        
    }

    public void view_OnQuestTimeout(object _obj, EventArgs _eventArgs)
    {
        
    }
}
