using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreenUIController : MonoBehaviour, IScreenController
{
    public LoginScreenUIController loginScreen;
    public string screenName;
    public string ScreenName { get => screenName; set => screenName = value; }
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }

    public ScreensUIController _controller;

    void Awake()
    {
        _controller = ScreensUIController.GetInstance();
        _controller.Add(this);
        NextScreen = loginScreen;
    }

    void OnEnable()
    {
        StartCoroutine(GoToNextScreen());
    }

    IEnumerator GoToNextScreen()
    {
        yield return new WaitForSeconds(2f);

        if (NextScreen != null) _controller.Activate(NextScreen);
    }
}
