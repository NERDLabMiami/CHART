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
        LoadSlideshow();
    }

    public void LoadSlideshow()
    {
        images = new List<GameObject>();
        Sprite[] sprites = Resources.LoadAll<Sprite>(PlayerPrefs.GetString("illustration_path"));
        Debug.Log("FOUND " + sprites.Length + " images in " + PlayerPrefs.GetString("illustration_path"));
        for (int i = 0; i < sprites.Length; i++)
        {
            GameObject go = Instantiate(imageTemplate, transform);
            go.GetComponent<Image>().sprite = sprites[i];
            images.Add(go);

        }

        ShowImage(currentImageIndex);
        timeUntilNextTransition = Time.time + timeBetweenImages;

    }

    public void ResetSlideshow()
    {
        images.Clear();
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
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
