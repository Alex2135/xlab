using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NewQuestionModel;
using UnityEngine.Events;

public class SubjectsTestPresenter : ATestPresenter<SubjectsQuestModel, AdaptedSubjectsQuestModel>, NewQuestionModel.ITestPresenter<SubjectsQuestView>
{
    // Set panels from view
    public SubjectsPanelUIController QuestPanel { get; set; }
    public SubjectsPanelUIController AnswerPanel { get; set; }
    // States of the quest buttons
    public SubjectsButtonsStates buttonsStates { get; set; }
    public int SelectedQuestId { get; set; }
    protected override Dictionary<int, AdaptedSubjectsQuestModel> AdaptedQuestionData { get; set; }
    public bool isRememberScreenState;
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
        isRememberScreenState = true;
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
        var resultQuestView = new SubjectsQuestView();
        var adaptedQuest = AdaptedQuestionData[0];
        if (isRememberScreenState)
        {
            resultQuestView.Quest = QuestPanel.GeneratePanel(adaptedQuest.RightAnswers);
        }
        else
        {
            resultQuestView.Quest = QuestPanel.GeneratePanel(adaptedQuest.Quest);
            foreach (var questButton in resultQuestView.Quest)
            {
                var button = questButton.Value.GetComponent<Button>();
                button.onClick.AddListener( ()=>_onAnswerClick(questButton.Key) );
            }
        }

        return resultQuestView;
    }

    void ResetSelectedButtonQuestSign()
    {
        var selectedQuestButton = testView.QuestionToView.Quest[SelectedQuestId];
        var buttonBG = selectedQuestButton.ChildByName("ButtonBG").GetComponent<Image>();
        LoadedImage.SetTextureToImage(ref buttonBG, buttonsStates.normalStateQuestImage);
        buttonBG.color = new Color(1f, 1f, 1f, 0f);
    }

    public void view_OnAnswering(object _userAnswer)
    {
        ResetSelectedButtonQuestSign();
        int ans = (int)_userAnswer;
        SelectedQuestId = ans;
        if (userAnswers[SelectedQuestId] != null) return;

        var selectedQuestButton = testView.QuestionToView.Quest[SelectedQuestId];
        var buttonBG = selectedQuestButton.ChildByName("ButtonBG").GetComponent<Image>();
        LoadedImage.SetTextureToImage(ref buttonBG, buttonsStates.questionSignImage);
        buttonBG.color = new Color(1f, 1f, 1f, 1f);

        // Get quest data and merge answers keys for shuffling
        var adaptedQuest = AdaptedQuestionData[0];
        var merged = new List<int>();
        merged.AddRange(adaptedQuest.RightAnswers.Keys);
        merged.AddRange(adaptedQuest.AdditionalAnswers.Keys);

        // Get shuffled keys for answers
        var rl = new RandomList<int>(merged);
        int answerPanelButtonsCount = 10;
        var panelKeys = rl.GetRandomSubsetWithRightItem(ans, answerPanelButtonsCount, (a, b) => a == b);
        
        // Create Answer buttons 
        var panelButtons = new Dictionary<int, Texture2D>();
        foreach (var key in panelKeys)
        {
            if (adaptedQuest.AdditionalAnswers.ContainsKey(key))
                panelButtons.Add(key, adaptedQuest.AdditionalAnswers[key]);
            else
                panelButtons.Add(key, adaptedQuest.RightAnswers[key]);
        }
        panelButtons = panelButtons.Shuffle();

        // Create answer panel
        var answersButtons = AnswerPanel.GeneratePanel(panelButtons);
        foreach (var answerButton in answersButtons)
        {
            var button = answerButton.Value.GetComponent<Button>();
            UnityAction onAnswerClick = () => {
                view_OnAnswerDid(answerButton.Key);
                ResetSelectedButtonQuestSign();
            };
            button?.onClick.AddListener( onAnswerClick );
        }
    }

    public void view_OnAnswerDid(object _userData)
    {
        int selectedAnswerId = (int)_userData;
        if (userAnswers[SelectedQuestId] != null) return;
        else userAnswers[SelectedQuestId] = selectedAnswerId;
        //Debug.Log($"{SelectedQuestId}, {selectedAnswerId}");

        GameObject questButton = testView.QuestionToView.Quest[SelectedQuestId];
        Texture2D answerImage;
        var adaptedQuest = AdaptedQuestionData[0];
        if (SelectedQuestId == selectedAnswerId)
        {
            answerImage = adaptedQuest.RightAnswers[selectedAnswerId];
            var buttonBG = questButton.GetComponent<Image>();
            LoadedImage.SetTextureToImage(ref buttonBG, buttonsStates.rightAnswerImage);
            testModel.RewardRightAnswer();
        }
        else
        {
            if (adaptedQuest.RightAnswers.ContainsKey(selectedAnswerId))
                answerImage = adaptedQuest.RightAnswers[selectedAnswerId];
            else
                answerImage = adaptedQuest.AdditionalAnswers[selectedAnswerId];
            var buttonBG = questButton.GetComponent<Image>();
            LoadedImage.SetTextureToImage(ref buttonBG, buttonsStates.wrongAnswerImage);
            testModel.PenaltieWrongAnswer();
        }

        var qstImg = questButton.ChildByName("ButtonIMG").GetComponent<Image>();
        qstImg.color = new Color(1f, 1f, 1f, 1f);
        LoadedImage.SetTextureToImage(ref qstImg, answerImage);
        testView.SetScore(testModel.GetScore());
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
