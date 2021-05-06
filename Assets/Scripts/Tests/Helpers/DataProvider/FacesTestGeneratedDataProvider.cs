using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewQuestionModel;

public class FacesTestGeneratedDataProvider : MonoBehaviour, IDataSource<FacesQuestModel>
{
    public List<FacesImage> loadedImages;
    public int questedFaces;

    public IEnumerable<FacesQuestModel> GetQuests(TestWholeStats test)
    {
        if (loadedImages == null) throw new NullReferenceException("loadedImages is null");
        if (questedFaces > loadedImages.Count || questedFaces == 0) throw new Exception("Parameter questedFaces invalid");

        loadedImages = loadedImages.Shuffle();

        var result = new List<FacesQuestModel>();
        var nameByFaceQuests = new List<NameByFaceQuestModel>();
        var faceByNameQuests = new List<FaceByNameQuestModel>();

        for (int i = 0; i < questedFaces; i++)
        {
            var img = loadedImages[i];

            // Generation name by face quest
            var questNF = new NameByFaceQuestModel();
            questNF.quest.Add(img._name, img._image);
            questNF.rightAnswers.Add(img._name);
            foreach (var ans in loadedImages)
                if (ans._name != img._name) questNF.additionalAnswers.Add(ans._name);
            nameByFaceQuests.Add(questNF);

            // Generation face by name quest
            var questFN = new FaceByNameQuestModel();
            questFN.quest.Add(img._name);
            questFN.rightAnswers.Add(img._name, img._image);
            foreach (var ans in loadedImages)
                if (ans._name != img._name) questFN.additionalAnswers.Add(ans._name, ans._image);
            faceByNameQuests.Add(questFN);
        }

        result.AddRange(nameByFaceQuests);
        result.AddRange(faceByNameQuests);

        return result;
    }
}
