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
        foreach (var ans in quest.RightAnswers)
        {
            Debug.Log(ans);
        }
        presenter = new WordsTestPresenter(model, this);
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
