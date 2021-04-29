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
    public TextMeshProUGUI instruct;
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

    void OnEnable()
    {
        var data = gameObject.GetComponent<SubjectsTestGeneratedDataProvider>() ?? throw new Exception("No data provider");
        var model = new SubjectsTestModel(data);
        presenter = new SubjectsTestPresenter(model, this);
        presenter.QuestPanel = questPanelUIC;
        presenter.AnswerPanel = answerPanelUIC;
        presenter.buttonsStates = buttonsStates;

        ShowQuestion();
    }

    void OnDisable()
    {
        ResetView();
    }

    public void OnRememberClick()
    {
        presenter.isRememberScreenState = false;
        instruct.gameObject.SetActive(false);
        rememberButton.SetActive(false);
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

        var button = QuestionToView.Quest[0];
        var icon = button.ChildByName("ButtonIMG");

        switch (QuestionToView.Quest.Count)
        {
            case 4: break;
            case 6: 

                break;
            default: 
                break;
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
}
