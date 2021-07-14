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
        (SubjectsQuestModel, int)? quest = testModel.GetNextQuestion();
        if (quest == null) return;
        (SubjectsQuestModel questData, int questIndex) = quest.Value;
        int answerIndex = 0;

        // Create adapted quest model
        // Set quest images for adampter model
        var adaptedQuest = new AdaptedSubjectsQuestModel();
        for (int i = 0; i < questData.Quest.Count; i++)
        {
            var questImg = questData.Quest[i];
            adaptedQuest.Quest.Add(i, questImg);
            if (questImg == null) userAnswers.Add(i, null);
            else userAnswers.Add(i, i);
        }

        // Set right answers images for adampter model
        for (int i = 0; i < questData.RightAnswers.Count; i++)
        {
            adaptedQuest.RightAnswers.Add(answerIndex, questData.RightAnswers[i]);
            answerIndex++;
        }

        // Set additional answers images for adampter model
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

    // On quested button click
    public void view_OnAnswering(object _userAnswer)
    {
        ResetSelectedButtonQuestSign();
        int answerId = (int)_userAnswer;
        SelectedQuestId = answerId;
        //if (userAnswers[SelectedQuestId] != null) return;

        var selectedQuestButton = testView.QuestionToView.Quest[SelectedQuestId];
        Image buttonBG = selectedQuestButton.ChildByName("ButtonBG").GetComponent<Image>();
        LoadedImage.SetTextureToImage(ref buttonBG, buttonsStates.questionSignImage);
        buttonBG.color = new Color(1f, 1f, 1f, 1f);

        // Get quest data and merge answers keys for shuffling
        var adaptedQuest = AdaptedQuestionData[0];
        var merged = new List<int>();
        merged.AddRange(adaptedQuest.RightAnswers.Keys);
        merged.AddRange(adaptedQuest.AdditionalAnswers.Keys);

        // RandomList class create copy of the list and allow shuffle items
        var rl = new RandomList<int>(merged);
        int answerPanelButtonsCount = 10;
        var panelKeys = rl.ShuffleSubsetWithItem(answerId, answerPanelButtonsCount, (a, b) => a == b);
        
        // Create Answer buttons data with textures 
        var panelButtons = new Dictionary<int, Texture2D>();
        foreach (var key in panelKeys)
        {
            if (adaptedQuest.AdditionalAnswers.ContainsKey(key))
                panelButtons.Add(key, adaptedQuest.AdditionalAnswers[key]);
            else
                panelButtons.Add(key, adaptedQuest.RightAnswers[key]);
        }
        UnityEngine.Random.InitState((int)answerId);
        panelButtons = panelButtons.Shuffle();

        // Create answer panel buttons objects
        var answersButtons = AnswerPanel.GeneratePanel(panelButtons);
        // Set event handler for every answer panel button
        foreach (var answerButton in answersButtons)
        {
            var button = answerButton.Value.GetComponent<Button>();
            UnityAction onAnswerClick = () => {
                view_OnAnswerDid(answerButton.Key);
                ResetSelectedButtonQuestSign();
            };
            button?.onClick.AddListener( onAnswerClick );
        }

        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
    }

    // On answer button click 
    public void view_OnAnswerDid(object _userData)
    {
        int selectedAnswerId = (int)_userData;
        //if (userAnswers[SelectedQuestId] != null) return;
        //else 
            userAnswers[SelectedQuestId] = selectedAnswerId;
        //Debug.Log($"{SelectedQuestId}, {selectedAnswerId}");

        GameObject questButton = testView.QuestionToView.Quest[SelectedQuestId];
        Texture2D answerImage;
        var adaptedQuest = AdaptedQuestionData[0];
        if (SelectedQuestId == selectedAnswerId)
        {
            answerImage = adaptedQuest.RightAnswers[selectedAnswerId];
            var buttonBG = questButton.GetComponent<Image>();
            //LoadedImage.SetTextureToImage(ref buttonBG, buttonsStates.rightAnswerImage);
            //testModel.RewardRightAnswer();
        }
        else
        {
            if (adaptedQuest.RightAnswers.ContainsKey(selectedAnswerId))
                answerImage = adaptedQuest.RightAnswers[selectedAnswerId];
            else
                answerImage = adaptedQuest.AdditionalAnswers[selectedAnswerId];
            var buttonBG = questButton.GetComponent<Image>();
            //LoadedImage.SetTextureToImage(ref buttonBG, buttonsStates.wrongAnswerImage);
            //testModel.PenaltieWrongAnswer();
        }

        var qstImg = questButton.ChildByName("ButtonIMG").GetComponent<Image>();
        qstImg.color = new Color(1f, 1f, 1f, 1f);
        LoadedImage.SetTextureToImage(ref qstImg, answerImage);
        //testView.SetScore(testModel.CalculateScore());
        //testView.ShowQuestResult();
        //testModel.RegisterScore();
    }

    public void view_OnQuestTimeout(object _obj, EventArgs _eventArgs)
    {
        testView.SetScore(testModel.CalculateScore());
        testView.ShowQuestResult();
        testModel.RegisterScore();
    }

    public float GetTestTime()
    {
        return testModel.GetTestTime();
    }
}
