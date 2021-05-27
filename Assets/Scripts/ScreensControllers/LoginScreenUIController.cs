using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginScreenUIController : MonoBehaviour, IScreenController
{
    public TestsScreensUIController testScreen;
    public ResetPasswordUIController resetPasswordScreen;
    public string screenName;
    public string ScreenName { get => screenName; set => screenName = value; }
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }

    public ScreensUIController _controller;

    void OnEnable()
    {
        _controller = ScreensUIController.GetInstance();
    }

    public void OnLoginClick()
    {
        NextScreen = testScreen;
        _controller.Activate(NextScreen, this);
    }

    public void OnForgotPasswordClick()
    {
        NextScreen = resetPasswordScreen;
        resetPasswordScreen.PrevScreen = this;
        _controller.Activate(NextScreen, this);
    }

}

