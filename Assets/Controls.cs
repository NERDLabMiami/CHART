using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public GameObject controls;
    private bool isVisible = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Handle tap or click here
            Toggle();
        }
        // Check for mouse input
        if (Input.GetMouseButtonDown(0))
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        controls.SetActive(isVisible);
        isVisible = !isVisible;
    }
}
