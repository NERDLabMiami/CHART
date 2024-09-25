using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlideController : MonoBehaviour
{
    public AudioClip slideAudioClip;           // Audio clip to play when the object becomes active
    private AudioSource audioSource;           // Reference to the AudioSource component
    private Slideshow parentSlideshow;         // Reference to the Slideshow component in the parent
    private int slideIndex;                    // Current slide index in the array

    private void OnEnable()
    {
        // Find the parent Slideshow component
        parentSlideshow = GetComponentInParent<Slideshow>();

        // Play the audio when the object becomes active
        audioSource = GetComponentInParent<AudioSource>();
        if (audioSource != null && slideAudioClip != null)
        {
            audioSource.clip = slideAudioClip;
            audioSource.Play();

            // Start a coroutine to wait for the audio to finish
            StartCoroutine(HandleAudioCompletion());
        }
    }

    private IEnumerator HandleAudioCompletion()
    {
        // Wait until the audio clip is done playing
        yield return new WaitWhile(() => audioSource.isPlaying);

        // Find the index of this slide in the parent Slideshow's slides array
        slideIndex = System.Array.IndexOf(parentSlideshow.slides, this.gameObject);

        // If we haven't reached the end of the array, move to the next slide
        if (slideIndex >= 0 && slideIndex < parentSlideshow.slides.Length - 1)
        {
            // Deactivate the current slide
            parentSlideshow.slides[slideIndex].SetActive(false);

            // Activate the next slide
            parentSlideshow.slides[slideIndex + 1].SetActive(true);
        }
        else
        {
            // Reached the end of the array, call a stub for advancing to a new set of objects
            AdvanceToNewSet();
        }
    }

    // Stub function for advancing to a new set of objects
    private void AdvanceToNewSet()
    {
        Debug.Log("Reached the end of the slideshow. Implement logic to advance to a new set of objects.");
        // TODO: Add logic to advance to a new set of objects
    }
}
