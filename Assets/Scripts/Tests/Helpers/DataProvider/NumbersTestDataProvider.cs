using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewQuestionModel;

public class NumbersTestDataProvider : MonoBehaviour, IDataSource<NumbersQuestModel>
{
    public int digits;
    public int maxValue;
    public int minValue;

    public IEnumerable<NumbersQuestModel> GetQuests()
    {
        var result = new List<NumbersQuestModel>();
        var quest = new NumbersQuestModel();
        int randomNumber;

        for (int i = 0; i < digits; i++)
        {
            randomNumber = Random.Range(minValue, maxValue);
            quest.RightAnswers.Add(randomNumber);
        }
        result.Add(quest);

        return result;
    }
}
