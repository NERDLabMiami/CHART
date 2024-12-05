using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LandmarkHeader : MonoBehaviour
{
    public TextMeshProUGUI title;
    // Start is called before the first frame update
    void Start()
    {
        SetTitle();
    }

    public void SetTitle()
    {
        title.text = PlayerPrefs.GetString("selected_landmark");
    }
}
