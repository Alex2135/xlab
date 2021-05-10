using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NewQuestionModel;

public class WordsColorTestPresenter : ATestPresenter<WordsColorQuestModel, AdaptedWordsColorQuestModel>, ITestPresenter<WordsColorAdaptedQuestToView>
{
    protected override Dictionary<int, AdaptedWordsColorQuestModel> AdaptedQuestionData { get; set; }

    public WordsColorUIGenerator UIGenerator { get; set; }
    private int _currentScore;

    public WordsColorTestPresenter(NewQuestionModel.ITestView _view, ATestModel<WordsColorQuestModel> _model)
    {
        testView = _view;
        testModel = _model;
        testView.OnAnswerDidEvent += view_OnAnswerDid;
        testView.OnAnsweringEvent += view_OnAnswering;
        testView.OnQuestTimeoutEvent += view_OnQuestTimeout;
        _currentScore = testModel.GetLastScore();
        testView.SetScore(_currentScore);

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

        var currentQuestModel = testModel.GetCurrentQuestion();
        if (currentQuestModel == null) return null;
        var (_, questIndex) = currentQuestModel.Value;

        var currentAdaptedQuest = AdaptedQuestionData[questIndex];
        var result = UIGenerator.GenerateQuest(currentAdaptedQuest);

        foreach (var go in result.RightAnswers)
        {
            var button = go.Value.GetComponent<Button>();
            button.onClick.AddListener(() => {
                _onAnswerAction(go.Key); 
            });
        }
        foreach (var go in result.AdditionalAnswers)
        {
            var button = go.Value.GetComponent<Button>();
            button.onClick.AddListener(() => { 
                _onAnswerAction(go.Key); 
            });
        }

        return result;
    }

    public float GetTestTime()
    {
        return testModel.GetTestTime();
    }

    public void view_OnAnswering(object _userAnswer)
    {
        var currentQuestModel = testModel.GetCurrentQuestion();
        if (currentQuestModel == null) return;
        var (_, questIndex) = currentQuestModel.Value;

        var currentAdaptedQuest = AdaptedQuestionData[questIndex];
        var userAnswer = (int)_userAnswer;
        if (currentAdaptedQuest.RightAnswers.ContainsKey(userAnswer))
            testModel.RewardRightAnswer();
        else
            testModel.PenaltieWrongAnswer();

        _currentScore += testModel.CalculateScore();
        testView.SetScore(_currentScore);
        testView.ShowQuestResult();
    }

    public void view_OnAnswerDid(object _userData)
    {
        testModel.RegisterScore();
    }

    public void view_OnQuestTimeout(object _obj, EventArgs _eventArgs)
    {
        
    }
}