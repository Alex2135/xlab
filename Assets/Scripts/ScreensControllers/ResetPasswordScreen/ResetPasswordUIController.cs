using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPasswordUIController : MonoBehaviour, IScreenController
{
    public TestsScreensUIController testScreen;
    public string screenName;
    public string ScreenName { get => screenName; set => screenName = value; }
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }
    public ScreensUIController _screensController;

    void Awake()
    {
        _screensController = ScreensUIController.GetInstance();
    }

    void OnEnable()
    {

    }

    public void OnBackButtonClick()
    {
        if (PrevScreen != null)
        {
            _screensController.DiactivateScreens();
            _screensController.Activate(PrevScreen);
        }
    }
}
