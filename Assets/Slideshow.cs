using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slideshow : MonoBehaviour
{
    public GameObject imageTemplate;
    public float timeBetweenImages;
    private float timeUntilNextTransition;
    private List<GameObject> images;
    private int currentImageIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        images = new List<GameObject>();
        Sprite[] sprites = Resources.LoadAll<Sprite>(PlayerPrefs.GetString("illustration_path"));
        Debug.Log("FOUND " + sprites.Length + " images in " + PlayerPrefs.GetString("illustration_path"));
        for(int i = 0; i < sprites.Length; i++)
        {
            GameObject go = Instantiate(imageTemplate, transform);
            go.GetComponent<Image>().sprite = sprites[i];
            images.Add(go);

        }


        Debug.Log("IMAGES: " + images.Count);
        ShowImage(currentImageIndex);
        timeUntilNextTransition = Time.time + timeBetweenImages;
    }

    void ShowImage(int index)
    {
        if(images.Count > index)
        {
            for (int i = 0; i < images.Count; i++)
            {
                images[i].SetActive(false);
            }
    
            images[index].SetActive(true);

        }

    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= timeUntilNextTransition)
        {
            currentImageIndex++;
            if(images.Count <= currentImageIndex)
            {
                currentImageIndex = 0;
            }
            ShowImage(currentImageIndex);
            timeUntilNextTransition = Time.time + timeBetweenImages;
        }
    }
}
