using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Populator : MonoBehaviour
{
    public SiteDatabase siteDatabase;
    public GameObject prefabTemplate;
    public TextMeshProUGUI headerTitle;
    // Start is called before the first frame update
    void Start()
    {
        if(!siteDatabase)
        {
            siteDatabase = FindObjectOfType<SiteDatabase>();
        }
        //POPULATOR NEEDS INDEX

        siteDatabase.PopulateList(this.transform, prefabTemplate, this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
