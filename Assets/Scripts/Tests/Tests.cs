using System.Collections.Generic;
using Newtonsoft.Json;

/*
 * Вопросы с ответами из теста загружаются из файла по очереди.
 * А изображения 
 */

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
    public int quesitonIdx;

    public Test()
    {
        _shuffled = false;
        quesitonIdx = -1;
        _resultScore = new Result(quests?.Count ?? 0);
        _questIndexes = new List<int>();
    }

    public Question currentQuestion 
    { 
        get 
        { 
            return  quesitonIdx < quests.Count ? quests[quesitonIdx] : null; 
        }
    }

    private void ShuffleQuests()
    {
        System.Random rng = new System.Random();
        int n = _questIndexes.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            var val = _questIndexes[k];
            _questIndexes[k] = _questIndexes[n];
            _questIndexes[n] = val;
        }
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
    Question currentQuestion { get; }
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

