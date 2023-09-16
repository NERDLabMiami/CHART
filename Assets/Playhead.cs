using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Playhead : MonoBehaviour
{
    public Podcast podcast;
    Slider slider;
    private bool audioLengthSet = false;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.direction = Slider.Direction.LeftToRight;
        slider.minValue = 0;
    }

    private void PodcastLoaded()
    {
        slider.maxValue = podcast.audioClip.length;
        audioLengthSet = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(podcast.isLoaded)
        {
            slider.value = podcast.tour.time;
            if(!audioLengthSet)
            {
                PodcastLoaded();
            }
        }
    }
}
