using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// Lets a RectTransform fly in from one of the edges of its canvas.
/// This can be used with an animator.
public class AnimatableScreen : MonoBehaviour
{

    public enum Sides
    {
        Left,
        Right,
        Top,
        Bottom
    }

    /// The side from where the rect transform should fly in.
    public Sides side;

    /// The transition factor (from 0 to 1) between inside and outside.
    [Range(0, 1)]
    public float transition;
    public float transitionSpeed = 1.0f;

    /// Inside is assumed to be the start position of the RectTransform.
    private Vector2 inside;
    private Vector2 globalPosition;

    /// Outside is the position
    /// where the rect transform is completely outside of its canvas on the given side.
    private Vector2 outside;

    /// Reference to the rect transform component.
    private RectTransform rectTransform;
    /// Reference to the canvas component this RectTransform is on.
    private Canvas canvas;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        globalPosition = GetGlobalPosition(rectTransform);
        inside = rectTransform.localPosition;
        outside = inside + GetDifferenceToOutside();
    }

    Vector2 GetGlobalPosition(RectTransform trans)
    {
        Vector3[] corners = new Vector3[4];
        trans.GetWorldCorners(corners);
        // The center of the rect is the average of its four corners
        return (corners[0] + corners[2]) / 2;
    }



    void Update()
    {
        transition = Mathf.Lerp(transition, 1, Time.deltaTime * transitionSpeed);
        rectTransform.localPosition = Vector2.Lerp(outside, inside, transition);
    }

    Vector2 GetDifferenceToOutside()
    {
        // Pixel size of this RectTransform in normal resolution
        var size = rectTransform.rect.size;
        var pivot = rectTransform.pivot;
        // Pixel size of the canvas in normal resolution
        var canvasSize = canvas.GetComponent<RectTransform>().rect.size;
        // The summed up position has its origin in the center of the canvas.
        // However, in the calculation below, the position is assumed to have its origin in the lower left corner.
        // So we move the coords by canvasSize/2.
        var pos = globalPosition + (canvasSize / 2.0f);

        switch (side)
        {
            case Sides.Top:
                var distanceToTop = canvasSize.y - pos.y;
                return new Vector2(0f, distanceToTop + size.y * (pivot.y));
            case Sides.Bottom:
                var distanceToBottom = pos.y;
                return new Vector2(0f, -distanceToBottom - size.y * (1 - pivot.y));
            case Sides.Left:
                var distanceToLeft = pos.x;
                return new Vector2(-distanceToLeft - size.x * (1 - pivot.x), 0f);
            case Sides.Right:
                var distanceToRight = canvasSize.x - pos.x;
                return new Vector2(distanceToRight + size.x * (pivot.x), 0f);
            default:
                return Vector2.zero;
        }
    }

    public void ToggleVisible()
    {
        var anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetBool("Hide", !anim.GetBool("Hide"));
        }
        else
        {
            Debug.LogWarning("Animator component not found!");
        }
    }
}
