using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slideshow : MonoBehaviour
{
    public AudioSource currentClip;
    public int chapter;
    private Slide[] slides;

    public bool fadeInEnabled; // Control whether to fade in the image
    public float fadeInDuration; // Duration of fade-in effect
    public bool fadeOutEnabled; // Control whether to fade out the image
    public float fadeOutDuration; // Duration of fade-out effect

    private int currentSlideIndex = 0;

    void Start()
    {
        slides = GetComponentsInChildren<Slide>();
        for (int i = 0; i < slides.Length; i++)
        {
            slides[i].gameObject.SetActive(false);
        }
        if (slides.Length > 0)
        {
            slides[currentSlideIndex].gameObject.SetActive(true);
            StartCoroutine(PlaySlide(slides[currentSlideIndex]));
        }
    }

    public Slide GetNextSlide()
    {
        currentSlideIndex++;
        if (currentSlideIndex < slides.Length)
        {
            return slides[currentSlideIndex];
        }
        return null;
    }

    public IEnumerator PlaySlide(Slide slide)
    {
        if (slide == null)
        {
            Reset();
            yield break;
        }

        slide.StartTransition(this);

        if (slide.clip != null)
        {
            currentClip.clip = slide.clip;
            currentClip.Play();

            // Wait for the clip to finish
            yield return new WaitForSeconds(slide.clip.length);
        }

        // Ensure the slide is completed before moving to the next slide
        yield return new WaitUntil(() => slide.slideCompleted);

        // Fade out the current slide if enabled
        if (fadeOutEnabled)
        {
            StartCoroutine(FadeOutImage(slide.gameObject.GetComponent<Image>()));
            yield return new WaitForSeconds(fadeOutDuration);
        }

        Slide nextSlide = GetNextSlide();
        if (nextSlide != null)
        {
            nextSlide.gameObject.SetActive(true);

            if (fadeInEnabled)
            {
                StartCoroutine(FadeInImage(nextSlide.gameObject.GetComponent<Image>()));
            }

            StartCoroutine(PlaySlide(nextSlide));
        }
        else
        {
            Reset();
        }
    }

    private void Reset()
    {
        Debug.Log("Slideshow completed. Resetting...");
        // Implement reset logic here if needed
    }

    IEnumerator FadeInImage(Image image)
    {
        if (image == null) yield break;

        Color originalColor = image.color;
        float elapsedTime = 0f;

        while (elapsedTime < fadeInDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);
            image.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        image.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
    }

    IEnumerator FadeOutImage(Image image)
    {
        if (image == null) yield break;

        Color originalColor = image.color;
        float elapsedTime = 0f;

        while (elapsedTime < fadeOutDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
            image.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        image.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }
}
