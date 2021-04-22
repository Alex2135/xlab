using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewQuestionModel;

public class SubjectsQuestModel : IGenericQuestModel<List<Texture2D>, List<Texture2D>>
{
    public List<Texture2D> Quest { get; set; }
    public List<Texture2D> RightAnswers { get; set; }
    public List<Texture2D> AdditionalAnswers { get; set; }
}

public class AdaptedSubjectsQuestModel : IAdaptedQuestModel<Texture2D, Texture2D>
{
    public Dictionary<int, Texture2D> Quest { get; set; }
    public Dictionary<int, Texture2D> RightAnswers { get; set; }
    public Dictionary<int, Texture2D> AdditionalAnswers { get; set; }
}

public class SubjectsQuestView : IAdaptedQuestToView
{
    public Dictionary<int, GameObject> Quest { get; set; }
    public Dictionary<int, GameObject> RightAnswers { get; set; }
    public Dictionary<int, GameObject> AdditionalAnswers { get; set; }
}
