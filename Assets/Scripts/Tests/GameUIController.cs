using System;
using System.IO;
using System.Collections;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;

public class GameUIController : MonoBehaviour
{
    public Loader _loader;
    public TestView _testView;
    public QuestionView _currentQuestionView;
    public TextMeshProUGUI _testName;
    public TextMeshProUGUI _timer;
    public TextMeshProUGUI _scoreText;
    public float _startTime;
    public float _currentTime;
    public int _score;

    private DownloadStrategy _strategy;
    private bool _isLoad = false;

    public void Update()
    {
        if (_currentTime > 0 && _isLoad)
        {
            _currentTime -= (Time.deltaTime * 1000);
            _timer.text = $"{ Math.Round(_currentTime / 1000) } sec";
        }
        else
        {
            _timer.text = $"{0} sec";
            Time.timeScale = 0;
        }
    }

    public void OnButtonClick(int id)
    {
        if (_testView.currentQuestion != null)
        {
            bool isRight = _testView.currentQuestion.answers[id].isRight;
            StartCoroutine(ShowQuestResult(isRight));
            if (isRight) 
                _score += _testView.GetReward();
            else 
                _score += _testView.GetPenaltie();
            _scoreText.text = $"Score: {_score}";


            _testView.GetNextQuestion();
            if (_testView.currentQuestion == null) EndGame();
            else _testView.SetQuestionView(ref _currentQuestionView, _strategy.downloadedImages);
        }
        else
        {
            EndGame();
        }
    }

    IEnumerator ShowQuestResult(bool _isRight)
    {
        yield return new WaitForEndOfFrame();
    }

    private void EndGame()
    {
        Time.timeScale = 0;
        _timer.text = "End";
    }

    public void Awake()
    {
        // TODO: Download test data from net
        string path = Path.Combine(Application.persistentDataPath, "q.txt");
        string content = System.IO.File.ReadAllText(path);
        _testView = JsonConvert.DeserializeObject<TestView>(content);
        _testName.text = _testView.name;

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
            _testView.SetQuestionView(ref _currentQuestionView, lst);
            gameObject.SetActive(true);
            _loader.DisableLoader();
            _isLoad = true;
        };

        _score = 0;
        _startTime = _testView.GetTime() * 1000;
        _currentTime = _startTime;
    }

    async void Start()
    {
        await _strategy.DownloadQuestImagesAsync(_testView);
    }
}