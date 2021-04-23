using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewQuestionModel;

public class SubjectsTestGeneratedDataProvider : MonoBehaviour, IDataSource<SubjectsQuestModel>
{
    public List<Texture2D> rightAnswers;
    public List<Texture2D> additionAnswers;

    public IEnumerable<SubjectsQuestModel> GetQuests()
    {
        var result = new List<SubjectsQuestModel>();
        var quest = new SubjectsQuestModel();

        quest.RightAnswers = rightAnswers;
        quest.RightAnswers = quest.RightAnswers.Shuffle();
        quest.AdditionalAnswers = additionAnswers;

        var quests = new Texture2D[rightAnswers.Count];
        int nullsCount = 0;
        int nullImagesCount = (int)(rightAnswers.Count * 3f / 4f);
        rightAnswers.CopyTo(quests);
        
        while (nullsCount != nullImagesCount)
        {
            var idx = Random.Range(0, nullImagesCount);
            if (quests[idx] != null)
            {
                quests = null;
                nullsCount++;
            }
        }

        quest.Quest = new List<Texture2D>(quests);
        return result;
    }
}
