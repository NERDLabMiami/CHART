using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Podcast : MonoBehaviour
{
    public AudioSource tour;
    public AudioClip audioClip;
    public Sprite pauseButton;
    public Sprite unpauseButton;
    public bool isLoaded = false;
    
    //TODO:
    //SCRUBBING

    // Start is called before the first frame update
    void Start()
    {
        StartTour();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTour()
    {
        audioClip = Resources.Load<AudioClip>(PlayerPrefs.GetString("podcast_path"));
        tour.clip = audioClip;
        tour.Play();
        isLoaded = true;
    }

    public void TogglePlayPause()
    {
        if(tour.isPlaying)
        {
            tour.Pause();
        }
        else
        {
            tour.UnPause();
        }

    }

}
