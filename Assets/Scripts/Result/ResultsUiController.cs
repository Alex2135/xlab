using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * Result - окно, которое должно отображать результат прохождения теста.
 * Результат должен отображаться в количестве балов, которое набрал
 * пользователь. Далее возможно добавление нового функционала с отображением
 * рейтинга, которого достиг пользователь.
 * 
 * У окон должна быть своя последовательность появления на экране. Система
 * должна знать какое окно за каким отображать. 
 * Когда окно которое загружается должен показываться загрузчик. после этого
 * само окно. Окно теста должно быть переведено в окно которое его вызывало
 * если тест обязательный для прохождения, то тест не будет считаться 
 * пройденым.
 */
public class ResultsUiController : MonoBehaviour, IScreenController
{
    public TextMeshProUGUI resultText;
    private IScreenController _nextScreen;
    public IScreenController NextScreen
    {
        get { return _nextScreen; }
        set
        {
            value.PrevScreen = this;
            _nextScreen = value;
        }
    }
    public IScreenController PrevScreen { get; set; }

    private void OnEnable()
    {
        if (PrevScreen != null && PrevScreen.GetResult() is Result result)
        {
            resultText.text = $"Правильных ответов:" +
                              $"\n{result.TruePositive} из {result.QuestsCount}" +
                              $"\n\nВы заработали" +
                              $"\n{result.Grade} очков";
        }
    }

    public object GetResult()
    {
        throw new System.NotImplementedException();
    }

}