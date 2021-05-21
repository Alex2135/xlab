using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewQuestionModel;

public class TongueTwistersQuestModel : IGenericQuestModel<List<string>, List<string>>
{
    public List<string> Quest { get; set; }
    public List<string> RightAnswers { get; set; } = null;
    public List<string> AdditionalAnswers { get; set; } = null;

    public TongueTwistersQuestModel()
    {
        Quest = new List<string>();
        RightAnswers = null;
        AdditionalAnswers = null;
    }
}

public class AdaptedTongueTwistersQuestModel : IAdaptedQuestModel<List<string>, object>
{
    public Dictionary<int, List<string>> Quest { get; set; }
    public Dictionary<int, object> RightAnswers { get; set; } = null;
    public Dictionary<int, object> AdditionalAnswers { get; set; } = null;

    public AdaptedTongueTwistersQuestModel()
    {
        Quest = new Dictionary<int, List<string>>();
    }
}

public class TongueTwistersQuestView : IAdaptedQuestToView
{
    public Dictionary<int, GameObject> Quest { get; set; }
    public Dictionary<int, GameObject> RightAnswers { get; set; } = null;
    public Dictionary<int, GameObject> AdditionalAnswers { get; set; } = null;

    public TongueTwistersQuestView()
    {
        Quest = new Dictionary<int, GameObject>();
    }
}

public class TongueTwistersTestModel : ATestModel<TongueTwistersQuestModel>
{
    private List<TongueTwistersQuestModel> _questions;
    // TODO: Add property video link

    public TongueTwistersTestModel(IDataSource<TongueTwistersQuestModel> _source)
    {
        var user = UserModel.GetInstance();
        var data = user.GetTestData("TongueTwisters");
        _dataSource = _source;
        _questions = _dataSource.GetQuests(data) as List<TongueTwistersQuestModel>;
        questionIndex = -1;
    }

    public override int CalculateScore()
    {
        return 0;
    }

    public override (TongueTwistersQuestModel, int)? GetCurrentQuestion()
    {
        return (_questions[questionIndex], questionIndex);
    }

    public override int GetLastScore()
    {
        return UserModel.GetLastScore("TongueTwisters");
    }

    public override (TongueTwistersQuestModel, int)? GetNextQuestion()
    {
        questionIndex++;
        if (questionIndex < _questions.Count)
            return GetCurrentQuestion();
        return null;        
    }

    public override int GetQuestsCount()
    {
        return _questions.Count;
    }

    public override float GetTestTime()
    {
        return 60f;
    }

    public override void RegisterScore() 
    { 
        // TODO: send test answer
    }

    public override void PenaltieWrongAnswer(){}
    public override void RewardRightAnswer(){}
}
