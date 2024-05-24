using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Podcast : MonoBehaviour
{
    public List<Slideshow> chapters;
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
        
        int chapter = PlayerPrefs.GetInt("chapter", 0);
        Debug.Log("CHAPTER SELECTED: " + chapter);
        for(int i = 0; i < chapters.Count; i++)
        {
            Debug.Log("CHAPTER: " + chapters[i].chapter);
            if(chapters[i].chapter == chapter)
            {
                chapters[i].gameObject.SetActive(true);
            }
            else
            {
                chapters[i].gameObject.SetActive(false);

            }
        }
//        StartTour();
    
        }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTour()
    {
        /*
        audioClip = Resources.Load<AudioClip>(PlayerPrefs.GetString("podcast_path"));
        tour.clip = audioClip;
        tour.Play();
        isLoaded = true;
    */
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
