using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loader: MonoBehaviour
{
    public Image _loaderImage;
    public Image _backgroundImage;

    public delegate void LoaderDelegate();
    public event LoaderDelegate OnLoaderStart;
    public event LoaderDelegate OnLoaderEnd;

    private bool isContinue;
    public float r = 0.0f;

    private void Awake()
    {
        if (OnLoaderStart != null) OnLoaderStart.Invoke();
        isContinue = true;
        EnableLoader();
    }

    public bool a = true;

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

    public void EnableLoader()
    {
        this.gameObject.SetActive(true);
        StartCoroutine(ShowLoader());
    }

    public void DisableLoader()
    {
        ToggleTransition();
        StartCoroutine(HideLoader());
    }

    public IEnumerator ShowLoader()
    {
        while (isContinue)
        {
            r += Time.deltaTime;
            float n = 5;
            _loaderImage.transform.rotation = new Quaternion(0, 0, Mathf.Sin(n * r), Mathf.Cos(n * r));
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    public IEnumerator HideLoader()
    {
        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
    }

    [System.Obsolete]
    public void OnEnable()
    {
        ToggleTransition();
        Debug.Log("Enable " + gameObject.active.ToString());
        if (OnLoaderStart != null) OnLoaderStart.Invoke();
        isContinue = true;
    }

    [System.Obsolete]
    public void OnDisable()
    {
        Debug.Log("Disable " + gameObject.active.ToString());
        if (OnLoaderEnd != null) OnLoaderEnd.Invoke();
        isContinue = false;
    }
}
