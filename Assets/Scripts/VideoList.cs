using UnityEngine;
using System.Collections.Generic;

public class VideoList : MonoBehaviour
{
    private Chapter[] chapters;  // Array to hold all Chapter components
    private int currentChapter = 0;  // Track the active chapter index
    public Controls controls;

    void Start()
    {
        // Collect all Chapter components from the children of this GameObject
        chapters = GetComponentsInChildren<Chapter>();

        // Ensure each chapter knows about this VideoList manager
        foreach (var chapter in chapters)
        {
            chapter.container = this;  // Assign this VideoList instance to each chapter
        }

        // Initialize by playing the starting chapter
        currentChapter = SetStartingChapter();
        chapters[currentChapter].Play();  // Start the first (or configured) chapter
        controls.ChapterControls(true);
        if (currentChapter == 0)
        {
            controls.previousChapterButton.SetActive(false);
        }
        if(currentChapter > chapters.Length - 1)
        {
            controls.nextChapterButton.SetActive(false);
        }
    }

    private int SetStartingChapter()
    {
        // Look for the first chapter marked as the starting point
        for (int i = 0; i < chapters.Length; i++)
        {
            if (chapters[i].IsStartingPoint())
                return i;
        }
        return 0;  // Default to the first chapter if no starting point is found
    }

    public void GoToNextChapter()
    {
        if (currentChapter < chapters.Length - 1)
        {
            controls.previousChapterButton.SetActive(true);
            chapters[currentChapter].Stop();  // Stop the current chapter
            currentChapter++;
            chapters[currentChapter].Play();  // Play the next chapter
        }
        else
        {
            controls.nextChapterButton.SetActive(false);
        }

    }

    public void GoToPreviousChapter()
    {
        if (currentChapter > 0)
        {
            controls.nextChapterButton.SetActive(true);
            chapters[currentChapter].Stop();  // Stop the current chapter
            currentChapter--;
            chapters[currentChapter].Play();  // Play the previous chapter
        }
        else
        {
            controls.previousChapterButton.SetActive(false);

        }
    }
}
