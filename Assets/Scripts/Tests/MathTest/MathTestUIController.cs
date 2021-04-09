using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;

public class MathTestUIController : MonoBehaviour, IResetableScreenController, IDecorableScreen, IResultableScreen
{
    // Screen objects
    public QuestionView _currentQuestionView;
    public TextMeshProUGUI _timer;
    public TextMeshProUGUI _scoreText;
    public Image Background;
    public ShowQuestResult _questResultView;

    // Logic objects 
    public MathTestView _testView;
    public LoaderUiController _loader;
    public ResultsUiController _testResultView;
    public float _startTime;
    public float _currentTime;
    private bool _isPressed = false;

    // Data objects
    private DownloadStrategy _strategy;
    public string _screenName;
    private bool _isLoad = false;

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
    public string ScreenName { 
        get { return _screenName; } 
        set { _screenName = value; }
    }

    public object GetResult()
    {
        var result = _testView.ResultScore;
        result.ResultTime = _currentTime;
        return result;
    }

    public void OnBackClick()
    {
        if (PrevScreen != null)
        {
            // TODO: Stop the timer, reset results.
            var screensController = ScreensUIController.GetInstance();
            screensController.DiactivateScreens();
            screensController.Activate(PrevScreen);
        }
        else
        {
            Debug.Log("Prev screen not set!");
        }
    }

    public void Update()
    {
        if (_currentTime > 0 && _isLoad)
        {
            _currentTime -= (Time.deltaTime * 1000);
            double castedTime = Math.Round(_currentTime / 1000);
            string mins = ((int)castedTime / 60).ToString();
            string secs = ((int)castedTime % 60).ToString();

            if (secs.Length == 0) secs = "00";
            else if (secs.Length == 1) secs = "0" + secs;

            _timer.text = mins + ":" + secs;
        }
        else
        {
            _timer.text = $"0:00";
            Time.timeScale = 0;
        }
    }

    /*
     * Получение ответа на вопрос может проходить в двух сценариях:
     * 1) Выбран правильный ответ, показывается на нем галочка
     * 2) Выбран не правильный ответ, показывается галочка на 
     * правильном ответе и крестик на выбранном не правильном 
     * ответе.
     */
    public void OnButtonClick(int id)
    {
        if (!_isPressed)
        {
            if (_testView.CurrentQuestion == null) throw new Exception("Current question not set");

            Answer[] answers = _testView.CurrentQuestion.answers;
            int trueId = 0;
            for (int i = 0; i < answers.Length; i++)
            {
                if (answers[i].isRight)
                {
                    trueId = i;
                    break;
                }
            }

            StartCoroutine(this.ShowResult(id, trueId));
        }
    }

    IEnumerator ShowResult(int _selectedId, int _rightId)
    {
        _isPressed = true;
        float delay = 0.3f;
        if (_testView.CurrentQuestion != null)
        {
            bool isRight = _testView.CurrentQuestion.answers[_selectedId].isRight;
            _questResultView.ShowQuestionResult(_selectedId, _rightId);
            yield return new WaitForSeconds(delay);
            _questResultView.ResetQuestResult();
            if (isRight)
                _testView.GetReward();
            else
                _testView.GetPenaltie();
            _scoreText.text = $"{_testView.ResultScore.Grade}";
            _testView.GetNextQuestion();
            if (_testView.CurrentQuestion == null) EndGame();
            else _testView.RefreshQuestDataOnQuestionView(_strategy.downloadedImages);
        }
        else
        {
            _questResultView.ShowQuestionResult(_selectedId, _rightId);
            yield return new WaitForSeconds(delay);
            _questResultView.ResetQuestResult();
            EndGame();
        }
        _isPressed = false;
    }

    private void EndGame()
    {
        Time.timeScale = 0;
        _timer.text = "End";
        gameObject.SetActive(false);
        (NextScreen as MonoBehaviour).gameObject.SetActive(true);
    }

    public void Awake()
    {
        string path = Path.Combine(Application.persistentDataPath, "q.txt");
        string content = System.IO.File.ReadAllText(path);

        _testView = new MathTestView();
        _testView.test = JsonConvert.DeserializeObject<Test>(content);
        _testView.CurrentQuestionView = _currentQuestionView;
        _questResultView.SetQuestionView(_currentQuestionView);
    }

    public async void Start()
    {
        _nextScreen = _testResultView;
        (_nextScreen as ResultsUiController).TestName.text = (_testView.test as Test).name;
        _startTime = _testView.GetTime() * 1000;
        _currentTime = _startTime;

        _strategy = new DownloadStrategy();
        var qd = new QuestionDownloader(new NetImageRequester());
        _strategy.SetImageDownloader(qd);
        _strategy.onLoadImagesBegin += obj => 
        {
            gameObject.SetActive(false);
            _loader.EnableLoader();
        };
        _strategy.onLoadImageEnd += (lst) =>
        {
            _testView.GetNextQuestion();
            _testView.RefreshQuestDataOnQuestionView(lst);
            gameObject.SetActive(true);
            _loader.DisableLoader();
            _isLoad = true;
        };


        await _strategy.DownloadQuestImagesAsync(_testView.test);
    }

    public Image GetBackground()
    {
        return Background;
    }

    public void ResetScreenState()
    {
        _currentTime = 0;
        _testView.ResetTestAndQuestView();
    }
}