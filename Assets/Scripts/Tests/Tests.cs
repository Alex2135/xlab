using System.Collections.Generic;
using Newtonsoft.Json;

public class Test : ITest, IRewarder
{
    [JsonProperty("quests")]
    public List<Question> quests;
    [JsonProperty("name")]
    public string name;
    [JsonProperty("time")]
    public float time;
    [JsonProperty("reward")]
    public int reward;
    [JsonProperty("penaltie")]
    public int penaltie;
    private bool shuffled = false;
    protected int quesitonIdx = -1;

    public Question currentQuestion { 
        get 
        { 
            return  quesitonIdx < quests.Count ? quests[quesitonIdx] : null; 
        }  
    }

    private void ShuffleQuests()
    {
        System.Random rng = new System.Random();
        int n = quests.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            var value = quests[k];
            quests[k] = quests[n];
            quests[n] = value;
        }
    }

    public Question GetNextQuestion()
    {
        Question result = null;
        quesitonIdx++;
        if (quests != null && quesitonIdx < quests.Count)
        {
            if (!shuffled)
            {
                //this.ShuffleQuests();
                this.shuffled = true;
            }
            result = quests[quesitonIdx];
            if (quesitonIdx >= quests.Count) shuffled = false;
        }

        return result;
    }

    public Result GetResult()
    {
        return new Result { grade = 0 };
    }

    public float GetTime()
    {
        return this.time;
    }

    public int GetReward()
    {
        return reward;
    }

    public int GetPenaltie()
    {
        return penaltie;
    }
}

interface ITest
{
    Question GetNextQuestion();
    float GetTime(); // Test time
    Result GetResult();
}

interface IRewarder
{
    int GetReward();
    int GetPenaltie();
}

public class Result
{
    public int grade;
}

