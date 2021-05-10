using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NewQuestionModel;
using TMPro;

public class WordsColorTestView : MonoBehaviour, IScreenController, NewQuestionModel.ITestView, NewQuestionModel.ITestScreenController
{
    public TextMeshProUGUI scoreTMP;
    public TextMeshProUGUI timeTMP;
    public string screenName;
    public WordsColorUIGenerator wordsColorUIGenerator;
    public GameObject rightAnswerSign;
    public GameObject wrongAnswerSign;

    public string ScreenName { get; set; }
    private IScreenController _nextScreen;
    public IScreenController NextScreen
    {
        get { return _nextScreen; }
        set
        {
            value.PrevScreen = this;
            _nextScreen = value;
        }
    }
    public IScreenController PrevScreen { get; set; }
    public IAdaptedQuestToView QuestionToView { get; set; }
    public string TestName => "WordsColor";

    public event Action<object> OnAnsweringEvent;
    public event Action<object> OnAnswerDidEvent;
    public event Action<object, EventArgs> OnQuestTimeoutEvent;

    private WordsColorTestPresenter presenter;
    private int _answerId;

    void OnEnable()
    {
        var dataProvider = gameObject.GetComponent<WordsColorTestGeneratedDataProvider>() ?? throw new Exception("Game object have not data provider");
        var model = new WordsColorTestModel(dataProvider);
        presenter = new WordsColorTestPresenter(this, model);
        presenter.UIGenerator = wordsColorUIGenerator;

        ShowQuestion();
    }

    public void ShowQuestion()
    {
        QuestionToView = presenter.GetAdaptedQuest(obj => {
            _answerId = (int)obj;
            OnAnsweringEvent.Invoke(obj);
        });
        if (QuestionToView == null)
        {
            OnAnswerDidEvent.Invoke(null);
            if (NextScreen != null)
            {
                var screensController = ScreensUIController.GetInstance();
                screensController.DiactivateScreens();
                screensController.Activate(NextScreen);
            }
            else
            {
                Debug.Log("Next screen not set!");
            }
        }
    }

    public void ShowQuestResult()
    {
        foreach (var go in QuestionToView.RightAnswers)
        {
            var button = go.Value.GetComponent<Button>();
            button.interactable = false;
        }
        foreach (var go in QuestionToView.AdditionalAnswers)
        {
            var button = go.Value.GetComponent<Button>();
            button.interactable = false;
        }

        StartCoroutine(ShowResult(1f));
    }

    IEnumerator ShowResult(float _delay)
    {
        if (!QuestionToView.AdditionalAnswers.ContainsKey(_answerId))
        {
            var buttonGO = QuestionToView.RightAnswers[_answerId];
            Instantiate(rightAnswerSign, buttonGO.transform);
        }
        else
        {
            var buttonGO = QuestionToView.AdditionalAnswers[_answerId];
            Instantiate(wrongAnswerSign, buttonGO.transform);
        }
        yield return new WaitForSecondsRealtime(_delay);
        ShowQuestion();
    }

    public void ResetView()
    {
        
    }

    public void SetScore(float _score)
    {
        scoreTMP.text = $"{_score}";
    }

    public void OnBackButtonClick()
    {
        if (PrevScreen != null)
        {
            var screensController = ScreensUIController.GetInstance();
            screensController.DiactivateScreens();
            screensController.Activate(PrevScreen);
        }
        else
        {
            Debug.Log("Prev screen not set!");
        }
    }
}
