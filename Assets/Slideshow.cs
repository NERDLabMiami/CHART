using System.Collections;
using UnityEngine;

public class Slideshow : MonoBehaviour
{
//    public Slide[] slides; // Array of slides
    public GameObject[] slides;
    private int currentSlideIndex = 0;

    private void Start()
    {
       slides[currentSlideIndex].gameObject.SetActive(true);
    }
}
