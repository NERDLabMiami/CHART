using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PresentDay : MonoBehaviour
{
    public GameObject pastLookView;
    public VideoPlayer video;
    // Start is called before the first frame update
    void Start()
    {
        video.clip = Resources.Load<VideoClip>(PlayerPrefs.GetString("video_path"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchToPastLookView()
    {
        pastLookView.SetActive(true);
        gameObject.SetActive(false);
    }
}
