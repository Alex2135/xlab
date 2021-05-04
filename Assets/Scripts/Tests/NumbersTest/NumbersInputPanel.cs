using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumbersInputPanel : MonoBehaviour
{
    public TMP_InputField inputField;

    public void OnButtonClick(string token)
    {
        if (token == "erase")
        {
            var txt = inputField.text;
            if (txt.Length != 0)
                inputField.text = txt.Substring(0, txt.Length-1);
        }
        else
        {
            inputField.text += token;
        }
    }
}
