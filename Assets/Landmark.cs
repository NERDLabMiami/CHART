using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Landmark : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public Image thumbnail;
    public int index;
//    public UnityEngine.Video.VideoClip presentDay360Video;
    public AudioClip podcast;
    public string pathToIllustrations;
    public string pathTo360Video;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey(title.text))
        {
            //HAS VISITED THIS LANDMARK. TRACK?
        }
        else
        {
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Visit()
    {
        //TRACK VISIT IN PREFS
        PlayerPrefs.SetInt(title.text, 1);
        PlayerPrefs.SetString("illustration_path", pathToIllustrations);
        PlayerPrefs.SetString("video_path", pathTo360Video);
    }

}
