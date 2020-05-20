using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentManager : MonoBehaviour
{
    public bool dontDestroyOnLoad = true;
    private static PersistentManager _sInstance = null;

    private void Awake()
    {
        if (_sInstance == null)
        {
            _sInstance = this;
            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(this);
            }
        }
        else
        { 
            Destroy(gameObject);
        }
    }
}
