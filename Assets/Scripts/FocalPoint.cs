using UnityEngine;

public class FocalPoint : MonoBehaviour
{
    public float zoomLevel = 1;
    public float waitTime = 1f;  // The amount of time to pause at this focal point

    // Get the anchored position from the RectTransform
    public Vector2 GetPosition()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        return rectTransform.anchoredPosition;
    }
}
