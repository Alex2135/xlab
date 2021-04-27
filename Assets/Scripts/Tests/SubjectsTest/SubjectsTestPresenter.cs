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
    public int QuestionId { get; set; }
    protected override Dictionary<int, AdaptedSubjectsQuestModel> AdaptedQuestionData { get; set; }
    public bool isRememberState;
    private Dictionary<int, int?> userAnswers;

    public SubjectsTestPresenter(ATestModel<SubjectsQuestModel> _model, NewQuestionModel.ITestView _view)
    {
        testModel = _model;
        testView = _view;
        testView.OnAnswerDidEvent += view_OnAnswerDid;
        testView.OnAnsweringEvent += view_OnAnswering;
        testView.OnQuestTimeoutEvent += view_OnQuestTimeout;

        AdaptedQuestionData = new Dictionary<int, AdaptedSubjectsQuestModel>();
        userAnswers = new Dictionary<int, int?>();
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
            var questImg = questData.Quest[i];
            adaptedQuest.Quest.Add(i, questImg);
            if (questImg == null) userAnswers.Add(i, null);
            else userAnswers.Add(i, i);
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
        QuestionId = ans;
        if (userAnswers[ans] != null) return;

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
            button?.onClick.AddListener( ()=>view_OnAnswerDid((ans, answerButton.Key)) );
        }
    }

    public void view_OnAnswerDid(object _userData)
    {
        testView.ShowQuestResult();
        (int rightId, int selectedId) = ((int, int))_userData;
        if (userAnswers[rightId] != null) return;
        else userAnswers[rightId] = selectedId;
        Debug.Log($"{rightId}, {selectedId}");

        GameObject questButton = testView.QuestionToView.Quest[rightId];
        Texture2D answerImage;
        var adaptedQuest = AdaptedQuestionData[0];
        if (adaptedQuest.RightAnswers.ContainsKey(selectedId))
            answerImage = adaptedQuest.RightAnswers[selectedId];
        else
            answerImage = adaptedQuest.AdditionalAnswers[selectedId];

        var qstImg = questButton.ChildByName("ButtonIMG").GetComponent<Image>();

        qstImg.color = new Color(1f, 1f, 1f, 1f);
        LoadedImage.SetTextureToImage(ref qstImg, answerImage);
        testView.ShowQuestResult();
    }

    public void view_OnQuestTimeout(object _obj, EventArgs _eventArgs)
    {
        
    }

    public float GetTestTime()
    {
        return testModel.GetTestTime();
    }
}
