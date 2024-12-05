using UnityEngine;

public class CanvasMetadata : MonoBehaviour
{
    [SerializeField] private string canvasTitle; // Title for this canvas

    public string CanvasTitle
    {
        get => canvasTitle;
        set => canvasTitle = value;
    }
}
