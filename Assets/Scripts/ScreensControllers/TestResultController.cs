using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestResultController : MonoBehaviour, IScreenController
{
    public TextMeshProUGUI rateTMP;
    public TextMeshProUGUI levelTMP;
    public Image background;
    public string screenName;

    public string ScreenName { get => screenName; set => screenName = value; }
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }

    private string testName;
    private bool isCounterRun;
    private int LastScore { get => UserModel.GetLastScore(testName); }
    private ScreensUIController screensController;
    private int PrevScore {
        get 
        {
            var testData = UserModel.GetInstance();
            var stats = testData.GetTestData(testName);
            var count = stats.testScores.Count;
            if (count > 1)
                return stats.testScores[count - 2].testScore;
            return 40;
        }
    }

    public void OnEnable()
    {
        screensController = ScreensUIController.GetInstance();
        var img = (PrevScreen.PrevScreen as IDecorableScreen).GetBackground();
        testName = (PrevScreen as NewQuestionModel.ITestScreenController).TestName;
        var testData = UserModel.GetInstance().GetTestData(testName);
        levelTMP.text = $"Вы успешно достигли {testData.testLevel} уровня";
        rateTMP.text = $"{LastScore}";

        StartCoroutine(BeginCounter());

        background.color = img.color;
        //LoadedImage.SetTextureToImage(ref background, img.sprite.texture);
    }

    IEnumerator BeginCounter()
    {
        isCounterRun = true;

        int last = Mathf.Max(LastScore, 0);
        int prev = Mathf.Max(PrevScore, 0);
        int delta = (last > prev) ? 1 : -1;
        float duration = 1f;
        float valsPerSecond = duration / Mathf.Max(1, (last - prev));

        for (float i = 0f; i < duration; i += valsPerSecond)
        {
            prev += delta;
            rateTMP.text = $"{prev}";
            yield return new WaitForSecondsRealtime(valsPerSecond);
        }

        isCounterRun = false;
    }

    public void OnContinueClick()
    {
        if (screensController != null &&
            PrevScreen != null)
        {
            screensController.Activate(PrevScreen);
        }
    }

    public void OnStopTrainingClick()
    {        
        if (!isCounterRun && 
            screensController != null &&
            PrevScreen.PrevScreen != null)
        {
            var screensController = ScreensUIController.GetInstance();
            screensController.Activate(PrevScreen.PrevScreen);
        }
    }
}
