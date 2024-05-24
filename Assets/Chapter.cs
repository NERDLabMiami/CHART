using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Chapter : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public Image thumbnail;
    public int chapter;

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

    public void QueueChapter()
    {
        Debug.Log("QUEUING CHAPTER " + chapter);
        PlayerPrefs.SetInt("chapter", chapter);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
