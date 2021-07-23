using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewQuestionModel;

class MathTestPresenter : ATestPresenter<MathQuestModel, MathAdaptedQuestModel>
{
    protected override Dictionary<int, MathAdaptedQuestModel> AdaptedQuestionData { get; set; }

    protected override void GenerateAnswersId()
    {

    }
}