using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public interface ILoginView
{
    void OnEmailError(string _error);
    void OnPasswordError(string _error);
    void SuccessLogin();
    bool IsRemember { get; }
}

public class LoginScreenView : MonoBehaviour, ILoginView, IScreenController
{
    public TestsScreensUIController testScreen;
    public ResetPasswordUIController resetPasswordScreen;
    public TMP_InputField emailTMPField;
    public TMP_InputField passwordTMPField;
    public TextMeshProUGUI emailErrorTMP;
    public TextMeshProUGUI passwordErrorTMP;

    private bool isRemember;
    private bool isShowPassword;
    private LoginScreenPresenter presenter;
    private ScreensUIController _controller;

    public string ScreenName { get; set; } = "LoginScreenView";
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }

    public bool IsRemember => isRemember;

    private bool isChecked;
    private string Email => emailTMPField.text;
    private string Password => passwordTMPField.text;

    private void OnEnable()
    {
        _controller = ScreensUIController.GetInstance();
        isRemember = false;
        isShowPassword = false;
        isChecked = false;

        var model = new LoginScreenModel();
        presenter = new LoginScreenPresenter(this, model);
    }

    public void OnEyeButtonClick()
    {
        isShowPassword = !isShowPassword;
        if (isShowPassword)
        {
            passwordTMPField.contentType = TMP_InputField.ContentType.Standard;
        }
        else
        {
            passwordTMPField.contentType = TMP_InputField.ContentType.Password;
        }
        passwordTMPField.ForceLabelUpdate();
    }

    public void OnRememberButtonClick()
    {
        isRemember = !isRemember;
    }

    public void OnLoginButtonClick()
    {
        emailErrorTMP.gameObject.SetActive(false);
        emailErrorTMP.gameObject.SetActive(false);
        if (presenter.CheckEmailAndPassword(Email, Password))
        {
            isChecked = true;
            SuccessLogin();
        }
    }

    public void OnEmailError(string _error)
    {
        emailErrorTMP.gameObject.SetActive(true);
        emailErrorTMP.text = _error;
    }

    public void OnPasswordError(string _error)
    {
        passwordErrorTMP.gameObject.SetActive(true);
        passwordErrorTMP.text = _error;
    }

    public void OnForgotPasswordClick()
    {
        NextScreen = resetPasswordScreen;
        resetPasswordScreen.PrevScreen = this;
        _controller.Activate(NextScreen, this);
    }

    public void SuccessLogin()
    {
        if (isChecked)
        {
            NextScreen = testScreen;
            testScreen.PrevScreen = this;
            _controller.Activate(NextScreen, this);
        }
    }
}
