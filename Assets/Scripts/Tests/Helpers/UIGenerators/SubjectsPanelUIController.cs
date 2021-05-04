using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubjectsPanelUIController : MonoBehaviour
{
    public GameObject grid;
    public GameObject buttonPrefab;
    public Dictionary<int, GameObject> Buttons { get; set; }

    public Dictionary<int, GameObject> GeneratePanel(Dictionary<int, Texture2D> _adaptedData)
    {
        if (_adaptedData == null) throw new ArgumentNullException("_adaptedData is null");
        if (grid == null) throw new NullReferenceException("grid is null");
        if (buttonPrefab == null) throw new NullReferenceException("buttonPrefab is null");

        grid.DestroyChildrenObjects();
        var result = new Dictionary<int, GameObject>();

        foreach (var data in _adaptedData)
        {
            GameObject newButton = Instantiate(buttonPrefab, grid.transform);
            var buttonImg = newButton.ChildByName("ButtonIMG"); //
            Image img = buttonImg?.GetComponent<Image>(); //
            if (data.Value != null)
            {
                LoadedImage.SetTextureToImage(ref img, data.Value);
                img.color = new Color(1f, 1f, 1f, 1f); //
            }
            //else img.color = new Color(1f, 1f, 1f, 0f); //
            result.Add(data.Key, newButton);
        }

        Buttons = result;
        return result;
    }
}

public static class GameObjectExtension
{
    public static GameObject ChildByName(this GameObject _obj, string _name)
    {
        GameObject result = null;

        Transform trans = _obj.gameObject.transform;
        Transform childTrans = trans.Find(_name);
        result = childTrans.gameObject;

        return result;
    }

    public static void DestroyChildrenObjects(this GameObject _obj)
    {
        var childs = _obj.GetComponentsInChildren<Transform>();
        var objTransform = _obj.GetComponent<Transform>();
        foreach (var child in childs)
        {
            if (!child.Equals(objTransform))
                UnityEngine.Object.Destroy(child.gameObject);
        }
    }
}