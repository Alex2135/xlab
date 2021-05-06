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
    public Button continueButton;
    public Texture2D rightAnswerState;
    public Texture2D worngAnswerState;
    public Texture2D unansweredState;

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
        presenter.digitsNumber = data.DigitsNumber;
        presenter.isRememberScreenState = true;
        presenter.numbersPanelCreator = numbersPanel;
        presenter.inputFieldsCreator = inputFieldsPanel;
        presenter.onFieldSelect = obj => {
            numbersInputPanel.inputField = (TMP_InputField)obj;
        };
        numbersInputPanel.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);

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
        scoreTMP.text = $"{_score}";
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

    public void OnBackButtonClick()
    {

    }

    public void ShowQuestResult()
    {
        StartCoroutine(ShowResult());
    }

    IEnumerator ShowResult()
    {
        var objects = inputFieldsPanel.CreatedGameObjects;
        var answers = presenter.rightAnswers;
        for (int i = 0; i < objects.Count; i++)
        {
            var img = objects[i].GetComponent<Image>();
            if (answers[i])
                LoadedImage.SetTextureToImage(ref img, rightAnswerState);
            else
                LoadedImage.SetTextureToImage(ref img, worngAnswerState);
        }
        //scoreTMP.text = $"";

        yield return new WaitForSecondsRealtime(4f);

        for (int i = 0; i < objects.Count; i++)
        {
            var img = objects[i].GetComponent<Image>();
            LoadedImage.SetTextureToImage(ref img, unansweredState);
        }

        //if (PrevScreen != null)
        //{
        //    var screensController = ScreensUIController.GetInstance();
        //    screensController.DiactivateScreens();
        //    screensController.Activate(PrevScreen);
        //}
        //else
        //{
        //    Debug.Log("Prev screen not set!");
        //}
    }
}