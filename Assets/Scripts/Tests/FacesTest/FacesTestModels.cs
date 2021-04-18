using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewQuestionModel
{
    public class FacesQuestModel : IGenericQuestModel<IEnumerable, IEnumerable>
    {
        virtual public IEnumerable Quest { get ; set; }
        virtual public IEnumerable RightAnswers { get; set ; }
        virtual public IEnumerable AdditionalAnswers { get; set ; }
    }

    public class NameByFaceQuestModel: FacesQuestModel 
    {
        public Dictionary<string, Texture2D> quest;
        public List<string> rightAnswers;
        public List<string> additionalAnswers;

        override public IEnumerable Quest 
        { 
            get => quest; 
            set => quest = (Dictionary<string, string>)value; 
        }
        override public IEnumerable RightAnswers 
        { 
            get => rightAnswers; 
            set => rightAnswers = (List<string>)value; 
        }
        override public IEnumerable AdditionalAnswers 
        { 
            get => additionalAnswers; 
            set => rightAnswers = (List<string>)value; 
        }
    }

    public class FaceByNameQuestModel : FacesQuestModel 
    {
        public List<string> quest;
        public Dictionary<string, Texture2D> rightAnswers;
        public Dictionary<string, Texture2D> additionalAnswers;

        override public IEnumerable Quest 
        { 
            get => quest; 
            set => quest = (List<string>)value; 
        }
        override public IEnumerable RightAnswers 
        { 
            get => rightAnswers; 
            set => rightAnswers = (Dictionary<string, string>)value; 
        }
        override public IEnumerable AdditionalAnswers 
        { 
            get => additionalAnswers; 
            set => additionalAnswers = (Dictionary<string, string>)value; 
        }
    }



    public class FacesAdaptedQuestModel : IAdaptedQuestModel<object, object>
    {
        virtual public Dictionary<int, object> Quest { get; set; }
        virtual public Dictionary<int, object> RightAnswers { get; set; }
        virtual public Dictionary<int, object> AdditionalAnswers { get; set; }
    }

    public class NameByFaceAdaptedQuestModel : FacesAdaptedQuestModel
    {
        public Dictionary<int, Texture2D> quest;
        public Dictionary<int, string> rightAnswer;
        public Dictionary<int, string> additionalAnswer;

        override public Dictionary<int, object> Quest 
        {
            get 
            {
                var result = new Dictionary<int, object>();
                foreach (var q in quest)
                    result.Add(q.Key, q.Value);
                return result;
            }
            set
            {
                foreach (var q in value)
                    quest.Add(q.Key, (Texture2D)q.Value);
            }
        }
        override public Dictionary<int, object> RightAnswers { get; set; }
        override public Dictionary<int, object> AdditionalAnswers { get; set; }
    }

    public class FaceByNameAdaptedQuestModel : FacesAdaptedQuestModel
    {
        public Dictionary<int, string> quest;
        public Dictionary<int, Texture2D> rightAnswer;
        public Dictionary<int, Texture2D> additionalAnswer;

        override public Dictionary<int, object> Quest { get; set; }
        override public Dictionary<int, object> RightAnswers { get; set; }
        override public Dictionary<int, object> AdditionalAnswers { get; set; }
    }



    public class FacesAdaptedQuestToViewModel : IAdaptedQuestToView
    {
        public Dictionary<int, GameObject> Quest { get; set ; }
        public Dictionary<int, GameObject> RightAnswers { get; set; }
        public Dictionary<int, GameObject> AdditionalAnswers { get; set; }
    }

    // TODO: Make MonoBehaviour data provider
    public class GeneratedDataSource : IDataSource<FacesQuestModel>
    {
        public IEnumerable<FacesQuestModel> GetQuests()
        {
            return null;
        }
    }

}