using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewQuestionModel
{
    // Data quests models
    public enum FaceQuestType { FaceByName, NameByFace, None }

    public class FacesQuestModel : IGenericQuestModel<IEnumerable, IEnumerable>
    {
        public FaceQuestType QuestType = FaceQuestType.None;
        virtual public IEnumerable Quest { get ; set; }
        virtual public IEnumerable RightAnswers { get; set ; }
        virtual public IEnumerable AdditionalAnswers { get; set ; }
    }

    public class NameByFaceQuestModel: FacesQuestModel 
    {
        public FaceQuestType QuestType = FaceQuestType.NameByFace;
        public Dictionary<string, Texture2D> quest;
        public List<string> rightAnswers;
        public List<string> additionalAnswers;

        public NameByFaceQuestModel()
        {
            quest = new Dictionary<string, Texture2D>();
            rightAnswers = new List<string>();
            additionalAnswers = new List<string>();
        }

        override public IEnumerable Quest 
        { 
            get => quest; 
            set => quest = (Dictionary<string, Texture2D>)value; 
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
        public FaceQuestType QuestType = FaceQuestType.FaceByName;
        public List<string> quest;
        public Dictionary<string, Texture2D> rightAnswers;
        public Dictionary<string, Texture2D> additionalAnswers;

        public FaceByNameQuestModel()
        {
            quest = new List<string>();
            rightAnswers = new Dictionary<string, Texture2D>();
            additionalAnswers = new Dictionary<string, Texture2D>();
        }

        override public IEnumerable Quest 
        { 
            get => quest; 
            set => quest = (List<string>)value; 
        }
        override public IEnumerable RightAnswers 
        { 
            get => rightAnswers; 
            set => rightAnswers = (Dictionary<string, Texture2D>)value; 
        }
        override public IEnumerable AdditionalAnswers 
        { 
            get => additionalAnswers; 
            set => additionalAnswers = (Dictionary<string, Texture2D>)value; 
        }
    }


    // Adapted question models
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
        override public Dictionary<int, object> RightAnswers {
            get
            {
                var result = new Dictionary<int, object>();
                foreach (var q in rightAnswer)
                    result.Add(q.Key, q.Value);
                return result;
            }
            set
            {
                foreach (var q in value)
                    rightAnswer.Add(q.Key, (Texture2D)q.Value);
            }
        }
        override public Dictionary<int, object> AdditionalAnswers {
            get
            {
                var result = new Dictionary<int, object>();
                foreach (var q in additionalAnswer)
                    result.Add(q.Key, q.Value);
                return result;
            }
            set
            {
                foreach (var q in value)
                    additionalAnswer.Add(q.Key, (Texture2D)q.Value);
            }
        }
    }


    // Question models to view
    public class FacesAdaptedQuestToViewModel : IAdaptedQuestToView
    {
        public Dictionary<int, GameObject> Quest { get; set ; }
        public Dictionary<int, GameObject> RightAnswers { get; set; }
        public Dictionary<int, GameObject> AdditionalAnswers { get; set; }
    }


    /// <summary>
    /// Model for test info
    /// </summary>
    public class FacesTestModel : ATestModel<FacesQuestModel>
    {
        private List<FacesQuestModel> _questions;
        public int PointsPerQuest { get; set; }

        public FacesTestModel(IDataSource<FacesQuestModel> _source)
        {
            rightQuestions = 0;
            wrongQuestions = 0;
            questionIndex = -1;
            _dataSource = _source;
            _questions = (List<FacesQuestModel>)_source.GetQuests();
            PointsPerQuest = 10;
        }

        public override (FacesQuestModel, int)? GetCurrentQuestion()
        {
            if (questionIndex < _questions.Count)
                return (_questions[questionIndex], questionIndex);
            return null;
        }

        public override (FacesQuestModel, int)? GetNextQuestion()
        {
            questionIndex++;
            return GetCurrentQuestion();
        }

        public override int GetQuestsCount()
        {
            return _questions.Count;
        }

        public override int GetScore()
        {
            int maxScore = _questions.Count * PointsPerQuest;
            int result = rightQuestions * PointsPerQuest - wrongQuestions * 1 / 4 * maxScore;
            return result;
        }

        public override float GetTestTime()
        {
            return 0f;
        }

        public override void PenaltieWrongAnswer()
        {
            wrongQuestions++;
        }

        public override void RewardRightAnswer()
        {
            rightQuestions++;
        }
    }
}