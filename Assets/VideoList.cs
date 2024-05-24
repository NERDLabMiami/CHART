using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[System.Serializable]
public struct Clip
{
    public VideoClip clip;
    public int clipNumber;
}
public class VideoList : MonoBehaviour
{
    public List<Clip> videos;
    public VideoPlayer player;
    // Start is called before the first frame update
    void Start()
    {
        
        int clipNumber = PlayerPrefs.GetInt("clip number", 1);
        for(int i = 0; i < videos.Count; i++)
        {
            
            if(videos[i].clipNumber == clipNumber)
            {
                player.clip = videos[i].clip;
            }
            player.Play();
        }
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
