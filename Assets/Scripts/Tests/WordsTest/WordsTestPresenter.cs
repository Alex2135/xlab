using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewQuestionModel;

class WordsTestPresenter : ATestPresenter<WordsQuestModel, WordsQuestView>, ITestPresenter<WordsQuestView>
{
    private List<WordsAdaptedQuestModel> adaptedQuests;
    protected override Dictionary<int, WordsQuestView> AdaptedQuestionData { get; set; }

    public WordsTestPresenter(ATestModel<WordsQuestModel> model, NewQuestionModel.ITestView view)
    {
        testModel = model;
        testQuestionsView = view;
        adaptedQuests = new List<WordsAdaptedQuestModel>();

        testQuestionsView.OnAnswering += view_OnAnswering;
        testQuestionsView.OnAnswerDid += view_OnAnswerDid;
    }

    public void view_OnAnswering(object _userAnswer)
    {
        string answerId = (string)_userAnswer;
    }

    public void view_OnAnswerDid(object _userData)
    {

    }

    protected override void GenerateAnswersId()
    {

    }

    public WordsQuestView GetAdaptedQuest()
    {
        var result = new WordsQuestView();

        if (adaptedQuests == null) throw new System.Exception("Data error");
        if (adaptedQuests.Count == 0) adaptedQuests = GetAdaptedQuestsForView<WordsAdaptedQuestModel>();

        return result;
    }

    protected override List<T> GetAdaptedQuestsForView<T>() 
    {
        var result = new List<T>();



        return result;
    }

    public IEnumerable<WordsQuestView> GetAdaptedQuestsForView()
    {
        throw new System.NotImplementedException();
    }
}
