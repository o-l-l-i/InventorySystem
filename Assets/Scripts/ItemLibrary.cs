using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLibrary : MonoBehaviour
{
    [Header("Available items")]
    public List<Item> itemPool;

    private static ItemLibrary _instance;
    public static ItemLibrary Instance { get { return _instance; } }


    void Awake ()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

}