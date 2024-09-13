using System.Collections;
using UnityEngine;

public class Podcast : MonoBehaviour
{
    public Slideshow[] chapters; // Array of chapters (slideshows)
    private int currentChapter = 0; // Make sure this starts at 0
    /*
    void Start()
    {
        // Load the saved chapter index, if any, otherwise default to 0
        currentChapter = PlayerPrefs.GetInt("chapter", 0);
        Debug.Log("Starting at chapter: " + currentChapter);

        // Ensure currentChapter is within bounds
        if (currentChapter < 0 || currentChapter >= chapters.Length)
        {
            currentChapter = 0; // Default to chapter 0 if out of bounds
        }

        // Start the first chapter
        if (chapters.Length > 0)
        {
            StartChapter(chapters[currentChapter]);
        }
    }

    public void StartChapter(Slideshow slideshow)
    {
        Debug.Log("Starting chapter: " + currentChapter);
        slideshow.gameObject.SetActive(true); // Activate the current chapter
        slideshow.StartSlideshow(); // Start the slideshow
    }

    public void NextChapter()
    {
        // Deactivate the current chapter and activate the next chapter if available
        if (currentChapter < chapters.Length)
        {
            chapters[currentChapter].gameObject.SetActive(false);
            currentChapter++;

            if (currentChapter < chapters.Length)
            {
                StartChapter(chapters[currentChapter]);
            }
            else
            {
                Debug.Log("No more chapters to play.");
            }
        }
    }
    */
}
