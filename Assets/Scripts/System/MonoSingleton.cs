using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour
{
    // Start is called before the first frame update
    protected void Start()
    {
        GameManager.Get().RegisterMonoSingleton<T>(this);
    }

    protected void OnDestroy()
    {
        var GM = GameManager.Get(false);
        if (GM)
        {
            GM.UnregisterMonoSingleton<T>(this);
        }
    }
}
