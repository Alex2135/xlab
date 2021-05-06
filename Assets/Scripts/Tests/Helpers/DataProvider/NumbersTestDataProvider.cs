using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewQuestionModel;

public class NumbersTestDataProvider : MonoBehaviour, IDataSource<NumbersQuestModel>
{
    public int digits;
    public int DigitsNumber { get; set; }

    public IEnumerable<NumbersQuestModel> GetQuests(TestWholeStats test)
    {
        DigitsNumber = test.testLevel;
        var result = new List<NumbersQuestModel>();
        var quest = new NumbersQuestModel();
        int randomNumber;
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
