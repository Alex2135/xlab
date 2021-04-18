using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewQuestionModel;

public class FacesTestGeneratedDataProvider : MonoBehaviour, IDataSource<FacesQuestModel>
{
    public List<FacesImage> loadedImages;
    public int questedFaces;

    public IEnumerable<FacesQuestModel> GetQuests()
    {
        loadedImages.Shuffle();

        var nameByFaceQuests = new List<NameByFaceQuestModel>();
        var faceByNameQuests = new List<FaceByNameQuestModel>();

        for (int i = 0; i < questedFaces; i++)
        {
            var quest = new NameByFaceQuestModel();
            //quest.
        }

        return null;
    }
}
