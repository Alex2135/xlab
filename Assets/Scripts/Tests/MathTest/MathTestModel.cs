using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewQuestionModel;
using UnityEngine;

public class MathQuestModel : NewQuestionModel.IGenericQuestModel<List<string>, List<string>>
{
    public List<string> Quest { get; set; }
    public List<string> RightAnswers { get; set; }
    public List<string> AdditionalAnswers { get; set; }

    public MathQuestModel()
    {
        Quest = new List<string>();
        RightAnswers = new List<string>();
        AdditionalAnswers = new List<string>();
    }
}

public class MathAdaptedQuestModel : IAdaptedQuestModel<List<string>, List<string>>
{
    public Dictionary<int, List<string>> Quest { get; set; }
    public Dictionary<int, List<string>> RightAnswers { get; set; }
    public Dictionary<int, List<string>> AdditionalAnswers { get; set; }

    public MathAdaptedQuestModel()
    {
        Quest = new Dictionary<int, List<string>>();
        RightAnswers = new Dictionary<int, List<string>>();
        AdditionalAnswers = new Dictionary<int, List<string>>();
    }
}

public class MathAdaptedQuestToView : IAdaptedQuestToView
{
    public Dictionary<int, GameObject> Quest { get; set; }
    public Dictionary<int, GameObject> RightAnswers { get; set; }
    public Dictionary<int, GameObject> AdditionalAnswers { get; set; }

    public MathAdaptedQuestToView()
    {
        Quest = new Dictionary<int, GameObject>();
        RightAnswers = new Dictionary<int, GameObject>();
        AdditionalAnswers = new Dictionary<int, GameObject>();
    }
}

public class MathTestModel : ATestModel<MathQuestModel>
{
    private List<MathQuestModel> _questions;

    public MathTestModel(IDataSource<MathQuestModel> _source)
    {
        var user = UserModel.GetInstance();
        var data = user.GetTestData("WordsColor");
        _dataSource = _source;
        _questions = _dataSource.GetQuests(data) as List<MathQuestModel>;
        questionIndex = -1;
        rightAnswers = 0;
        wrongAnswers = 0;
    }

    public override (MathQuestModel, int)? GetCurrentQuestion()
    {
        if (questionIndex < _questions.Count)
            return (_questions[questionIndex], questionIndex);
        return null;
    }

    public override (MathQuestModel, int)? GetNextQuestion()
    {
        questionIndex++;
        return GetCurrentQuestion();
    }
    public override void PenaltieWrongAnswer()
    {
        wrongAnswers++;
    }

    public override void RewardRightAnswer()
    {
        rightAnswers++;
    }

    public override int CalculateScore()
    {
        throw new NotImplementedException();
    }

    public override int GetLastScore()
    {
        throw new NotImplementedException();
    }

    public override int GetQuestsCount()
    {
        throw new NotImplementedException();
    }

    public override float GetTestTime()
    {
        throw new NotImplementedException();
    }

    public override void RegisterScore()
    {
        throw new NotImplementedException();
    }
}
