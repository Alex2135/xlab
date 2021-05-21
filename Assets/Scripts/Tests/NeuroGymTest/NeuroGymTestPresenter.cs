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
        var quest = testModel.GetNextQuestion();
        if (quest == null) throw new Exception("No quests");
        var (questData, questId) = quest.Value;

        var adaptedQuest = new AdaptedNeuroGymQuestModel();
        adaptedQuest.Quest.Add(questId, questData.Quest);
    }

    public NeuroGymQuestToView GetAdaptedQuest(Action<object> _onAnswerAction)
    {
        var quest = testModel.GetCurrentQuestion();
        if (quest == null) throw new Exception("No quests");
        var (_, questId) = quest.Value;



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