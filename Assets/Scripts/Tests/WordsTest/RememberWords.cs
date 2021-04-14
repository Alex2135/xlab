using NewQuestionModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RememberWords : MonoBehaviour, IScreenController, NewQuestionModel.ITestView
{
    public VerticalLayoutGroup verticalLayout;
    public TextMeshProUGUI scoreTMP;
    public TextMeshProUGUI timeTMP;
    public TextMeshProUGUI questScoreTMP;
    public WordsPanelUIController wordsPanelUIC;
    public string screenName;

    public string ScreenName { get => screenName; set => screenName = value; }
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }
    IAdaptedQuestToView NewQuestionModel.ITestView.QuestionView { get; set; }
    public Dictionary<string, GameObject> ContextViewElements { get ; set; }

    public event Action<object> OnAnswering;
    public event Action<object> OnAnswerDid;
    private WordsTestPresenter presenter;

    private void OnEnable()
    {
        var model = new WordsTestModel(new WordsTestGeneratedDataSource());
        (WordsQuestModel, int)? modelQuest = model.GetNextQuestion();
        (var quest, var index) = modelQuest.Value;
        presenter = new WordsTestPresenter(model, this);
        var content = quest.RightAnswers.ConvertAll(s => s);
        content.AddRange(quest.AdditionalAnswers);
        content.ShuffleItems();
        wordsPanelUIC.CreatePanel(content);
    }

    void OnAnsweringClick(object _o)
    {
        OnAnswering.Invoke(_o);
    }

    private void OnDisable()
    {
        
    }

    public void ShowQuestion()
    {
       
    }

    public void ShowQuestResult()
    {
        
    }

    public void ResetView()
    {
        
    }
}
