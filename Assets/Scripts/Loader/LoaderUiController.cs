using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoaderUiController: MonoBehaviour, IScreenController
{
    public Image _loaderImage;
    public Image _backgroundImage;

    private bool isContinue;
    public float r = 0.0f;
    private IScreenController _nextScreen;

    public IScreenController NextScreen
    {
        get { return _nextScreen; }
        set
        {
            value.PrevScreen = this;
            _nextScreen = value;
        }
    }
    public IScreenController PrevScreen { get; set; }
    public string _screenName;
    public string ScreenName 
    {
        get { return _screenName; }
        set { _screenName = value; }
    }

    private void Awake()
    {
        ScreenName = "LoaderScreen";
        isContinue = true;
        EnableLoader();
    }

    private void ToggleTransition()
    {
        var animator = this.GetComponent<Animator>();
        if (animator != null)
        {
            bool isLoad = animator.GetBool("isLoad");
            if (!isLoad) animator.SetBool("isLoad", true);
            else animator.SetTrigger("MyTrigger");
        }
    }

    // Начало работы загрузчика
    public void EnableLoader()
    {
        this.gameObject.SetActive(true);
        StartCoroutine(ShowLoader());
    }

    // Конец работы загрузчика
    public void DisableLoader()
    {
        ToggleTransition();
        StartCoroutine(HideLoader());
    }

    private IEnumerator ShowLoader()
    {
        while (isContinue)
        {
            
            r += Time.deltaTime;
            float n = 5;
            // Анимация поворота загрузчика каждые 0.1 секунд реального времени
            _loaderImage.transform.rotation = new Quaternion(0, 0, Mathf.Sin(n * r), Mathf.Cos(n * r));
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    private IEnumerator HideLoader()
    {
        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
    }

    public void OnEnable()
    {
        ToggleTransition();
        isContinue = true;
    }

    public void OnDisable()
    {
        isContinue = false;
    }
}
