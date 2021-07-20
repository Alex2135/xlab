using System;
using System.Linq;
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
    public SubjectsButtonsStates ButtonsStates { get; set; }
    // Canvas for drag and drop system
    public Canvas Canvas { get; set; }

    public int SelectedQuestId { get; set; }
    protected override Dictionary<int, AdaptedSubjectsQuestModel> AdaptedQuestionData { get; set; }
    public bool isRememberScreenState;
    private Dictionary<int, int?> userAnswers;
    private int initialRandomTerm;

    public SubjectsTestPresenter(ATestModel<SubjectsQuestModel> _model, NewQuestionModel.ITestView _view)
    {
        testModel = _model;
        testView = _view;
        testView.OnAnswerDidEvent += view_OnAnswerDid;
        testView.OnAnsweringEvent += view_OnAnswering;
        testView.OnQuestTimeoutEvent += view_OnQuestTimeout;
        initialRandomTerm = UnityEngine.Random.RandomRange(0, 100);

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
                var dragComp = questButton.Value.GetComponentInChildren<DragScript>();
                dragComp.canvas = Canvas;
            }
        }

        return resultQuestView;
    }

    void ResetSelectedButtonQuestSign()
    {
        if (testView.QuestionToView.Quest.ContainsKey(SelectedQuestId))
        {
            var selectedQuestButton = testView.QuestionToView.Quest[SelectedQuestId];
            var buttonBG = selectedQuestButton.ChildByName("ButtonBG").GetComponent<Image>();
            LoadedImage.SetTextureToImage(ref buttonBG, ButtonsStates.normalStateQuestImage);
            buttonBG.color = new Color(1f, 1f, 1f, 0f);
        }
    }

    // On quested button click
    public void view_OnAnswering(object _userAnswer)
    {
        ResetSelectedButtonQuestSign();
        int questedId = (int)_userAnswer;
        Debug.Log($"Quested ID: {questedId}");

        if (!testView.QuestionToView.Quest.ContainsKey(questedId))
            return;

        SelectedQuestId = questedId;
        int randomSeed = questedId + initialRandomTerm;
        var selectedQuestButton = testView.QuestionToView.Quest[SelectedQuestId];
        Image buttonBG = selectedQuestButton.ChildByName("ButtonBG").GetComponent<Image>();
        //Image buttonBG = selectedQuestButton.GetComponent<Image>();
        LoadedImage.SetTextureToImage(ref buttonBG, ButtonsStates.wrongAnswerImage);
        buttonBG.color = new Color(1f, 1f, 1f, 1f);

        // Get quest data and merge answers keys for shuffling
        var adaptedQuest = AdaptedQuestionData[0];
        var mergedAnswers = new List<int>();
        mergedAnswers.AddRange(adaptedQuest.RightAnswers.Keys);
        mergedAnswers.AddRange(adaptedQuest.AdditionalAnswers.Keys);

        // RandomList class create copy of the list and allow shuffle items
        var answersRL = new RandomList<int>(mergedAnswers);
        int answerPanelButtonsCount = 10;
        var panelKeys = answersRL.ShuffleSubsetWithItem(questedId, answerPanelButtonsCount, (a, b) => a == b, randomSeed);
        
        // Create Answer buttons data with textures 
        var panelButtons = new Dictionary<int, Texture2D>();
        foreach (var key in panelKeys)
        {
            if (adaptedQuest.AdditionalAnswers.ContainsKey(key))
                panelButtons.Add(key, adaptedQuest.AdditionalAnswers[key]);
            else
                panelButtons.Add(key, adaptedQuest.RightAnswers[key]);
        }

        // Shuffle answers with ordered positions
        UnityEngine.Random.seed = randomSeed;
        panelButtons = panelButtons
            .OrderBy(x => UnityEngine.Random.RandomRange(0, panelButtons.Count))
            .ToDictionary(item => item.Key, item => item.Value);
        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);

        // Create answer panel buttons objects
        var answersButtons = AnswerPanel.GeneratePanel(panelButtons);
        // Set event handler for every answer panel button
        foreach (var answerButton in answersButtons)
        {
            var buttonGO = answerButton.Value;
            var button = buttonGO.GetComponent<Button>();
            buttonGO.GetComponentInChildren<DragScript>().canvas = Canvas;
            UnityAction onAnswerClick = () => view_OnAnswerDid(answerButton.Key);
            button?.onClick.AddListener( onAnswerClick );
        }
    }

    // On answer button click 
    public void view_OnAnswerDid(object _userData)
    {
        int selectedAnswerId = (int)_userData;
        Debug.Log($"Answer ID: {selectedAnswerId}");
        userAnswers[SelectedQuestId] = selectedAnswerId;

        GameObject questButton = testView.QuestionToView.Quest[SelectedQuestId];
        Texture2D answerImage;
        var adaptedQuest = AdaptedQuestionData[0];
        if (SelectedQuestId == selectedAnswerId)
        {
            answerImage = adaptedQuest.RightAnswers[selectedAnswerId];
            var buttonBG = questButton.GetComponent<Image>();
        }
        else
        {
            if (adaptedQuest.RightAnswers.ContainsKey(selectedAnswerId))
                answerImage = adaptedQuest.RightAnswers[selectedAnswerId];
            else
                answerImage = adaptedQuest.AdditionalAnswers[selectedAnswerId];
            var buttonBG = questButton.GetComponent<Image>();
        }

        var qstImg = questButton.ChildByName("ButtonIMG").GetComponent<Image>();
        qstImg.color = new Color(1f, 1f, 1f, 1f);
        LoadedImage.SetTextureToImage(ref qstImg, answerImage);
        //testView.SetScore(testModel.CalculateScore());
        //testView.ShowQuestResult();
        //testModel.RegisterScore();

        ResetSelectedButtonQuestSign();
        GameObject nextButton;
        if (QuestPanel.Buttons.ContainsKey(SelectedQuestId + 1))
        {
            nextButton = QuestPanel.Buttons[++SelectedQuestId];
            nextButton.GetComponent<Button>().onClick.Invoke();
        }
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
