using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewQuestionModel;
using UnityEngine;

public class NeuroGymQuestModel : IGenericQuestModel<List<string>, List<string>>
{
    public List<string> Quest { get; set; }
    public List<string> RightAnswers { get; set; } = null;
    public List<string> AdditionalAnswers { get; set; } = null;

    public NeuroGymQuestModel()
    {
        Quest = new List<string>();
    }
}

public class AdaptedNeuroGymQuestModel : IAdaptedQuestModel<List<string>, List<string>>
{
    public Dictionary<int, List<string>> Quest { get; set; }
    public Dictionary<int, List<string>> RightAnswers { get; set; } = null;
    public Dictionary<int, List<string>> AdditionalAnswers { get; set; } = null;

    public AdaptedNeuroGymQuestModel()
    {
        Quest = new Dictionary<int, List<string>>();
    }
}

public class NeuroGymQuestToView : IAdaptedQuestToView
{
    public Dictionary<int, GameObject> Quest { get; set; }
    public Dictionary<int, GameObject> RightAnswers { get; set; } = null;
    public Dictionary<int, GameObject> AdditionalAnswers { get; set; } = null;

    public NeuroGymQuestToView()
    {
        Quest = new Dictionary<int, GameObject>();
    }
}

public class NeuroGymTestModel : ATestModel<NeuroGymQuestModel>
{
    private List<NeuroGymQuestModel> _questions;

    public NeuroGymTestModel(IDataSource<NeuroGymQuestModel> _source)
    {
        var user = UserModel.GetInstance();
        var data = user.GetTestData("NeuroGym");
        _dataSource = _source;
        _questions = _source.GetQuests(data) as List<NeuroGymQuestModel>;
        rightAnswers = 0;
        wrongAnswers = 0;
        questionIndex = -1;
    }

    public override int CalculateScore()
    {
        return 0;
    }

    public override (NeuroGymQuestModel, int)? GetCurrentQuestion()
    {
        return (_questions[questionIndex], questionIndex);
    }

    public override int GetLastScore()
    {
        return UserModel.GetLastScore("NeuroGym");
    }

    public override (NeuroGymQuestModel, int)? GetNextQuestion()
    {
        questionIndex++;
        return GetCurrentQuestion();
    }


    public override float GetTestTime()
    {
        return 0f;
    }

    public override void RegisterScore()
    {
        // TODO: send test answer
    }

    public override int GetQuestsCount() => _questions.Count;
    public override void PenaltieWrongAnswer() => wrongAnswers++;
    public override void RewardRightAnswer() => rightAnswers++;
}
