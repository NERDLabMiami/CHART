using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
