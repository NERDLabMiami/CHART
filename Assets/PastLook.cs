using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PastLook : MonoBehaviour
{
    public GameObject presentDayView;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchToPresent()
    {
        presentDayView.SetActive(true);
        gameObject.SetActive(false);
    }
}
