using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubjectsPanelUIController : MonoBehaviour
{
    public GameObject grid;
    public GameObject buttonPrefab;

    public Dictionary<int, GameObject> GeneratePanel(Dictionary<int, Texture2D> _adaptedData)
    {
        if (_adaptedData == null) throw new ArgumentNullException("_adaptedData is null");
        if (grid == null) throw new NullReferenceException("grid is null");
        if (buttonPrefab == null) throw new NullReferenceException("buttonPrefab is null");

        var result = new Dictionary<int, GameObject>();
        GameObject newButton;
        Image img;

        foreach (var data in _adaptedData)
        {
            newButton = Instantiate(buttonPrefab, grid.transform);
            img = newButton.FindByName("ButtonIMG").GetComponent<Image>();
            if (data.Value != null) LoadedImage.SetTextureToImage(ref img, data.Value);
        }

        return result;
    }
}

public static class GameObjectExtension
{
    public static GameObject FindByName(this GameObject _obj, string _name)
    {
        GameObject result = null;

        Transform trans = _obj.gameObject.transform;
        Transform childTrans = trans.Find(_name);

        return result;
    }
}