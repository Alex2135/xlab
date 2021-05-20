using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewQuestionModel;

public class NeuroGymTestDataProvider : MonoBehaviour, NewQuestionModel.IDataSource<NeuroGymQuestModel>
{
    public string questFolder;

    public IEnumerable<NeuroGymQuestModel> GetQuests(TestWholeStats _test)
    {
        var result = new List<NeuroGymQuestModel>();

        // TODO: Download data from net and save it in local machine

        //result.Add()
        var questPath = Path.Combine(Application.persistentDataPath, questFolder, "quest_1.mp4");

        var newQuest = new NeuroGymQuestModel();
        newQuest.Quest.Add(questPath);
        result.Add(newQuest);

        return result;
    }
}
