using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewQuestionModel;

public class NumbersTestDataProvider : MonoBehaviour, IDataSource<NumbersQuestModel>
{
    // Количество чисел
    public int digits;
    // Количество цифр в числах
    public int DigitsNumber { get; set; }

    public IEnumerable<NumbersQuestModel> GetQuests(TestWholeStats test)
    {
        var result = new List<NumbersQuestModel>();

        int level = (Random.Range(0, 1) > 0.5)? test.testLevel : test.testLevel+1;

        digits = 5 + (test.testLevel - 1);
        //test.testLevel;
        var quest = new NumbersQuestModel();
        int randomNumber;

        // Digit range
        DigitsNumber = 1;
        int minValue = Mathf.RoundToInt(Mathf.Pow(10, DigitsNumber - 1));
        int maxValue = Mathf.RoundToInt(Mathf.Pow(10, DigitsNumber)) - 1;

        for (int i = 0; i < digits; i++)
        {
            randomNumber = Random.Range(minValue, maxValue);
            quest.RightAnswers.Add(randomNumber);
        }
        result.Add(quest);

        return result;
    }
}
