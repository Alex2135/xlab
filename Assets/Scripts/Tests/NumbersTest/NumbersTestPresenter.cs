using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NewQuestionModel;


class NumbersTestPresenter : ATestPresenter<NumbersQuestModel, NumbersAdaptedQuestModel>, ITestPresenter<NumbersQuestView>
{
    protected override Dictionary<int, NumbersAdaptedQuestModel> AdaptedQuestionData { get; set; }
    public NumbersPanelCreator numbersPanelCreator;
    public NumbersPanelCreator inputFieldsCreator;
    public int digitsNumber;
    public bool isRememberScreenState;
    public Action<object> onFieldSelect;
    public Dictionary<int, bool> rightAnswers;

    public NumbersTestPresenter(NewQuestionModel.ITestView _view, ATestModel<NumbersQuestModel> _model)
    {
        testView = _view;
        testModel = _model;
        testView.OnAnswerDidEvent += view_OnAnswerDid;
        testView.OnAnsweringEvent += view_OnAnswering;
        testView.OnQuestTimeoutEvent += view_OnQuestTimeout;
        AdaptedQuestionData = new Dictionary<int, NumbersAdaptedQuestModel>();
        rightAnswers = new Dictionary<int, bool>();

        testView.SetScore(testModel.GetLastScore());
        GenerateAnswersId();
    }

    protected override void GenerateAnswersId()
    {
        var quest = testModel.GetNextQuestion();
        if (quest == null) return;
        var (questData, questIndex) = quest.Value;

        var adaptedQuest = new NumbersAdaptedQuestModel();
        var clonedList = new List<int>();

        foreach(var numb in questData.RightAnswers)
        {
            clonedList.Add(numb);
        }
        adaptedQuest.RightAnswers.Add(questIndex, clonedList);
        AdaptedQuestionData.Add(0, adaptedQuest);
    }

    public NumbersQuestView GetAdaptedQuest(Action<object> _onAnswerAction)
    {
        NumbersQuestView questView = new NumbersQuestView();
        var adaptedQuest = AdaptedQuestionData[0];
        var (_, questIndex) = testModel.GetCurrentQuestion().Value;

        if (isRememberScreenState)
        {
            var buttons = numbersPanelCreator.CreatePanel(adaptedQuest.RightAnswers[questIndex]);
            for (int i = 0; i < buttons.Count; i++)
                questView.RightAnswers.Add(i, buttons[i]);
        }
        else
        {
            var inputFields = inputFieldsCreator.CreatePanel(adaptedQuest.RightAnswers[questIndex]);
            for (int i = 0; i < inputFields.Count; i++)
            {
                var fieldComponent = inputFields[i].GetComponentInChildren<TMP_InputField>();
                fieldComponent.onSelect.AddListener(val => onFieldSelect(fieldComponent));
                fieldComponent.characterLimit = digitsNumber;
                int index = i;
                if (i + 1 < inputFields.Count)
                {
                    var nextField = inputFields[i + 1].GetComponentInChildren<TMP_InputField>();
                    fieldComponent.onValueChanged.AddListener(val => {
                        if (val.Length == fieldComponent.characterLimit)
                        {
                            (int, string) data = (index, val);
                            _onAnswerAction(data);
                            nextField.Select();
                        }
                    });
                }
                else
                {
                    fieldComponent.onValueChanged.AddListener(val => {
                        if (val.Length == fieldComponent.characterLimit)
                        {
                            (int, string) data = (index, val);
                            _onAnswerAction(data);
                            view_OnAnswerDid(null);
                        }
                    });
                }
                questView.Quest.Add(i, inputFields[i]);
            }
        }

        return questView;
    }

    public float GetTestTime()
    {
        return testModel.GetTestTime();
    }

    public void view_OnAnswering(object _userAnswer)
    {
        (int id, string val) = ((int, string))_userAnswer;
        Debug.Log($"{id} - {val}");
        var adaptedQuest = AdaptedQuestionData[0];
        var (_, questIndex) = testModel.GetCurrentQuestion().Value;
        var result = Convert.ToInt32(val) == adaptedQuest.RightAnswers[questIndex][id];
        if (!rightAnswers.ContainsKey(id))
        {
            rightAnswers[id] = result;
            if (result)
                testModel.RewardRightAnswer();
            else
                testModel.PenaltieWrongAnswer();
        }
    }

    public void view_OnAnswerDid(object _userData)
    {
        testModel.RegisterScore();
        testView.SetScore(testModel.GetLastScore());
        testView.ShowQuestResult();
    }

    public void view_OnQuestTimeout(object _obj, EventArgs _eventArgs)
    {

    }
}