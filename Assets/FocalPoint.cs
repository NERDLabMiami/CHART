using UnityEngine;

public class FocalPoint : MonoBehaviour
{
    public float zoomLevel = 1;
    // Get the anchored position from the RectTransform
    public Vector2 GetPosition()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        return rectTransform.anchoredPosition;
    }
}
