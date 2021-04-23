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
    public TextMeshProUGUI instructionTMP;
    public WordsPanelUIController wordsPanelUIC;
    public TextMeshProUGUI rememberButtonText;
    public Timer timer;
    public string screenName;

    public string ScreenName { get => screenName; set => screenName = value; }
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }
    public IAdaptedQuestToView QuestionToView { get; set; }

    public event Action<object> OnAnswering;
    public event Action<object> OnAnswerDid;
    public event Action<object, EventArgs> OnQuestTimeout;

    private WordsTestPresenter presenter;
    private bool isRemember;
    private bool isAnswered;

    private void OnEnable()
    {
        var model = new WordsTestModel(new WordsTestGeneratedDataSource());
        presenter = new WordsTestPresenter(model, this, wordsPanelUIC);
        timer = new Timer(presenter.GetTestTime());
        timer.OnTimerStart += Timer_OnTimerStart;
        timer.OnTimerTick += Timer_OnTimerTick;
        timer.OnTimerStop += Timer_OnTimerStop;
        timer.OnTimeout += presenter.view_OnQuestTimeout;
        timer.StartTimer();
        isRemember = true;
        isAnswered = false;

        ShowQuestion();
    }

    void Update()
    {
        if (isRemember) timer.TimerTick(Time.deltaTime);
    }

    private void Timer_OnTimerStart(object arg1, EventArgs arg2)
    {
        timeTMP.gameObject.SetActive(true);
    }

    private void Timer_OnTimerStop(object _obj, EventArgs _args)
    {
        timeTMP.gameObject.SetActive(false);
    }

    private void Timer_OnTimerTick(object _obj, EventArgs _args)
    {
        timeTMP.text = timer.ToString();
    }

    public void OnRememberClick()
    {
        if (isRemember)
        {
            wordsPanelUIC.ClearPanel();
            isRemember = false;
            rememberButtonText.text = "Проверить";
            ShowQuestion();
        }
        else if (!isAnswered)
        {
            rememberButtonText.text = "Продолжить";
            ShowQuestResult();
        }
        else
        {
            var screenController = ScreensUIController.GetInstance();
            screenController.Activate(NextScreen);
        }
    }

    public void ShowQuestion()
    {
        if (isRemember)
        {
            QuestionToView = presenter.GetAdaptedQuest((obj) => { });
            timer.StartTimer();
        }
        else
        {
            instructionTMP.gameObject.SetActive(true);
            QuestionToView = presenter.GetAdaptedQuest(
                (obj) => { 
                    if (!isAnswered) OnAnswering.Invoke(obj); 
                }
            );
            timer.StopTimer();
        }
    }

    public void ShowQuestResult()
    {
        instructionTMP.gameObject.SetActive(false);
        questScoreTMP.gameObject.SetActive(true);
        OnAnswerDid?.Invoke(questScoreTMP);
        isAnswered = true;
    }

    private void OnDisable()
    {
        ResetView();
    }

    public void ResetView()
    {
        isAnswered = false;
        isRemember = true;
        timer.StopTimer();
        wordsPanelUIC.ClearPanel();
        questScoreTMP.gameObject.SetActive(false);
    }
}
