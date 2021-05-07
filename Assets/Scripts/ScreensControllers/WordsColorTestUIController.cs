﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

class WordsColorTestUIController : MonoBehaviour, IScreenController, IDecorableScreen
{
    public Image background;
    public string screenName;
    public WordsColorTestView view;

    public string ScreenName { get => screenName; set => screenName = value; }
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }

    public Image GetBackground() => background;
    private ScreensUIController _screensController;

    void OnEnable()
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