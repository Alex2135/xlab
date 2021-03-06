using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using NewQuestionModel;
using TMPro;

public class NeuroGymTestView : MonoBehaviour, NewQuestionModel.ITestView, NewQuestionModel.ITestScreenController, IScreenController
{
    public TextMeshProUGUI scoreTMP;
    
    public GameObject questPanel;
    public GameObject testPanel;
    public GameObject testSendPanel;

    public GameObject executeGymButton;
    public GameObject sendButton;
    public string screenName;

    public RecordPlayer player;

    public IAdaptedQuestToView QuestionToView { get; set; }

    public string TestName => "NeuroGym";

    public string ScreenName { get => screenName; set => screenName = value; }
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }

    public event Action<object> OnAnsweringEvent;
    public event Action<object> OnAnswerDidEvent;
    public event Action<object, EventArgs> OnQuestTimeoutEvent;

    private NeuroGymTestPresenter presenter;

    public void OnEnable()
    {
        var data = GetComponent<NeuroGymTestDataProvider>() as IDataSource<NeuroGymQuestModel>;
        var model = new NeuroGymTestModel(data);
        presenter = new NeuroGymTestPresenter(this, model);
        presenter.Player = player;

        executeGymButton.SetActive(false);
        sendButton.SetActive(false);

        questPanel.SetActive(true);
        testPanel.SetActive(false);
        testSendPanel.SetActive(false);

        ShowQuestion();
    }

    public void SetScore(float _score)
    {
        scoreTMP.text = $"{_score}";
    }

    public void OnExecuteGymButtonClick()
    {
        questPanel.SetActive(false);
        testPanel.SetActive(true);
    }

    public void OnSendButtonClick()
    {
        testPanel.SetActive(false);
        testSendPanel.SetActive(true);
    }

    public void OnGoNextButtonClick()
    {
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

    public void ShowQuestion() 
    {
        presenter.GetAdaptedQuest(null);
    }

    public void ShowQuestResult() { }
    public void ResetView() { }
}