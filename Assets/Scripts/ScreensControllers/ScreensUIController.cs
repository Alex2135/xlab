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
            if (screen.ScreenName == _screenName) return screen;
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
        ScreenControllers.Add(_newScreen);
    }

    public void Activate(IScreenController _screen, IScreenController _context = null, bool _diactivateScreens = true)
    {
        if (_screen is MonoBehaviour mb)
        {
            if (!ScreenControllers.Contains(_screen)) ScreenControllers.Add(_screen);
            if (!ScreenControllers.Contains(_context)) ScreenControllers.Add(_context);
            if (_diactivateScreens) DiactivateScreens();
            if (_context != null) (mb as IScreenController).PrevScreen = _context;
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


