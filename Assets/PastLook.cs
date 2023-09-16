using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PastLook : MonoBehaviour
{
    public GameObject presentDayView;
    public GameObject controls;
    private RectTransform controlsRect;
    private RectTransform canvas;
    private bool isVisible = true;
    private bool moving = false;
    // Start is called before the first frame update
    void Start()
    {
        controlsRect = controls.GetComponent<RectTransform>();
        canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();
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

    public void ToggleControls()
    {
        Vector2 newPos = controls.transform.position;
        if (isVisible)
        {
            float y = controlsRect.offsetMin.y;
            Debug.Log("Y: " + y);
            newPos.y = y;

            controls.transform.position = newPos;
                //            newPos.y += controlsRect.rect.height;
        }
        else
        {
            float y = controlsRect.offsetMax.y;

            //            newPos.y -= controlsRect.rect.height;
        }
        controls.transform.position = newPos;

        isVisible = !isVisible;
    }

}
