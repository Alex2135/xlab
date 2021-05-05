using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NewQuestionModel;
using System;
using TMPro;

public class NumbersTestView : MonoBehaviour, NewQuestionModel.ITestView, IScreenController
{
    public TextMeshProUGUI timeTMP;
    public TextMeshProUGUI scoreTMP;
    public TextMeshProUGUI instructionTMP;
    public Button rememberButton;
    public VerticalLayoutGroup verticalGroupRemember;
    public VerticalLayoutGroup verticalGroupQuestion;
    public string screenName;
    public NumbersPanelCreator numbersPanel;
    public NumbersPanelCreator inputFieldsPanel;
    public NumbersInputPanel numbersInputPanel;


    public IAdaptedQuestToView QuestionToView { get; set; }
    public string ScreenName { get => screenName; set => screenName = value; }
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }

    public event Action<object> OnAnsweringEvent;
    public event Action<object> OnAnswerDidEvent;
    public event Action<object, EventArgs> OnQuestTimeoutEvent;

    private NumbersTestPresenter presenter;

    void OnEnable()
    {
        var data = gameObject.GetComponent<NumbersTestDataProvider>() ?? throw new Exception("Numbers test have no data provider");
        var model = new NumbersTestModel(data);
        presenter = new NumbersTestPresenter(this, model);
        presenter.digitsNumber = data.digitsNumber;
        presenter.isRememberScreenState = true;
        presenter.numbersPanelCreator = numbersPanel;
        presenter.inputFieldsCreator = inputFieldsPanel;
        presenter.onFieldSelect = obj => {
            numbersInputPanel.inputField = (TMP_InputField)obj;
        };
        numbersInputPanel.gameObject.SetActive(false);

        ShowQuestion();
    }

    public void OnRememberClick()
    {
        presenter.isRememberScreenState = false;
        ShowQuestion();
    }

    public void ResetView()
    {
        
    }

    public void SetScore(float _score)
    {
        
    }

    public void ShowQuestion()
    {
        verticalGroupQuestion.gameObject.DestroyChildrenObjects();
        verticalGroupRemember.gameObject.DestroyChildrenObjects();
        if (presenter.isRememberScreenState)
        {
            QuestionToView = presenter.GetAdaptedQuest(obj => { });
        }
        else
        {
            numbersInputPanel.gameObject.SetActive(true);
            QuestionToView = presenter.GetAdaptedQuest(obj => {
                presenter.view_OnAnswering(obj);
            });
            foreach (var keyVal in QuestionToView.Quest)
            {
                numbersInputPanel.inputField = keyVal.Value.GetComponentInChildren<TMP_InputField>();
                break;
            }
        }
    }

    public void ShowQuestResult()
    {
        
    }
}