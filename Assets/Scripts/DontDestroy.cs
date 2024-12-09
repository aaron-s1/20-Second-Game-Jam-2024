using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public static DontDestroy blah;
    void Awake()
    {
        if (blah == null)
            blah = this;
        else if (blah != this)
            Destroy(gameObject);
        DontDestroyOnLoad(this.gameObject);
    }
}
