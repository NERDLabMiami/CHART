using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    public Podcast podcast;
    public Sprite play;
    public Sprite pause;


    private void Start()
    {
        Toggle();
    }

    private void OnEnable()
    {
        Toggle();
    }
    public void Toggle()
    {
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
