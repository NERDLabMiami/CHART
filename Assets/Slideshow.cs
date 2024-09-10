using System.Collections;
using UnityEngine;

public class Slideshow : MonoBehaviour
{
    public Slide[] slides; // Array of slides
    private int currentSlideIndex = 0;

    public void StartSlideshow()
    {
        slides = GetComponentsInChildren<Slide>(true); // Get all slides (including inactive)
        if (slides.Length > 0)
        {
            StartCoroutine(PlaySlide(slides[0]));
        }
    }

    private IEnumerator PlaySlide(Slide slide)
    {
        slide.gameObject.SetActive(true); // Activate slide
        slide.StartSlide();

        // Wait for the slide to complete (based on its audio clip)
        yield return new WaitUntil(() => slide.slideCompleted);

        // Move to the next slide if available
        currentSlideIndex++;
        if (currentSlideIndex < slides.Length)
        {
            StartCoroutine(PlaySlide(slides[currentSlideIndex]));
        }
        else
        {
            // Inform the parent Podcast that the chapter is complete
            GetComponentInParent<Podcast>().NextChapter();
        }
    }
}
