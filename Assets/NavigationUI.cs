using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationUI : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator previousScreen;
    private string sceneToUnload;
    private string sceneToLoad;
    private SiteDatabase siteDatabase;
    void Start()
    {
        siteDatabase = FindObjectOfType<SiteDatabase>();

    }


    public void LoadScene(string scene)
    {
        
        if(transform.parent.parent.parent.parent.GetComponentInParent<AnimatableScreen>())
        {
            transform.parent.parent.parent.parent.GetComponentInParent<AnimatableScreen>().GetComponentInParent<Animator>().SetBool("Open", false);
        }
        previousScreen = transform.parent.parent.parent.parent.GetComponentInParent<AnimatableScreen>().GetComponentInParent<Animator>();
        sceneToLoad = scene;
        StartCoroutine(LoadScene());
//        SceneManager.LoadScene(scene, LoadSceneMode.Additive);
    }


    IEnumerator LoadScene()
    {
        yield return null;

        //Begin to load the Scene you specify

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        //Don't let the Scene activate until you allow it to
//        asyncOperation.allowSceneActivation = false;
        //When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {
            //Output the current progress?

            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {
                //Wait to you press the space key to activate the Scene?
            }

            yield return null;
        }
        if (siteDatabase)
        {
            if(GetComponent<SiteListing>())
            {
                //WHICH SITE WAS SELECTED:
                siteDatabase.selectedLocationIndex = GetComponent<SiteListing>().index;
            }
        }
        //SET NEW ACTIVE SCENE

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToLoad));


    }


    IEnumerator UnloadScene()
    {
        yield return null;

        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(sceneToUnload);
        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;
        //When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {
            //Output the current progress?

            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {
                //Wait to you press the space key to activate the Scene?
            }

            yield return null;
        }
    }



    public void SetSceneToUnload(string scene)
    {
       
        if(previousScreen)
        {
            Debug.Log("Previous Scene: " + scene);
            previousScreen.SetBool("Open", true);
        }
        else
        {
            Debug.Log("No previous scene found...");
        }
        int previousSceneBuildIndex = SceneManager.GetActiveScene().buildIndex - 1;
        if(previousSceneBuildIndex < 0)
        {
            previousSceneBuildIndex = 0;
        }
        GameObject[] previousSceneGameObjects = SceneManager.GetSceneByBuildIndex(previousSceneBuildIndex).GetRootGameObjects();
        for (int i = 0; i < previousSceneGameObjects.Length; i++)
        {
            if(previousSceneGameObjects[i].GetComponent<Canvas>())
            {
                if(previousSceneGameObjects[i].GetComponent<Canvas>().GetComponent<Animator>())
                {
                    previousSceneGameObjects[i].GetComponent<Canvas>().GetComponent<Animator>().SetBool("Open", true);
                }
            }
        }

            sceneToUnload = scene;
        StartCoroutine(UnloadScene());
    }
}
