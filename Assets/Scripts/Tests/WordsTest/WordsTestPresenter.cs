using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewQuestionModel;

class WordsTestPresenter : ATestPresenter<WordsQuestModel, WordsQuestView>, ITestPresenter<WordsQuestView>
{
    public WordsTestPresenter(WordsTestModel model, NewQuestionModel.ITestView view)
    {
        testModel = model;
        testQuestionsView = view;
    }

    public override Dictionary<int, WordsQuestView> AdaptedQuestionData { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public WordsQuestView GetAdaptedQuests()
    {
        throw new System.NotImplementedException();
    }

    public void view_OnAnswerDid()
    {
        throw new System.NotImplementedException();
    }

    public override int view_OnAnswering(int _userAnswer)
    {
        throw new System.NotImplementedException();
    }

    protected override void GenerateAnswersId()
    {
        throw new System.NotImplementedException();
    }

    protected override List<T> GetAdaptedQuestsForView<T>()
    {
        throw new System.NotImplementedException();
    }
}
