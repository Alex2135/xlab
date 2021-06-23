using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public interface IResetPasswordView
{
    event Func<string, bool> ResetPasswordEvent;
    void SetError(string _error);
    void SuccessReset();
}

public class ResetPasswordView : MonoBehaviour, IScreenController, IResetPasswordView
{
    public TestsScreensUIController testScreen;
    public TMP_InputField emailTMP;
    public TextMeshProUGUI errorTMP;
    public GameObject resetPasswordPanel;
    public GameObject readyResetPanel;
    public string screenName;

    public string Email => emailTMP.text;
    public string ScreenName { get => screenName; set => screenName = value; }
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }
    public ScreensUIController _screensController;
    public event Func<string, bool> ResetPasswordEvent;
    private ResetPasswordPresenter presenter;

    void OnEnable()
    {
        _screensController = ScreensUIController.GetInstance();
        var model = new ResetPasswordModel();
        presenter = new ResetPasswordPresenter(this, model);
    }

    public void OnResetButtonClick()
    {
        errorTMP.gameObject.SetActive(false);
        ResetPasswordEvent?.Invoke(Email);
    }

    public void OnBackButtonClick()
    {
        if (PrevScreen != null)
        {
            _screensController.DiactivateScreens();
            _screensController.Activate(PrevScreen);
        }
    }

    public void SetError(string _error)
    {
        errorTMP.gameObject.SetActive(true);
        errorTMP.text = _error;
    }

    public void SuccessReset()
    {
        if (presenter.IsChecked)
        {
            readyResetPanel.SetActive(true);
            resetPasswordPanel.SetActive(false);
        }
    }
}
