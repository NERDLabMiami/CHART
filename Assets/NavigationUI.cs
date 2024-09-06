using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationUI : MonoBehaviour
{
    private Animator previousScreen;
    private string sceneToUnload;
    private string sceneToLoad;
    private SiteDatabase siteDatabase;

    void Start()
    {
        // siteDatabase = FindObjectOfType<SiteDatabase>();
    }

    public void LoadScene(string scene)
    {
        if (transform.parent.parent.parent.parent.GetComponentInParent<AnimatableScreen>())
        {
            transform.parent.parent.parent.parent.GetComponentInParent<AnimatableScreen>().GetComponentInParent<Animator>().SetBool("Open", false);
        }
        previousScreen = transform.parent.parent.parent.parent.GetComponentInParent<AnimatableScreen>().GetComponentInParent<Animator>();
        sceneToLoad = scene;
        if (GetComponent<SiteListing>())
        {
            // WHICH SITE WAS SELECTED:
            Debug.Log("CHANGING SELECTED LOCATION INDEX!");
            // siteDatabase.selectedLocationIndex = GetComponent<SiteListing>().index;
        }

        StartCoroutine(LoadSceneWithCameraHandling());
    }

    IEnumerator LoadSceneWithCameraHandling()
    {
        yield return null;

        // Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);

        while (!asyncOperation.isDone)
        {
            // Output the current progress
            if (asyncOperation.progress >= 0.9f)
            {
                // Scene is almost loaded, deactivate any other cameras
                DeactivateOtherCameras(sceneToLoad);
            }

            yield return null;
        }

        if (siteDatabase)
        {
            if (GetComponent<SiteListing>())
            {
                // WHICH SITE WAS SELECTED:
                Debug.Log("CHANGING SELECTED LOCATION INDEX!");
                siteDatabase.selectedLocationIndex = GetComponent<SiteListing>().index;
                if (GetComponent<SiteListing>().GetComponentInParent<Populator>())
                {
                    GetComponent<SiteListing>().GetComponentInParent<Populator>().headerTitle.text = siteDatabase.GetLocationTitle(GetComponent<SiteListing>().index);
                }
            }
        }

        // Set new active scene
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToLoad));
    }

    IEnumerator UnloadScene()
    {
        yield return null;

        // Begin to unload the Scene you specify
        AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(sceneToUnload);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            // Check if the load has finished
            yield return null;
        }
    }

    public void SetSceneToUnload(string scene)
    {
        if (previousScreen)
        {
            Debug.Log("Previous Scene: " + scene);
            previousScreen.SetBool("Open", true);
        }
        else
        {
            Debug.Log("No previous scene found...");
        }

        sceneToUnload = scene;
        StartCoroutine(UnloadScene());
    }

    private void DeactivateOtherCameras(string activeSceneName)
    {
        // Deactivate all cameras except for the one in the newly loaded scene
        Camera[] allCameras = Camera.allCameras;

        foreach (Camera cam in allCameras)
        {
            if (cam.gameObject.scene.name != activeSceneName && cam.CompareTag("MainCamera"))
            {
                // Deactivate cameras in other scenes
                cam.gameObject.SetActive(false);
            }
        }
    }
}
