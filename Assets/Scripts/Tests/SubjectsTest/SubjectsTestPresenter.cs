using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NewQuestionModel;

public class SubjectsTestPresenter : ATestPresenter<SubjectsQuestModel, AdaptedSubjectsQuestModel>, NewQuestionModel.ITestPresenter<SubjectsQuestView>
{
    public SubjectsPanelUIController QuestPanel { get; set; }
    public SubjectsPanelUIController AnswerPanel { get; set; }
    protected override Dictionary<int, AdaptedSubjectsQuestModel> AdaptedQuestionData { get; set; }
    public bool isRememberState;

    public SubjectsTestPresenter(ATestModel<SubjectsQuestModel> _model, NewQuestionModel.ITestView _view)
    {
        testModel = _model;
        testView = _view;
        testView.OnAnswerDidEvent += view_OnAnswerDid;
        testView.OnAnsweringEvent += view_OnAnswering;
        testView.OnQuestTimeoutEvent += view_OnQuestTimeout;

        AdaptedQuestionData = new Dictionary<int, AdaptedSubjectsQuestModel>();
        isRememberState = true;
        GenerateAnswersId();
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
        var result = new SubjectsQuestView();
        var adaptedQuest = AdaptedQuestionData[0];
        if (isRememberState)
        {
            result.Quest = QuestPanel.GeneratePanel(adaptedQuest.RightAnswers);
        }
        else
        {
            result.Quest = QuestPanel.GeneratePanel(adaptedQuest.Quest);
            foreach (var questButton in result.Quest)
            {
                var button = questButton.Value.GetComponent<Button>();
                button.onClick.AddListener( ()=>_onAnswerClick(questButton.Key) );
            }
        }

        return result;
    }

    public void view_OnAnswering(object _userAnswer)
    {
        int ans = (int)_userAnswer;
        Debug.Log($"{ans}");

        var adaptedQuest = AdaptedQuestionData[0];
        var merged = new List<int>();
        merged.AddRange(adaptedQuest.RightAnswers.Keys);
        merged.AddRange(adaptedQuest.AdditionalAnswers.Keys);

        var rl = new RandomList<int>(merged);
        int answerPanelButtonsCount = 10;
        var panelKeys = rl.GetRandomSubsetWithRightItem(ans, answerPanelButtonsCount, (a, b) => a == b);
        var panelButtons = new Dictionary<int, Texture2D>();
        foreach (var key in panelKeys)
        {
            if (adaptedQuest.AdditionalAnswers.ContainsKey(key))
                panelButtons.Add(key, adaptedQuest.AdditionalAnswers[key]);
            else
                panelButtons.Add(key, adaptedQuest.RightAnswers[key]);
        }
        panelButtons = panelButtons.Shuffle();
        var answersButtons = AnswerPanel.GeneratePanel(panelButtons);
        foreach (var answerButton in answersButtons)
        {
            var button = answerButton.Value.GetComponent<Button>();
            button?.onClick.AddListener( ()=>view_OnAnswerDid((answerButton.Key, ans)) );
        }    
    }

    public void view_OnAnswerDid(object _userData)
    {
        testView.ShowQuestResult();
        (int, int) data = ((int, int))_userData;
        Debug.Log($"{data.Item1}, {data.Item2}");
    }

    public void view_OnQuestTimeout(object _obj, EventArgs _eventArgs)
    {
        
    }

    public float GetTestTime()
    {
        return testModel.GetTestTime();
    }
}
