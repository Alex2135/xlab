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

        return result;
    }
}
