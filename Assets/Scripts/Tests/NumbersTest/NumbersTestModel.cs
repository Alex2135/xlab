using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewQuestionModel;

class NumbersQuestModel : IGenericQuestModel<List<string>, List<string>>
{
    public List<string> Quest { get; set; } = null;
    public List<string> RightAnswers { get; set; }
    public List<string> AdditionalAnswers { get; set; } = null;
}

class NumbersAdaptedQuestModel : IAdaptedQuestModel<List<string>, List<string>>
{
    public Dictionary<int, List<string>> Quest { get; set; } = null;
    public Dictionary<int, List<string>> RightAnswers { get; set; }
    public Dictionary<int, List<string>> AdditionalAnswers { get; set; } = null;
}

class NumbersQuestView : IAdaptedQuestToView
{
    public Dictionary<int, GameObject> Quest { get; set; } = null;
    public Dictionary<int, GameObject> RightAnswers { get; set; }
    public Dictionary<int, GameObject> AdditionalAnswers { get; set; } = null;
}

class NumbersTestModel : ATestModel<NumbersQuestModel>
{
    private List<NumbersQuestModel> _questions;
    private int PointsPerQuest { get; set; }

    public NumbersTestModel(IDataSource<NumbersQuestModel> _source)
    {
        _dataSource = _source;
        _questions = (List<NumbersQuestModel>)_dataSource.GetQuests();
        PointsPerQuest = 10;
        rightQuestions = 0;
        wrongQuestions = 0;
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
        int result = rightQuestions * PointsPerQuest - wrongQuestions * (int)(1f / 4f * maxScore);
        return result;
    }

    public override float GetTestTime()
    {
        return 60f;
    }

    public override void PenaltieWrongAnswer()
    {
        wrongQuestions++;
    }

    public override void RewardRightAnswer()
    {
        rightQuestions++;
    }
}
