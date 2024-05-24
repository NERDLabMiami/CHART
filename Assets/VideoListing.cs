using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VideoListing : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public Image thumbnail;
    public int clipNumber;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey(title.text))
        {
            //HAS VISITED THIS CHAPTER. TRACK?
        }
        else
        {
        }
    }

    public void QueueVideo()
    {
        Debug.Log("QUEUING VIDEO " + clipNumber);
        PlayerPrefs.SetInt("clip number", clipNumber);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
