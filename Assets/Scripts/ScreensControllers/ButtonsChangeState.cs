using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This script need attach to button component
public class ButtonsChangeState : MonoBehaviour
{
    public Image changableImage;
    public Texture2D initialState;
    public Texture2D additionState;

    public bool IsInitialState => isInitialState;
    private bool isInitialState;
    private Button button;

    private void OnEnable()
    {
        isInitialState = true;
        button = GetComponent<Button>();
        button.onClick.AddListener(() => OnButtonClick());
    }

    void OnButtonClick()
    {
        isInitialState = !isInitialState;
        if (isInitialState)
        {
            LoadedImage.SetTextureToImage(ref changableImage, initialState);
        }
        else
        {
            LoadedImage.SetTextureToImage(ref changableImage, additionState);
        }
    }
}
