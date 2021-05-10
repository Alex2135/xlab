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

    public void OnEnable()
    {
        var img = (PrevScreen.PrevScreen as IDecorableScreen).GetBackground();
        var testName = (PrevScreen as NewQuestionModel.ITestScreenController).TestName;
        var testData = UserModel.GetInstance().GetTestData(testName);
        rateTMP.text = $"{UserModel.GetLastScore(testName)}";
        levelTMP.text = $"Вы успешно достигли {testData.testLevel} уровня";

        LoadedImage.SetTextureToImage(ref background, img.sprite.texture);

    }

    public void OnContinueClick()
    {
        var screenController = ScreensUIController.GetInstance();
        screenController.DiactivateScreens();
        screenController.Activate(PrevScreen.PrevScreen);
    }
}
