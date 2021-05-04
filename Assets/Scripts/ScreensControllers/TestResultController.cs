using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestResultController : MonoBehaviour, IScreenController
{
    public TextMeshProUGUI rateTMP;
    public string screenName;

    public string ScreenName { get => screenName; set => screenName = value; }
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }

    public void OnEnable()
    {
        
    }
}
