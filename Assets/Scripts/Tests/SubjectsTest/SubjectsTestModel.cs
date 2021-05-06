using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewQuestionModel;

public class SubjectsQuestModel : IGenericQuestModel<List<Texture2D>, List<Texture2D>>
{
    public List<Texture2D> Quest { get; set; }
    public List<Texture2D> RightAnswers { get; set; }
    public List<Texture2D> AdditionalAnswers { get; set; }

    public SubjectsQuestModel()
    {
        Quest = new List<Texture2D>();
        RightAnswers = new List<Texture2D>();
        AdditionalAnswers = new List<Texture2D>();
    }
}

public class AdaptedSubjectsQuestModel : IAdaptedQuestModel<Texture2D, Texture2D>
{
    public Dictionary<int, Texture2D> Quest { get; set; }
    public Dictionary<int, Texture2D> RightAnswers { get; set; }
    public Dictionary<int, Texture2D> AdditionalAnswers { get; set; }

    public AdaptedSubjectsQuestModel()
    {
        Quest = new Dictionary<int, Texture2D>();
        RightAnswers = new Dictionary<int, Texture2D>();
        AdditionalAnswers = new Dictionary<int, Texture2D>();
    }
}

public class SubjectsQuestView : IAdaptedQuestToView
{
    public Dictionary<int, GameObject> Quest { get; set; }
    public Dictionary<int, GameObject> RightAnswers { get; set; }
    public Dictionary<int, GameObject> AdditionalAnswers { get; set; }

    public SubjectsQuestView()
    {
        Quest = new Dictionary<int, GameObject>();
        RightAnswers = new Dictionary<int, GameObject>();
        AdditionalAnswers = new Dictionary<int, GameObject>();
    }
}

public class SubjectsTestModel : ATestModel<SubjectsQuestModel>
{
    private List<SubjectsQuestModel> _questions;
    public int PointsPerQuest { get; set; }
    public IDataSource<SubjectsQuestModel> DataSource { set => _dataSource = value; }

    public SubjectsTestModel(IDataSource<SubjectsQuestModel> _source)
    {
        var user = UserModel.GetInstance();
        var data = user.GetTestData("Subjects");
        DataSource = _source;
        _questions = _dataSource.GetQuests(data) as List<SubjectsQuestModel>;
        PointsPerQuest = 10;
        rightAnswers = 0;
        wrongAnswers = 0;
        questionIndex = -1;
    }

    public override (SubjectsQuestModel, int)? GetCurrentQuestion()
    {
        if (questionIndex < _questions.Count)
            return (_questions[questionIndex], questionIndex);
        return null;
    }

    public override (SubjectsQuestModel, int)? GetNextQuestion()
    {
        questionIndex++;
        return GetCurrentQuestion();
    }

    public override int GetQuestsCount()
    {
        return _questions.Count;
    }

    public override int CalculateScore()
    {
        int maxScore = _questions[0].Quest.Count * PointsPerQuest;
        int result = rightAnswers * PointsPerQuest - wrongAnswers * (int)(1f/4f * maxScore);
        return result;
    }

    public override float GetTestTime()
    {
        return 10f;
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
            "Subjects", 
            CalculateScore(), 
            rightAnswers, 
            wrongAnswers
        );
        user.SaveData();
    }

    public override int GetLastScore()
    {
        return UserModel.GetLastScore("Subjects");
    }
}