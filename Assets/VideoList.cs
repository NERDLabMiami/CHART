using UnityEngine;
using System.Collections.Generic;

public class VideoList : MonoBehaviour
{
    private Chapter[] chapters;  // Array to hold all Chapter components
    private int currentChapter = 0;  // Track the active chapter index
    public Controls controls;  // Link to the Controls class

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
            chapters[currentChapter].Stop();  // Stop the current chapter
            currentChapter++;
            chapters[currentChapter].Play();  // Play the next chapter
        }
    }

    public void GoToPreviousChapter()
    {
        if (currentChapter > 0)
        {
            chapters[currentChapter].Stop();  // Stop the current chapter
            currentChapter--;
            chapters[currentChapter].Play();  // Play the previous chapter
        }
    }
}
