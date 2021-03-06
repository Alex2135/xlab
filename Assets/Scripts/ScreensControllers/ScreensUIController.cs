using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IScreenController
{
    string ScreenName { get; set; }
    IScreenController NextScreen { get; set; }
    IScreenController PrevScreen { get; set; }
}

public interface IResetableScreenController: IScreenController
{
    void ResetScreenState();
}

namespace NewQuestionModel
{

    public interface ITestScreenController
    {
        string TestName { get; }
    }
}

interface IResultableScreen
{
    object GetResult();
}

interface IDecorableScreen
{
    Image GetBackground();
}

public class ScreensUIController
{
    public List<IScreenController> ScreenControllers { get; set; }
    public int Count { get => ScreenControllers.Count; }
    static private ScreensUIController _instance;

    private ScreensUIController()
    {
        ScreenControllers = new List<IScreenController>();
    }

    public static ScreensUIController GetInstance()
    {
        if (_instance == null) _instance = new ScreensUIController();
        return _instance;
    }

    public IScreenController GetScreenByName(string _screenName)
    {
        foreach (var screen in ScreenControllers)
        {
            if (screen != null &&
                screen.ScreenName == _screenName) 
                return screen;
        }
        return null;
    }

    public void DiactivateScreens()
    {
        foreach (var screen in ScreenControllers)
        {
            if (screen is MonoBehaviour mb)
                mb.gameObject.SetActive(false);
        }
    }

    public void Add(IScreenController _newScreen)
    {
        if (_newScreen != null &&
            _newScreen.ScreenName != null) 
            ScreenControllers.Add(_newScreen);
    }

    public void Activate(IScreenController _screen, IScreenController _prevContext = null, bool _diactivateScreens = true)
    {
        if (_screen == null) throw new System.ArgumentNullException("Argument _screen is null");
        if (_screen is MonoBehaviour mb)
        {
            if (!ScreenControllers.Contains(_screen)) ScreenControllers.Add(_screen);
            if (!ScreenControllers.Contains(_prevContext)) ScreenControllers.Add(_prevContext);
            if (_diactivateScreens) DiactivateScreens();
            if (_prevContext != null)
            {
                (mb as IScreenController).PrevScreen = _prevContext;
                if (_prevContext is MonoBehaviour mbContext)
                    mbContext.gameObject.SetActive(false);
            }
            mb.gameObject.SetActive(true);
        }
        else
        {
            throw new System.Exception("Screen is not parent of MonoBehaviour");
        }
    }

    public void LinkedScreens()
    {
        int size = ScreenControllers.Count;
        if (size > 0)
        {
            for (int i = 0; i < size - 1; i++)
            {
                ScreenControllers[i].NextScreen = ScreenControllers[i + 1];
            }
        }
    }
}


