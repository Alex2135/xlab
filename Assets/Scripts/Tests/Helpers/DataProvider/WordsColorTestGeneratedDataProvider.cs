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

        var shuffledUnits = new ColorUnit[colorUnits.Count]; // For quests
        colorUnits.Shuffle().CopyTo(shuffledUnits); // For answers

        for (int i = 0; i < questsCount; i++)
        {
            colorUnits.Shuffle();
            var newQuest = new WordsColorQuestModel();
            var quest = new ColorUnit()
            {
                color = colorUnits[0].color,
                colorName = shuffledUnits[i].colorName
            };
            newQuest.Quest.Add(quest);
            newQuest.RightAnswers.Add(colorUnits[0]);
            newQuest.AdditionalAnswers.AddRange(
                colorUnits.GetRange(1, colorUnits.Count - 1));
            result.Add(newQuest);
        }

        return result;
    }
}
