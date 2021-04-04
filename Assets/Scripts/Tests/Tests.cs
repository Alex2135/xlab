using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine.UI;
using TMPro;

class QuestImagesMapper
{
    public Dictionary<Question, List<LoadedImage>> mapQuestAndImages;

    public QuestImagesMapper()
    {
        mapQuestAndImages = new Dictionary<Question, List<LoadedImage>>();
    }

    public void MapQuestsAndImages(ITest test, List<LoadedImage> _netImages)
    {
        var testQuestions = (test as Test).quests;
        if (testQuestions == null) throw new NullReferenceException("Test not set");

        var size = testQuestions.Count;

        for (int i = 0; i < size; i++)
        {
            var key = testQuestions[i];
            List<LoadedImage> val = null;
            if (_netImages != null && _netImages.Count != 0)
                val = GetImagesFromListByIndex(_netImages, i);
            mapQuestAndImages.Add(key, val);
        }
    }

    public static List<LoadedImage> GetImagesFromListByIndex(List<LoadedImage> _images, int _index)
    {
        if (_images.Count == 0)
            throw new Exception("GetImagesFromListByIndex: images list is empty");

        var result = _images.Where(
            questImage =>
            {
                var qIdx = questImage?._name?.Split('_')?.Last() ?? "-1";
                return Convert.ToInt32(qIdx) == _index;
            }
        );

        return result.ToList();
    }
}


public interface ITestView
{
    QuestionView CurrentQuestionView { get; set; }
    ITest test { get; set; }
    void SetQuestImages(List<LoadedImage> _images);
    void RefreshQuestDataOnQuestionView(List<LoadedImage> _netImages);
    void SetQuestText(List<LoadedImage> _netImages);
}

[Serializable]
public class QuestionView
{
    public DataUI _quest;
    public List<DataUI> _answers;

    public QuestionView()
    {
        _quest = new DataUI();
        _answers = new List<DataUI>();
    }

    public void SetQuestText(string _text)
    {
        _quest._text.text = _text;
    }

    public void SetAnswerText(int _id, string _text)
    {
        if (_id < 0 || _id >= _answers.Count)
            throw new ArgumentOutOfRangeException("Id out of range");

        _answers[_id]._text.text = _text;
    }
}

[Serializable]
public class DataUI
{
    public TextMeshProUGUI _text;
    public Image _image;

    public void ResetImage()
    {
        _image.sprite = null;
    }

    public void ResetText()
    {
        _text.text = "";
    }

    public void ResetImageAndText()
    {
        ResetImage();
        ResetText();
    }
}

/*
 * Вопросы с ответами из теста загружаются из файла по очереди.
 * А изображения 
 */

public static class ExtensionList
{
    public static List<T> ShuffleItems<T>(this List<T> _list)
    {
        System.Random rng = new System.Random();
        int n = _list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            var val = _list[k];
            _list[k] = _list[n];
            _list[n] = val;
        }
        return _list;
    }
}

public class Test : ITest, IRewarder
{
    private List<Question> _quests;
    [JsonProperty("quests")]
    public List<Question> quests 
    { 
        get { return _quests; }
        set
        {
            _quests = value;
            for (int i = 0; i < _quests.Count; i++)
            {
                _questIndexes.Add(i);
            }
        }
    }
    [JsonProperty("name")]
    public string name;
    [JsonProperty("time")]
    public float time;
    [JsonProperty("reward")]
    public int reward;
    [JsonProperty("penaltie")]
    public int penaltie;
    private bool _shuffled;
    protected Result _resultScore;
    // TODO: Сформировать массив целых чисел из номеров(индексов) вопросов,
    // перемешать этот массив и поочереди брать из него элементы.
    private List<int> _questIndexes;
    public Result ResultScore 
    { 
        set
        {
            _resultScore = value;
        }
        get
        {
            _resultScore.QuestsCount = quests?.Count ?? 0;
            _resultScore.TestTime = time;
            return _resultScore;
        }
    }
    public Question CurrentQuestion
    {
        get
        {
            return quesitonIdx < quests.Count ? quests[quesitonIdx] : null;
        }
    }
    public int quesitonIdx;

    public Test()
    {
        _shuffled = false;
        quesitonIdx = -1;
        _resultScore = new Result(quests?.Count ?? 0);
        _questIndexes = new List<int>();
        _quests = _quests ?? new List<Question>();
    }

    public Question GetNextQuestion()
    {
        Question result = null;
        quesitonIdx++;
        if (quests != null && quesitonIdx < quests.Count)
        {
            if (!_shuffled)
            {
                //this.ShuffleQuests();
                this._shuffled = true;
            }
            result = quests[quesitonIdx];
            if (quesitonIdx >= quests.Count) _shuffled = false;
        }

        return result;
    }

    public float GetTime()
    {
        return this.time;
    }

    public int GetReward()
    {
        _resultScore += reward;
        return reward;
    }

    public int GetPenaltie()
    {
        _resultScore -= penaltie;
        return penaltie;
    }
}

public interface ITest
{
    Result ResultScore { get; set; }
    Question CurrentQuestion { get; }
    Question GetNextQuestion();
    float GetTime(); // Test time
}

public interface IRewarder
{
    int GetReward();
    int GetPenaltie();
}

public class Result
{
    public int Grade { get; set; }
    public int TruePositive { get; set; }
    public int QuestsCount { get; set; }
    public float ResultTime { get; set; }
    public float TestTime { get; set; }


    public Result(int _questsCount = 0)
    {
        Grade = 0;
        TruePositive = 0;
        QuestsCount = _questsCount;
    }
    
    public static Result operator +(Result _res, int _reward)
    {
        _res.TruePositive++;
        _res.Grade += _reward;
        return _res;
    }

    public static Result operator -(Result _res, int _penaltie)
    {
        _res.Grade -= _penaltie;
        return _res;
    }
}

