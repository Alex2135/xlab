using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using System.Linq;

/*
 * TestView предоставляет 
 */
public class TestView: Test
{
    /*
     * QuestionView - изображения и текст вопроса и ответов
     * List<NetImage> - набор изображений всех вопросов и ответов на них
     * 
     */
    public void SetQuestionView(QuestionView _quest, List<NetImage> _netImages)
    {
        int idx = this.quesitonIdx;
        var quest = this.GetQuestion();
        var questImages = _netImages.Where(
            questImage =>
            {
                var qIdx = questImage?._name?.Split('_')?.Last() ?? "-1";
                var result = Convert.ToInt32(qIdx) == idx;
                //Debug.Log($"Quest id {idx}, name {questImage._name}, result: {result}");
                return result;
            }
        ).ToList();

        foreach (var img in questImages)
        {
            var splitedName = img._name.Split('_');

            if (splitedName[0] == "quest")
            {
                NetImage.SetTextureToImage(ref _quest._quest._image, img._image);
                _quest._quest._text.text = quest.question;
            }
            else if (splitedName[0] == "answer")
            {
                var answerIdx = Convert.ToInt32(splitedName[1]);
                NetImage.SetTextureToImage(ref _quest._answers[answerIdx]._image, img._image);
                _quest._answers[answerIdx]._text.text = quest.answers[answerIdx].content;
            }
            else
            {
                throw new Exception("Invalid image name");
            }
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
    protected int quesitonIdx = 0;

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

