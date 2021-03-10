using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using UnityAsyncAwaitUtil;
using UnityEngine.Networking;

public class GameUIController : MonoBehaviour
{
    public Test _test;
    public TextMeshProUGUI _testName;
    public TextMeshProUGUI _timer;

    public float _startTime;
    public float _currentTime;

    public TextMeshProUGUI _scoreText;
    public int _score;

    public DataUI _question;
    public List<DataUI> _questButtons = new List<DataUI>();
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


    /* TODO: Conver List<NetImage> to view
     * with quest DataUI and answers List<DataUI>
     */
    private void processList(List<NetImage> _testImages)
    {
        //Debug.Log("Images name");
        foreach (var img in _testImages)
        {
            var idxs = img._name.Split('_');

            switch(idxs.Length)
            {
                case 2:
                    int questIdx = Convert.ToInt32(idxs[1]);
                    //_test.quests[questIdx].fil
                    break;
                case 3:
                    break;
                default:
                    break;
            };
        }
    }

    public void Awake()
    {
        _score = 0;
        _test = null;
        string path = Path.Combine(Application.persistentDataPath, "q.txt");
        string content = System.IO.File.ReadAllText(path);
        _test = JsonConvert.DeserializeObject<Test>(content);
        
        _strategy.SetImageDownloader(new QuestionDownloader());
        _strategy.onLoadImageBegin += obj => 
        {
            gameObject.SetActive(false);
            _loader.EnableLoader();
        };
        _strategy.onLoadImageEnd += obj =>
        {
            gameObject.SetActive(true);
            _loader.DisableLoader();
        };
        _strategy.onLoadImageEnd += processList;
    }

    async void Start()
    {
        await _strategy.DownloadImagesAsync(_test);

        _currentQuestion = _test.GetQuestion();
        DrawQuest();
        _testName.text = _test.name;
        _startTime = _test.GetTime() * 1000;
        _currentTime = _startTime;
    }

    private void DrawQuest()
    {
        _question._text.text = _currentQuestion.question;
        if (_currentQuestion.file != null && 
            _currentQuestion.file.isLinksExist())
        {
            _question._image.gameObject.SetActive(true);
        }
        else
        {
            _question._image.gameObject.SetActive(false);
        }
        for (int i = 0; i < _questButtons.Count; i++)
        {
            _questButtons[i]._text.text = _currentQuestion.answers[i].content;
            if (_currentQuestion.answers[i].file != null &&
                _currentQuestion.answers[i].file.isLinksExist())
            {
                _questButtons[i]._image.gameObject.SetActive(true);
                //StartCoroutine(GetImage(_currentQuestion.answers[i].file.link, _questButtons[i]._image));
                //_questButtons[i]._text.rectTransform.sizeDelta = new Vector2(_questButtons[i]._text.rectTransform.sizeDelta.x / 2
                //                                                            , _questButtons[i]._text.rectTransform.sizeDelta.y);
            }
            else
            {
                //_questButtons[i]._text.rectTransform.sizeDelta = new Vector2(540, 310);
                _questButtons[i]._image.gameObject.SetActive(false);
            }
        }
    }

    //public IEnumerator GetImage(string link, Image toImage)
    //{
    //    Texture2D texture;
    //    using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(link))
    //    {
    //        //yield return new WaitForSeconds(0.5f);
    //        yield return uwr.SendWebRequest();
    //        if (uwr.isNetworkError || uwr.isHttpError)
    //        {
    //            Debug.Log(uwr.error);
    //        }
    //        else
    //        {
    //            texture = DownloadHandlerTexture.GetContent(uwr);
    //            toImage.sprite = Sprite.Create(texture,
    //                                            new Rect(0, 0, texture.width, texture.height),
    //                                            new Vector2(0.5f, 0.5f)
    //                                           );
    //        }
    //    }
    //}
}