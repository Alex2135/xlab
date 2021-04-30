using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NewQuestionModel;

public class SubjectsTestView : MonoBehaviour, IScreenController, NewQuestionModel.ITestView
{
    public string screenName;
    public TextMeshProUGUI instructTMP;
    public TextMeshProUGUI timeTMP;
    public TextMeshProUGUI scoreTMP;
    public GameObject rememberButton;
    public GameObject answerPanel;
    public SubjectsPanelUIController questPanelUIC;
    public SubjectsPanelUIController answerPanelUIC;
    public SubjectsButtonsStates buttonsStates;

    public event Action<object> OnAnsweringEvent;
    public event Action<object> OnAnswerDidEvent;
    public event Action<object, EventArgs> OnQuestTimeoutEvent;

    public string ScreenName { get => screenName; set => screenName = value; }
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }
    public IAdaptedQuestToView QuestionToView { get; set; }
    private SubjectsTestPresenter presenter;
    private Timer timer;

    void OnEnable()
    {
        var data = gameObject.GetComponent<SubjectsTestGeneratedDataProvider>() ?? throw new Exception("No data provider");
        var model = new SubjectsTestModel(data);
        presenter = new SubjectsTestPresenter(model, this);
        presenter.QuestPanel = questPanelUIC;
        presenter.AnswerPanel = answerPanelUIC;
        presenter.buttonsStates = buttonsStates;
        
        timeTMP.gameObject.SetActive(true);
        timer = new Timer(presenter.GetTestTime());
        timer.OnTimerTickEvent += Timer_OnTimerTickEvent;
        timer.OnTimeoutEvent += Timer_OnTimerStopEvent;
        timer.StartTimer(this);

        ShowQuestion();
    }

    private void Timer_OnTimerStopEvent(object arg1, EventArgs arg2)
    {
        OnRememberClick();
    }

    void Update()
    {
        timer.TimerTick(Time.deltaTime);
    }

    private void Timer_OnTimerTickEvent(object _sender, EventArgs _args)
    {
        timeTMP.text = $"{timer}";
    }

    void OnDisable()
    {
        ResetView();
    }

    public void OnRememberClick()
    {
        presenter.isRememberScreenState = false;
        instructTMP.gameObject.SetActive(false);
        rememberButton.SetActive(false);
        timeTMP.gameObject.SetActive(false);
        timer.StopTimer();
        ShowQuestion();
    }

    public void ShowQuestion()
    {
        if (presenter.isRememberScreenState)
        {
            QuestionToView = presenter.GetAdaptedQuest(obj => { });
        }
        else
        {
            Action callback = () => {
                QuestionToView = presenter.GetAdaptedQuest(obj =>
                {
                    answerPanel.gameObject.SetActive(true);
                    OnAnsweringEvent.Invoke(obj);
                });
            };

            StartCoroutine( ShowPreQuestedState(3f, callback) );
        }

        var questButton = QuestionToView.Quest[0];
        var buttonIMG = questButton.ChildByName("ButtonIMG");
        var buttonBG = questButton.ChildByName("ButtonBG");
        var rtIMG = buttonIMG.GetComponent<RectTransform>();
        var rtBG = buttonBG.GetComponent<RectTransform>();
        float gridWidth = questPanelUIC.grid.GetComponent<RectTransform>().rect.width;
        float buttonWidth = 0f;
        var gridGroup = questPanelUIC.grid.GetComponent<GridLayoutGroup>();

        if (QuestionToView.Quest.Count == 6)
        {
            buttonWidth = (gridWidth - 16 * 2) / 3;
            gridGroup.constraintCount = 3;
        }
        else if (QuestionToView.Quest.Count > 6)
        {
            buttonWidth = (gridWidth - 16 * 3) / 4;
            gridGroup.constraintCount = 4;
        }

        float ratio = 70f / 150f;

        if (buttonWidth != 0f)
        {
            gridGroup.cellSize = new Vector2(buttonWidth, buttonWidth);
            foreach (var buttonGO in questPanelUIC.Buttons.Values)
            {
                var buttonImgRT = buttonGO.ChildByName("ButtonIMG").GetComponent<RectTransform>();
                buttonImgRT.sizeDelta = new Vector2(buttonWidth * ratio, buttonWidth * ratio);
            }
        }
    }

    IEnumerator ShowPreQuestedState(float _duration, Action _callback = null)
    {
        foreach (var button in QuestionToView.Quest)
        {
            var buttonBG = button.Value.ChildByName("ButtonBG");
            var buttonIMG = button.Value.ChildByName("ButtonIMG");
            var bg = buttonBG.GetComponent<Image>();
            var img = buttonIMG.GetComponent<Image>();
            bg.color = new Color(1f, 1f, 1f, 1f);
            img.color = new Color(1f, 1f, 1f, 0f);
            LoadedImage.SetTextureToImage(ref bg, buttonsStates.questionSignImage);
        }
        yield return new WaitForSecondsRealtime(_duration);
        foreach (var button in QuestionToView.Quest)
        {
            var buttonBG = button.Value.ChildByName("ButtonBG");
            var buttonIMG = button.Value.ChildByName("ButtonIMG");
            var bg = buttonBG.GetComponent<Image>();
            var img = buttonIMG.GetComponent<Image>();
            bg.color = new Color(1f, 1f, 1f, 0f);
            img.color = new Color(1f, 1f, 1f, 0f);
            LoadedImage.SetTextureToImage(ref bg, buttonsStates.normalStateQuestImage);
        }
        if (_callback != null) _callback();
    }

    public void ShowQuestResult()
    {
        answerPanel.SetActive(false);
    }

    public void ResetView()
    {
        
    }

    public void SetScore(float _score)
    {
        scoreTMP.text = $"{_score}";
    }
}
