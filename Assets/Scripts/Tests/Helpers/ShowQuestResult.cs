using UnityEngine;

public class ShowQuestResult : MonoBehaviour
{
    private QuestionView _questionView;
    public Color DefaultTextColor;
    public Color DefaultButtonColor;
    public Color greenBG;
    public Color redBG;

    private void SetColors()
    {
        if (_questionView != null)
        {
            DefaultTextColor = _questionView._answers[0]._text.color;
            DefaultButtonColor = _questionView._answers[0]._image.color;
        }
    }

    private void Awake()
    {
        SetColors();
        greenBG = new Color(0x63 / 255f, 0xCA / 255f, 0x85 / 255f);
        redBG = new Color(255f/255f, 105f/255f, 105f/255f);
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
            _questionView._answers[_selectedAnswer]._image.color = greenBG;
            _questionView._answers[_selectedAnswer]._text.color = new Color(1, 1, 1);
        }
        else
        {
            _questionView._answers[_selectedAnswer]._image.color = redBG;
            _questionView._answers[_selectedAnswer]._text.color = new Color(1, 1, 1);
            _questionView._answers[_rightAnswer]._image.gameObject.SetActive(true);
            _questionView._answers[_rightAnswer]._image.color = greenBG;
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
