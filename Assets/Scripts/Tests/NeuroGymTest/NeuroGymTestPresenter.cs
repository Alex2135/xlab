using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewQuestionModel;

public class NeuroGymTestPresenter : ATestPresenter<NeuroGymQuestModel, AdaptedNeuroGymQuestModel>, ITestPresenter<NeuroGymQuestToView>
{
    protected override Dictionary<int, AdaptedNeuroGymQuestModel> AdaptedQuestionData { get; set; }

    public NeuroGymTestPresenter(NewQuestionModel.ITestView _view, ATestModel<NeuroGymQuestModel> _model)
    {
        testView = _view;
        testModel = _model;
        testView.OnAnsweringEvent += view_OnAnswering;
        testView.OnAnswerDidEvent += view_OnAnswerDid;
        testView.OnQuestTimeoutEvent += view_OnQuestTimeout;
        AdaptedQuestionData = new Dictionary<int, AdaptedNeuroGymQuestModel>();

        testView.SetScore(testModel.GetLastScore());
        GenerateAnswersId();
    }

    protected override void GenerateAnswersId()
    {

    }

    public NeuroGymQuestToView GetAdaptedQuest(Action<object> _onAnswerAction)
    {
        return null;
    }

    public float GetTestTime()
    {
        return 0f;
    }

    public void view_OnAnswerDid(object _userData)
    {
        
    }

    public void view_OnAnswering(object _userAnswer)
    {
        
    }

    public void view_OnQuestTimeout(object _obj, EventArgs _eventArgs)
    {
        
    }

}