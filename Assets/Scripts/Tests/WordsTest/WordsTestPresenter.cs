using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NewQuestionModel;
using TMPro;

class WordsTestPresenter : ATestPresenter<WordsQuestModel, WordsQuestView>, ITestPresenter<WordsQuestView>
{
    private WordsAdaptedQuestModel adaptedQuests;
    protected override Dictionary<int, WordsQuestView> AdaptedQuestionData { get; set; }
    private WordsPanelUIController wordsPanelUIC;
    private List<int> _userAnswers;
    private bool isQuestShow;

    public WordsTestPresenter(ATestModel<WordsQuestModel> model, NewQuestionModel.ITestView view, WordsPanelUIController _wordsPanel)
    {
        testModel = model;
        testView = view;
        adaptedQuests = new WordsAdaptedQuestModel();
        wordsPanelUIC = _wordsPanel;
        isQuestShow = true;
        AdaptedQuestionData = new Dictionary<int, WordsQuestView>();
        _userAnswers = new List<int>();
        GenerateAnswersId();

        testView.OnAnsweringEvent += view_OnAnswering;
        testView.OnAnswerDidEvent += view_OnAnswerDid;
    }

    public void view_OnAnswering(object _userAnswer)
    {
        var answerWord = (KeyValuePair<int, string>)_userAnswer;
        foreach (var ans in AdaptedQuestionData[0].RightAnswers)
        {
            if (ans.Key == answerWord.Key)
            {
                ChangeButtonState(ans.Key, ans.Value);
                return;
            }
        }
        foreach (var ans in AdaptedQuestionData[0].AdditionalAnswers)
        {
            if (ans.Key == answerWord.Key)
            {
                ChangeButtonState(ans.Key, ans.Value);
                return;
            }
        }
    }

    private void ChangeButtonState(int _id, GameObject _buttonGO)
    {
        var image = _buttonGO.GetComponentInChildren<Image>();
        var text = _buttonGO.GetComponentInChildren<TextMeshProUGUI>();

        if (_userAnswers.Contains(_id))
        {
            image.color = new Color(242/255f, 242/255f, 242/255f);
            text.color = new Color(0x3C/255f, 0x40/255f, 0x54/255f);
            _userAnswers.Remove(_id);
        }
        else
        {
            image.color = new Color(0x37/255f, 0x96/255f, 0xFB/255f);
            text.color = new Color(1f, 1f, 1f);
            _userAnswers.Add(_id);
        }
    }

    public void view_OnAnswerDid(object _userData)
    {
        var answers = AdaptedQuestionData[0];
        GameObject button;
        Image img;
        TextMeshProUGUI txt;
        int rightQuests = 0;
        foreach (var ansId in _userAnswers)
        {
            if (answers.RightAnswers.ContainsKey(ansId))
            {
                button = answers.RightAnswers[ansId];
                img = button.GetComponent<Image>();
                img.color = new Color(0x63/255f, 0xCA/255f, 0x85/255f);
                testModel.RewardRightAnswer();
                rightQuests++;
            }
            else
            {
                button = answers.AdditionalAnswers[ansId];
                img = button.GetComponent<Image>();
                img.color = new Color(0xFF/255f, 0x69/255f, 0x69/255f);
                testModel.PenaltieWrongAnswer();
            }
            txt = button.GetComponentInChildren<TextMeshProUGUI>();
            txt.color = new Color(1f, 1f, 1f);
        }

        (_userData as TextMeshProUGUI).text = $"{rightQuests} из {answers.RightAnswers.Count}";
    }

    protected override void GenerateAnswersId()
    {
        var quest = testModel.GetNextQuestion();
        if (quest == null) return;
        var (questData, questIndex) = quest.Value;
        int answerIndex = 0;

        foreach (var answer in questData.RightAnswers)
        {
            adaptedQuests.RightAnswers?.Add(answerIndex, answer);
            answerIndex++;
        }
        foreach (var answer in questData.AdditionalAnswers)
        {
            adaptedQuests.AdditionalAnswers?.Add(answerIndex, answer);
            answerIndex++;
        }
    }

    public WordsQuestView GetAdaptedQuest(Action<object> _onAnswerClick)
    {
        if (adaptedQuests == null) throw new System.Exception("adaptedQuests is null");
        var result = new WordsQuestView();

        // Shuffled answers ids and names
        Dictionary<int, string> answersDict;
        if (isQuestShow)
            answersDict = adaptedQuests.RightAnswers;
        else
            answersDict = adaptedQuests.GetAllQuests();
        answersDict = answersDict.Shuffle();

        // Generate buttons by list
        wordsPanelUIC.CreatePanel(_onAnswerClick, answersDict);

        // Map answer indexes and buttons
        var buttons = wordsPanelUIC.Buttons;
        var buttonIndex = 0;
        foreach (var keyVal in buttons)
        {
            if (adaptedQuests.RightAnswers.ContainsKey(keyVal.Key))
                result.RightAnswers.Add(keyVal.Key, keyVal.Value);
            else
                result.AdditionalAnswers.Add(keyVal.Key, keyVal.Value);
            buttonIndex++;
        }

        if (!isQuestShow)
            AdaptedQuestionData.Add(0, result);
        isQuestShow = false;
        return result;
    }

    public void view_OnQuestTimeout(object _obj, EventArgs _eventArgs)
    {
        (testView as RememberWords).OnRememberClick();
    }

    public float GetTestTime()
    {
        return testModel.GetTestTime();
    }
}
