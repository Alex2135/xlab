using System;
using System.Collections.Generic;
using NewQuestionModel;

public class RememberFacesPresenter : ATestPresenter<FacesQuestModel, FacesAdaptedQuestModel>, ITestPresenter<FacesAdaptedQuestToViewModel>
{
    protected override Dictionary<int, FacesAdaptedQuestModel> AdaptedQuestionData { get; set; }

    public RememberFacesPresenter(ATestModel<FacesQuestModel> model, NewQuestionModel.ITestView view)
    {
        testModel = model;
        testView = view;

        testView.OnAnswerDid += view_OnAnswerDid;
        testView.OnAnswering += view_OnAnswering;
    }

    public FacesAdaptedQuestToViewModel GetAdaptedQuest(Action<object> _onAnswerClick)
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

    protected override void GenerateAnswersId()
    {
        
    }
}