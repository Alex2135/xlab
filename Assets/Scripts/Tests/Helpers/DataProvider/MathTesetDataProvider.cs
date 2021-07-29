using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewQuestionModel;

public class MathTesetDataProvider : MonoBehaviour, IDataSource<MathQuestModel>
{
    private MathQuestModel GenerateQuests(int lvl)
    {
        MathQuestModel result = new MathQuestModel();



        return result;
    }

    public IEnumerable<MathQuestModel> GetQuests(TestWholeStats _test)
    {
        IEnumerable<MathQuestModel> result = new List<MathQuestModel>();



        return result;
    }
}
