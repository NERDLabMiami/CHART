using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NoDuplicate : MonoBehaviour
{
    public GameObject eventSystem;

    // Start is called before the first frame update
    void Start()
    {
        if(!EventSystem.current)
        {
            Instantiate(eventSystem);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
