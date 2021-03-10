using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;

/*
 * TestView предоставляет 
 */
public class TestView: Test
{
    public List<QuestionView> _quests;

    // TODO: Process results images request
    public void SetQuestions(List<NetImage> _netImages)
    {
        _quests = new List<QuestionView>(this.quests.Count);

        foreach (var img in _netImages)
        {
            var idxs = img._name.Split('_');

            switch (idxs.Length)
            {
                case 2: 
                {
                    int questIdx = Convert.ToInt32(idxs[1]);
                    NetImage.SetTextureToImage(ref _quests[questIdx]._quest._image, img._image);
                }
                break;
                case 3: 
                {
                    int questIdx = Convert.ToInt32(idxs[2]);
                    int ansIdx = Convert.ToInt32(idxs[1]);
                    NetImage.SetTextureToImage(ref _quests[questIdx]._answers[ansIdx]._image, img._image);
                }
                break;
                default:
                    Debug.Log("Image error!");
                    break;
            };
        }
    }
}



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
    private int quesitonIdx = 0;

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

    public Question GetQuestion()
    {
        Question result = null;
        if (quests != null && quesitonIdx < quests.Count)
        {
            if (!shuffled)
            {
                this.ShuffleQuests();
                this.shuffled = true;
            }
            result = quests[quesitonIdx];
            quesitonIdx++;
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
    Question GetQuestion();
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

