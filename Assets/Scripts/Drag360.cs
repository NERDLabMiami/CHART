using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // New Input System namespace

public class Drag360 : MonoBehaviour
{
    public GameObject invertedSphere; // The sphere that holds the video
    public float rotationSpeed = 0.2f; // Sensitivity of the drag rotation
    public float verticalRotationClamp = 60f; // Clamp for vertical rotation

    private InputAction dragAction; // Input Action for drag
    private Vector2 dragDelta;
    private float xRotation = 0f;
    private float yRotation = 0f;

    // Reference to the InputActionAsset (generated from Input Actions)
    public InputActionAsset inputActions;

    private void OnEnable()
    {
        // Enable the Input Action Map for pointer controls (touch/mouse)
        var pointerActionMap = inputActions.FindActionMap("UI"); // Replace with your actual action map
        dragAction = pointerActionMap.FindAction("Point"); // Replace with your actual action name for dragging

        dragAction.performed += OnDrag;
        dragAction.Enable();
    }

    private void OnDisable()
    {
        // Disable and unsubscribe
        dragAction.performed -= OnDrag;
        dragAction.Disable();
    }

    private void OnDrag(InputAction.CallbackContext context)
    {
        // Get the drag delta (from mouse or touch)
        dragDelta = context.ReadValue<Vector2>();

        // Apply the rotation based on the delta
        float rotationX = dragDelta.x * rotationSpeed;
        float rotationY = dragDelta.y * rotationSpeed;

        xRotation += rotationX;
        yRotation -= rotationY;

        // Clamp the vertical rotation to prevent flipping
        yRotation = Mathf.Clamp(yRotation, -verticalRotationClamp, verticalRotationClamp);

        // Apply the rotation to the camera
        Camera.main.transform.localRotation = Quaternion.Euler(yRotation, xRotation, 0f);
    }

    void Update()
    {
        // If needed, you can add additional logic to control input in Update
    }
}
