using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewQuestionModel;
using System;

public class NumbersTestView : MonoBehaviour, NewQuestionModel.ITestView, IScreenController
{
    public IAdaptedQuestToView QuestionToView { get; set; }
    public string ScreenName { get; set; }
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }

    public event Action<object> OnAnsweringEvent;
    public event Action<object> OnAnswerDidEvent;
    public event Action<object, EventArgs> OnQuestTimeoutEvent;

    public void ResetView()
    {
        
    }

    public void SetScore(float _score)
    {
        
    }

    public void ShowQuestion()
    {
        
    }

    public void ShowQuestResult()
    {
        
    }
}