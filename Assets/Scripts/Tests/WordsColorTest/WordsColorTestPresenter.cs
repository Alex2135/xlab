using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewQuestionModel;

public class WordsColorTestPresenter : ATestPresenter<WordsColorQuestModel, AdaptedWordsColorQuestModel>, ITestPresenter<WordsColorAdaptedQuestToView>
{
    protected override Dictionary<int, AdaptedWordsColorQuestModel> AdaptedQuestionData { get; set; }

    public WordsColorTestPresenter(NewQuestionModel.ITestView _view, ATestModel<WordsColorQuestModel> _model)
    {
        testView = _view;
        testModel = _model;
        testView.OnAnswerDidEvent += view_OnAnswerDid;
        testView.OnAnsweringEvent += view_OnAnswering;
        testView.OnQuestTimeoutEvent += view_OnQuestTimeout;

        AdaptedQuestionData = new Dictionary<int, AdaptedWordsColorQuestModel>();
    }

    protected override void GenerateAnswersId()
    {
        var quest = testModel.GetNextQuestion();
        if (quest == null) return;
        var (questData, questIndex) = quest.Value;
        int answerIndex = 0;

        var adaptedQuest = new AdaptedWordsColorQuestModel();
        adaptedQuest.Quest.Add(answerIndex, questData.Quest);
        adaptedQuest.RightAnswers.Add(answerIndex, questData.RightAnswers);

        var additionalAnswers = questData.AdditionalAnswers;
        for (int i = 0; i < additionalAnswers.Count; i++)
        {
            answerIndex++;
            var list = new List<ColorUnit>();
            list.Add(additionalAnswers[i]);
            adaptedQuest.AdditionalAnswers.Add(answerIndex, list);
        }
        AdaptedQuestionData.Add(questIndex, adaptedQuest);
    }

    public WordsColorAdaptedQuestToView GetAdaptedQuest(Action<object> _onAnswerAction)
    {
        GenerateAnswersId();

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