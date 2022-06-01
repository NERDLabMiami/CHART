using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Landmark : MonoBehaviour
{
    public GameObject mapIcon;
    public TextMeshProUGUI title;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey(title.text))
        {
            mapIcon.SetActive(true);
        }
        else
        {
            mapIcon.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Visit()
    {
        mapIcon.SetActive(true);
        PlayerPrefs.SetInt(title.text, 1);
    }

}
