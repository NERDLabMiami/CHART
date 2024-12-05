using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;




public class AudioTour : MonoBehaviour
{
    [Serializable]
    public struct RotationTimePoint
    {
        public float timeInSeconds;
        public float degreesToRotate;
    }


    public RenderTexture renderTexture;
    public RotationTimePoint[] timePoints;
    public float rotationDuration = .5f;
    public Camera skyboxCamera;

    private int timePointIndex = 0;
    
    private VideoPlayer video;

    private bool playingTour = false;
    private bool lastStopOnTour = false;
    private float tourTime;
       
    // Start is called before the first frame update
    void Start()
    {
        video = GetComponent<VideoPlayer>();
        UserControl();
    }


    // Update is called once per frame
    void Update()
    {
        if (playingTour)
        {
            tourTime += Time.deltaTime;
            //This method works for a singular audio file with timestamps for rotation
            //It could also be done with an array of audio files where each end of file triggered a new rotation

            if (tourTime >= timePoints[timePointIndex].timeInSeconds && !lastStopOnTour)
            {
                //CHANGE CAMERA TARGET ROTATION
                SetRotation();
            }

            if (timePointIndex >= timePoints.Length)
            {
                Debug.Log("Resetting Time Points");
                timePointIndex = 0;
                lastStopOnTour = true;
            }


        }

    }


    public void StartTour()
    {
        video.renderMode = VideoRenderMode.RenderTexture;
        video.targetTexture = renderTexture;
        SetRotation();
        tourTime = 0;
        playingTour = true;
        lastStopOnTour = false;

    }


    public void UserControl()
    {
        video.renderMode = VideoRenderMode.CameraFarPlane;
        video.targetCamera = skyboxCamera;
        playingTour = false;
    }


    private void SetRotation()
    {
        Debug.Log("Hit Time Point #" + timePointIndex);
        StartCoroutine(RotateSkybox());
        timePointIndex++;
    }
    
    IEnumerator RotateSkybox()
    {
        float timeElapsed = 0;
        Quaternion startRotation = skyboxCamera.transform.rotation;
        Quaternion targetRotation = skyboxCamera.transform.rotation * Quaternion.Euler(0, timePoints[timePointIndex].degreesToRotate, 0);
        while (timeElapsed < rotationDuration)
        {
            skyboxCamera.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / rotationDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        skyboxCamera.transform.rotation = targetRotation;
    }
    


}
