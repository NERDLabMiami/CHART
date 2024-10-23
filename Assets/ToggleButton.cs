using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    public Sprite play;
    public Sprite pause;


    private void Start()
    {
    }

    private void OnEnable()
    {
    }
    public void Toggle()
    {
        if(GetComponentInParent<Controls>())
        {
            if (GetComponentInParent<Controls>().video.isPlaying)
            {
                GetComponent<Image>().sprite = pause;
            }
            else
            {
                GetComponent<Image>().sprite = play;

            }
        }
/*        if(podcast.tour.isPlaying)
        {
            GetComponent<Image>().sprite = pause;
        }
        else
        {
            GetComponent<Image>().sprite = play;
        }
*/
        }
    
}
