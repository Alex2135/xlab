using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewQuestionModel;

public class NumbersQuestModel : IGenericQuestModel<List<int>, List<int>>
{
    public List<int> Quest { get; set; } = null;
    public List<int> RightAnswers { get; set; }
    public List<int> AdditionalAnswers { get; set; } = null;

    public NumbersQuestModel()
    {
        RightAnswers = new List<int>();
    }
}

public class NumbersAdaptedQuestModel : IAdaptedQuestModel<List<int>, List<int>>
{
    public Dictionary<int, List<int>> Quest { get; set; } = null;
    public Dictionary<int, List<int>> RightAnswers { get; set; }
    public Dictionary<int, List<int>> AdditionalAnswers { get; set; } = null;

    public NumbersAdaptedQuestModel()
    {
        RightAnswers = new Dictionary<int, List<int>>();
    }
}

public class NumbersQuestView : IAdaptedQuestToView
{
    public Dictionary<int, GameObject> Quest { get; set; }
    public Dictionary<int, GameObject> RightAnswers { get; set; }
    public Dictionary<int, GameObject> AdditionalAnswers { get; set; } = null;

    public NumbersQuestView()
    {
        RightAnswers = new Dictionary<int, GameObject>();
        Quest = new Dictionary<int, GameObject>();
    }
}

public class NumbersTestModel : ATestModel<NumbersQuestModel>
{
    private List<NumbersQuestModel> _questions;
    private int PointsPerQuest { get; set; }

    public NumbersTestModel(IDataSource<NumbersQuestModel> _source)
    {
        _dataSource = _source;
        _questions = (List<NumbersQuestModel>)_dataSource.GetQuests();
        PointsPerQuest = 10;
        rightAnswers = 0;
        wrongAnswers = 0;
        questionIndex = -1;
    }

    public override (NumbersQuestModel, int)? GetCurrentQuestion()
    {
        return (_questions[questionIndex], questionIndex);
    }

    public override (NumbersQuestModel, int)? GetNextQuestion()
    {
        questionIndex++;
        return GetCurrentQuestion();
    }

    public override int GetQuestsCount()
    {
        return _questions.Count;
    }

    public override int GetScore()
    {
        int maxScore = _questions[0].Quest.Count * PointsPerQuest;
        int result = rightAnswers * PointsPerQuest - wrongAnswers * (int)(1f / 4f * maxScore);
        return result;
    }

    public override float GetTestTime()
    {
        return 60f;
    }

    public override void PenaltieWrongAnswer()
    {
        wrongAnswers++;
    }

    public override void RewardRightAnswer()
    {
        rightAnswers++;
    }

    public override void RegisterScore()
    {
        var user = UserModel.GetInstance();
        user.AddNewScore(
            "Numbers",
            GetScore(),
            rightAnswers,
            wrongAnswers
        );
    }
}
