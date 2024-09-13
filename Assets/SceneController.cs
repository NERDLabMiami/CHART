using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour
{
    private static SceneController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Make this object persist across scenes
        }
        else
        {
            Destroy(gameObject);  // Ensure there's only one instance of SceneController
        }
    }

    public static SceneController Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("SceneController");
                instance = go.AddComponent<SceneController>();
            }
            return instance;
        }
    }

    // Method to unload a scene and optionally activate a canvas in the next scene
    public void UnloadScene(string sceneToUnload)
    {
        StartCoroutine(UnloadSceneCoroutine(sceneToUnload));
    }

    private IEnumerator UnloadSceneCoroutine(string sceneToUnload)
    {
        yield return null;

        // Deactivate the old scene's canvases and cameras
        Canvas[] allCanvases = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in allCanvases)
        {
            if (canvas.gameObject.scene.name == sceneToUnload)
            {
                canvas.gameObject.SetActive(false);
            }
        }

        Camera[] allCameras = FindObjectsOfType<Camera>();
        foreach (Camera cam in allCameras)
        {
            if (cam.gameObject.scene.name == sceneToUnload)
            {
                cam.gameObject.SetActive(false);
            }
        }

        // Unload the scene asynchronously
        AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(sceneToUnload);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        Debug.Log($"Scene {sceneToUnload} unloaded.");
    }


}
