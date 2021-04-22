using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SubjectsTestView : MonoBehaviour, IScreenController
{
    public string screenName;
    public TextMeshProUGUI instruct;
    public GameObject subjectsGrid;
    public GameObject answersGrid;

    public string ScreenName { get => screenName; set => screenName = value; }
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }

    void OnEnable()
    {

    }

    void OnDisable()
    {

    }
}
