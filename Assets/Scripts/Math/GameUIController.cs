using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using UnityEngine.Networking;

public class GameUIController : MonoBehaviour
{
    public TestView _test;
    public TextMeshProUGUI _testName;
    public TextMeshProUGUI _timer;

    public float _startTime;
    public float _currentTime;

    public TextMeshProUGUI _scoreText;
    public int _score;

    public DataUI _question;
    public List<DataUI> _questButtons = new List<DataUI>();
    public QuestionView _questionView;
    private Question _currentQuestion;
    private DownloadStrategy _strategy = new DownloadStrategy();

    public Loader _loader;

    public void Update()
    {
        if (_currentTime > 0)
        {
            _currentTime -= (Time.deltaTime * 1000);
            _timer.text = $"{ Math.Round(_currentTime / 1000) } sec";
        }
        else
        {
            _timer.text = $"{0}";
            Time.timeScale = 0;
        }
    }

    public void OnButtonClick(int id)
    {
        if (_currentQuestion != null)
        {
            if (_currentQuestion.answers[id].isRight) 
                _score += _test.GetReward();
            else 
                _score += _test.GetPenaltie();
            _scoreText.text = $"Score: {_score}";

            _currentQuestion = _test.GetQuestion();
            if (_currentQuestion == null) EndGame();
            else DrawQuest();
        }
        else
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        Time.timeScale = 0;
        _timer.text = "End";
    }

    public void Awake()
    {
        
        _score = 0;
        _test = null;
        string path = Path.Combine(Application.persistentDataPath, "q.txt");
        string content = System.IO.File.ReadAllText(path);
        _test = JsonConvert.DeserializeObject<TestView>(content);
        _questionView = new QuestionView();
        _questionView._quest = _question;
        _questionView._answers = _questButtons;

        _strategy.SetImageDownloader(new QuestionDownloader());
        _strategy.onLoadImageBegin += obj => 
        {
            gameObject.SetActive(false);
            _loader.EnableLoader();
        };
        _strategy.onLoadImageEnd += (lst) =>
        {
            _test.SetQuestionView(_questionView, lst);
        };
        _strategy.onLoadImageEnd += obj =>
        {
            gameObject.SetActive(true);
            _loader.DisableLoader();
        };
    }

    async void Start()
    {
        await _strategy.DownloadQuestImagesAsync(_test);

        _currentQuestion = _test.GetQuestion();
        DrawQuest();
        _testName.text = _test.name;
        _startTime = _test.GetTime() * 1000;
        _currentTime = _startTime;
    }

    private void DrawQuest()
    {
        
    }
}