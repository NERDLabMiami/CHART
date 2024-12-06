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
    private Vector2 lastPointerPosition;
    private Vector2 dragDelta;
    private float xRotation = 0f;
    private float yRotation = 0f;

    // Reference to the InputActionAsset (generated from Input Actions)
    public InputActionAsset inputActions;

    private void Start()
    {
        Vector3 initialRotation = Camera.main.transform.localEulerAngles;
        xRotation = initialRotation.x;
        yRotation = initialRotation.y;
        var pointerActionMap = inputActions.FindActionMap("UI"); // Replace with your actual action map
        if(pointerActionMap == null)
        {
            Debug.Log("Action Map Not Found");
            return;
        }
        dragAction = pointerActionMap.FindAction("Point"); // Replace with your actual action name for dragging
        if(dragAction == null)
        {
            Debug.Log("Action Map Point not found in UI");
            return;
        }
        dragAction.Enable();
    }

    void Update()
    {
        if (dragAction == null) return;

        Vector2 currentPointerPosition = dragAction.ReadValue<Vector2>();

        if(lastPointerPosition != Vector2.zero)
        {
            Vector2 dragDelta = currentPointerPosition - lastPointerPosition;
            float deltaX = dragDelta.x * rotationSpeed;
            float deltaY = dragDelta.y * rotationSpeed;
            xRotation += deltaX;
            yRotation -= deltaY;
            yRotation = Mathf.Clamp(yRotation, -verticalRotationClamp, verticalRotationClamp);
            Camera.main.transform.localRotation = Quaternion.Euler(yRotation, xRotation, 0f);
        }
        lastPointerPosition = currentPointerPosition;
    }
}
