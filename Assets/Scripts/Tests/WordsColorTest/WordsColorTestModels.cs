using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using NewQuestionModel;

[Serializable]
public class ColorUnit
{
    public Color color;
    public string colorName;
}

public class WordsColorQuestModel : IGenericQuestModel<List<ColorUnit>, List<ColorUnit>>
{
    public List<ColorUnit> Quest { get; set; }
    public List<ColorUnit> RightAnswers { get; set; }
    public List<ColorUnit> AdditionalAnswers { get; set; }
}

public class AdaptedWordsColorQuestModel : IAdaptedQuestModel<List<ColorUnit>, List<ColorUnit>>
{
    public Dictionary<int, List<ColorUnit>> Quest { get; set; }
    public Dictionary<int, List<ColorUnit>> RightAnswers { get; set; }
    public Dictionary<int, List<ColorUnit>> AdditionalAnswers { get; set; }
}

public class WordsColorAdaptedQuestToView : IAdaptedQuestToView
{
    public Dictionary<int, GameObject> Quest { get; set; }
    public Dictionary<int, GameObject> RightAnswers { get; set; }
    public Dictionary<int, GameObject> AdditionalAnswers { get; set; }
}

public class WordsColorTestModel : ATestModel<WordsColorQuestModel>
{
    private List<WordsColorQuestModel> _questions;
    private int PointsPerQuest { get; set; } = 10;

    public WordsColorTestModel(IDataSource<WordsColorQuestModel> _source)
    {
        var user = UserModel.GetInstance();
        var data = user.GetTestData("WordsColor");
        _dataSource = _source;
        _questions = _dataSource.GetQuests(data) as List<WordsColorQuestModel>;
        questionIndex = -1;
        rightAnswers = 0;
        wrongAnswers = 0;
    }

    public override int CalculateScore()
    {
        int maxScore = _questions[0].Quest.Count * PointsPerQuest;
        int result = rightAnswers * PointsPerQuest - wrongAnswers * (int)(1f / 4f * maxScore);
        return result;
    }

    public override (WordsColorQuestModel, int)? GetCurrentQuestion()
    {
        return (_questions[questionIndex], questionIndex);
    }

    public override int GetLastScore()
    {
        return UserModel.GetLastScore("WordsColor");
    }

    public override (WordsColorQuestModel, int)? GetNextQuestion()
    {
        questionIndex++;
        return GetCurrentQuestion();
    }

    public override int GetQuestsCount()
    {
        return _questions.Count;
    }

    public override float GetTestTime()
    {
        return 10f;
    }

    public override void PenaltieWrongAnswer()
    {
        wrongAnswers++;
    }

    public override void RegisterScore()
    {
        var user = UserModel.GetInstance();
        user.AddNewScore(
            "WordsColor",
            CalculateScore(),
            rightAnswers,
            wrongAnswers
        );
        user.SaveData();
    }

    public override void RewardRightAnswer()
    {
        rightAnswers++;
    }
}