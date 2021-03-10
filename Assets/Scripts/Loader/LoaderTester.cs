using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderTester : MonoBehaviour
{
    public Loader _loader;

    public bool isSet = false;
    public bool isEnable = false;

    public int i = 0;

    void Update()
    {
        i++;
        if ( i >= 500 && isSet == false)
        {
            _loader.DisableLoader();
            isSet = true;
        }
    }
}
