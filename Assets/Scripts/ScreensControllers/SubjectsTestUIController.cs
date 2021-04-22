using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubjectsTestUIController : MonoBehaviour, IScreenController, IDecorableScreen
{
    public string screenName;
    public Image background;
    public SubjectsTestView view;
    public string ScreenName { get => screenName; set => screenName = value; }
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }

    private ScreensUIController _screensController;

    public Image GetBackground()
    {
        return background;
    }

    void Awake()
    {
        _screensController = ScreensUIController.GetInstance();
        _screensController.Add(view);

        _screensController.Activate(view, null, false);
    }

    public void OnBackClick()
    {
        if (PrevScreen != null)
        {
            _screensController.DiactivateScreens();
            _screensController.Activate(PrevScreen);
        }
        else
        {
            Debug.Log("Prev screen not set!");
        }
    }

}
