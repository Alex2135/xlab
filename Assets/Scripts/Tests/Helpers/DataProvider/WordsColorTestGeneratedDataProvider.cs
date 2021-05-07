using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewQuestionModel;

public class WordsColorTestGeneratedDataProvider : MonoBehaviour, IDataSource<WordsColorQuestModel>
{
    public int questsCount;
    public List<ColorUnit> colorUnits;

    public IEnumerable<WordsColorQuestModel> GetQuests(TestWholeStats _test)
    {
        var result = new List<WordsColorQuestModel>();

        colorUnits = colorUnits.Shuffle();

        for (int i = 0; i < questsCount; i++)
        {
            // TODO: form colors quests
        }

        return result;
    }
}
