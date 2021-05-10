using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WordsColorUIGenerator : MonoBehaviour
{
    public GameObject colorText;
    public GameObject horizontalPanel;
    public GameObject buttonsPrefab;

    public Dictionary<int, GameObject> Buttons { get; set; }

    public WordsColorAdaptedQuestToView GenerateQuest(AdaptedWordsColorQuestModel _adaptedQuest)
    {
        horizontalPanel.DestroyChildrenObjects();

        var result = new WordsColorAdaptedQuestToView();
        int rightKey = 0;
        foreach (var key in _adaptedQuest.Quest.Keys)
        {
            var questText = colorText.GetComponent<TextMeshProUGUI>();
            questText.text = _adaptedQuest.Quest[key][0].colorName;
            questText.color = _adaptedQuest.Quest[key][0].color;
            result.Quest.Add(key, questText.gameObject);
            rightKey = key;
            break;
        }

        var combinedAnswers = new Dictionary<int, ColorUnit>();

        foreach (var ans in _adaptedQuest.RightAnswers)
            combinedAnswers.Add(ans.Key, ans.Value[0]);
        foreach (var ans in _adaptedQuest.AdditionalAnswers)
            combinedAnswers.Add(ans.Key, ans.Value[0]);

        combinedAnswers = combinedAnswers.Shuffle();

        foreach (var ans in combinedAnswers)
        {
            GameObject newButton = Instantiate(buttonsPrefab, horizontalPanel.transform);
            var bgImage = newButton.GetComponent<Image>();
            var fgImage = newButton.ChildByName("CircleImage").GetComponent<Image>();
            var ansColor = ans.Value.color;
            bgImage.color = new Color(ansColor.r, ansColor.g, ansColor.b, bgImage.color.a);
            fgImage.color = new Color(ansColor.r, ansColor.g, ansColor.b, fgImage.color.a);
            if (ans.Key == rightKey)
                result.RightAnswers.Add(ans.Key, newButton);
            else
                result.AdditionalAnswers.Add(ans.Key, newButton);
        }

        return result;
    }

}
