using UnityEngine;
//using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;

public class Chapter : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public Image thumbnail;
    //    public VideoClip videoClip;
    public string videoHost = "https://nerdlab.miami/chart/Videos/";
    public string videoURL;
    public Controls controls;  // Reference to Controls
    public VideoList container;  // Reference to the parent VideoList manager
    public bool isStartingPoint = false;  // To set the starting point

    public void Play()
    {
        // Use the Controls class to start the video playback
        isStartingPoint = true;
        controls.video.url = videoHost + videoURL;
        controls.PlayChapter(this);
    }

    public void Stop()
    {
        // Use the Controls class to stop the video playback
        controls.StopVideo();
    }

    public bool IsStartingPoint()
    {
        return isStartingPoint;
    }
}
