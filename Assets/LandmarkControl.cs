using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandmarkControl : MonoBehaviour
{
    public Podcast podcast;
    public PresentDay presentDay;
    public Slideshow slideshow;
    public LandmarkHeader pastHeader;
    public LandmarkHeader navigationHeader;

    private SiteDatabase siteDatabase;
    
    // Start is called before the first frame update
    void Start()
    {
        siteDatabase = FindObjectOfType<SiteDatabase>();

    }

    public void NextLandmark()
    {
        siteDatabase.NextSite();
        LoadLandmark();
    }

    public void PreviousLandmark()
    {
        siteDatabase.PreviousSite();
        LoadLandmark();
    }

    public void LoadLandmark()
    {
        pastHeader.SetTitle();
        navigationHeader.SetTitle();
        presentDay.LoadClip();
        podcast.StartTour();
        /*
        slideshow.ResetSlideshow();
        slideshow.LoadSlideshow();
        */
    }
}
