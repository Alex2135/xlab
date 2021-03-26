using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowQuestResult : MonoBehaviour
{
    private QuestionView _questionView;
    public Color DefaultTextColor;
    public Color DefaultButtonColor;

    private void SetColors()
    {
        if (_questionView != null)
        {
            DefaultTextColor = _questionView._answers[0]._text.color;
            DefaultButtonColor = _questionView._answers[0]._image.color;
        }
    }

    private void Start()
    {
        SetColors();
    }

    public void SetQuestionView(QuestionView _qv)
    {
        _questionView = _qv;
        SetColors();
    }

    public void ShowQuestionResult(int _selectedAnswer, int _rightAnswer)
    {
        if (_selectedAnswer == _rightAnswer)
        {
            _questionView._answers[_selectedAnswer]._image.color = new Color(0, 255/255, 89/255);
            _questionView._answers[_selectedAnswer]._text.color = new Color(1, 1, 1);
        }
        else
        {
            _questionView._answers[_selectedAnswer]._image.color = new Color(255/255, 105/255, 105/255);
            _questionView._answers[_selectedAnswer]._text.color = new Color(1, 1, 1);
            _questionView._answers[_rightAnswer]._image.gameObject.SetActive(true);
            _questionView._answers[_rightAnswer]._image.color = new Color(0, 255/255, 89/255);
            _questionView._answers[_rightAnswer]._text.color = new Color(1, 1, 1);
        }
    }

    public void ResetQuestResult()
    {
        for (int i = 0; i < _questionView._answers.Count; i++)
        {
            _questionView._answers[i]._image.color = DefaultButtonColor;
            _questionView._answers[i]._text.color = DefaultTextColor;
        }
    }
}
