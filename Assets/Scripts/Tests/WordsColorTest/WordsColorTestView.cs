using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewQuestionModel;

public class WordsColorTestView : MonoBehaviour, IScreenController, NewQuestionModel.ITestScreenController
{
    public string ScreenName { get; set; }
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }

    public string TestName => "WordsColor";
    private WordsColorTestPresenter presenter;

    void OnEnable()
    {

    }
}
